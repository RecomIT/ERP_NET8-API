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
    public class RepairedController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IRepairedBusiness _repairedBusiness;        

        public RepairedController(
           ISysLogger sysLogger,
           IRepairedBusiness repairedBusiness,            
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _repairedBusiness = repairedBusiness;            
        }  

        [HttpGet, Route("GetRepairedData")]
        public async Task<IActionResult> GetRepairedDataAsync([FromQuery] Servicing_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _repairedBusiness.GetRepairedDataAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ServicingController", "GetServicingDataAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }        

        [HttpGet, Route("GetServicingAsset")]
        public async Task<IActionResult> GetServicingAssetAsync([FromQuery] Servicing_Filter filter)
        {           
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _repairedBusiness.GetServicingAssetAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RepairedController", "GetServicingAssetAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }

        [HttpPost, Route("SaveRepaired")]
        public async Task<IActionResult> SaveRepairedAsync(Repaired_DTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var data = await _repairedBusiness.SaveRepairedAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ServicingController", "SaveRepairedAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

      

    }
}
