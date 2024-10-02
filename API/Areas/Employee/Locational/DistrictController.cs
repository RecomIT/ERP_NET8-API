
using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.Employee.DTO.Locational;
using Shared.Employee.Filter.Locational;
using BLL.Employee.Interface.Locational;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Employee.Locational
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class DistrictController : ApiBaseController
    {
        private readonly IDistrictBusiness _districtBusiness;
        private readonly ISysLogger _sysLogger;

        public DistrictController(
            IDistrictBusiness districtBusiness,
            ISysLogger sysLogger,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _districtBusiness = districtBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet,Route("GetDistricts")]
        public async Task<IActionResult> GetDistrictsAsync([FromQuery] District_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _districtBusiness.GetDistrictsAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DistrictController", "GetCountiesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("SaveDistrict")]
        public async Task<IActionResult> SaveDistrictAsync(DistrictDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var status = await _districtBusiness.SaveDistrictAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DistrictController", "SaveDistrictAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
