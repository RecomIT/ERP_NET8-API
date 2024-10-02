using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using API.Base;
using BLL.Employee.Interface.Info;
using DAL.DapperObject.Interface;
using Shared.Employee.DTO.Info;
using Shared.Employee.Filter.Info;

namespace API.Areas.Employee.Info
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]")]
    public class HierarchyController : ApiBaseController
    {
        private readonly IHierarchyBusiness _employeeHierarchyBusiness;
        private readonly ISysLogger _sysLogger;
        public HierarchyController(IHierarchyBusiness employeeHierarchyBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _employeeHierarchyBusiness = employeeHierarchyBusiness;
            _sysLogger = sysLogger;
        }

        [HttpPost, Route("SaveEmployeeHierarchy")]
        public async Task<IActionResult> SaveEmployeeHierarchyAsync(EmployeeHierarchyDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {

                    var data = await _employeeHierarchyBusiness.SaveEmployeeHierarchyAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeHierarchyController", "SaveEmployeeHierarchyAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeHierarchy")]
        public async Task<IActionResult> GetEmployeeHierarchyAsync([FromQuery] EmployeeHierarchy_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _employeeHierarchyBusiness.GetEmployeeHierarchyAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeHierarchyController", "SaveEmployeeHierarchyAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeActiveHierarchy")]
        public async Task<IActionResult> GetEmployeeActiveHierarchy(long id)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && id > 0) {
                    var data = await _employeeHierarchyBusiness.GetEmployeeActiveHierarchyAsync(id, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeHierarchyController", "SaveEmployeeHierarchyAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSubordinates/{id:long}")]
        public async Task<IActionResult> GetSubordinatesAsync(long id)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && id > 0) {
                    var data = await _employeeHierarchyBusiness.GetSubordinatesAsync(id, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeHierarchyController", "GetSubordinatesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
