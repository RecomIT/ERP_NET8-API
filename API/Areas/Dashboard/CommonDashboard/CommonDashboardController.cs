
using System.Web;
using Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared.Models.Dashboard.CommonDashboard.CompanyEvents;
using Shared.Models.Dashboard.CommonDashboard.QueryParam.EmployeeContact;
using DAL.DapperObject.Interface;
using BLL.Dashboard.CommonDashboard.Interface;
using API.Base;

namespace API.Areas.Dashboard.CommonDashboard
{
    [ApiController, Area("hrms"), Route("api/[area]/dashboard/[controller]"), Authorize]
    public class CommonDashboardController : ApiBaseController
    {
        private readonly ICommonDashboardBusiness _commonDashboard;

        public CommonDashboardController(
            IClientDatabase clientDatabase,
            ICommonDashboardBusiness commonDashboard

            ) : base(clientDatabase)
        {
            _commonDashboard = commonDashboard;

        }

        // ------------------------- >>> GetCompanyHolidayAndEvents
        [HttpGet, Route("GetCompanyHolidayAndEvents")]
        public async Task<IActionResult> GetCompanyHolidayAndEvents()
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var allData = await _commonDashboard.GetCompanyHolidayAndEventsAsync(user);


                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message.ToString());

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("SaveCompanyEvent")]
        public async Task<IActionResult> SaveCompanyEvent([FromBody] CompanyEventModel model)
        {
            var user = AppUser();

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                if (user.HasBoth) {
                    var allData = await _commonDashboard.SaveCompanyEventsAsync(model, user);


                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                // Log the exception
                Console.WriteLine(ex.Message.ToString());
                // Return an error response
                return StatusCode(500, "An error occurred while saving the company event");
            }
        }

        // ------------------------- >>> GetEmployeeContact
        [HttpGet, Route("GetBloodGroups")]
        public async Task<IActionResult> GetBloodGroups()
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var allData = await _commonDashboard.GetEmployeeBloodGroupsAsync(user);

                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message.ToString());

                return StatusCode(500, ex.Message);
            }
        }

        // ------------------------- >>> GetEmployeeContact
        [HttpGet, Route("GetEmployeeContact")]
        public async Task<IActionResult> GetEmployeeContact([FromQuery] EmployeeContact_Filter filter)
        {
            var user = AppUser();
            try {
                filter.BloodGroup = filter.BloodGroup != null && filter.BloodGroup !=""?  HttpUtility.UrlDecode(filter.BloodGroup) : filter.BloodGroup;
                if (user.HasBoth) {
                    var allData = await _commonDashboard.GetEmployeeContactAsync(filter, user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message.ToString());
                return StatusCode(500, ex.Message);
            }
        }


    }
}
