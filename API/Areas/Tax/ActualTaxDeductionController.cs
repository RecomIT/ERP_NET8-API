using API.Base;
using API.Services;
using BLL.Base.Interface;
using BLL.Tax.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Shared.Payroll.DTO.Tax;
using Shared.Payroll.Filter.Tax;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Tax
{
    [ApiController, Area("Payroll"), Route("api/[area]/Tax/[controller]"), Authorize]
    public class ActualTaxDeductionController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IActualTaxDeductionBusiness _actualTaxDeductionBusiness;
        public ActualTaxDeductionController(IActualTaxDeductionBusiness actualTaxDeductionBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _actualTaxDeductionBusiness = actualTaxDeductionBusiness;
        }

        [HttpPost, Route("SaveUploadActualTaxDeductionAmount")]
        public async Task<IActionResult> SaveUploadActualTaxDeductionAmountAsync([FromForm] UploadActualTaxDeductionDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    if (model.File?.Length > 0)
                    {
                        var stream = model.File.OpenReadStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            List<ActualTaxDeductionDTO> list = new List<ActualTaxDeductionDTO>();
                            for (var row = 2; row <= rowCount; row++)
                            {
                                ActualTaxDeductionDTO actualTax = new ActualTaxDeductionDTO();
                                actualTax.EmployeeCode = worksheet.Cells[row, 1].Value?.ToString();
                                actualTax.ActualTaxAmount = Convert.ToDecimal(worksheet.Cells[row, 2].Value?.ToString());
                                actualTax.SalaryMonth = model.SalaryMonth;
                                actualTax.SalaryYear = model.SalaryYear;
                                actualTax.ActualFileName = model.File.FileName;
                                actualTax.SystemFileName = model.File.FileName;
                                actualTax.FileFormat = model.File.FileName.Substring(model.File.FileName.LastIndexOf(".") + 1);
                                list.Add(actualTax);
                            }
                            var data = await _actualTaxDeductionBusiness.SaveUploadInfosAsync(list, user);
                            return Ok(data);
                        }
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ActualTaxDeductionController", "SaveUploadActualTaxDeductionAmount", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveActualTaxDeduction")]
        public async Task<IActionResult> SaveActualTaxDeductionAsync(ActualTaxDeductionDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _actualTaxDeductionBusiness.SaveActualTaxDeductionAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ActualTaxDeductionController", "SaveActualTaxDeductionAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetActualTaxDeductionInfos")]
        public async Task<IActionResult> GetActualTaxDeductionInfosAsync([FromQuery] ActualTaxDeduction_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _actualTaxDeductionBusiness.GetActualTaxDeductionInfosAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ActualTaxDeductionController", "GetActualTaxDeductionInfos", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetActualTaxDeductionById")]
        public async Task<IActionResult> GetActualTaxDeductionById(int id)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = (await _actualTaxDeductionBusiness.GetActualTaxDeductionInfosAsync(new ActualTaxDeduction_Filter { ActualTaxDeductionId = id.ToString() }, user)).ToList().FirstOrDefault();
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ActualTaxDeductionController", "GetActualTaxDeductionById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveActualTaxApproval")]
        public async Task<IActionResult> SaveActualTaxApproval(ActualTaxDeductionApprovalDTO model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var data = await _actualTaxDeductionBusiness.SaveApprovalAsync(model, user);
                    var approvalState = await _actualTaxDeductionBusiness.UpdateActaulTaxDeductedInSalaryAndTaxAsync(new UpdateActaulTaxDeductedDTO() { SalaryMonth = model.SalaryMonth, SalaryYear = model.SalaryYear }, user);
                    return Ok(new { approvalUpdates = data, approvalState });
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ActualTaxDeductionController", "SaveActualTaxApproval", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UpdateActaulTaxDeductedInSalaryAndTax")]
        public async Task<IActionResult> UpdateActaulTaxDeductedInSalaryAndTaxAsync(UpdateActaulTaxDeductedDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _actualTaxDeductionBusiness.UpdateActaulTaxDeductedInSalaryAndTaxAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ActualTaxDeductionController", "UpdateActaulTaxDeductedInSalaryAndTax", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
