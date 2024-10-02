using API.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.Asset.Filter.Setting;
using Shared.Asset.DTO.Setting;
using API.Base;
using DAL.DapperObject.Interface;
using BLL.Asset.Interface.Setting;

namespace API.Asset.Setting
{
    [ApiController, Area("Asset"), Route("api/[area]/Setting/[controller]"), Authorize]
    public class BrandController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IBrandBusiness _brandBusiness;

        public BrandController(
           ISysLogger sysLogger,
           IBrandBusiness brandBusiness,
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _brandBusiness = brandBusiness;            
        }

        [HttpGet, Route("GetBrand")]
        public async Task<IActionResult> GetBrandAsync([FromQuery] Brand_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _brandBusiness.GetBrandAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BrandController", "GetBrandAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetBrandById")]
        public async Task<IActionResult> GetBrandIdAsync([FromQuery] Brand_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _brandBusiness.GetBrandAsync(filter, user)).FirstOrDefault();
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BrandController", "GetBrandIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveBrand")]
        public async Task<IActionResult> SaveBrandAsync(Brand_DTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _brandBusiness.ValidatorBrandAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var data = await _brandBusiness.SaveBrandAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BrandController", "SaveBrandAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetBrandDropdown")]
        public async Task<IActionResult> GetBrandDropdownAsync([FromQuery] Brand_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _brandBusiness.GetBrandDropdownAsync(filter, user));
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BrandController", "GetBrandDropdownIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

       

    }
}
