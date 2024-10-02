using API.Base;
using API.Services;
using BLL.Attendance.Interface.WorkShift;
using BLL.Base.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Attendance.DTO.Workshift;
using Shared.Attendance.Filter.Shift;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Areas.Attendance.WorkShift
{
    [ApiController, Area("HRMS"), Route("api/[area]/Attendance/[controller]"), Authorize]

    public class EmployeeShiftController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IEmployeeWorkShiftBusiness _employeeWorkShiftBusiness;
        public EmployeeShiftController(ISysLogger sysLogger, IEmployeeWorkShiftBusiness employeeWorkShiftBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _employeeWorkShiftBusiness = employeeWorkShiftBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetEmployeeWorkShifts")]
        public async Task<IActionResult> GetEmployeeWorkShiftsAsync([FromQuery] EmployeeShift_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _employeeWorkShiftBusiness.GetEmployeeWorkShiftsAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeShiftController", "GetEmployeeWorkShiftsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveEmployeesWorkShift")]
        public async Task<IActionResult> SaveEmployeesWorkShiftAsync(List<EmployeeWorkShiftDTO> model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var status = await _employeeWorkShiftBusiness.SaveEmployeesWorkShiftAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeShiftController", "SaveSaveEmployeesWorkShiftAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveEmployeesWorkShiftChecking")]
        public async Task<IActionResult> SaveEmployeesWorkShiftCheckingAsync(List<EmployeeWorkShiftStatusDTO> model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var status = await _employeeWorkShiftBusiness.SaveEmployeesWorkShiftCheckingAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeShiftController", "SaveEmployeesWorkShiftCheckingAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
