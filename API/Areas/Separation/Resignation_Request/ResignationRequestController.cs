using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using System.Threading.Tasks;
using System;
using DAL.DapperObject.Interface;
using BLL.Separation.Interface;
using API.Base;
using Shared.Separation.Filter.Resignation_Request;
using Shared.Separation.DTO.Resignation_Request;
using Shared.Separation.Filter.Category.Supervisor;


namespace API.Areas.Separation.Resignation_Request
{
    [ApiController, Area("HRMS"), Route("api/[area]/Separation_Module/[controller]"), Authorize]
    public class ResignationRequestController : ApiBaseController
    {

        private readonly IEmployeeResignationBusiness _resignationRequest;
        public ResignationRequestController(IClientDatabase clientDatabase, IEmployeeResignationBusiness resignationRequest) : base(clientDatabase)
        {
            _resignationRequest = resignationRequest;
        }




        [HttpGet, Route("GetEmployeeResignationList")]
        public async Task<IActionResult> GetEmployeeResignationList([FromQuery] ResignationRequest_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _resignationRequest.GetEmployeeResignationListAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }






        [HttpGet, Route("GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _resignationRequest.GetEmployeesAsync(user);
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
                    var data = await _resignationRequest.GetEmployeeDetailsAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }




        [HttpGet, Route("GetEmployeesDetails")]
        public async Task<IActionResult> GetEmployeesDetails([FromQuery] Supervisor_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _resignationRequest.GetEmployeesDetailsAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }





        [HttpPost, Route("SubmitResignationRequest")]
        public async Task<IActionResult> SubmitResignationRequest([FromForm] ResignationRequestDTO filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _resignationRequest.SaveEmployeeResignationAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }





        // --------------------------------------------------- User Cancel

        [HttpPost, Route("CancelResignationRequest")]
        public async Task<IActionResult> CancelResignationRequest([FromForm] ResignationRequestDTO filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _resignationRequest.CancelEmployeeResignationAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }








        [HttpGet]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile([FromQuery] DownloadResignationLetter_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var filePath = await _resignationRequest.DownloadResignationLetterAsync(filter, user);

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                        return File(fileBytes, "application/octet-stream", filter.FileName);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }






        // ----------------------------------------- User

        [HttpGet, Route("GetUserResignationList")]
        public async Task<IActionResult> GetUserResignationList()
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _resignationRequest.GetUserResignationListAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }



        // -------------------------------------- Supervisor


        [HttpGet, Route("GetEmployeeResignationListForSupervisor")]
        public async Task<IActionResult> GetEmployeeResignationListForSupervisor([FromQuery] ResignationRequest_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _resignationRequest.GetEmployeeResignationListForSupervisorAsync(filter, user);
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
