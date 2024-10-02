using API.Base;
using API.Services;
using BLL.Attendance.Interface.Holiday;
using BLL.Base.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Attendance.DTO.Holiday;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Areas.Attendance.Holiday
{
    [ApiController, Area("HRMS"), Route("api/[area]/Attendance/[controller]"), Authorize]
    public class YearlyHolidayController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IYearlyHolidayBusiness _yearlyHolidayBusiness;
        public YearlyHolidayController(ISysLogger sysLogger, IYearlyHolidayBusiness yearlyHolidayBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _yearlyHolidayBusiness = yearlyHolidayBusiness;
        }

        [HttpPost, Route("SaveYearlyHoliday")]
        public async Task<IActionResult> SaveYearlyHolidayAsync(YearlyHolidayDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _yearlyHolidayBusiness.SaveYearlyHolidayAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "YearlyHolidayController", "SaveYearlyHolidayAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetYearlyHolidays")]
        public async Task<IActionResult> GetYearlyHolidaysAsync(long yearlyHolidayId)
        {
            var user = AppUser();
            try
            {
                if (yearlyHolidayId >= 0 && user.HasBoth)
                {
                    var data = await _yearlyHolidayBusiness.GetYearlyHolidaysAsync(yearlyHolidayId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "YearlyHolidayController", "GetYearlyHolidays", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetAssignYearlyHoliday")]
        public async Task<IActionResult> AssignYearlyHolidayAsync()
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _yearlyHolidayBusiness.AssignYearlyHolidayAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "YearlyHolidayController", "GetAssignYearlyHoliday", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveYearlyPublicHoliday")]
        public async Task<IActionResult> SaveYearlyPublicHolidayAsync(List<YearlyHolidayDTO> models)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _yearlyHolidayBusiness.SaveYearlyPublicHolidayAsync(models, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "YearlyHolidayController", "GetAssignYearlyHoliday", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
