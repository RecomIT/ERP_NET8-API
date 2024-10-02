using API.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Microsoft.AspNetCore.Authorization;
using DAL.DapperObject.Interface;
using BLL.Dashboard.Admin.HR.Interface;
using API.Base;

namespace API.Areas.Dashboard.Admin.HR
{
    [ApiController, Area("hrms"), Route("api/[area]/dashboard/[controller]"), Authorize]
    public class HrDashboardController : ApiBaseController
    {
        private readonly IHrDashboardBusiness _hrDashboardBusiness;
        public HrDashboardController(IClientDatabase clientDatabase, IHrDashboardBusiness hrDashboardBusiness) : base(clientDatabase)
        {
            _hrDashboardBusiness = hrDashboardBusiness;
        }


        [HttpGet, Route("GetTotalEmployees")]
        public async Task<IActionResult> GetTotalEmployees()
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var allData = await _hrDashboardBusiness.GetTotalEmployeeAsync(user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


        [HttpGet, Route("GetReligions")]
        public async Task<IActionResult> GetReligions()
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var allData = await _hrDashboardBusiness.GetReligionsAsync(user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }



        [HttpGet, Route("GetAverageEmployeeDetails")]
        public async Task<IActionResult> GetAverageEmployeeDetails()
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var allData = await _hrDashboardBusiness.GetAverageEmployeeDetailsAsync(user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }



        [HttpGet, Route("GetHrDashboardDetails")]
        public async Task<IActionResult> GetHrDashboard()
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var allData = await _hrDashboardBusiness.GetHrDashboardDataAsync(user);
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
