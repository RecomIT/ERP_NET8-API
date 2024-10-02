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
using System.Threading.Tasks;

namespace API.Areas.Attendance.Holiday
{
    [ApiController, Area("HRMS"), Route("api/[area]/Attendance/[controller]"), Authorize]

    public class PublicHolidayController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IPublicHolidayBusiness _publicHolidayBusiness;
        public PublicHolidayController(ISysLogger sysLogger, IPublicHolidayBusiness publicHolidayBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _publicHolidayBusiness = publicHolidayBusiness;
        }

        [HttpGet, Route("GetPublicHolidays")]
        public async Task<IActionResult> GetPublicHolidaysAsync(long publicHolidayId)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _publicHolidayBusiness.GetPublicHolidaysAsync(publicHolidayId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PublicHolidayController", "GetPublicHolidaysAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SavePublicHoliday")]
        public async Task<IActionResult> SavePublicHolidayAsync(PublicHolidayDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var validator = await _publicHolidayBusiness.PublicHolidayValidatorAsync(model, user);
                    if (validator != null && validator.Status == true)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var status = await _publicHolidayBusiness.SavePublicHolidayAsync(model, user);
                        return Ok(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PublicHolidayController", "GetPublicHolidaysAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
