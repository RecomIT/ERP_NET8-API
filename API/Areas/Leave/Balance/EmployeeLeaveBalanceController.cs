using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DAL.DapperObject.Interface;
using BLL.Leave.Interface.Balance;
using API.Base;
using Shared.Leave.DTO.Balance;
using Shared.Leave.Filter.Report;
using Shared.Leave.Filter.Balance;

namespace API.Areas.Leave.Balance
{
    [ApiController, Area("HRMS"), Route("api/[area]/Leave/[controller]"), Authorize]
    public class EmployeeLeaveBalanceController : ApiBaseController
    {
        private readonly IEmployeeLeaveBalanceBusiness _employeeLeaveBalanceBusiness;
        private readonly ISysLogger _sysLogger;



        public EmployeeLeaveBalanceController(ISysLogger sysLogger, IClientDatabase clientDatabase, IEmployeeLeaveBalanceBusiness employeeLeaveBalanceBusiness) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _employeeLeaveBalanceBusiness = employeeLeaveBalanceBusiness;
        }




        [HttpGet, Route("GetEmployeeLeaveBalances")]
        public async Task<IActionResult> GetEmployeeLeaveBalancesAsync([FromQuery] LeaveBalance_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _employeeLeaveBalanceBusiness.GetEmployeeLeaveBalancesAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveBalanceController", "GetEmployeeLeaveBalancesAsync", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
        }









        [HttpGet, Route("GetEmployeeLeaveBalancesDropdown")]
        public async Task<IActionResult> GetEmployeeLeaveBalancesDropdownAsync(long employeeId)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _employeeLeaveBalanceBusiness.GetEmployeeLeaveBalancesDropdownAsync(employeeId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveBalanceController", "GetEmployeeLeaveBalancesDropdownAsync", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }

        }



        [HttpGet, Route("GetEmployeeLeaveBalancesDropdownInEdit")]
        public async Task<IActionResult> GetEmployeeLeaveBalancesDropdownInEditAsync(long employeeLeaveRequestId, long employeeId)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _employeeLeaveBalanceBusiness.GetEmployeeLeaveBalancesDropdownInEditAsync(employeeLeaveRequestId, employeeId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveBalanceController", "GetEmployeeLeaveBalancesDropdownInEditAsync", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
        }

        [HttpGet, Route("GetLeaveBalance")]
        public async Task<IActionResult> GetLeaveBalanceAsync(long employeeId)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && employeeId > 0)
                {
                    var data = await _employeeLeaveBalanceBusiness.GetLeaveBalanceAsync(employeeId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveBalanceController", "GetLeaveBalanceAsync", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
        }










        [HttpPost("SaveLeaveBalance")]
        public async Task<IActionResult> SaveLeaveBalance([FromBody] LeaveBalanceRequestModel leaveBalanceDto)
        {
            var user = AppUser();
            var result = await _employeeLeaveBalanceBusiness.SaveLeaveBalanceAsync(leaveBalanceDto, user);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }





        [HttpPost("EmployeeLeaveBalances")]
        public async Task<IActionResult> GetEmployeeLeaveBalances2([FromBody] LeaveBalanceFilter filter)
        {
            var user = AppUser();
            try
            {
                var balances = await _employeeLeaveBalanceBusiness.GetEmployeeLeaveBalances(filter, user);

                if (balances == null)
                {
                    return NoContent();
                }

                return Ok(balances);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }




        [HttpGet("GetAllEmployeeLeaveBalances")]
        public async Task<IActionResult> GetAllEmployeeLeaveBalances()
        {
            var user = AppUser();
            try
            {
                var leaveBalances = await _employeeLeaveBalanceBusiness.GetAllEmployeeLeaveBalance(user);
                return Ok(leaveBalances);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



    }
}
