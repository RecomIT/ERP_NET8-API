using API.Base;
using API.Services;
using OfficeOpenXml;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using BLL.Salary.Salary.Interface;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.DTO.SalaryHold;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Salary.Process
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class SalaryHoldController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly ISalaryHoldBusiness _salaryHoldBusiness;
        public SalaryHoldController(ISysLogger sysLogger, ISalaryHoldBusiness salaryHoldBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _salaryHoldBusiness = salaryHoldBusiness;
        }

        [HttpGet, Route("GetSalaryHoldInfos")]
        public async Task<IActionResult> GetSalaryHoldInfosAsync([FromQuery] SalaryHold_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _salaryHoldBusiness.GetSalaryHoldListAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryController", "GetSalaryHoldInfosAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSalaryHoldInfoById")]
        public async Task<IActionResult> GetSalaryHoldInfoByIdAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = (await _salaryHoldBusiness.GetSalaryHoldListAsync(new SalaryHold_Filter() { SalaryHoldId = id.ToString() }, user)).ToList().FirstOrDefault();
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryController", "GetSalaryHoldInfosAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveSalaryHold")]
        public async Task<IActionResult> SaveSalaryHoldAsync(SalaryHoldDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var validator = await _salaryHoldBusiness.ValidatorSalaryHoldAsync(model, user);
                    if (validator != null && validator.Status == false)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _salaryHoldBusiness.SaveSalaryHoldAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryController", "SaveSalaryHold", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveUploadHoldSalary")]
        public async Task<IActionResult> SaveUploadHoldSalaryAsync([FromForm] UploadSalaryHoldInfoDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    if (model.File?.Length > 0)
                    {
                        List<SalaryHoldDTO> list = new List<SalaryHoldDTO>();
                        var stream = model.File.OpenReadStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            var days = DateTime.DaysInMonth(model.SalaryYear, model.SalaryMonth);
                            var salaryFromDate = new DateTime(model.SalaryYear, model.SalaryMonth, 1);
                            var daysInMonth = DateTime.DaysInMonth(model.SalaryYear, model.SalaryMonth);
                            var salaryToDate = new DateTime(model.SalaryYear, model.SalaryMonth, daysInMonth);
                            for (var row = 2; row <= rowCount; row++)
                            {
                                SalaryHoldDTO salary = new SalaryHoldDTO();
                                var employeeCode = worksheet.Cells[row, 1].Value?.ToString();
                                var holdReason = worksheet.Cells[row, 2].Value?.ToString();
                                DateTime salaryHoldFrom;
                                DateTime.TryParse(worksheet.Cells[row, 3].Value?.ToString(), out salaryHoldFrom);
                                DateTime salaryHoldTo;
                                DateTime.TryParse(worksheet.Cells[row, 4].Value?.ToString(), out salaryHoldTo);
                                var withSalary = worksheet.Cells[row, 5].Value?.ToString();
                                var pfContinue = worksheet.Cells[row, 6].Value?.ToString();
                                var gfContinue = worksheet.Cells[row, 7].Value?.ToString();
                                if (!Utility.IsNullEmptyOrWhiteSpace(employeeCode)
                                    && salaryHoldFrom >= salaryFromDate.Date
                                    && salaryHoldTo <= salaryFromDate.Date
                                    && salaryHoldFrom >= salaryToDate.Date
                                    && salaryHoldTo <= salaryToDate.Date)
                                {
                                    salary.EmployeeCode = employeeCode;
                                    salary.Month = model.SalaryMonth;
                                    salary.Year = model.SalaryYear;
                                    salary.HoldReason = holdReason;
                                    salary.HoldFrom = salaryHoldFrom;
                                    salary.HoldTo = salaryHoldTo;
                                    salary.WithSalary = withSalary == "Yes" ? true : false;
                                    salary.PFContinue = pfContinue == "Yes" ? true : false;
                                    salary.GFContinue = gfContinue == "Yes" ? true : false;
                                    list.Add(salary);
                                }
                            }
                            if (list.Count > 0)
                            {
                                var data = await _salaryHoldBusiness.SaveUploadHoldSalaryAsync(list, user);
                                return Ok(data);
                            }
                        }
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryController", "SaveUploadHoldSalary", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }
    }
}
