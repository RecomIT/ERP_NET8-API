using API.Base;
using AutoMapper;
using API.Services;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.Payroll.DTO.Variable;
using BLL.Salary.Variable.Interface;
using Shared.Payroll.Domain.Variable;
using Shared.Payroll.ViewModel.Salary;
using DAL.Payroll.Repository.Interface;
using Shared.Payroll.ViewModel.Variable;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Salary.Variable
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class VariableAllowanceController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly ISysLogger _sysLogger;
        private readonly IMonthlyVariableAllowanceBusiness _monthlyVariableAllowanceBusiness;
        private readonly IMonthlyVariableAllowanceRepository _monthlyVariableAllowanceRepository;
        private readonly IPeriodicallyVariableAllowanceBusiness _periodicallyVariableAllowanceBusiness;
        public VariableAllowanceController(ISysLogger sysLogger, IMonthlyVariableAllowanceBusiness monthlyVariableAllowanceBusiness,
            IPeriodicallyVariableAllowanceBusiness periodicallyVariableAllowanceBusiness, IMapper mapper, IMonthlyVariableAllowanceRepository monthlyVariableAllowanceRepository, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _mapper = mapper;
            _sysLogger = sysLogger;
            _monthlyVariableAllowanceBusiness = monthlyVariableAllowanceBusiness;
            _monthlyVariableAllowanceRepository = monthlyVariableAllowanceRepository;
            _periodicallyVariableAllowanceBusiness = periodicallyVariableAllowanceBusiness;
        }

        #region Monthly Variable Allowance
        [HttpPost, Route("SaveMonthlyVariableAllowances")]
        public async Task<IActionResult> SaveMonthlyVariableAllowancesAsync(List<MonthlyVariableAllowanceViewModel> allowances)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var validator = await _monthlyVariableAllowanceBusiness.MonthlyVariableAllowanceValidatorAsync(allowances, user);
                    if (validator != null && !validator.Status)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _monthlyVariableAllowanceBusiness.SaveMonthlyVariableAllowancesAsync(allowances, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableAllowanceController", "SaveMonthlyVariableAllowancesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetMonthlyVariableAllowances")]
        public async Task<IActionResult> GetMonthlyVariableAllowancesAsync(long? mvAllowanceId, long? employeeId, long? allowanceNameId, short salaryMonth, short salaryYear, string stateStatus, int pageNumber = 1, int pageSize = 15)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    pageNumber = Utility.PageNumber(pageNumber);
                    pageSize = Utility.PageSize(pageSize);
                    var allData = await _monthlyVariableAllowanceBusiness.GetMonthlyVariableAllowancesAsync(mvAllowanceId ?? 0, employeeId ?? 0, allowanceNameId ?? 0, salaryMonth, salaryYear, stateStatus, user);
                    var data = PagedList<MonthlyVariableAllowanceViewModel>.ToPagedList(allData, pageNumber, pageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableAllowanceController", "GetMonthlyVariableAllowancesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UpdateMonthlyVariableAllowance")]
        public async Task<IActionResult> UpdateMonthlyVariableAllowanceAsync(MonthlyVariableAllowanceViewModel model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && model.MonthlyVariableAllowanceId > 0 && user.HasBoth)
                {
                    var validator = await _monthlyVariableAllowanceBusiness.MonthlyVariableAllowanceValidatorAsync(new List<MonthlyVariableAllowanceViewModel>() { model }, user);
                    if (validator != null)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _monthlyVariableAllowanceBusiness.UpdateMonthlyVariableAllowanceAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableAllowanceController", "UpdateMonthlyVariableAllowanceAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("SaveMonthlyVariableAllowanceStatus")]
        public async Task<IActionResult> SaveMonthlyVariableAllowanceStatusAsync(MonthlyVariableAllowanceStatusDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && Utility.StatusChecking(model.StateStatus, new string[] { "Approved", "Cancelled", "Recheck" }))
                {
                    var data = await _monthlyVariableAllowanceBusiness.SaveMonthlyVariableAllowanceStatusAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableAllowanceController", "SaveMonthlyVariableAllowanceStatus", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UploadMonthlyVariableAllowances")]
        public async Task<IActionResult> UploadMonthlyVariableAllowancesAsync([FromForm] SalaryAllowanceUpload model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    List<MonthlyVariableAllowanceViewModel> list = new List<MonthlyVariableAllowanceViewModel>();
                    if (model.UploadedFile?.Length > 0)
                    {
                        var stream = model.UploadedFile.OpenReadStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;

                            var days = DateTime.DaysInMonth(model.SalaryYear, model.SalaryMonth);
                            var salaryDate = new DateTime(model.SalaryYear, model.SalaryMonth, 1);

                            for (var row = 2; row <= rowCount; row++)
                            {
                                var employeeCode = worksheet.Cells[row, 1].Value?.ToString();
                                var amount = worksheet.Cells[row, 2].Value?.ToString();

                                if (!Utility.IsNullEmptyOrWhiteSpace(employeeCode) && !Utility.IsNullEmptyOrWhiteSpace(amount))
                                {
                                    MonthlyVariableAllowanceViewModel item = new MonthlyVariableAllowanceViewModel();
                                    item.AllowanceNameId = model.AllowanceNameId;
                                    item.EmployeeCode = employeeCode;
                                    item.SalaryMonth = model.SalaryMonth;
                                    item.SalaryYear = model.SalaryYear;
                                    item.SalaryMonthYear = salaryDate;
                                    item.Amount = Convert.ToDecimal(amount);
                                    list.Add(item);
                                }
                            }
                        }
                    }
                    var validator = await _monthlyVariableAllowanceBusiness.MonthlyVariableAllowanceValidatorAsync(list, user);
                    if (validator != null)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _monthlyVariableAllowanceBusiness.UploadMonthlyVariableAllowancesAsync(list, user);
                        return Ok(data);
                    }
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableAllowanceController", "UploadMonthlyVariableAllowancesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("DeletePendingMonthlyVariableAllowance/{id:long}")]
        public async Task<IActionResult> DeletePendingMonthlyVariableAllowanceAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (id > 0)
                {
                    var itemInDb = await _monthlyVariableAllowanceRepository.GetByIdAsync(id, user);
                    if (itemInDb == null)
                    {
                        return NotFound(new { message = "Item is not found." });
                    }
                    else
                    {
                        if (itemInDb.StateStatus == StateStatus.Pending)
                        {
                            var rowCount = await _monthlyVariableAllowanceRepository.DeleteByIdAsync(id, user);
                            if (rowCount > 0)
                            {
                                return Ok(new ExecutionStatus()
                                {
                                    Status = true,
                                    Msg = ResponseMessage.Successfull
                                });
                            }
                            else
                            {
                                return BadRequest(new { message = "Data has been failed to delete" });
                            }
                        }
                        else
                        {
                            return NotFound(new { message = "Item is not found in pending status." });
                        }
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableAllowanceController", "DeletePendingMonthlyVariableAllowanceAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetById/{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (id > 0)
                {
                    var data = await _monthlyVariableAllowanceBusiness.GetByIdAsync(id, user);
                    if (data != null)
                    {
                        return Ok(data);
                    }
                    return NoContent();

                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableAllowanceController", "GetByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UpdateApprovedAllowance")]
        public async Task<IActionResult> UpdateApprovedAllowanceAysnc(MonthlyVariableAllowanceViewModel model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var status = await _monthlyVariableAllowanceBusiness.UpdateApprovedAllowanceAysnc(model, user);
                    if (status.Status)
                    {
                        return Ok(status);
                    }
                    else
                    {
                        return BadRequest(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableAllowanceController", "UpdateApprovedAllowanceAysnc", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        #endregion

        #region Periodically Variable Allowance
        [HttpPost, Route("SavePeriodicallyVariableAllowanceInfo")]
        public async Task<IActionResult> SavePeriodicallyVariableAllowanceInfoAsync(PeriodicallyVariableAllowanceInfoViewModel info)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && info.CompanyId > 0 && info.OrganizationId > 0)
                {
                    var data = await _periodicallyVariableAllowanceBusiness.SavePeriodicallyVariableAllowanceAsync(_mapper.Map<PeriodicallyVariableAllowanceInfo>(info), info.PeriodicalDetails, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableAllowanceController", "SavePeriodicallyVariableAllowanceInfo", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetPeriodicallyVariableAllowanceInfos")]
        public async Task<IActionResult> GetPeriodicallyVariableAllowanceInfosAsync(long? id, string salaryVariableFor, string amountBaseOn, long? allowanceNameId, int pageNumber = 1, int pageSize = 15)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    pageNumber = Utility.PageNumber(pageNumber);
                    pageSize = Utility.PageSize(pageSize);
                    var allData = await _periodicallyVariableAllowanceBusiness.GetPeriodicallyVariableAllowanceInfosAsync(id ?? 0, salaryVariableFor ?? "", amountBaseOn ?? "", allowanceNameId ?? 0, user);
                    var data = PagedList<PeriodicallyVariableAllowanceInfoViewModel>.ToPagedList(allData, pageNumber, pageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);

                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableAllowanceController", "GetPeriodicallyVariableAllowanceInfosAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetPeriodicallyVariableAllowanceInfo")]
        public async Task<IActionResult> GetPeriodicallyVariableAllowanceInfoAsync(long? id, long? allowanceNameId)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _periodicallyVariableAllowanceBusiness.GetPeriodicallyVariableAllowanceInfoAsync(id ?? 0, allowanceNameId ?? 0, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableAllowanceController", "GetPeriodicallyVariableAllowanceInfoAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("SavePeriodicallyVariableAllowanceStatus")]
        public async Task<IActionResult> SavePeriodicallyVariableAllowanceStatusAsync(long periodicallyVariableAllowanceInfoId, string status, string remarks)
        {
            var user = AppUser();
            try
            {
                if (periodicallyVariableAllowanceInfoId > 0 && Utility.StatusChecking(status, new string[] { "Approved", "Cancelled", "Recheck" })
                    && user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _periodicallyVariableAllowanceBusiness.SavePeriodicallyVariableAllowanceStatusAsync(periodicallyVariableAllowanceInfoId, status, remarks, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableAllowanceController", "SavePeriodicallyVariableAllowanceStatusAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        #endregion
    }
}
