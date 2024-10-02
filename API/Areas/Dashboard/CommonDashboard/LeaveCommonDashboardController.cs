using API.Base;
using API.Services;
using BLL.Dashboard.CommonDashboard.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Helpers;
using Shared.Models.Dashboard.CommonDashboard.QueryParam.Leave;
using Shared.Services;


namespace API.Areas.Dashboard.CommonDashboard
{
    [ApiController, Area("hrms"), Route("api/[area]/dashboard/[controller]"), Authorize]
    public class LeaveCommonDashboardController : ApiBaseController
    {
        private readonly ILeaveCommonDashboardBusiness _leaveCommonDashboardBusiness;



        public LeaveCommonDashboardController(
           IClientDatabase clientDatabase,
          ILeaveCommonDashboardBusiness leaveCommonDashboardBusiness

           ) : base(clientDatabase)
        {
            _leaveCommonDashboardBusiness = leaveCommonDashboardBusiness;

        }


        // ------------------------- >>> GetMyleaveSummery
        [HttpGet, Route("GetMyleaveSummary")]
        public async Task<IActionResult> GetMyleaveSummary([FromQuery] MyLeaveSummary_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var allData = await _leaveCommonDashboardBusiness.GetMyLeaveSummaryAsync(filter, user);

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




        // ------------------------- >>> GetMyLeaveTypeSummery
        [HttpGet, Route("GetMyLeaveTypeSummery")]
        public async Task<IActionResult> GetMyLeaveTypeSummery()
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var allData = await _leaveCommonDashboardBusiness.GetMyLeaveTypeSummaryAsync(user);

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






        [HttpGet, Route("GetMyLeaveAppliedRecords")]
        public async Task<IActionResult> GetMyLeaveAppliedRecords([FromQuery] MyLeaveAppliedRecords filter)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {

                    var allData = await _leaveCommonDashboardBusiness.GetMyLeaveAppliedRecordsAsync(filter, user);

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
