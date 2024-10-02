using API.Base;
using BLL.Base.Interface;
using BLL.Tax.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog.Fluent;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Payroll.DTO.Tax;
using Shared.Payroll.Filter.Tax;
using Shared.Payroll.ViewModel.Tax;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Tax
{
    [ApiController, Area("Payroll"), Route("api/[area]/Tax/[controller]"), Authorize]
    public class InvestmentSubmissionController : ApiBaseController
    {
        private readonly IEmployeeInvestmentSubmissionBusiness _employeeInvestmentSubmissionBusiness;
        private readonly ISysLogger _sysLogger;
        public InvestmentSubmissionController(IEmployeeInvestmentSubmissionBusiness employeeInvestmentSubmissionBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _employeeInvestmentSubmissionBusiness = employeeInvestmentSubmissionBusiness;
        }

        [HttpPost, Route("SaveEmployeeYearlyInvestment")]
        public async Task<IActionResult> SaveEmployeeYearlyInvestmentAsync(EmployeeYearlyInvestmentDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _employeeInvestmentSubmissionBusiness.SaveEmployeeYearlyInvestmentAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "TaxController", "SaveEmployeeYearlyInvestmentAsync", user);
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetEmployeeYearlyInvestments")]
        public async Task<IActionResult> GetEmployeeYearlyInvestmentsAsync([FromQuery] EmployeeYearInvestment_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _employeeInvestmentSubmissionBusiness.GetEmployeeYearlyInvestmentsAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "TaxController", "SaveEmployeeYearlyInvestmentAsync", user);
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet("GetEmployeeYearlyInvestmentById")]
        public async Task<IActionResult> GetEmployeeYearlyInvestmentByIdAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (id > 0 && user.HasBoth)
                {
                    var data = await _employeeInvestmentSubmissionBusiness.GetEmployeeYearlyInvestmentByIdAsync(id, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "InvestmentSubmissionController", "GetEmployeeYearlyInvestmentByIdAsync", user);
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("UploadEmployeeYearlyInvestmentExcel")]
        public async Task<IActionResult> UploadEmployeeYearlyInvestmentExcelAsync([FromForm] UploadEmployeeYearlyInvestmentDTO uploadedFile)
        {
            try
            {
                var appUser = AppUser();

                if (ModelState.IsValid)
                {
                    if (uploadedFile.ExcelFile?.Length > 0)
                    {
                        var stream = uploadedFile.ExcelFile.OpenReadStream();
                        List<EmployeeYearlyInvestmentViewModel> viewModels = new List<EmployeeYearlyInvestmentViewModel>();

                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++)
                            {
                                var code = worksheet.Cells[row, 1].Value.ToString();
                                if (code != null)
                                {
                                    EmployeeYearlyInvestmentViewModel model = new EmployeeYearlyInvestmentViewModel();
                                    var empCode = worksheet.Cells[row, 1].Value.ToString();
                                    var amount = Convert.ToDecimal(worksheet.Cells[row, 2].Value?.ToString());
                                    model.EmployeeCode = empCode;
                                    model.InvestmentAmount = amount;
                                    model.FiscalYearId = uploadedFile.FiscalYearId;
                                    viewModels.Add(model);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        var data = await _employeeInvestmentSubmissionBusiness.UploadEmployeeYearlyInvestmentExcelAsync(viewModels, appUser);
                        return Ok(data);

                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }
    }
}
