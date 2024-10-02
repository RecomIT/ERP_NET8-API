
using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Employee.DTO.Info;
using DAL.DapperObject.Interface;
using Shared.Employee.Filter.Info;
using Shared.OtherModels.Response;
using BLL.Employee.Interface.Education;
using DAL.Repository.Employee.Interface;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Employee.Info
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class EducationController : ApiBaseController
    {
        private readonly IEducationBusiness _employeeEducationBusiness;
        private readonly IEmployeeEducationRepository _employeeEducationRepository;
        private readonly ISysLogger _sysLogger;
        public EducationController(IEducationBusiness employeeEducationBusiness, IEmployeeEducationRepository employeeEducationRepository, ISysLogger sysLogger,
            IClientDatabase clientDatabase): base(clientDatabase)
        {
            _employeeEducationBusiness = employeeEducationBusiness;
            _employeeEducationRepository = employeeEducationRepository;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetEmployeeEducations")]
        public async Task<IActionResult> GetEmployeeEducationsAsync([FromQuery] Education_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _employeeEducationBusiness.GetEmployeeEducationsAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EducationController", "GetEmployeeEducationsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeEducationById")]
        public async Task<IActionResult> GetEmployeeExperienceByIdAsync([FromQuery] Education_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = (await _employeeEducationBusiness.GetEmployeeEducationsAsync(filter, user)).FirstOrDefault();
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeExperienceController", "GetEmployeeExperienceByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveEmployeeEducation")]
        public async Task<IActionResult> SaveEmployeeExperienceAsync(EmployeeEducationDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var isExist = await _employeeEducationRepository.GetEmployeeEducationByEmployeeDegreeId(model.EmployeeId, model.DegreeId, user);
                    if (isExist != null && isExist.EmployeeEducationId > 0) {
                        if(isExist.EmployeeEducationId != model.EmployeeEducationId) {
                            return Ok(new ExecutionStatus() {
                                Status = false,
                                Msg = "You are trying to entry duplicate academic degree. Which is already exist."
                            });
                        }
                    }
                    var data = await _employeeEducationBusiness.SaveEmployeeEducationAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeExperienceController", "SaveEmployeeExperienceAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("DeleteEmployeeEducation")]
        public async Task<IActionResult> DeleteEmployeeEducationAsync(DeleteEmployeeEducationDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _employeeEducationBusiness.DeleteEmployeeEducationAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeExperienceController", "DeleteEmployeeEducation", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
