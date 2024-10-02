using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using System.Threading.Tasks;
using System;
using Shared.Helpers;
using DAL.DapperObject.Interface;
using BLL.Separation.Interface.User;
using API.Base;
using Shared.Separation.Filter.User;

namespace API.Areas.Separation.User
{
    [ApiController, Area("HRMS"), Route("api/[area]/Separation_Module/[controller]"), Authorize]
    public class UserResignationRequestController : ApiBaseController
    {
        private readonly IUserResignationBusiness _userResignationBusiness;
        public UserResignationRequestController(IClientDatabase clientDatabase, IUserResignationBusiness userResignationBusiness) : base(clientDatabase)
        {
            _userResignationBusiness = userResignationBusiness;
        }


        [HttpGet, Route("GetUserResignations")]
        public async Task<IActionResult> GetUserResignations([FromQuery] UserResignationRequest_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var allData = await _userResignationBusiness.GetUserResignationRequestsAsync(filter, user);
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
