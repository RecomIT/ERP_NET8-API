using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using DAL.DapperObject.Interface;
using BLL.Separation.Interface;
using API.Base;
using Shared.Separation.Filter.Category;

namespace API.Areas.Separation.Resignation_Category
{
    [ApiController, Area("HRMS"), Route("api/[area]/Separation_Module/[controller]"), Authorize]
    public class ResignationCategoryController : ApiBaseController
    {

        private readonly IResignationCategoryBusiness _resignationCategory;
        public ResignationCategoryController(IClientDatabase clientDatabase,
            IResignationCategoryBusiness resignationCategory) : base(clientDatabase)
        {
            _resignationCategory = resignationCategory;
        }



        [HttpGet, Route("GetResignationCategory")]
        public async Task<IActionResult> GetResignationCategory()
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _resignationCategory.GetResignationCategoryAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }




        [HttpGet, Route("GetResignationSubCategory")]
        public async Task<IActionResult> GetResignationSubCategory([FromQuery] ResignationCategoryFilter filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _resignationCategory.GetResignationSubCategoryAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }






        [HttpGet, Route("GetResignationNoticePeriod")]
        public async Task<IActionResult> GetResignationNoticePeriod([FromQuery] ResignationNoticePeriod filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _resignationCategory.GetResignationNoticePeriodAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


    }
}
