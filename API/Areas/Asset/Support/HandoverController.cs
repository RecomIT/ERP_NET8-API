using API.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.Helpers;
using Shared.Asset.Filter.Support;
using BLL.Asset.Interface.Support;
using Shared.Asset.DTO.Support;
using API.Base;
using DAL.DapperObject.Interface;



namespace API.Asset.Support
{
    [ApiController, Area("Asset"), Route("api/[area]/Support/[controller]"), Authorize]
    public class HandoverController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        //private readonly IReplacementBusiness _replacementBusiness;
        private readonly IHandoverBusiness _handoverBusiness;
        

        public HandoverController(
           ISysLogger sysLogger,
           //IReplacementBusiness replacementBusiness,
           IHandoverBusiness handoverBusiness,            
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            //_replacementBusiness = replacementBusiness;
            _handoverBusiness = handoverBusiness;
            
        }  

        [HttpGet, Route("GetHandoverData")]
        public async Task<IActionResult> GetHandoverDataAsync([FromQuery] Handover_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _handoverBusiness.GetHandoverDataAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "HandoverController", "GetAssetDataAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }        

        [HttpGet, Route("GetEmployeeAsset")]
        public async Task<IActionResult> GetAssignedDataAsync([FromQuery] Handover_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _handoverBusiness.GetAssignedDataAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "HandoverController", "GetAssignedDataAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }           

        }

        [HttpPost, Route("SaveHandover")]
        public async Task<IActionResult> SaveHandoverAsync(Handover_DTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var data = await _handoverBusiness.SaveHandoverAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "HandoverController", "SaveHandover", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

      

    }
}
