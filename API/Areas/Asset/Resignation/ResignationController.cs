using API.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.Helpers;
using BLL.Asset.Interface.Resignation;
using Shared.Asset.Filter.Resignation;
using Shared.Asset.DTO.Resignation;
using API.Base;
using DAL.DapperObject.Interface;


namespace API.Areas.Asset_Module.Resignation
{
    [ApiController, Area("Asset"), Route("api/[area]/Resignation/[controller]"), Authorize]
    public class ResignationController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IResignationBusiness _resignationBusiness;

        public ResignationController(
           ISysLogger sysLogger,
           IResignationBusiness resignationBusiness,
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _resignationBusiness = resignationBusiness;
        }

        [HttpGet, Route("GetEmployeeResignation")]
        public async Task<IActionResult> GetEmployeeResignationAsync([FromQuery] Resignation_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _resignationBusiness.GetEmployeeResignationAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ResignationController", "GetEmployeeResignationAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }

        [HttpGet, Route("GetAssignedData")]
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

        [HttpPost, Route("SaveAsset")]
        public async Task<IActionResult> SaveAssetAsync(Resignation_DTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                        var data = await _resignationBusiness.SaveAssetAsync(model, user);                    
                        return Ok(data);                   
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ResignationController", "SaveAssetAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


    }
}
