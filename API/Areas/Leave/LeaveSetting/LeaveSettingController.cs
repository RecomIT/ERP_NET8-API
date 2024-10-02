using System;
using System.Linq;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DAL.DapperObject.Interface;
using BLL.Leave.Interface.LeaveSetting;
using API.Base;
using Shared.Leave.DTO.Setup;
using Shared.Leave.Filter.Request;
using Shared.Leave.Filter.Setup;

namespace API.Areas.Leave.LeaveSetting
{
    [ApiController, Area("HRMS"), Route("api/[area]/Leave/[controller]"), Authorize]
    public class LeaveSettingController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly ILeaveSettingBusiness _leaveSettingBusiness;
        public LeaveSettingController(ILeaveSettingBusiness leaveSettingBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _leaveSettingBusiness = leaveSettingBusiness;
        }



        [HttpPost, Route("SaveLeaveSetting")]
        public async Task<IActionResult> SaveLeaveSettingAsync(LeaveSettingDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var validator = await _leaveSettingBusiness.LeaveSettingValidatorAsync(model, user);
                    if (validator != null && !validator.Status)
                    {
                        return Ok(validator);
                    }
                    var data = await _leaveSettingBusiness.SaveLeaveSettingAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingController", "SaveLeaveTypeAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetLeaveSettings")]
        public async Task<IActionResult> GetLeaveSettingsAsync([FromQuery] LeaveSetting_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _leaveSettingBusiness.GetLeaveSettingsAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingController", "GetLeaveSettings", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }



        [HttpGet, Route("GetLeaveSettingById")]
        public async Task<IActionResult> GetLeaveSettingByIdAsync([FromQuery] LeaveSetting_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = (await _leaveSettingBusiness.GetLeaveSettingsAsync(filter, user)).FirstOrDefault();
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingController", "GetLeaveSettings", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }




        [HttpGet, Route("GetLeavePeriod")]
        public async Task<IActionResult> GetLeavePeriodAsync(long employeeId)
        {
            var user = AppUser();
            try
            {
                if (employeeId > 0 && user.HasBoth)
                {
                    var data = (await _leaveSettingBusiness.GetLeavePeriodAsync(employeeId, user)).FirstOrDefault();
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingController", "GetLeavePeriodAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetLeaveTypeSetting")]
        public async Task<IActionResult> GetLeaveTypeSettingAsync(long leaveTypeId, long employeeId)
        {
            var user = AppUser();
            try
            {
                if (employeeId > 0 && user.HasBoth)
                {
                    var data = (await _leaveSettingBusiness.GetLeaveTypeSettingAsync(leaveTypeId, employeeId, user)).FirstOrDefault();
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingController", "GetLeavePeriodAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetTotalRequestDays")]
        public async Task<IActionResult> GetTotalRequestDaysAsync([FromQuery] TotalRequestDays_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _leaveSettingBusiness.GetTotalRequestDaysAsync(filter, user);
                    return Ok(new { leaveCount = data.ItemCount, list = data.Json });
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingController", "GetTotalRequestDays", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


    }
}
