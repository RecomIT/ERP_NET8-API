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




namespace API.Asset_Module.Support
{
    [ApiController, Area("Asset"), Route("api/[area]/Support/[controller]"), Authorize]
    public class ServicingController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IServicingBusiness _servicingBusiness;        

        public ServicingController(
           ISysLogger sysLogger,
           IServicingBusiness servicingBusiness,            
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _servicingBusiness = servicingBusiness;            
        }  

        [HttpGet, Route("GetServicingData")]
        public async Task<IActionResult> GetServicingDataAsync([FromQuery] Servicing_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _servicingBusiness.GetServicingDataAsync(filter, user);
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

        [HttpGet, Route("GetReceivedAsset")]
        public async Task<IActionResult> GetReceivedAssetAsync([FromQuery] Servicing_Filter filter)
        {           
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _servicingBusiness.GetReceivedAssetAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ResignationController", "GetReceivedAssetAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }

        [HttpPost, Route("SaveServicing")]
        public async Task<IActionResult> SaveServicingAsync(Servicing_DTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var data = await _servicingBusiness.SaveServicingAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ServicingController", "SaveServicingAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

      

    }
}
