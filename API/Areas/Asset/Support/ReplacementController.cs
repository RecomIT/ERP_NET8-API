using API.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.Helpers;
using Shared.Asset.Filter.Support;
using Shared.Asset.DTO.Support;
using BLL.Asset.Interface.Support;
using BLL.Asset.Interface.Assigning;
using Shared.Asset.Filter.Resignation;
using BLL.Asset.Interface.Resignation;
using API.Base;
using DAL.DapperObject.Interface;


namespace API.Asset_Module.Support
{
    [ApiController, Area("Asset"), Route("api/[area]/Support/[controller]"), Authorize]
    public class ReplacementController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IReplacementBusiness _replacementBusiness;
        private readonly IAssigningBusiness _assigningBusiness;
        private readonly IResignationBusiness _resignationBusiness;

        public ReplacementController(
           ISysLogger sysLogger,
           IReplacementBusiness replacementBusiness,
           IAssigningBusiness assigningBusiness,
            IResignationBusiness resignationBusiness,
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _replacementBusiness = replacementBusiness;
            _assigningBusiness = assigningBusiness;
            _resignationBusiness = resignationBusiness;
        }  

        [HttpGet, Route("GetReplacementData")]
        public async Task<IActionResult> GetAssetDataAsync([FromQuery] Replacement_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _replacementBusiness.GetAssetAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ITSupportController", "GetAssignedDataAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }

        [HttpPost, Route("UpdateProduct")]
        public async Task<IActionResult> UpdateProductAsync(Replacement_DTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var data = await _replacementBusiness.UpdateProductAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ITSupportController", "UpdateProductAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeAsset")]
        public async Task<IActionResult> GetAssignedDataAsync([FromQuery] AssetList_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _resignationBusiness.GetAssignedDataAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ResignationController", "GetAssignedDataAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }

        [HttpPost, Route("SaveReplacement")]
        public async Task<IActionResult> SaveReplacementAsync(Replacement_DTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var data = await _replacementBusiness.SaveReplacementAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReplacementController", "SaveReplacementAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        //[HttpPost, Route("SaveReceived")]
        //public async Task<IActionResult> SaveReceivedAsync(Received_DTO model)
        //{
        //    var user = AppUser();
        //    try {
        //        if (ModelState.IsValid) {
        //            var data = await _replacementBusiness.SaveReceivedAsync(model, user);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "ReplacementController", "SaveReceivedAsync", user);
        //        return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
        //    }
        //}

    }
}
