using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using BLL.Employee.Interface.Organizational;
using Shared.Employee.Filter.Organizational;

namespace API.Areas.Employee.Organizational
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]

    public class EmployeeTypeController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IEmployeeTypeBusiness _employeeTypeBusiness; 
        public EmployeeTypeController(IEmployeeTypeBusiness employeeTypeBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _employeeTypeBusiness = employeeTypeBusiness;
        }

        [HttpGet,Route("GetEmployeeTypeDropdown")]
        public async Task<IActionResult> GetEmployeeTypeDropdownAsync()
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _employeeTypeBusiness.GetEmployeeTypeDropdownAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeTypeController", "GetEmployeeTypeDropdown", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeTypes")]
        public async Task<IActionResult> GetEmployeeTypesAsync([FromQuery]EmployeeType_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _employeeTypeBusiness.GetEmployeeTypesAsync(filter,user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeTypeController", "GetEmployeeTypesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
