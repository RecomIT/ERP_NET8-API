
using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Employee.DTO.Info;
using DAL.DapperObject.Interface;
using BLL.Employee.Interface.Info;
using Shared.Employee.Filter.Info;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Employee.Info
{

    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class SkillController : ApiBaseController
    {
        private readonly ISkillBusiness _employeeSkilBusiness;
        private readonly ISysLogger _sysLogger;
        public SkillController(ISkillBusiness employeeSkilBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _employeeSkilBusiness = employeeSkilBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetEmployeeSkills")]
        public async Task<IActionResult> GetEmployeesSkilsAsync([FromQuery] Skill_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _employeeSkilBusiness.GetEmployeeSkillsAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeSkilController", "GetEmployeesSkilsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeSkillById")]
        public async Task<IActionResult> GetEmployeeSkilByIdAsync([FromQuery] Skill_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = (await _employeeSkilBusiness.GetEmployeeSkillsAsync(filter, user)).FirstOrDefault();
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeSkilController", "GetEmployeeSkilById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveEmployeeSkill")]
        public async Task<IActionResult> SaveEmployeeSkilAsync(EmployeeSkillDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _employeeSkilBusiness.SaveEmployeeSkillAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeSkillController", "SaveEmployeeSkilAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("DeleteEmployeeSkill")]
        public async Task<IActionResult> DeleteEmployeeSkilAsync(DeleteEmployeeSkillDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _employeeSkilBusiness.DeleteEmployeeSkillAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeSkillController", "DeleteEmployeeSkilAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
