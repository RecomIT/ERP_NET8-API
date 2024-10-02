using API.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Helpers;
using Shared.Services;
using Microsoft.AspNetCore.Authorization;
using DAL.DapperObject.Interface;
using BLL.Separation.Interface.Admin;
using API.Base;
using Shared.Separation.Filter.Admin;

namespace API.Areas.Separation.Admin
{
    [ApiController, Area("HRMS"), Route("api/[area]/Separation_Module/[controller]"), Authorize]
    public class RRApprovalForAdminController : ApiBaseController
    {
        private readonly IEmployeeResignationsApprovalBusiness _employeeResignationsApproval;


        public RRApprovalForAdminController(IClientDatabase clientDatabase, IEmployeeResignationsApprovalBusiness employeeResignationsApproval) : base(clientDatabase)
        {
            _employeeResignationsApproval = employeeResignationsApproval;
        }




        [HttpGet, Route("GetUserApprovedResignationsBySupervisor")]
        public async Task<IActionResult> GetUserApprovedResignationsBySupervisor([FromQuery] ResignationRequest_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var allData = await _employeeResignationsApproval.GetApprovedResignationRequestsBySupervisorAsync(filter, user);
                    var data = PagedList<object>.ToPagedList(allData, filter.PageNumber, filter.PageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }



    }
}
