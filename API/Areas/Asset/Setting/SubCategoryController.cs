using API.Services;
using BLL.Asset.Interface.Setting;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.Asset.Filter.Setting;
using Shared.Asset.DTO.Setting;
using API.Base;
using DAL.DapperObject.Interface;


namespace API.Asset.Setting
{
    [ApiController, Area("Asset"), Route("api/[area]/Setting/[controller]"), Authorize]
    public class SubCategoryController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly ISubCategoryBusiness _subCategoryBusiness;

        public SubCategoryController(
           ISysLogger sysLogger,
           ISubCategoryBusiness subCategoryBusiness,
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _subCategoryBusiness = subCategoryBusiness;            
        }

        [HttpGet, Route("GetSubCategory")]
        public async Task<IActionResult> GetSubCategoryAsync([FromQuery] SubCategory_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _subCategoryBusiness.GetSubCategoryAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubCategoryController", "GetSubCategoryAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSubCategoryById")]
        public async Task<IActionResult> GetSubCategoryByIdAsync([FromQuery] SubCategory_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _subCategoryBusiness.GetSubCategoryAsync(filter, user)).FirstOrDefault();
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubCategoryController", "GetSubCategoryByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveSubCategory")]
        public async Task<IActionResult> SaveSubCategoryAsync(SubCategory_DTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _subCategoryBusiness.ValidatorSubCategoryAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var data = await _subCategoryBusiness.SaveSubCategoryAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubCategoryController", "SaveCategoryAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSubCategoryDropdown")]
        public async Task<IActionResult> GetSubCategoryDropdownAsync([FromQuery] SubCategory_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _subCategoryBusiness.GetSubCategoryDropdownAsync(filter, user));
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubCategoryController", "GetSubCategoryDropdown", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        

    }
}
