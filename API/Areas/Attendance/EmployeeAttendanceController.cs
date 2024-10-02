using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BLL.Base.Interface;
using System;
using API.Services;
using Shared.Helpers;
using Shared.Services;
using System.Threading.Tasks;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.Attendance;
using API.Base;
using Shared.Attendance.ViewModel.Attendance;
using Shared.Attendance.Filter.Attendance;

namespace API.Areas.Attendance
{
    [ApiController, Area("HRMS"), Route("api/[area]/Attendance/[controller]"), Authorize]
    public class EmployeeAttendanceController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IEmployeeAttendanceDataBusiness _employeeAttendanceDataBusiness;
        public EmployeeAttendanceController(ISysLogger sysLogger, IEmployeeAttendanceDataBusiness employeeAttendanceDataBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _employeeAttendanceDataBusiness = employeeAttendanceDataBusiness;
        }

        [HttpGet, Route("GetEmployeesAttendanceSummery")]
        public async Task<IActionResult> GetEmployeesAttendanceSummeryAsync([FromQuery] AttendanceSummary_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    filter.PageNumber = Utility.PageNumber(filter.PageNumber); filter.PageSize = Utility.PageSize(filter.PageSize);
                    var allData = await _employeeAttendanceDataBusiness.GetEmployeesAttendanceSummeryAsync(filter, user);
                    var data = PagedList<AttendanceSummeryViewModel>.ToPagedList(allData, filter.PageNumber, filter.PageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeAttendanceController", "GetEmployeesAttendanceSummeryAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeesDailyAttendance")]
        public async Task<IActionResult> GetEmployeesDailyAttendanceAsync([FromQuery] DailyAttendance_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var allData = await _employeeAttendanceDataBusiness.GetEmployeesDailyAttendanceAsync(filter, user);
                    var data = PagedList<EmployeeDailyAttendanceViewModel>.ToPagedList(allData, filter.PageNumber, filter.PageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeAttendanceController", "GetEmployeesDailyAttendanceAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
