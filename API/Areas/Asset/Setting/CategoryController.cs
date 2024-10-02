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
    public class CategoryController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly ICategoryBusiness _categoryBusiness;

        public CategoryController(
           ISysLogger sysLogger,
           ICategoryBusiness categoryBusiness,
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _categoryBusiness = categoryBusiness;            
        }

        [HttpGet, Route("GetCategory")]
        public async Task<IActionResult> GetCategoryAsync([FromQuery] Category_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _categoryBusiness.GetCategoryAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CategoryController", "GetCategoryAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetCategoryById")]
        public async Task<IActionResult> GetCategoryByIdAsync([FromQuery] Category_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _categoryBusiness.GetCategoryAsync(filter, user)).FirstOrDefault();
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CategoryController", "GetCategoryByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveCategory")]
        public async Task<IActionResult> SaveCategoryAsync(Category_DTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _categoryBusiness.ValidatorCategoryAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var data = await _categoryBusiness.SaveCategoryAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CategoryController", "SaveCategoryAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


        [HttpGet, Route("GetCategoryDropdown")]
        public async Task<IActionResult> GetCategoryDropdownIdAsync([FromQuery] Category_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _categoryBusiness.GetCategoryDropdownAsync(filter, user));
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CategoryController", "GetCategoryDropdownIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


    }
}
