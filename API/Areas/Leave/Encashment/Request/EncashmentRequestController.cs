using BLL.Leave.Interface.Encashment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Leave.Filter.Encashment;


namespace API.Areas.Leave.Encashment.Request
{
    [ApiController, Area("HRMS"), Route("api/[area]/Leave/[controller]"), Authorize]

    public class EncashmentRequestController : ControllerBase
    {
        private readonly ILeaveEncashmentBusiness _leaveEncashmentBusiness;
        public EncashmentRequestController(ILeaveEncashmentBusiness leaveEncashmentBusiness)
        {
            _leaveEncashmentBusiness = leaveEncashmentBusiness;
        }


        [HttpGet, Route("GetLeaveEncashmentBalance")]
        public async Task<IActionResult> LeaveEncashableBalance([FromQuery] LeaveEncashable_Filter filter)
        {
            try
            {
                var totalLeave = await _leaveEncashmentBusiness.GetTotalLeaveBalanceAsync(filter);
                return Ok(totalLeave);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
