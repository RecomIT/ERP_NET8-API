using API.Base;
using API.Services;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using BLL.Tax.Interface;
using BLL.Base.Interface;
using Shared.Payroll.DTO.Tax;
using Microsoft.AspNetCore.Mvc;
using Shared.Payroll.Filter.Tax;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using BLL.Tax.Implementation;

namespace API.Areas.Tax
{
    [ApiController, Area("Payroll"), Route("api/[area]/Tax/[controller]"), Authorize]
    public class TaxRefundController : ApiBaseController
    {
        private readonly ITaxRefundBusiness _taxRefundBusiness;
        private readonly ISysLogger _sysLogger;
        public TaxRefundController(
            ITaxRefundBusiness taxRefundBusiness,
            ISysLogger _sysLogger,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _taxRefundBusiness = taxRefundBusiness;
        }

        [HttpPost("Save")]
        public async Task<IActionResult> SaveAsync(TaxRefundSubmissionDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _taxRefundBusiness.SaveAsync(model, user);
                    if (data.Status)
                    {
                        return Ok(data);
                    }
                    else
                    {
                        return BadRequest(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxRefundController", "SaveTaxRefund", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromQuery] TaxDocumentQuery filter)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _taxRefundBusiness.GetAllAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxRefundController", "GetAllAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetById")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _taxRefundBusiness.GetAllAsync(new TaxDocumentQuery() { SubmissionId = id.ToString() }, user);
                    return Ok(data.ListOfObject.FirstOrDefault());
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxRefundController", "GetAllAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UploadRefund")]
        public async Task<IActionResult> UploadAsync([FromForm] UploadTaxDocumentDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    if (model.File?.Length > 0)
                    {
                        var stream = model.File.OpenReadStream();
                        List<TaxDocumentSubmissionDTO> list = new List<TaxDocumentSubmissionDTO>();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                            var rowCount = worksheet!.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++)
                            {

                                var employeeCode = worksheet.Cells[row, 1].Value.ToString();
                                var amount = Utility.TryParseDecimal(worksheet.Cells[row, 2].Value?.ToString());

                                if (Utility.IsNullEmptyOrWhiteSpace(employeeCode) == false && amount > 0)
                                {
                                    TaxDocumentSubmissionDTO item = new TaxDocumentSubmissionDTO()
                                    {
                                        EmployeeCode = employeeCode,
                                        FiscalYearId = model.FiscalYearId,
                                        CertificateType = model.CertificateType,
                                        Amount = amount
                                    };
                                    list.Add(item);
                                }
                            }
                        }
                        if (list.Any())
                        {
                            var status = await _taxRefundBusiness.UploadAsync(list, user);
                            if (status.Status)
                            {
                                return Ok(status);
                            }
                            else
                            {
                                return BadRequest(status);
                            }
                        }
                    }
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITController", "UploadAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("DeleteRefund")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (id > 0 && user.HasBoth)
                {
                    var data = await _taxRefundBusiness.DeleteAsync(id, user);
                    if (data.Status)
                    {
                        return Ok(data);
                    }
                    else
                    {
                        return BadRequest(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxRefundController", "DeleteAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
