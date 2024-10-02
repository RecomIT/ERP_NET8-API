using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.Models.Dashboard.SubordinatesLeave.Filter;
using Shared.Helpers;
using Shared.Models.Dashboard.SubordinatesLeave.Approval.Filter;
using DAL.DapperObject.Interface;
using BLL.Dashboard.SubordiantesLeave.Interface;
using API.Base;

namespace API.Areas.Dashboard.SubordinatesLeave
{
    [ApiController, Area("hrms"), Route("api/[area]/dashboard/[controller]"), Authorize]
    public class SubordinatesLeaveController : ApiBaseController
    {

        private readonly ISubordinatesLeaveBusiness _subordinatesLeave;

        public SubordinatesLeaveController(IClientDatabase clientDatabase, ISubordinatesLeaveBusiness subordinatesLeave) : base(clientDatabase)
        {
            _subordinatesLeave = subordinatesLeave;
        }


        [HttpGet, Route("IsSupervisorOrFinalApproval")]
        public async Task<IActionResult> IsSupervisorOrFinalApproval()
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _subordinatesLeave.IsSupervisorOrFinalApprovalAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }





        [HttpGet, Route("GetSubordinatesEmployees")]
        public async Task<IActionResult> GetSubordinatesEmployees()
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _subordinatesLeave.GetSubordinatesEmployeesAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }





        /// ------------------------
        ///  Leave History
        /// ------------------------

        //[HttpGet, Route("GetSubordinatesLeaveDetails")]
        //public async Task<IActionResult> GetSubordinatesLeaveDetails([FromQuery] SubordinatesLeave_Filter filter)
        //{
        //    var user = AppUser();
        //    try {
        //        if (ModelState.IsValid && user.HasBoth) {
        //            var data = await _subordinatesLeave.GetSubordinatesLeaveAsync(filter, user);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {

        //        return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
        //    }
        //}




        [HttpGet, Route("GetSubordinatesLeaveDetails")]
        public async Task<IActionResult> GetSubordinatesLeaveDetails([FromQuery] SubordinatesLeave_Filter filter)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var allData = await _subordinatesLeave.GetSubordinatesLeaveAsync(filter, user);

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



        /// ------------------------
        ///  Approval
        /// ------------------------

        [HttpGet, Route("GetSubordinatesLeaveApproval")]
        public async Task<IActionResult> GetSubordinatesLeaveApproval([FromQuery] SubordinatesLeaveApproval_Filter filter)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {


                    var allData = await _subordinatesLeave.GetSubordinatesLeaveApprovalAsync(filter, user);

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
