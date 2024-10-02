using API.Services;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.Attendance;
using API.Base;
using Shared.Attendance.ViewModel.Attendance;
using Shared.Attendance.DTO.Attendance;
using Shared.Attendance.Filter.Attendance;

namespace API.Areas.Attendance
{
    [ApiController, Area("HRMS"), Route("api/[area]/Attendance/[controller]"), Authorize]
    public class ManualAttendanceController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IManualAttendanceBusiness _manualAttendanceBusiness;
        public ManualAttendanceController(
            IManualAttendanceBusiness manualAttendanceBusiness,
            ISysLogger sysLogger,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _manualAttendanceBusiness = manualAttendanceBusiness;
        }

        [HttpGet, Route("GetEmployeeManualAttendances")]
        public async Task<IActionResult> GetEmployeeManualAttendancesAsync([FromQuery] EmployeeManualAttendance_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    filter.PageNumber = Utility.PageNumber(filter.PageNumber); filter.PageSize = Utility.PageSize(filter.PageSize);
                    var allData = await _manualAttendanceBusiness.GetEmployeeManualAttendancesAsync(filter, user);
                    var data = PagedList<EmployeeManualAttendanceViewModel>.ToPagedList(allData, filter.PageNumber, filter.PageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidExecution);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ManualAttendanceController", "GetEmployeeManualAttendancesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveManualAttendance")]
        public async Task<IActionResult> SaveManualAttendanceAsync(EmployeeManualAttendanceDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var status = await _manualAttendanceBusiness.SaveManualAttendanceAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ManualAttendanceController", "SaveManualAttendanceAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveManualAttendanceStatus")]
        public async Task<IActionResult> SaveManualAttendanceStatusAsync(ManualAttendanceStatusDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var status = await _manualAttendanceBusiness.SaveManualAttendanceStatusAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ManualAttendanceController", "SaveManualAttendanceStatusAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("DeleteManualAttendance")]
        public async Task<IActionResult> DeleteManualAttendanceAsync(DeleteManualAttendanceDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var status = await _manualAttendanceBusiness.DeleteManualAttendanceAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ManualAttendanceController", "DeleteManualAttendanceAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSubordinatesManualAttendancesRequests")]
        public async Task<IActionResult> GetSubordinatesManualAttendancesRequestsAsync([FromQuery] SubordinatesManualAttendances_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    filter.PageNumber = Utility.PageNumber(filter.PageNumber); filter.PageSize = Utility.PageSize(filter.PageSize);
                    var allData = await _manualAttendanceBusiness.GetSubordinatesManualAttendancesRequestsAsync(filter, user);
                    var data = PagedList<EmployeeManualAttendanceViewModel>.ToPagedList(allData, filter.PageNumber, filter.PageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ManualAttendanceController", "GetSubordinatesManualAttendancesRequestsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveManualAttendancePermission")]
        public async Task<IActionResult> SaveManualAttendancePermission(ManualAttendanceStatusDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var status = await _manualAttendanceBusiness.SaveManualAttendanceStatusAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ManualAttendanceController", "SaveManualAttendancePermission", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
