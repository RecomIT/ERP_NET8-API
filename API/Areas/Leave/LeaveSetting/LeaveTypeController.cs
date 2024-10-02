using System;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using DAL.DapperObject.Interface;
using BLL.Leave.Interface.LeaveSetting;
using API.Base;
using Shared.Leave.DTO.Setup;

namespace API.Areas.Leave.LeaveSetting
{
    [ApiController, Area("HRMS"), Route("api/[area]/Leave/[controller]"), Authorize]
    public class LeaveTypeController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly ILeaveTypeBusiness _leaveTypeBusiness;
        public LeaveTypeController(ILeaveTypeBusiness leaveTypeBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _leaveTypeBusiness = leaveTypeBusiness;
        }

        [HttpPost, Route("SaveLeaveType")]
        public async Task<IActionResult> SaveLeaveTypeAsync(LeaveTypeDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var validator = await _leaveTypeBusiness.LeaveTypeValidatorAsync(model, user);
                    if (validator != null && !validator.Status)
                    {
                        return Ok(validator);
                    }
                    var data = await _leaveTypeBusiness.SaveLeaveTypeAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveTypeController", "SaveLeaveTypeAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetLeaveTypes")]
        public async Task<IActionResult> GetLeaveTypesAsync([FromQuery] LeaveType_Filter model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _leaveTypeBusiness.GetLeaveTypesAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "GetLeaveTypes", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetLeaveTypeById")]
        public async Task<IActionResult> GetLeaveTypeByIdAsync([FromQuery] LeaveType_Filter model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = (await _leaveTypeBusiness.GetLeaveTypesAsync(model, user)).FirstOrDefault();
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "GetLeaveTypes", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetLeaveTypesDropdown")]
        public async Task<IActionResult> GetLeaveTypesDropdownAsync()
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _leaveTypeBusiness.GetLeaveTypesDropdownAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "GetLeaveTypes", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

    }
}
