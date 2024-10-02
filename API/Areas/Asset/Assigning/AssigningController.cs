
using API.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.Helpers;
using BLL.Asset.Interface.Assigning;
using Shared.Asset.Filter.Assigning;
using Shared.Asset.DTO.Assigning;
using Shared.Asset.Filter.Create;
using API.Base;
using DAL.DapperObject.Interface;



namespace API.Areas.Asset.Assigning
{
    [ApiController, Area("Asset"), Route("api/[area]/Assigning/[controller]"), Authorize]
    public class AssigningController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IAssigningBusiness _assigningBusiness;

        public AssigningController(
           ISysLogger sysLogger,
           IAssigningBusiness assigningBusiness,
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _assigningBusiness = assigningBusiness;
        }

        [HttpGet, Route("GetAssignedData")]
        public async Task<IActionResult> GetAssignedDataAsync([FromQuery] Assigning_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _assigningBusiness.GetAssignedDataAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssigningController", "GetAssignedDataAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }

        [HttpGet, Route("GetAssignedDataById")]
        public async Task<IActionResult> GetAssignedDataByIdAsync(long assigningId)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _assigningBusiness.GetAssignedDataAsync(new Assigning_Filter { AssigningId = assigningId }, user);
                    if (data.ListOfObject.Count() > 0) {
                        return Ok(data.ListOfObject.FirstOrDefault());
                    }
                    else {
                        return NoContent();
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssigningController", "GetAssignedDataByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveAssetAssigning")]
        public async Task<IActionResult> SaveAssetAssigningAsync(Assigning_DTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _assigningBusiness.ValidatorAssetAssigningAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var data = await _assigningBusiness.SaveAssetAssigningAsync(model, user);                    
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssigningController", "SaveAssetAssigningAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetAsset")]
        public async Task<IActionResult> GetAssetAsync([FromQuery] Create_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _assigningBusiness.GetAssetAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssetController", "GetAssetAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }

        [HttpGet, Route("GetProduct")]
        public async Task<IActionResult> GetProductAsync([FromQuery] Product_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _assigningBusiness.GetProductAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssigningController", "GetProductAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }

        [HttpGet, Route("SendEmail")]
        public async Task<IActionResult> EmailSendAsync(long employeeId)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var data = await _assigningBusiness.EmailSendAsync(employeeId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssigningController", "EmailSendAsync", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
        }

        //[HttpPost, Route("ApprovedAsset")]
        //public async Task<IActionResult> ApprovedAssetAsync(int assetId, int assigningId, string activeTab)
        //{
        //    var user = AppUser();
        //    try {
        //        if (ModelState.IsValid) {
        //            var data = await _assigningBusiness.ApprovedAssetAsync(assetId, assigningId, activeTab, user);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "AssigningController", "ApprovedAssetAsync", user);
        //        return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
        //    }
        //}


        [HttpGet, Route("GetProductDropdown")]
        public async Task<IActionResult> GetProductDropdownAsync([FromQuery] Product_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _assigningBusiness.GetProductDropdownAsync(filter, user));
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CreateController", "GetProductDropdownAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
