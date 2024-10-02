using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using System.Threading.Tasks;
using System;
using API.Services;
using Shared.Helpers;
using DAL.DapperObject.Interface;
using BLL.Separation.Interface;
using API.Base;
using Shared.Separation.DTO;
using Shared.Separation.Models.Filter.Settlement_Setup;

namespace API.Areas.Separation.Settlement.Settlement_Setup
{


    [ApiController, Area("HRMS"), Route("api/[area]/Separation_Module/[controller]"), Authorize]
    public class EmployeeSettlementSetup : ApiBaseController
    {
        private readonly IEmployeeSettlementSetupBusiness _employeeSettlementSetupBusiness;
        public EmployeeSettlementSetup(
            IClientDatabase clientDatabase,
            IEmployeeSettlementSetupBusiness employeeSettlementSetupBusiness
            ) : base(clientDatabase)
        {
            _employeeSettlementSetupBusiness = employeeSettlementSetupBusiness;
        }


        // ------------------------- >>> GetPendingSettlementSetupList
        [HttpGet, Route("GetPendingSettlementSetupList")]
        public async Task<IActionResult> GetPendingSettlementSetupList([FromQuery] SettlementSetup_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {

                    var allData = await _employeeSettlementSetupBusiness.GetPendingSettlementSetupListAsync(filter, user);

                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return StatusCode(500, ex.Message);
            }
        }














        // ------------------------- >>> GetEmployeeSettlementSetup
        [HttpGet, Route("GetEmployeeSettlementSetup")]
        public async Task<IActionResult> GetEmployeeSettlementSetup([FromQuery] SettlementSetup_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {

                    var allData = await _employeeSettlementSetupBusiness.GetPendingSettlementSetupListAsync(filter, user);

                    //return Ok(allData);

                    var data = PagedList<object>.ToPagedList(allData, filter.PageNumber, filter.PageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return StatusCode(500, ex.Message);
            }
        }








        // ------------------------- >>> GetSettlementSetup
        [HttpGet, Route("GetSettlementSetup")]
        public async Task<IActionResult> GetSettlementSetup([FromQuery] SettlementSetup_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {

                    var allData = await _employeeSettlementSetupBusiness.GetSettlementSetupListAsync(filter, user);

                    //return Ok(allData);

                    var data = PagedList<object>.ToPagedList(allData, filter.PageNumber, filter.PageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return StatusCode(500, ex.Message);
            }
        }




        [HttpPost, Route("SaveEmployeeSettlementSetup")]
        public async Task<IActionResult> SaveEmployeeSettlementSetupAsync(EmployeeSettlementSetupDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _employeeSettlementSetupBusiness.SaveEmployeeSettlementSetupAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }










        // ------------------------- >>> GetResignationApprovedEmployeeList
        [HttpGet, Route("GetPendingSettlementSetupEmployees")]
        public async Task<IActionResult> GetPendingSettlementSetupEmployees()

        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {

                    var allData = await _employeeSettlementSetupBusiness.GetPendingSettlementSetupEmployeesAsync(user);

                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return StatusCode(500, ex.Message);
            }
        }








        // ------------------------- >>> GetResignationSetupEmployees
        [HttpGet, Route("GetResignationSetupEmployees")]
        public async Task<IActionResult> GetResignationSetupEmployees()

        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {

                    var allData = await _employeeSettlementSetupBusiness.GetResignationSetupEmployeeListAsync(user);

                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return StatusCode(500, ex.Message);
            }
        }



    }
}
