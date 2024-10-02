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



namespace API.Asset.Setting
{
    [ApiController, Area("Asset"), Route("api/[area]/Setting/[controller]"), Authorize]
    public class VendorController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IVendorBusiness _vendorBusiness;

        public VendorController(
           ISysLogger sysLogger,
           IVendorBusiness vendorBusiness,
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _vendorBusiness = vendorBusiness;            
        }

        [HttpGet, Route("GetVendor")]
        public async Task<IActionResult> GetVendorAsync([FromQuery] Vendor_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _vendorBusiness.GetVendorAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VendorController", "GetVendorAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetVendorById")]
        public async Task<IActionResult> GetVendorByIdAsync([FromQuery] Vendor_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _vendorBusiness.GetVendorAsync(filter, user)).FirstOrDefault();
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VendorController", "GetVendorByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveVendor")]
        public async Task<IActionResult> SaveVendorAsync(Vendor_DTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _vendorBusiness.ValidatorVendorAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var data = await _vendorBusiness.SaveVendorAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VendorController", "SaveVendorAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetVendorDropdown")]
        public async Task<IActionResult> GetVendorDropdownAsync([FromQuery] Vendor_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _vendorBusiness.GetVendorDropdownAsync(filter, user));
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VendorController", "GetVendorDropdownAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }



    }
}
