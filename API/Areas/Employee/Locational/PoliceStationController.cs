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
    public class PoliceStationController : ApiBaseController
    {
        private readonly IPoliceStationBusiness _policeStationBusiness;
        private readonly ISysLogger _sysLogger;

        public PoliceStationController(
            IPoliceStationBusiness policeStationBusiness,
            ISysLogger sysLogger,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _policeStationBusiness = policeStationBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetPoliceStations")]
        public async Task<IActionResult> GetPoliceStationsAsync([FromQuery] PoliceStation_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _policeStationBusiness.GetPoliceStationsAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PoliceStationController", "GetPoliceStationsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SavePoliceStation")]
        public async Task<IActionResult> SavePoliceStationAsync(PoliceStationDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var status = await _policeStationBusiness.SavePoliceStationAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PoliceStationController", "SavePoliceStationAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
