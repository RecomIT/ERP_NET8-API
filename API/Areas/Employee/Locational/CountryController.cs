using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BLL.Employee.Interface.Locational;
using DAL.DapperObject.Interface;
using Shared.Employee.Filter.Locational;
using Shared.Employee.DTO.Locational;


namespace API.Areas.Employee.Locational
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class CountryController : ApiBaseController
    {
        private readonly ICountryBusiness _countryBusiness;
        private readonly ISysLogger _sysLogger;

        public CountryController(
            ICountryBusiness countryBusiness,
            ISysLogger sysLogger,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _countryBusiness = countryBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet,Route("GetCounties")]
        public async Task<IActionResult> GetCountiesAsync([FromQuery] Country_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _countryBusiness.GetCountriesAsync(filter,user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CountryController", "GetCountiesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        [HttpPost, Route("SaveCountry")]
        public async Task<IActionResult> SaveCountryAsync(CountryDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var status = await _countryBusiness.SaveCountryAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CountryController", "SaveCountryAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
