using AutoMapper;
using API.Services;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Base;
using Shared.Control_Panel.Domain;
using Shared.Control_Panel.ViewModels;
using DAL.DapperObject.Interface;
using BLL.Administration.Interface;

namespace API.Areas.ControlPanel.Controllers
{
    //[SysAuthorize]
    [ApiController, Area("ControlPanel"), Route("api/[area]/[controller]"), Authorize]
    public class AdministrationController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly IAppConfigBusiness _appConfigBusiness;
        private readonly IOrgInitBusiness _orgInitBusiness;
        private readonly IOrganizationConfig _organizationConfig;
        private readonly IUserConfigBusiness _userConfigBusiness;
        private readonly IModuleConfigBusiness _moduleConfigBusiness;
        private readonly ISysLogger _sysLogger;
        public AdministrationController(
            IMapper mapper,
            IAppConfigBusiness appConfigBusiness,
            IOrgInitBusiness orgConfigBusiness,
            IOrganizationConfig organizationConfig,
            IUserConfigBusiness userConfigBusiness,
            IModuleConfigBusiness moduleConfigBusiness,
            ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _mapper = mapper;
            _sysLogger = sysLogger;
            _appConfigBusiness = appConfigBusiness;
            _orgInitBusiness = orgConfigBusiness;
            _organizationConfig = organizationConfig;
            _userConfigBusiness = userConfigBusiness;
            _moduleConfigBusiness = moduleConfigBusiness;
        }

        #region App-Config
        // Application
        [HttpGet, Route("GetApplications")]
        public async Task<IActionResult> GetApplicationsAsync(string ApplicationName, string ApplicationType, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            var user = AppUser();
            try {
                var data = await _appConfigBusiness.GetApplicationsAsync(ApplicationName, ApplicationType, IsActive, OrgId, ComId, BranchId);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetApplicationsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        [HttpPost, Route("SaveApplication")]
        public async Task<IActionResult> SaveApplicationAsync(ApplicationViewModel application, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var save = await this._appConfigBusiness.SaveApplicationAsync(_mapper.Map<Application>(application), OrgId, ComId, BranchId, UserId);
                    return Ok(save);
                }
                return Ok(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveApplicationAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpDelete, Route("DeleteApplication")]
        public async Task<IActionResult> DeleteApplicationAsync(long ApplicationId, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (ApplicationId > 0) {
                    var status = await this._appConfigBusiness.DeleteApplicationAsync(ApplicationId, OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return Ok(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "DeleteApplicationAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        // Module
        [HttpGet, Route("GetModules")]
        public async Task<IActionResult> GetModuleAsync(string ModuleName, long ApplicationId, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            var user = AppUser();
            try {
                var data = await _appConfigBusiness.GetModuleAsync(ModuleName, ApplicationId, IsActive, OrgId, ComId, BranchId);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetModuleAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        [HttpPost, Route("SaveModule")]
        public async Task<IActionResult> SaveModuleAsync(ModuleViewModel module, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var save = await _appConfigBusiness.SaveModuleAsync(_mapper.Map<Module>(module), OrgId, ComId, BranchId, UserId);
                    return Ok(save);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveModuleAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpDelete, Route("DeleteModule")]
        public async Task<IActionResult> DeleteModuleAsync(long moduleId, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (moduleId > 0) {
                    var status = await this._appConfigBusiness.DeleteModuleAsync(moduleId, OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "DeleteModuleAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        // Mainmenu
        [HttpGet, Route("GetMainmenus")]
        public async Task<IActionResult> GetMainMenusAsync(string mainMenuName, long ModuleId, long ApplicationId, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            var user = AppUser();
            try {
                var data = await _appConfigBusiness.GetMainMenusAsync(mainMenuName, ModuleId, ApplicationId, IsActive, OrgId, ComId, BranchId);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetMainMenusAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        [HttpPost, Route("SaveMainmenu")]
        public async Task<IActionResult> SaveMainmenuAsync(MainMenuViewModel Mainmenu, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var save = await _appConfigBusiness.SaveMainmenuAsync(_mapper.Map<MainMenu>(Mainmenu), OrgId, ComId, BranchId, UserId);
                    return Ok(save);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveMainmenuAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        [HttpDelete, Route("DeleteMainmenu")]
        public async Task<IActionResult> DeleteMainmenuAsync(long MainmenuId, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (MainmenuId > 0) {
                    var status = await this._appConfigBusiness.DeleteMainmenuAsync(MainmenuId, OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveMainmenuAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        // Submenu
        [HttpGet, Route("GetSubmenus")]
        public async Task<IActionResult> GetSubmenusAsync(string SubmenuName, long? MainmenuId, long? ModuleId, long? ParentSubmenuId, long? ApplicationId, int pageNumber = 1, int pageSize = 15)
        {
            var user = AppUser();
            try {
                var allData = await _appConfigBusiness.GetSubmenusAsync(SubmenuName, MainmenuId, ParentSubmenuId ?? 0, ModuleId, ApplicationId);
                var data = PagedList<SubmenuViewModel>.ToPagedList(allData, pageNumber, pageSize);
                Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetSubmenusAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveSubmenu")]
        public async Task<IActionResult> SaveSubmenuAsync(SubmenuViewModel submenu, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var save = await _appConfigBusiness.SaveSubmenuAsync(_mapper.Map<SubMenu>(submenu), OrgId, ComId, BranchId, UserId);
                    return Ok(save);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveSubmenuAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpDelete, Route("DeleteSubmenu")]
        public async Task<IActionResult> DeleteSubmenuAsync(long SubmenuId, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (SubmenuId > 0) {
                    var status = await this._appConfigBusiness.DeleteSubmenuAsync(SubmenuId, OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "DeleteSubmenuAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        // Pagetab
        [HttpGet, Route("GetPageTabs")]
        public async Task<IActionResult> GetPageTabsAsync(string TabName, long? SubmenuId, long? MainmenuId, int pageNumber = 1, int pageSize = 15)
        {
            var user = AppUser();
            try {
                var allData = await _appConfigBusiness.GetPageTabAsync(TabName, SubmenuId, MainmenuId);
                var data = PagedList<PageTabViewModel>.ToPagedList(allData, pageNumber, pageSize);
                Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetPageTabsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SavePageTab")]
        public async Task<IActionResult> SavePageTabAsync(PageTabViewModel pagetab, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var save = await _appConfigBusiness.SavePageTabAsync(_mapper.Map<PageTab>(pagetab), OrgId, ComId, BranchId, UserId);
                    return Ok(save);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SavePageTabAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpDelete, Route("DeletePageTab")]
        public async Task<IActionResult> DeletePageTabAsync(long tabId, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (tabId > 0) {
                    var status = await this._appConfigBusiness.DeletePageTabAsync(tabId, OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "DeletePageTabAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        #endregion

        #region Org-Init
        // Organization
        [HttpGet, Route("GetOrganizations")]
        public async Task<IActionResult> GetOrganizationsAsync(string OrgName, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            var user = AppUser();
            try {
                var data = await _orgInitBusiness.GetOrganizationsAsync(OrgName, IsActive, OrgId, ComId, BranchId);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetOrganizationsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveOrgnaization")]
        public async Task<IActionResult> SaveOrgnaizationAsync([FromForm] OrganizationViewModel organization, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var status = await _orgInitBusiness.SaveOrganizationAsync(organization, OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveOrgnaizationAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpDelete, Route("DeleteOrganization")]
        public async Task<IActionResult> DeleteOrganizationAsync(long OrganizationId, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (OrganizationId > 0) {
                    var status = await _orgInitBusiness.DeleteOrganizationAsync(OrganizationId, OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "DeleteOrganizationAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetCompanies")]
        public async Task<IActionResult> GetCompaniesAsync(string CompanyName, long ComOrgId, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            var user = AppUser();
            try {
                var data = await _orgInitBusiness.GetCompanyAsync(CompanyName, ComOrgId, IsActive, OrgId, ComId, BranchId);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetCompaniesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveCompany")]
        public async Task<IActionResult> SaveCompanyAsync([FromForm] CompanyViewModel company, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var status = await _orgInitBusiness.SaveCompanyAsync(company, OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetCompaniesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpDelete, Route("DeleteCompany")]
        public async Task<IActionResult> DeleteCompanyAsync(long CompanyId, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (CompanyId > 0) {
                    var status = await _orgInitBusiness.DeleteCompanyAsync(CompanyId, OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "DeleteCompanyAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetDivisions")]
        public async Task<IActionResult> GetDivisionsAsync(string DivisionName, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            var user = AppUser();
            try {
                var data = await _orgInitBusiness.GetDivisionsAsync(DivisionName, IsActive, OrgId, ComId, BranchId);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetDivisions", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveDivision")]
        public async Task<IActionResult> SaveDivisionAsync(DivisionViewModel division, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {

                    var status = await _orgInitBusiness.SaveDivisionAsync(_mapper.Map<Division>(division), OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveDivisionAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpDelete, Route("DeleteDivision")]
        public async Task<IActionResult> DeleteDivisionAsync(long DivisionId, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (DivisionId > 0) {
                    var status = await _orgInitBusiness.DeleteDivisionAsync(DivisionId, OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveDivisionAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        // District
        [HttpGet, Route("GetDistricts")]
        public async Task<IActionResult> GetDistrictsAsync(string DistrictName, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            var user = AppUser();
            try {
                var data = await _orgInitBusiness.GetDistrictsAsync(DistrictName, IsActive, OrgId, ComId, BranchId);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetDistrictsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveDistrict")]
        public async Task<IActionResult> SaveDistrictAsync(DistrictViewModel district, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var status = await _orgInitBusiness.SaveDistrictAsync(_mapper.Map<District>(district), OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetDistrictsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpDelete, Route("DeleteDistrict")]
        public async Task<IActionResult> DeleteDistrictAsync(long DistrictId, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (DistrictId > 0) {
                    var status = await _orgInitBusiness.DeleteDistrictAsync(DistrictId, OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "DeleteDistrictAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetZones")]
        public async Task<IActionResult> GetZonesAsync(string ZoneName, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            var user = AppUser();
            try {
                var data = await _orgInitBusiness.GetZonesAsync(ZoneName, IsActive, OrgId, ComId, BranchId);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "DeleteDistrictAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveZone")]
        public async Task<IActionResult> SaveZoneAsync(ZoneViewModel zone, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {

                    var status = await _orgInitBusiness.SaveZoneAsync(_mapper.Map<Zone>(zone), OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveZoneAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpDelete, Route("DeleteZone")]
        public async Task<IActionResult> DeleteZoneAsync(long ZoneId, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (ZoneId > 0) {
                    var status = await _orgInitBusiness.DeleteZoneAsync(ZoneId, OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "DeleteZoneAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        //Branch
        [HttpGet("GetBranches")]
        public async Task<IActionResult> GetBranchesAsync(string BranchName, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            var user = AppUser();
            try {
                var data = await _orgInitBusiness.GetBranchesAsync(BranchName, IsActive, OrgId, ComId, BranchId);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetBranchesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveBranch")]
        public async Task<IActionResult> SaveBranchAsync(BranchViewModel branch, string UserId)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var status = await _orgInitBusiness.SaveBranchAsync(_mapper.Map<Branch>(branch), UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveBranchAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpDelete, Route("DeleteBranch")]
        public async Task<IActionResult> DeleteBranchAsync(long Id, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (Id > 0) {
                    var status = await _orgInitBusiness.DeleteBranchAsync(Id, OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "DeleteBranchAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        #endregion

        #region Org-Config
        [HttpGet, Route("GetOrgAuths")]
        public async Task<IActionResult> GetOrgAuthsAsync(long OrgId)
        {
            var user = AppUser();
            try {
                var data = await _organizationConfig.GetOrganizationAuths(OrgId);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetOrgAuthsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveOrgAuth")]
        public async Task<IActionResult> SaveOrgAuthsAsync(AuthApp orgAuth, long AuthOrgId, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (orgAuth != null) {
                    var status = await _organizationConfig.SaveOrgAuthsAsync(orgAuth, AuthOrgId, OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveOrgAuthsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetComAuths")]
        public async Task<IActionResult> GetCompanyAuthsAsync(long OrgId, long ComId)
        {
            var user = AppUser();
            try {
                var data = await _organizationConfig.GetCompanyAuths(OrgId, ComId);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetCompanyAuthsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveComAuth")]
        public async Task<IActionResult> SaveComAuthsAsync(AuthApp comAuth, long AuthOrgId, long AuthComId, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                if (comAuth != null) {
                    var status = await _organizationConfig.SaveComAuthsAsync(comAuth, AuthOrgId, AuthComId, OrgId, ComId, BranchId, UserId);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveComAuthsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        // Module-Config
        [HttpGet, Route("GetModuleConfigs")]
        public async Task<IActionResult> GetModuleConfigsAsync(long mainemnuId, long? moduleId, long? branchId, long comId, long orgId)
        {
            var user = AppUser();
            try {
                if (mainemnuId > 0 && comId > 0 && orgId > 0) {
                    var data = await _organizationConfig.GetModuleConfigsAsync(mainemnuId, moduleId ?? 0, branchId ?? 0, comId, orgId);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetModuleConfigsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveModuleConfig")]
        public async Task<IActionResult> SaveModuleConfigAsync(List<ModuleConfigViewModel> moduleConfigs, string userId)
        {
            var user = AppUser();
            try {
                if (moduleConfigs.Count > 0 && !Utility.IsNullEmptyOrWhiteSpace(userId)) {
                    var data = await _organizationConfig.SaveModuleConfigAsync(_mapper.Map<List<ModuleConfig>>(moduleConfigs), userId);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveModuleConfigAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        // Role
        [HttpGet, Route("GetApplicationRoles")]
        public async Task<IActionResult> GetApplicationRolesAsync(string RoleName, string RoleId, long RoleComId, long RoleOrgId, bool? IsActive, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                var data = await _organizationConfig.GetApplicationRolesAsync(RoleName, RoleId, RoleComId, RoleOrgId, IsActive, OrgId, ComId, BranchId, UserId);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetApplicationRolesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveApplicationRole")]
        public async Task<IActionResult> SaveApplicationRoleAsync(ApplicationRoleViewModel role, long OrgId, long ComId, long BranchId, string UserId)
        {
            var user = AppUser();
            try {
                var status = await _organizationConfig.SaveApplicationRoleAsync(_mapper.Map<ApplicationRole>(role), OrgId, ComId, BranchId, UserId);
                return Ok(status);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetApplicationRolesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetPayrollModuleConfigs")]
        public async Task<IActionResult> GetPayrollModuleConfigsAsync(long companyId, long organizationId)
        {
            var user = AppUser();
            try {
                if (companyId > 0 && organizationId > 0) {
                    var data = await _moduleConfigBusiness.GetPayrollModuleConfigsAsync(companyId, organizationId);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetPayrollModuleConfigsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("SavePayrollModuleConfigs")]
        public async Task<IActionResult> SavePayrollModuleConfigsAsync(PayrollModuleConfigViewModel model)
        {
            var user = AppUser();
            try {
                if (model.CompanyId > 0 && model.OrganizationId > 0) {
                    var data = await _moduleConfigBusiness.SavePayrollModuleConfigsAsync(model);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SavePayrollModuleConfigsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetHRModuleConfigs")]
        public async Task<IActionResult> GetHRModuleConfigsAsync(long companyId, long organizationId)
        {
            var user = AppUser();
            try {
                if (companyId > 0 && organizationId > 0) {
                    var data = await _moduleConfigBusiness.GetHRModuleConfigsAsync(companyId, organizationId);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SavePayrollModuleConfigsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("SaveHRModuleConfigs")]
        public async Task<IActionResult> SaveHRModuleConfigsAsync(HRModuleConfigViewModel model)
        {
            var user = AppUser();
            try {
                if (model.CompanyId > 0 && model.OrganizationId > 0) {
                    var data = await _moduleConfigBusiness.SaveHRModuleConfigsAsync(model);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SavePayrollModuleConfigsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetPFModuleConfigs")]
        public async Task<IActionResult> GetPFModuleConfigsAsync(long companyId, long organizationId)
        {
            var user = AppUser();
            try {
                if (companyId > 0 && organizationId > 0) {
                    var data = await _moduleConfigBusiness.GetPFModuleConfigsAsync(companyId, organizationId);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SavePayrollModuleConfigsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("SavePFModuleConfigs")]
        public async Task<IActionResult> SavePFModuleConfigsAsync(PFModuleConfigViewModel model)
        {
            var user = AppUser();
            try {
                if (model.CompanyId > 0 && model.OrganizationId > 0) {
                    var data = await _moduleConfigBusiness.SavePFModuleConfigsAsync(model);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SavePFModuleConfigsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        #endregion

        #region User-Config
        [HttpGet, Route("GetApplicationUsers")]
        public async Task<IActionResult> GetApplicationUsersAsync(string fullName, string userName, string roleId, string address, string email, string phoneNumber, string fromDate, string toDate, long branchId = 0, long divisionId = 0, long companyId = 0, long organizationId = 0)
        {
            var user = AppUser();
            try {
                var data = await _userConfigBusiness.GetApplicationUsers(fullName, userName, roleId, address, email, phoneNumber, branchId, divisionId, companyId, organizationId, fromDate, toDate);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetApplicationUsersAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetAppMenusForPermission")]
        public async Task<IActionResult> GetAppMenusForPermissionAsync(string userId, string roleId, long companyId, long organizationId, string flag)
        {
            var user = AppUser();
            try {
                var data = await _userConfigBusiness.GetAppMenusForPermissionAsync(userId, roleId, companyId, organizationId, flag);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetApplicationUsersAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveUserAuth")]
        public async Task<IActionResult> SaveUserAsync(UserInfoData userInfo, long BranchId, long ComId, long OrgId, string UserId)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && BranchId > 0 && ComId > 0 && OrgId > 0 && !string.IsNullOrEmpty(UserId) && !string.IsNullOrWhiteSpace(UserId)) {
                    var data = await _userConfigBusiness.SaveUserInfo(userInfo, BranchId, ComId, OrgId, UserId);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveUserAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [AllowAnonymous, HttpPost, Route("SaveUserInfo")]
        public async Task<IActionResult> SaveUserInfoAysnc(long companyId, long organizationId)
        {
            //var user = AppUser();
            try {
                await _userConfigBusiness.GetUserInfosAsync(companyId, organizationId);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
            catch (Exception ex) {
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveRoleAuth")]
        public async Task<IActionResult> SaveRoleAuthAsync(List<AppMainMenuForPermission> appMainMenus, string roleId, long? branchId, long companyId, long organizationId, string userId)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && !Utility.IsNullEmptyOrWhiteSpace(roleId) && companyId > 0 && organizationId > 0 && !Utility.IsNullEmptyOrWhiteSpace(userId)) {
                    var data = await _userConfigBusiness.SaveRoleAuthAsync(appMainMenus, roleId, branchId ?? 0, companyId, organizationId, userId);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveUserAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("ChangeUserPassword")]
        public async Task<IActionResult> ChangeUserPasswordAsync(ChangePasswordViewModel model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && model.CompanyId > 0 && model.OrganizationId > 0) {
                    var validator = await _userConfigBusiness.CheckUserAsync(model.UserId, model.CurrentPassword);
                    if (validator != null) {
                        var data = await _userConfigBusiness.ChangeUserPasswordAsync(model);
                        return Ok(data);
                    }
                    return Ok(validator);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "SaveUserAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("ChangeUserDefaultPassword")]
        public async Task<IActionResult> ChangeUserDefaultPasswordAsync(ChangePasswordViewModel model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && model.CompanyId > 0 && model.OrganizationId > 0) {
                    var validator = await _userConfigBusiness.CheckUserAsync(model.UserId, model.CurrentPassword);
                    if (validator != null) {
                        var data = await _userConfigBusiness.ChangeUserDefaultPasswordAsync(model);
                        return Ok(data);
                    }
                    return Ok(validator);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "ChangeUserDefaultPasswordAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetUserInfoById")]
        public async Task<IActionResult> GetUserInfoByIdAsync(string id)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _userConfigBusiness.GetUserInfoByIdAysc(id);
                    if (data != null) {
                        return Ok(new { FullName = data.FullName, Email = data.Email, PhoneNumber = data.PhoneNumber, Address = data.Address });
                    }
                    return NotFound("Data not found");

                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdministrationController", "GetUserInfoByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        #endregion
    }
}
