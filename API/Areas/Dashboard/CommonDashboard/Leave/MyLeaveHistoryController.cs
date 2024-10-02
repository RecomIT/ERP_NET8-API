using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.Models.Dashboard.CommonDashboard.Leave.Filter;
using DAL.DapperObject.Interface;
using BLL.Dashboard.User.Leave.Interface;
using API.Base;

namespace API.Areas.Dashboard.CommonDashboard.Leave
{
    [ApiController, Area("hrms"), Route("api/[area]/dashboard/[controller]"), Authorize]
    public class MyLeaveHistoryController : ApiBaseController
    {
        private readonly IMyLeaveHistoryBusiness _myLeaveHistoryBusiness;
        public MyLeaveHistoryController(IClientDatabase clientDatabase, IMyLeaveHistoryBusiness myLeaveHistoryBusiness) : base(clientDatabase)
        {
            _myLeaveHistoryBusiness = myLeaveHistoryBusiness;
        }


        [HttpGet, Route("GetLeavePeriodsYears")]
        public async Task<IActionResult> GetLeavePeriodsYears()
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var allData = await _myLeaveHistoryBusiness.GetLeavePeriodYearAsync(user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


        [HttpGet, Route("GetLeavePeriodMonths")]
        public async Task<IActionResult> GetLeavePeriodMonths([FromQuery] LeavePeriodMonths_Filter filter)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var allData = await _myLeaveHistoryBusiness.GetLeavePeriodMonthAsync(filter,user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


        [HttpGet, Route("GetMyLeaveHistory")]
        public async Task<IActionResult> GetSubordinatesLeaveDetails([FromQuery] MyLeaveHistory_Filter filter)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var allData = await _myLeaveHistoryBusiness.GetMyLeaveHistoryAsync(filter, user);    
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
