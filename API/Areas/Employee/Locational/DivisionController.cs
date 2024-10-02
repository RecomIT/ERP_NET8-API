using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Base;
using BLL.Employee.Interface.Locational;
using DAL.DapperObject.Interface;
using Shared.Employee.Filter.Locational;
using Shared.Employee.DTO.Locational;

namespace API.Areas.Employee.Locational
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class DivisionController : ApiBaseController
    {
        private readonly IDivisionBusiness _divisionBusiness;
        private readonly ISysLogger _sysLogger;

        public DivisionController(
            IDivisionBusiness divisionBusiness,
            ISysLogger sysLogger,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _divisionBusiness = divisionBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet,Route("GetDivisions")]
        public async Task<IActionResult> GetDivisionsAsync([FromQuery] Division_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _divisionBusiness.GetDivisionsAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DivisionController", "GetDivisionsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveDivision")]
        public async Task<IActionResult> SaveDivisionAsync(DivisionDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var status = await _divisionBusiness.SaveDivisionAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DivisionController", "SaveDivisionAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
