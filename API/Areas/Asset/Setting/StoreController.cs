using API.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.Asset.Filter.Setting;
using BLL.Asset.Interface.Setting;
using Shared.Asset.DTO.Setting;
using API.Base;
using DAL.DapperObject.Interface;


namespace API.Asset_Module.Setting
{
    [ApiController, Area("Asset"), Route("api/[area]/Setting/[controller]"), Authorize]
    public class StoreController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IStoreBusiness _storeBusiness;

        public StoreController(
           ISysLogger sysLogger,
           IStoreBusiness storeBusiness,
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _storeBusiness = storeBusiness;            
        }

        [HttpGet, Route("GetStore")]
        public async Task<IActionResult> GetStoreAsync([FromQuery] Store_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _storeBusiness.GetStoreAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "StoreController", "GetStoreAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetStoreById")]
        public async Task<IActionResult> GetStoreByIdAsync([FromQuery] Store_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _storeBusiness.GetStoreAsync(filter, user)).FirstOrDefault();
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "StoreController", "GetStoreByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveStore")]
        public async Task<IActionResult> SaveStoreAsync(Store_DTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _storeBusiness.ValidatorStoreAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var data = await _storeBusiness.SaveStoreAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "StoreController", "SaveStoreAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


        [HttpGet, Route("GetStoreDropdown")]
        public async Task<IActionResult> GetStoreDropdownAsync([FromQuery] Store_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _storeBusiness.GetStoreDropdownAsync(filter, user));
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "StoreController", "GetStoreDropdownAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


    }
}
