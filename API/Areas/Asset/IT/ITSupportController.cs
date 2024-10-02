
using API.Services;
using BLL.Base.Interface;
using DAL.DapperObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using System.Threading.Tasks;
using System;
using System.Linq;
using Shared.Helpers;
using BLL.Asset.Interface.IT;
using Shared.Asset.Filter.IT;
using Shared.Asset.DTO.IT;
using API.Base;
using DAL.DapperObject.Interface;


namespace API.Asset.IT
{
    [ApiController, Area("Asset"), Route("api/[area]/IT/[controller]"), Authorize]
    public class ITSupportController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IITSupportBusiness _itSupportBusiness;

        public ITSupportController(
           ISysLogger sysLogger,
           IITSupportBusiness itSupportBusiness,
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _itSupportBusiness = itSupportBusiness;            
        }
  

        [HttpGet, Route("GetAssetData")]
        public async Task<IActionResult> GetAssetDataAsync([FromQuery] ITSupport_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _itSupportBusiness.GetAssetAsync(filter, user);
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

        [HttpGet, Route("GetAssetDataById")]
        public async Task<IActionResult> GetAssetIdAsync(long assigningId)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _itSupportBusiness.GetAssetAsync(new ITSupport_Filter { AssigningId = assigningId }, user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "ITSupportController", "GetAssetIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UpdateProduct")]
        public async Task<IActionResult> UpdateProductAsync(ITSupport_DTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var data = await _itSupportBusiness.UpdateProductAsync(model, user);
                    return Ok(data);                    
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ITSupportController", "UpdateProductAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


           
    }
}
