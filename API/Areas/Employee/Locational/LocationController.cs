
using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.Employee.DTO.Locational;
using BLL.Employee.Interface.Locational;
using Shared.Employee.Filter.Locational;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Employee.Locational
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class LocationController : ApiBaseController
    {
        private readonly ILocationBusiness _locationBusiness;
        private readonly ISysLogger _sysLogger;

        public LocationController(
            ILocationBusiness locationBusiness,
            ISysLogger sysLogger,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _locationBusiness = locationBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetLocations")]
        public async Task<IActionResult> GetLocationsAsync([FromQuery] Location_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _locationBusiness.GetLocationsAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LocationController", "GetLocationsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveLocation")]
        public async Task<IActionResult> SaveLocationAsync(LocationDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var status = await _locationBusiness.SaveLocationAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LocationController", "SaveLocationAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
