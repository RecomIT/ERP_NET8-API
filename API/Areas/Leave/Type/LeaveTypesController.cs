using BLL.Leave.Interface.Type;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Leave.Filter.Type;


namespace API.Areas.Leave.Type
{
    [ApiController, Area("HRMS"), Route("api/[area]/Leave/[controller]"), Authorize]

    public class LeaveTypesController : ControllerBase
    {
        private readonly ILeaveTypeRepo _leaveTypeRepo;

        public LeaveTypesController(ILeaveTypeRepo leaveTypeRepo)
        {
            _leaveTypeRepo = leaveTypeRepo;
        }


        [HttpGet("GetSelect2LeaveTypes")]
        public async Task<IActionResult> GetSelect2LeaveTypes()
        {
            try
            {
                var select2LeaveTypes = await _leaveTypeRepo.GetSelect2LeaveTypesAsync();
                return Ok(select2LeaveTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet("GetSelect2LeaveTypesWithParameter")]
        public async Task<IActionResult> GetSelect2LeaveTypes([FromQuery] LeaveType_Filter filter)
        {
            try
            {
                var select2LeaveTypes = await _leaveTypeRepo.GetSelect2LeaveTypesAsync(filter);
                return Ok(select2LeaveTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }



        [HttpGet("GetLeaveTypes")]
        public async Task<IActionResult> GetLeaveTypes([FromQuery] LeaveType_Filter filter)
        {
            try
            {
                var leaveTypes = await _leaveTypeRepo.GetLeaveTypesAsync(filter);
                return Ok(leaveTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }



        [HttpGet("GetLeaveTypesWithSettings")]
        public async Task<IActionResult> GetLeaveTypesWithSettings([FromQuery] LeaveType_Filter filter)
        {
            try
            {
                var leaveTypes = await _leaveTypeRepo.GetLeaveTypesWithSettingsAsync(filter);
                return Ok(leaveTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }






        [HttpGet("GetSelect2EncashableLeaveTypes")]
        public async Task<IActionResult> GetSelect2EncashableLeaveTypes()
        {
            try
            {
                var select2EncashableLeaveTypes = await _leaveTypeRepo.GetSelect2EncashableLeaveTypesAsync();
                return Ok(select2EncashableLeaveTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }




        [HttpPost("GetEncashableLeaveSettings")]
        public async Task<IActionResult> GetEncashableLeaveSettings([FromBody] LeaveType_Filter filter)
        {
            try
            {
                var select2EncashableLeaveTypes = await _leaveTypeRepo.GetEncashableLeaveSettingsAsync(filter);
                return Ok(select2EncashableLeaveTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }



    }
}
