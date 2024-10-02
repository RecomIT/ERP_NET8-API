using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Shared.Employee.DTO.Info;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.Employee.Filter.Info;
using BLL.Employee.Interface.Info;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Employee.Info
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class ExperienceController : ApiBaseController
    {
        private readonly IExperienceBusiness _employeeExperienceBusiness;
        private readonly ISysLogger _sysLogger;
        public ExperienceController(IExperienceBusiness employeeExperienceBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _employeeExperienceBusiness = employeeExperienceBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetEmployeeExperiences")]
        public async Task<IActionResult> GetEmployeesExperiencesAsync([FromQuery]EmployeeExperience_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _employeeExperienceBusiness.GetEmployeeExperiencesAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeExperienceController", "GetEmployeesExperiencesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeExperienceById")]
        public async Task<IActionResult> GetEmployeeExperienceByIdAsync([FromQuery] EmployeeExperience_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = (await _employeeExperienceBusiness.GetEmployeeExperiencesAsync(filter, user)).FirstOrDefault();
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeExperienceController", "GetEmployeeExperienceById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveEmployeeExperience")]
        public async Task<IActionResult> SaveEmployeeExperienceAsync(EmployeeExperienceDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _employeeExperienceBusiness.SaveEmployeeExperienceAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeExperienceController", "GetEmployeeExperienceById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("DeleteEmployeeExperience")]
        public async Task<IActionResult> DeleteEmployeeExperienceAsync(DeleteEmployeeExperienceDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _employeeExperienceBusiness.DeleteEmployeeExperienceAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeExperienceController", "DeleteEmployeeExperience", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
