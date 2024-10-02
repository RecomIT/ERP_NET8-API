using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.Models.Dashboard.SubordinatesLeave.Filter;
using Shared.Helpers;
using Shared.Models.Dashboard.SubordinatesLeave.Approval.Filter;
using DAL.DapperObject.Interface;
using BLL.Dashboard.Admin.Interface;
using API.Base;

namespace API.Areas.Dashboard.Admin
{
    [ApiController, Area("hrms"), Route("api/[area]/dashboard/[controller]"), Authorize]
    public class EmployeeLeaveDetailsController : ApiBaseController
    {

        private readonly IEmployeeLeaveBusiness _employeeLeaveBusiness;
        public EmployeeLeaveDetailsController(IClientDatabase clientDatabase, IEmployeeLeaveBusiness employeeLeaveBusiness) : base(clientDatabase)
        {
            _employeeLeaveBusiness = employeeLeaveBusiness;
        }



        [HttpGet, Route("GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _employeeLeaveBusiness.GetEmployeesAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }





        [HttpGet, Route("GetEmplloyeesLeaveDetails")]
        public async Task<IActionResult> GetEmplloyeesLeaveDetails([FromQuery] SubordinatesLeave_Filter filter)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var allData = await _employeeLeaveBusiness.GetEmployeesLeaveAsync(filter, user);

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





        //[HttpGet, Route("")]
        //public async Task<IActionResult> GetSubordinatesLeaveDetails([FromQuery] SubordinatesLeave_Filter filter)
        //{
        //    var user = AppUser();
        //    try {
        //        if (ModelState.IsValid && user.HasBoth) {
        //            var data = await _employeeLeaveBusiness.GetEmployeesLeaveAsync(filter, user);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {

        //        return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
        //    }
        //}



        /// ------------------------
        ///  Approval
        /// ------------------------

        [HttpGet, Route("GetEmployeesLeaveApproval")]
        public async Task<IActionResult> GetEmployeesLeaveApproval([FromQuery] SubordinatesLeaveApproval_Filter filter)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    //var data = await _subordinatesLeave.GetSubordinatesLeaveApprovalAsync(filter, user);
                    // return Ok(data);

                    var allData = await _employeeLeaveBusiness.GetEmployeesLeaveApprovalAsync(filter, user);

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
