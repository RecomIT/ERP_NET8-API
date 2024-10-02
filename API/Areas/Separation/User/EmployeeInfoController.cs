using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using System.Threading.Tasks;
using System;
using DAL.DapperObject.Interface;
using BLL.Separation.Interface.User;
using API.Base;
using Shared.Separation.Filter.Category.Supervisor;

namespace API.Areas.Separation.User
{
    [ApiController, Area("HRMS"), Route("api/[area]/Separation_Module/[controller]"), Authorize]
    public class EmployeeInfoController : ApiBaseController
    {

        private readonly IEmployeeInfoBusiness _employeeInfoBusiness;
        public EmployeeInfoController(IClientDatabase clientDatabase, IEmployeeInfoBusiness employeeInfoBusiness) : base(clientDatabase)
        {
            _employeeInfoBusiness = employeeInfoBusiness;
        }


        [HttpGet, Route("GetEmployeeInfo")]
        public async Task<IActionResult> GetEmployeeInfo()
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _employeeInfoBusiness.GetEmployeesInfoAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }





        [HttpGet, Route("GetEmployeeDetails")]
        public async Task<IActionResult> GetEmployeeDetails([FromQuery] Supervisor_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _employeeInfoBusiness.GetEmployeeDetailsAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }



        [HttpGet, Route("GetAllEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _employeeInfoBusiness.GetEmployeesAsync(user);
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
