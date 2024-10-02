using API.Base;
using API.Services;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.Employee.Filter.Stage;
using BLL.Employee.Interface.Stage;
using Shared.Employee.ViewModel.Stage;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Employee.Stage
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class ConfirmationController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IEmploymentConfirmationBusiness _employmentConfirmationBusiness;
        public ConfirmationController(ISysLogger sysLogger, IEmploymentConfirmationBusiness employmentConfirmationBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _employmentConfirmationBusiness = employmentConfirmationBusiness;
        }

        [HttpPost("SaveEmploymentConfirmation")]
        public async Task<IActionResult> SaveEmploymentConfirmationAsync(EmploymentConfirmationDTO model)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _employmentConfirmationBusiness.SaveEmploymentConfirmationAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConfirmationController", "SaveEmploymentConfirmationAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetEmploymentConfirmations")]
        public async Task<IActionResult> GetEmploymentConfirmationsAsync([FromQuery] Confimation_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _employmentConfirmationBusiness.GetEmploymentConfirmationsAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConfirmationController", "GetEmploymentConfirmationsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetEmploymentConfirmationsDropdown")]
        public async Task<IActionResult> GetEmploymentConfirmationsDropdownAsync([FromQuery] Confimation_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _employmentConfirmationBusiness.GetEmploymentConfirmationsDropdownAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConfirmationController", "GetEmploymentConfirmationsDropdown", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetEmploymentConfirmationById")]
        public async Task<IActionResult> GetEmploymentConfirmationByIdAsync(long id)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _employmentConfirmationBusiness.GetEmploymentConfirmationsAsync(new Confimation_Filter { ConfirmationProposalId = id.ToString() }, user);
                    if (data.ListOfObject.Count() > 0) {
                        return Ok(data.ListOfObject.FirstOrDefault());
                    }
                    else {
                        return NoContent();
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConfirmationController", "GetEmploymentConfirmationsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("SaveEmploymentConfirmationStatus")]
        public async Task<IActionResult> SaveEmploymentConfirmationStatusAsync(EmploymentConfirmationStatusDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _employmentConfirmationBusiness.SaveEmploymentConfirmationStatusAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConfirmationController", "SaveEmploymentConfirmationStatus", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetUnconfirmedEmployeeInfosInApply")]
        public async Task<IActionResult> GetUnconfirmedEmployeeInfosInApplyAsync()
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _employmentConfirmationBusiness.GetUnconfirmedEmployeeInfosInApplyAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                await _sysLogger.SaveHRMSException(ex, user.Database, "ConfirmationController", "GetUnconfirmedEmployeeInfosInApplyAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetUnconfirmedEmployeeInfosInUpdate")]
        public async Task<IActionResult> GetUnconfirmedEmployeeInfosInUpdateAsync()
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _employmentConfirmationBusiness.GetUnconfirmedEmployeeInfosInUpdateAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                await _sysLogger.SaveHRMSException(ex, user.Database, "ConfirmationController", "GetUnconfirmedEmployeeInfosInUpdateAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
