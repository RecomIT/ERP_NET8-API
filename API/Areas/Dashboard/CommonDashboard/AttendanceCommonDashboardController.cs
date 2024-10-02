using API.Base;
using API.Services;
using BLL.Dashboard.CommonDashboard.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Helpers;
using Shared.Models.Dashboard.CommonDashboard.QueryParam.Attendance;
using Shared.Services;


namespace API.Areas.Dashboard.CommonDashboard
{
    [ApiController, Area("hrms"), Route("api/[area]/dashboard/[controller]"), Authorize]
    public class AttendanceCommonDashboardController : ApiBaseController
    {
        private readonly IAttendanceCommonDashboardBusiness _attendanceCommonDashboardBusiness;

        public AttendanceCommonDashboardController(
            IClientDatabase clientDatabase,
            IAttendanceCommonDashboardBusiness attendanceCommonDashboardBusiness
            ) : base(clientDatabase)
        {
            _attendanceCommonDashboardBusiness = attendanceCommonDashboardBusiness;
        }

        // ------------------------- >>> GetEmployeeAttendanceYears
        [HttpGet("GetEmployeeAttendanceYears")]
        public async Task<IActionResult> GetEmployeeAttendanceYears()
        {
            var user = AppUser();

            if (!user.HasBoth)
                return BadRequest(ResponseMessage.InvalidParameters);

            try
            {
                var data = await _attendanceCommonDashboardBusiness.GetEmployeeAttendanceYearsAsync(user);

                return data != null ? Ok(data) : NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        // ------------------------- >>> GetAttendanceMonthWithDataByYear
        [HttpGet, Route("GetAttendanceMonthWithDataByYear")]
        public async Task<IActionResult> GetAttendanceMonthWithDataByYear([FromQuery] MyAttendanceMonth_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var allData = await _attendanceCommonDashboardBusiness.GetAttendanceMonthWithDataByYearAsync(filter, user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return StatusCode(500, ex.Message);
            }
        }


        // ------------------------- >>> GetMyAttendanceSummary
        [HttpGet, Route("GetMyAttendanceSummary")]
        public async Task<IActionResult> GetMyAttendanceSummary([FromQuery] MyAttendanceSummary_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var allData = await _attendanceCommonDashboardBusiness.GetMyAttendanceSummaryAsync(filter, user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return StatusCode(500, ex.Message);
            }
        }



        // ------------------------- >>> GetMyRecentAttendanceSummary
        [HttpGet("GetMyRecentAttendanceSummary")]
        public async Task<IActionResult> GetMyRecentAttendanceSummary()
        {
            var user = AppUser();

            if (!user.HasBoth)
                return BadRequest(ResponseMessage.InvalidParameters);

            try
            {
                var data = await _attendanceCommonDashboardBusiness.GetMyRecentAttendanceSummaryAsync(user);

                return data != null ? Ok(data) : NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }




        // ------------------------- >>> GetGeoLocationAttendanceData
        [HttpGet("GetGeoLocationAttendanceData")]
        public async Task<IActionResult> GetGeoLocationAttendanceData([FromQuery] GeoLocationAttendance filter)
        {
            var user = AppUser();
            if (!user.HasBoth)
                return BadRequest(ResponseMessage.InvalidParameters);

            try {
                var data = await _attendanceCommonDashboardBusiness.GetGeoLocationAttendanceAsync(filter, user);
                return data != null ? Ok(data) : NotFound();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }









        [HttpGet, Route("GetEmployeeWorkShift")]
        public async Task<IActionResult> GetEmployeeWorkShift()
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var allData = await _attendanceCommonDashboardBusiness.GetEmployeeWorkShiftAsync(user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }





        [HttpGet, Route("CheckPunchInAndPunchOut")]
        public async Task<IActionResult> CheckPunchInAndPunchOut()
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var allData = await _attendanceCommonDashboardBusiness.GetCheckPunchInPunchOutAsync(user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetMyGeoLocationAttendance")]
        public async Task<IActionResult> GetMyGeoLocationAttendance([FromQuery] GeoLocationAttendance filter)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var allData = await _attendanceCommonDashboardBusiness.GetMyGeoLocationAttendanceAsync(filter, user);

                    var data = PagedList<object>.ToPagedList(allData, filter.PageNumber, filter.PageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


    }
}
