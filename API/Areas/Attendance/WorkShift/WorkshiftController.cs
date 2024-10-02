using System;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.WorkShift;
using API.Base;
using Shared.Attendance.DTO.Workshift;
using Shared.Attendance.Filter.Shift;

namespace API.Areas.Attendance.WorkShift
{
    [ApiController, Area("HRMS"), Route("api/[area]/Attendance/[controller]"), Authorize]
    public class WorkshiftController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IWorkShiftBusiness _workShiftBusiness;
        public WorkshiftController(ISysLogger sysLogger,
            IClientDatabase clientDatabase, IWorkShiftBusiness workShiftBusiness) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _workShiftBusiness = workShiftBusiness;
        }

        [HttpGet("GetWorkShiftDropdown")]
        public async Task<IActionResult> GetWorkShiftDropdownAsync()
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _workShiftBusiness.GetWorkShiftDropdownAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "WorkshiftController", "GetWorkShiftDropdownAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetWorkShiftsAsync")]
        public async Task<IActionResult> GetWorkShiftsAsync([FromQuery] WorkShift_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _workShiftBusiness.GetWorkShiftsAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "WorkshiftController", "GetWorkShiftsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveWorkShift")]
        public async Task<IActionResult> SaveWorkShiftAsync(WorkShiftDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var status = await _workShiftBusiness.SaveWorkShiftAsync(model, model.Weekends, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "WorkshiftController", "SaveWorkShiftAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveWorkShiftChecking")]
        public async Task<IActionResult> SaveWorkShiftCheckingAsync(WorkShiftCheckingDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _workShiftBusiness.SaveWorkShiftChecking(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "WorkshiftController", "SaveWorkShiftChecking", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
