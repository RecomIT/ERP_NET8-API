
using API.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using BLL.Asset.Interface.Dashboard;
using Shared.Asset.Filter.Report;
using API.Base;
using DAL.DapperObject.Interface;


namespace API.Asset.Dashboard
{
    [ApiController, Area("Asset"), Route("api/[area]/Dashboard/[controller]"), Authorize]
    public class AdminController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IAdminBusiness _adminBusiness;

        public AdminController(
           ISysLogger sysLogger,
           IAdminBusiness adminBusiness,
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _adminBusiness = adminBusiness;
        }


        [HttpGet, Route("GetAssetCreationData")]
        public async Task<IActionResult> GetAssetCreationData()
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _adminBusiness.GetAssetCreationDataAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


        [HttpGet, Route("GetAssetAssigningData")]
        public async Task<IActionResult> GetAssetAssigningData()
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _adminBusiness.GetAssetAssigningDataAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));


            }
        }


        [HttpGet, Route("GetStock")]
        public async Task<IActionResult> GetAssetAsync([FromQuery] Report_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _adminBusiness.GetAssetAsync(filter, user);    
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdminController", "GetAssetAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }

    }


    }