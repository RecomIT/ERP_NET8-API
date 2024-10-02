
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Base;
using BLL.Employee.Interface.Termination;
using DAL.DapperObject.Interface;
using Shared.Employee.Filter.Termination;
using Shared.Employee.DTO.Termination;

namespace API.Areas.HRMS.Employee_Module.Termination
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class DiscontinuedEmployeeController : ApiBaseController
    {
        private readonly IDiscontinuedEmployeeBusiness _discontinuedEmployeeBusiness;
        private readonly ISysLogger _sysLogger;
        public DiscontinuedEmployeeController(
            ISysLogger sysLogger, 
            IDiscontinuedEmployeeBusiness discontinuedEmployeeBusiness, 
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _discontinuedEmployeeBusiness = discontinuedEmployeeBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetDiscontinuedEmployees")]
        public async Task<IActionResult> GetDiscontinuedEmployeesAsync([FromQuery] Termination_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _discontinuedEmployeeBusiness.GetDiscontinuedEmployeesAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepartmentController", "GetDepartmentsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetDiscontinuedEmployeeById")]
        public async Task<IActionResult> GetDiscontinuedEmployeeByIdAsync([FromQuery] Termination_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _discontinuedEmployeeBusiness.GetDiscontinuedEmployeesAsync(filter, user)).FirstOrDefault();
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepartmentController", "GetDepartmentById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveDiscontinuedEmployee")]
        public async Task<IActionResult> SaveDiscontinuedEmployeeAsync(DiscontinuedEmployeeDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _discontinuedEmployeeBusiness.ValidateDiscontinuedEmployeeAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var status = await _discontinuedEmployeeBusiness.SaveDiscontinuedEmployeeAsync(model, user);
                        return Ok(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepartmentController", "SaveDepartmentAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("ApprovalDiscontinuedEmployee")]
        public async Task<IActionResult> ApprovalDiscontinuedEmployee(DiscontinuedEmployeeApprovalDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var status = await _discontinuedEmployeeBusiness.ApprovalDiscontinuedEmployeeAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepartmentController", "SaveDepartmentAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("DeleteDiscontinuedEmployee")]
        public async Task<IActionResult> DeleteDiscontinuedEmployeeAsync(Termination_Filter filter)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    if(Utility.TryParseLong(filter.EmployeeId) > 0 && Utility.TryParseLong(filter.DiscontinuedId) > 0) {
                        var status = await _discontinuedEmployeeBusiness.DeleteDiscontinuedEmployeeAsync(filter, user);
                        return Ok(status);
                    }
                    return BadRequest("Employee Id/Discontinued Id is missing");
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepartmentController", "SaveDepartmentAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

    }
}
