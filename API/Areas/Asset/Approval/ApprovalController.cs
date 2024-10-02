
using API.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using BLL.Asset.Interface.Approval;
using API.Base;
using DAL.DapperObject.Interface;

namespace API.Areas.Asset_Module.Approval
{
    [ApiController, Area("Asset"), Route("api/[area]/Approval/[controller]"), Authorize]
    public class ApprovalController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IApprovalBusiness _approvalBusiness;

        public ApprovalController(
           ISysLogger sysLogger,
           IApprovalBusiness approvalBusiness,
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _approvalBusiness = approvalBusiness;
        }


        [HttpGet, Route("SendEmail")]
        public async Task<IActionResult> EmailSendAsync(long employeeId, string sendingType)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var data = await _approvalBusiness.EmailSendAsync(employeeId, sendingType, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssigningController", "EmailSendAsync", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
        }

        [HttpPost, Route("ApprovedAsset")]
        public async Task<IActionResult> ApprovedAssetAsync(int assetId, int assigningId, string activeTab)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var data = await _approvalBusiness.ApprovedAssetAsync(assetId, assigningId, activeTab, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssigningController", "ApprovedAssetAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


    }
}
