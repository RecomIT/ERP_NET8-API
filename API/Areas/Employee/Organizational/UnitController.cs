using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using BLL.Employee.Interface.Organizational;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.DTO.Organizational;

namespace API.Areas.Employee.Organizational
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class UnitController : ApiBaseController
    {
        private readonly IUnitBusiness _sectionBusiness;
        private readonly ISysLogger _sysLogger;
        public UnitController(ISysLogger sysLogger, IUnitBusiness sectionBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sectionBusiness = sectionBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetUnits")]
        public async Task<IActionResult> GetUnitsAsync([FromQuery] Unit_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _sectionBusiness.GetUnitsAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "UnitController", "GetUnitsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveUnit")]
        public async Task<IActionResult> SaveUnitAsync(UnitDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _sectionBusiness.ValidateUnitAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var status = await _sectionBusiness.SaveUnitAsync(model, user);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "UnitController", "SaveUnitAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
