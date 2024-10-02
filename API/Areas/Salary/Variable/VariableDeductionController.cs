using API.Base;
using AutoMapper;
using API.Services;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.Payroll.DTO.Variable;
using Shared.OtherModels.Response;
using BLL.Salary.Variable.Interface;
using Shared.Payroll.ViewModel.Salary;
using DAL.Payroll.Repository.Interface;
using Shared.Payroll.ViewModel.Variable;
using Microsoft.AspNetCore.Authorization;


namespace API.Areas.Salary.Variable
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class VariableDeductionController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly ISysLogger _sysLogger;
        private readonly IMonthlyVariableDeductionBusiness _monthlyVariableDeductionBusiness;
        private readonly IPeriodicallyVariableDeductionBusiness _periodicallyVariableDeductionBusiness;
        private readonly IMonthlyVariableDeductionRepository _monthlyVariableDeductionRepository;
        public VariableDeductionController(ISysLogger sysLogger,
            IMonthlyVariableDeductionBusiness monthlyVariableDeductionBusiness,
            IPeriodicallyVariableDeductionBusiness periodicallyVariableDeductionBusiness,
            IMonthlyVariableDeductionRepository monthlyVariableDeductionRepository,
            IMapper mapper,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _mapper = mapper;
            _sysLogger = sysLogger;
            _monthlyVariableDeductionBusiness = monthlyVariableDeductionBusiness;
            _periodicallyVariableDeductionBusiness = periodicallyVariableDeductionBusiness;
            _monthlyVariableDeductionRepository = monthlyVariableDeductionRepository;
        }

        #region Monthly Variable Allowance

        [HttpPost, Route("SaveMonthlyVariableDeductions")]
        public async Task<IActionResult> SaveMonthlyVariableDeductionsAsync(List<MonthlyVariableDeductionViewModel> deductions)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var validator = await _monthlyVariableDeductionBusiness.MonthlyVariableDeductionValidatorAsync(deductions, user);
                    if (validator != null)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _monthlyVariableDeductionBusiness.SaveMonthlyVariableDeductionsAsync(deductions, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableDeductionController", "SaveMonthlyVariableDeductionsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetMonthlyVariableDeductions")]
        public async Task<IActionResult> GetMonthlyVariableDeductionsAsync(long? mvDeductionId, long? employeeId, long? deductionNameId, short salaryMonth, short salaryYear, string stateStatus, int pageNumber = 1, int pageSize = 15)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    pageNumber = Utility.PageNumber(pageNumber);
                    pageSize = Utility.PageSize(pageSize);
                    var allData = await _monthlyVariableDeductionBusiness.GetMonthlyVariableDeductionsAsync(mvDeductionId ?? 0, employeeId ?? 0, deductionNameId ?? 0, salaryMonth, salaryYear, stateStatus, user);
                    var data = PagedList<MonthlyVariableDeductionViewModel>.ToPagedList(allData, pageNumber, pageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableDeductionController", "GetMonthlyVariableDeductionsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPut, Route("UpdateMonthlyVariableDeduction")]
        public async Task<IActionResult> UpdateMonthlyVariableDeductionAsync(MonthlyVariableDeductionViewModel model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var validator = await _monthlyVariableDeductionBusiness.MonthlyVariableDeductionValidatorAsync(new List<MonthlyVariableDeductionViewModel>() { model }, user);
                    if (validator != null)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _monthlyVariableDeductionBusiness.UpdateMonthlyVariableDeductionAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableDeductionController", "UpdateMonthlyVariableDeduction", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("SaveMonthlyVariableDeductionStatus")]
        public async Task<IActionResult> SaveMonthlyVariableDeductionStatusAsync(MonthlyVariableDeductionStatusDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && Utility.StatusChecking(model.StateStatus, new string[] { "Approved", "Cancelled", "Recheck" })
                    && user.HasBoth)
                {
                    var data = await _monthlyVariableDeductionBusiness.SaveMonthlyVariableDeductionStatusAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableDeductionController", "SaveMonthlyVariableDeductionStatus", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UploadMonthlyVariableDeductions")]
        public async Task<IActionResult> UploadMonthlyVariableDeductionsAsync([FromForm] SalaryDeductionUpload model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    List<MonthlyVariableDeductionViewModel> list = new List<MonthlyVariableDeductionViewModel>();
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
                                    MonthlyVariableDeductionViewModel item = new MonthlyVariableDeductionViewModel();
                                    item.DeductionNameId = model.DeductionNameId;
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

                    var validator = await _monthlyVariableDeductionBusiness.MonthlyVariableDeductionValidatorAsync(list, user);
                    if (validator != null)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _monthlyVariableDeductionBusiness.UploadMonthlyVariableDeductionsAsync(list, user);
                        return Ok(data);
                    }
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableDeductionController", "UploadMonthlyVariableDeductions", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("DeletePendingMonthlyVariableDeduction/{id:long}")]
        public async Task<IActionResult> DeletePendingMonthlyVariableAllowanceAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (id > 0)
                {
                    var itemInDb = await _monthlyVariableDeductionRepository.GetByIdAsync(id, user);
                    if (itemInDb == null)
                    {
                        return NotFound(new { message = "Item is not found." });
                    }
                    else
                    {
                        if (itemInDb.StateStatus == StateStatus.Pending)
                        {
                            var rowCount = await _monthlyVariableDeductionRepository.DeleteByIdAsync(id, user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "VariableAllowanceController", "DeletePendingMonthlyVariableDeduction", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        #endregion


    }
}
