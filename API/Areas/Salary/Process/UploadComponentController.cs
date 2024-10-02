using API.Base;
using API.Services;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using BLL.Salary.Salary.Interface;
using Shared.Payroll.ViewModel.Salary;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Salary.Process
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class UploadComponentController : ApiBaseController
    {
        private readonly IUploadSalaryComponentBusiness _uploadSalaryComponentBusiness;
        private readonly ISysLogger _sysLogger;
        public UploadComponentController(ISysLogger sysLogger, IUploadSalaryComponentBusiness uploadSalaryComponentBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _uploadSalaryComponentBusiness = uploadSalaryComponentBusiness;
            _sysLogger = sysLogger;
        }

        [HttpPost, Route("UploadExcel")]
        public async Task<IActionResult> UploadExcelAsync([FromForm] SalaryComponentUpload uploadedFile)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    if (uploadedFile.ExcelFile?.Length > 0)
                    {
                        var stream = uploadedFile.ExcelFile.OpenReadStream();
                        if (uploadedFile.SalaryComponent == "Allowance")
                        {
                            List<SalaryAllowanceViewModel> allowances = new List<SalaryAllowanceViewModel>();
                            using (var package = new ExcelPackage(stream))
                            {
                                var worksheet = package.Workbook.Worksheets.First();
                                var rowCount = worksheet.Dimension.Rows;

                                var monthAndYear = uploadedFile.SalaryMonthAndYear.Split('-');
                                var month = Convert.ToInt16(monthAndYear[0]);
                                var year = Convert.ToInt16(monthAndYear[1]);
                                var days = DateTime.DaysInMonth(year, month);
                                var salaryDate = new DateTime(year, month, days);

                                for (var row = 2; row <= rowCount; row++)
                                {
                                    var amount = worksheet.Cells[row, 2].Value?.ToString();
                                    SalaryAllowanceViewModel salaryAllowance = new SalaryAllowanceViewModel();
                                    salaryAllowance.EmployeeCode = worksheet.Cells[row, 1].Value?.ToString();
                                    salaryAllowance.AllowanceNameId = uploadedFile.AllowanceId;
                                    salaryAllowance.Amount = Convert.ToDecimal(worksheet.Cells[row, 2].Value?.ToString());
                                    salaryAllowance.Remarks = worksheet.Cells[row, 3].Value?.ToString();
                                    salaryAllowance.SalaryMonth = month;
                                    salaryAllowance.SalaryYear = year;
                                    salaryAllowance.SalaryDate = salaryDate;
                                    salaryAllowance.CompanyId = uploadedFile.CompanyId;
                                    salaryAllowance.OrganizationId = uploadedFile.OrganizationId;
                                    salaryAllowance.CreatedBy = uploadedFile.UserId;
                                    allowances.Add(salaryAllowance);
                                }
                            }

                            var data = await _uploadSalaryComponentBusiness.UploadAllowanceAsync(allowances, user);
                            return Ok(data);
                        }

                        else if (uploadedFile.SalaryComponent == "Deduction")
                        {
                            List<SalaryDeductionViewModel> deductions = new List<SalaryDeductionViewModel>();
                            using (var package = new ExcelPackage(stream))
                            {
                                var worksheet = package.Workbook.Worksheets.First();
                                var rowCount = worksheet.Dimension.Rows;

                                var monthAndYear = uploadedFile.SalaryMonthAndYear.Split('-');
                                var month = Convert.ToInt16(monthAndYear[0]);
                                var year = Convert.ToInt16(monthAndYear[1]);
                                var days = DateTime.DaysInMonth(year, month);
                                var salaryDate = new DateTime(year, month, days);

                                for (var row = 2; row <= rowCount; row++)
                                {
                                    SalaryDeductionViewModel salaryDeduction = new SalaryDeductionViewModel();
                                    salaryDeduction.EmployeeCode = worksheet.Cells[row, 1].Value?.ToString();
                                    salaryDeduction.DeductionNameId = uploadedFile.DeductionId;
                                    salaryDeduction.Amount = Convert.ToDecimal(worksheet.Cells[row, 2].Value?.ToString());
                                    salaryDeduction.Remarks = worksheet.Cells[row, 3].Value?.ToString();
                                    salaryDeduction.SalaryMonth = month;
                                    salaryDeduction.SalaryYear = year;
                                    salaryDeduction.SalaryDate = salaryDate;
                                    salaryDeduction.CompanyId = uploadedFile.CompanyId;
                                    salaryDeduction.OrganizationId = uploadedFile.OrganizationId;
                                    salaryDeduction.CreatedBy = uploadedFile.UserId;
                                    deductions.Add(salaryDeduction);
                                }
                            }

                            var data = await _uploadSalaryComponentBusiness.UploadDeductionAsync(deductions, user);
                            return Ok(data);
                        }
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploadComponent", "UploadExcel", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetUploadAllowances")]
        public async Task<IActionResult> GetUploadAllowancesAsync(long? uploadId, long? allowanceNameId, long? employeeId, short? month, short? year, long companyId, long organizationId, int pageNumber = 1, int pageSize = 15)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    pageNumber = Utility.PageNumber(pageNumber);
                    pageSize = Utility.PageSize(pageSize);
                    var allData = await _uploadSalaryComponentBusiness.GetUploadAllowancesAsync(uploadId ?? 0, allowanceNameId ?? 0, employeeId ?? 0, month ?? 0, year ?? 0, user);
                    var data = PagedList<UploadAllowanceViewModel>.ToPagedList(allData, pageNumber, pageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploadComponent", "GetUploadAllowances", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetUploadDeductions")]
        public async Task<IActionResult> GetUploadDeductionsAsync(long? uploadId, long? deductionNameId, long? employeeId, short? month, short? year, long companyId, long organizationId, int pageNumber = 1, int pageSize = 15)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    pageNumber = Utility.PageNumber(pageNumber);
                    pageSize = Utility.PageSize(pageSize);
                    var allData = await _uploadSalaryComponentBusiness.GetUploadDeductionsAsync(uploadId ?? 0, deductionNameId ?? 0, employeeId ?? 0, month ?? 0, year ?? 0, user);
                    var data = PagedList<UploadDeductionViewModel>.ToPagedList(allData, pageNumber, pageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploadComponent", "GetUploadDeductions", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UpdateAllowance")]
        public async Task<IActionResult> UpdateAllowanceAsync(UploadAllowanceViewModel model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _uploadSalaryComponentBusiness.UpdateAllowanceAsync(model.Id, model.AllowanceNameId, model.EmployeeId, model.Year, model.Month, model.Amount, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploadComponent", "UpdateAllowance", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UpdateDeduction")]
        public async Task<IActionResult> UpdateDeductionAsync(UploadDeductionViewModel model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _uploadSalaryComponentBusiness.UpdateDeductionAsync(model.Id, model.DeductionNameId, model.EmployeeId, model.Year, model.Month, model.Amount, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploadComponentController", "UpdateDeduction", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
