using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.Employee.DTO.Organizational;
using Microsoft.AspNetCore.Authorization;
using BLL.Employee.Interface.Organizational;
using Shared.Employee.Filter.Organizational;


namespace API.Areas.Employee.Organizational
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class CostCenterController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly ICostCenterBusiness _costCenterBusiness;
        public CostCenterController(ICostCenterBusiness costCenterBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _costCenterBusiness= costCenterBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetCostCenters")]
        public async Task<IActionResult> GetCostCentersAsync([FromQuery] CostCenter_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _costCenterBusiness.GetCostCentersAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CostCenterController", "GetCostCentersAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetCostCenterById")]
        public async Task<IActionResult> GetCostCenterById([FromQuery] CostCenter_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _costCenterBusiness.GetCostCentersAsync(filter, user)).FirstOrDefault();
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CostCenterController", "GetCostCenterById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveCostCenter")]
        public async Task<IActionResult> SaveCostCenterAsync(CostCenterDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _costCenterBusiness.ValidateCostCenterAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var status = await _costCenterBusiness.SaveCostCenterAsync(model, user);
                        return Ok(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CostCenterController", "SaveCostCenterAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetCostCenterDropdown")]
        public async Task<IActionResult> GetCostCenterDropdownAsync()
        {
            var user = AppUser();
            try {
                if (user != null && user.HasBoth) {
                    var data_list = await _costCenterBusiness.GetCostCenterDropdownAsync(user);
                    return Ok(data_list);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CostCenterController", "GetCostCenterDropdownAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
