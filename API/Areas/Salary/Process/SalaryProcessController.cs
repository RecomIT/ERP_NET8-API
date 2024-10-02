using API.Base;
using API.Services;
using Shared.Helpers;
using Shared.Services;
using BLL.Tax.Interface;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Payroll.DTO.Salary;
using Shared.Payroll.Process.Tax;
using DAL.DapperObject.Interface;
using BLL.Salary.Salary.Interface;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.Process.Salary;
using Shared.Payroll.ViewModel.Salary;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Salary.Process
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class SalaryProcessController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly ISalaryProcessBusiness _salaryProcessBusiness;
        private readonly IExecuteSalaryProcess _executeSalaryProcess;
        private readonly IExecuteTaxProcess _executeTaxProcess;
        public SalaryProcessController(ISysLogger sysLogger, ISalaryProcessBusiness salaryProcessBusiness, IExecuteSalaryProcess executeSalaryProcess, IExecuteTaxProcess executeTaxProcess, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _salaryProcessBusiness = salaryProcessBusiness;
            _executeSalaryProcess = executeSalaryProcess;
            _executeTaxProcess = executeTaxProcess;
        }

        [HttpPost, Route("SalaryProcess")]
        public async Task<IActionResult> SalaryProcessAsync(SalaryProcessExecution model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _salaryProcessBusiness.SalaryProcessAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryController", "SalaryProcess", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("ExecuteSalaryProcess")]
        public async Task<IActionResult> ExecuteSalaryProcessAsync(SalaryProcessExecution model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _executeSalaryProcess.ExecuteProcess(model, user);
                    if ((model.WithTaxProcess ?? false) == true && data.Status == true)
                    {
                        TaxProcessExecution tax = new TaxProcessExecution()
                        {
                            ExecutionOn = "Selected Employees",
                            SelectedEmployees = data.Ids,
                            Month = model.Month,
                            Year = model.Year,
                            EffectOnSalary = true
                        };
                        var taxProcess = await _executeTaxProcess.SalaryTaxProcessAsync(tax, user);
                        if (taxProcess.Status == false)
                        {
                            return BadRequest("Something went wrong in Tax Process");
                        }
                    }
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryController", "SalaryProcess", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SalaryReprocess")]
        public async Task<IActionResult> SalaryReprocessAsync(SalaryReprocess model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _executeSalaryProcess.ReProcess(model, user);
                    if (data.Status && model.WithTaxProcess == true && !string.IsNullOrEmpty(data.Ids) && !string.IsNullOrWhiteSpace(data.Ids))
                    {
                        TaxProcessExecution tax = new TaxProcessExecution()
                        {
                            ExecutionOn = "Selected Employees",
                            SelectedEmployees = data.Ids,
                            Month = model.Month,
                            Year = model.Year,
                            EffectOnSalary = true
                        };
                        var taxProcess = await _executeTaxProcess.SalaryTaxProcessAsync(tax, user);
                        if (taxProcess.Status == false)
                        {
                            return BadRequest("Something went wrong in Tax Process");
                        }
                    }
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryController", "SalaryProcess", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSalaryProcessInfos")]
        public async Task<IActionResult> GetSalaryProcessInfosAsync(long? salaryProcessId, long? fiscalYearId, short? month, short? year, DateTime? salaryDate, string batchNo)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _salaryProcessBusiness.GetSalaryProcessInfosAsync(salaryProcessId ?? 0, fiscalYearId ?? 0, month ?? 0, year ?? 0, salaryDate, 0, batchNo, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryController", "GetSalaryProcessInfos", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        //[HttpGet, Route("GetSalaryProcessDetails")]
        //public async Task<IActionResult> GetSalaryProcessDetailsAsync(long? salaryProcessId, long? salaryProcessDetailId, short? month, short? year, long? employeeId, long? branchId, string batchNo)
        //{
        //    var user = AppUser();
        //    try {
        //        if (user.HasBoth) {
        //            var data = await _salaryProcessBusiness.GetSalaryProcessDetailsAsync(salaryProcessId ?? 0, salaryProcessDetailId ?? 0, employeeId ?? 0, 0, month ?? 0, year ?? 0, branchId ?? 0, batchNo, user);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.SomthingWentWrong);
        //    }
        //    catch (Exception ex) {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryController", "GetSalaryProcessDetails", user);
        //        return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
        //    }
        //}

        [HttpGet, Route("GetSalaryProcessDetails")]
        public async Task<IActionResult> GetSalaryProcessDetailsAsync([FromQuery] SalaryProcessDetail_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _salaryProcessBusiness.GetSalaryProcessDetailsAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryController", "GetSalaryProcessDetails", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeSalaryAllowanceById")]
        public async Task<IActionResult> GetEmployeeSalaryAllowanceByIdAsync(long? employeeId, long? salaryProcessDetailId)
        {
            var user = AppUser();
            try
            {
                if (employeeId > 0 && user.HasBoth && salaryProcessDetailId > 0)
                {
                    var data = await _salaryProcessBusiness.GetEmplyeeSalaryAllowancesAsync(employeeId, 0, 0, salaryProcessDetailId, 0, 0, 0, null, user);
                    return Ok(data);
                }
                return BadRequest(new { meassage = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryController", "GetEmployeeSalaryAllowanceById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeSalaryDeductionById")]
        public async Task<IActionResult> GetEmployeeSalaryDeductionByIdAsync(long? employeeId, long? salaryProcessDetailId)
        {
            var user = AppUser();
            try
            {
                if (employeeId > 0 && user.HasBoth && salaryProcessDetailId > 0)
                {
                    var data = await _salaryProcessBusiness.GetEmplyeeSalaryDeductionsAsync(employeeId, 0, 0, salaryProcessDetailId, 0, 0, null, user);
                    return Ok(data);
                }
                return BadRequest(new { meassage = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryController", "GetEmployeeSalaryDeductionById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SalaryProcessDisbursedOrUndo")]
        public async Task<IActionResult> SalaryProcessDisbursedOrUndoAsync(SalaryProcessDisbursedOrUndoDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    if (model.ActionName == "Disbursed")
                    {
                        var data = await _salaryProcessBusiness.DisbursedSalaryProcessAsync(model.SalaryProcessId, user);
                        return Ok(data);
                    }
                    if (model.ActionName == "Undo Process")
                    {
                        var data = await _salaryProcessBusiness.UndoSalaryProcessAsync(model.SalaryProcessId, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryController", "SalaryProcessDisbursedOrUndo", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSalaryProcessBatchNoDropdown")]
        public async Task<IActionResult> GetSalaryProcessBatchNoDropdownAsync(string isDisbursed)
        {
            var user = AppUser();
            try
            {
                var list = await _salaryProcessBusiness.GetSalaryProcessBatchNoDropdownAsync(isDisbursed, user);
                return Ok(list);
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryController", "GetSalaryProcessBatchNoDropdown", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
