using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using BLL.Administration.Interface;
using Shared.OtherModels.DataService;
using Shared.Control_Panel.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Areas.WebServices.Controllers
{
    //[Authorize]
    [ApiController, Area("WS"), Route("api/[area]/[controller]"), Authorize]
    public class ControlPanelServiceController : ApiBaseController
    {
        private readonly IOrgInitBusiness _orgInitBusiness;
        private readonly IOrganizationConfig _organizationConfig;
        private readonly IAppConfigBusiness _appConfigBusiness;
        private readonly IModuleConfigBusiness _moduleConfigBusiness;
        private readonly ISysLogger _sysLogger;
        public ControlPanelServiceController(ISysLogger sysLogger, IOrgInitBusiness orgInitBusiness, IAppConfigBusiness appConfigBusiness, IOrganizationConfig organizationConfig, IModuleConfigBusiness moduleConfigBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _orgInitBusiness = orgInitBusiness;
            _appConfigBusiness = appConfigBusiness;
            _organizationConfig = organizationConfig;
            _moduleConfigBusiness = moduleConfigBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetOrganizationExtension")]
        public async Task<ActionResult<Select2Dropdown>> GetOrganizationExtensionAsync()
        {
            var user = AppUser();
            try
            {
                var data = await _orgInitBusiness.GetOrganizationExtensionAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ControlPanelServiceController", "GetOrganizationExtensionAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetCompanyExtension")]
        public async Task<ActionResult<Select2Dropdown>> GetCompanyExtensionAsync(long? orgId)
        {
            var user = AppUser();
            try
            {
                var data = await _orgInitBusiness.GetCompanyExtensionAsync(orgId ?? 0);
                return Ok(data);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ControlPanelServiceController", "GetCompanyExtensionAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetBranchExtension")]
        public async Task<IActionResult> GetBranchExtensionAsync(string flag, long ComId, long OrgId)
        {
            var user = AppUser();
            try
            {
                if (!string.IsNullOrWhiteSpace(flag)
                    && (Utility.ExtensionFlags().Find(f => f == flag) != null)
                    && ComId > 0
                    && OrgId > 0
                  )
                {
                    var data = await _orgInitBusiness.BranchExtension(flag, ComId, OrgId);
                    return Ok(data);
                }
                return BadRequest("Invalid Parameters");
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveHRMSException(ex, user.Database, "ControlPanelServiceController", "GetBranchExtension", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSubemenuExtension")]
        public async Task<IActionResult> GetSubemenus(bool HasTab, long? mainMenuId, bool IsActAsParent)
        {
            var user = AppUser();
            try
            {
                var data = ((List<SubmenuViewModel>)await _appConfigBusiness.GetSubmenusAsync(string.Empty, (mainMenuId ?? 0), 0, 0, 0));
                if (data.Count > 0)
                {
                    var list = data.Where(s => (HasTab == false || s.HasTab == HasTab) &&
                    (IsActAsParent == false || s.IsActAsParent == IsActAsParent)
                    ).Select(s => new Dropdown { Text = s.SubmenuName + " [" + s.MenuName + "]", Value = s.SubmenuId.ToString() }).ToList();
                    return Ok(list);
                }
                return BadRequest("Something went wrong");
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ControlPanelServiceController", "GetSubemenus", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetCompanyAuthMainmenuExtension")]
        public async Task<ActionResult<Select2Dropdown>> GetCompanyAuthMainmenuExtensionAsync(long comId)
        {
            var user = AppUser();
            try
            {
                var data = await _organizationConfig.GetCompanyAuthMainmenuExtensionAsync(comId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ControlPanelServiceController", "GetCompanyAuthMainmenuExtension", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRolesAsync(long orgId, long comId)
        {
            var user = AppUser();
            try
            {
                if (orgId > 0 && comId > 0)
                {
                    var data = await _organizationConfig.GetApplicationRolesExtensionAsync(orgId, comId);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ControlPanelServiceController", "GetUserRolesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        [HttpGet, Route("GetPayrollModuleConfig")]
        public async Task<IActionResult> GetPayrollBonusUnitAsync()
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = (await _moduleConfigBusiness.GetPayrollModuleConfigsAsync(user.CompanyId, user.OrganizationId)).ToList().FirstOrDefault();
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ControlPanelServiceController", "GetPayrollModuleConfig", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

    }
}

