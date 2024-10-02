using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Helpers;
using Shared.Services;
using DAL.DapperObject.Interface;
using BLL.Separation.Interface.Supervisor;
using API.Base;
using Shared.Separation.Filter.Admin;


namespace API.Areas.Separation.Supervisor
{
    [ApiController, Area("HRMS"), Route("api/[area]/Separation_Module/[controller]"), Authorize]
    public class RRApprovalForSupervisorController : ApiBaseController
    {
        private readonly ISupervisorApprovalBusiness _supervisorApprovalBusiness;


        public RRApprovalForSupervisorController(IClientDatabase clientDatabase, ISupervisorApprovalBusiness supervisorApprovalBusiness) : base(clientDatabase)
        {
            _supervisorApprovalBusiness = supervisorApprovalBusiness;
        }




        [HttpGet, Route("GetUserResignationsForSupervisor")]
        public async Task<IActionResult> GetUserApprovedResignationsBySupervisor([FromQuery] ResignationRequest_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var allData = await _supervisorApprovalBusiness.GetEmployeeResignationListForSupervisorAsync(filter, user);
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
