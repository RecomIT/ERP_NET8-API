using AutoMapper;
using Microsoft.AspNetCore.Identity;
using DAL.DapperObject;
using Shared.Services;
using Dapper;
using System.Data;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using BLL.Base.Interface;
using DAL.UnitOfWork.Control_Panel.Interface;
using DAL.Repository.Control_Panel;
using DAL.DapperObject.Interface;
using Shared.Control_Panel.Domain;
using Shared.Control_Panel.ViewModels;
using BLL.Administration.Interface;

namespace BLL.Administration.Implementation
{
    public class OrganizationConfig : IOrganizationConfig
    {
        private readonly IControlPanelUnitOfWork _controlPanelDbContext;
        private readonly OrganizationAuthorizationRepository _organizationAuthorizationRepository;
        private readonly CompanyAuthorizationRepository _companyAuthorizationRepository;
        private readonly ApplicationRoleRepository _applicationRoleRepository;
        private readonly OrganizationRepository _organizationRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly ModuleRepository _moduleRepository;
        private readonly MainMenuRepository _mainMenuRepository;
        private readonly ModuleConfigReposiitory _moduleConfigReposiitory;
        private readonly IMapper _mapper;
        private readonly IDapperData _dapperData;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private string sqlQuery = null;
        private readonly ISysLogger _sysLogger;
        public OrganizationConfig(IControlPanelUnitOfWork controlPanelDbContext, DapperContext dapperContext, IMapper mapper, IDapperData dapperData, RoleManager<ApplicationRole> roleManager, ISysLogger sysLogger)
        {
            _controlPanelDbContext = controlPanelDbContext;
            _mapper = mapper;
            _dapperData = dapperData;
            _sysLogger = sysLogger;
            _organizationAuthorizationRepository = new OrganizationAuthorizationRepository(_controlPanelDbContext);
            _companyAuthorizationRepository = new CompanyAuthorizationRepository(_controlPanelDbContext);
            _applicationRoleRepository = new ApplicationRoleRepository(_controlPanelDbContext);
            _organizationRepository = new OrganizationRepository(_controlPanelDbContext);
            _companyRepository = new CompanyRepository(_controlPanelDbContext);
            _moduleRepository = new ModuleRepository(_controlPanelDbContext);
            _mainMenuRepository = new MainMenuRepository(_controlPanelDbContext);
            _moduleConfigReposiitory = new ModuleConfigReposiitory(_controlPanelDbContext);
            _roleManager = roleManager;
        }
        public async Task<AuthApp> GetOrganizationAuths(long OrgId)
        {
            AuthApp orgAuthApp = new AuthApp();
            try
            {
                sqlQuery = string.Format(@"Exec spOrganizationAuthorization @orgId={0}", OrgId);
                var orgAuth = await _dapperData.SqlQueryListAsync<AuthViewModel>(Database.ControlPanel, sqlQuery);

                // return obj


                if (orgAuth.Count() > 0)
                {
                    // app
                    var app = orgAuth.Select(s => new { s.ApplicationId, s.ApplicationName }).Distinct().SingleOrDefault();

                    if (app != null)
                    {
                        orgAuthApp.AppId = app.ApplicationId;
                        orgAuthApp.AppName = app.ApplicationName;
                    }

                    // module
                    orgAuthApp.Modules = new List<AuthModule>();
                    var modules = orgAuth.Select(s => new { s.ModuleId, s.ModuleName }).Distinct().ToList();

                    foreach (var item in modules)
                    {
                        AuthModule orgAuthModule = new AuthModule()
                        {
                            ModuleId = item.ModuleId,
                            ModuleName = item.ModuleName,
                            MainMenus = orgAuth.Where(m => m.ModuleId == item.ModuleId).Select(s => new AuthMainmenu { MainMenuId = s.MMId, MainMenuName = s.MenuName, HasPermision = s.HasPermision }).ToList()
                        };
                        orgAuthApp.Modules.Add(orgAuthModule);
                    }
                }

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrganizationConfig", "GetOrganizationAuths", "", 0, 0, 0);
            }
            return orgAuthApp;
        }
        public async Task<bool> SaveOrgAuthsAsync(AuthApp orgAuth, long AuthOrgId, long OrgId, long ComId, long BranchId, string UserId)
        {
            List<OrganizationAuthorization> list = new List<OrganizationAuthorization>();
            try
            {

                foreach (var m in orgAuth.Modules)
                {
                    foreach (var mm in m.MainMenus)
                    {
                        if (mm.HasPermision)
                        {
                            OrganizationAuthorization item = new OrganizationAuthorization();
                            item.ApplicationId = orgAuth.AppId;
                            item.ModuleId = m.ModuleId;
                            item.MainmenuId = mm.MainMenuId;
                            item.OrganizationId = AuthOrgId;
                            item.CreatedBy = UserId;
                            item.CreatedDate = DateTime.Now;
                            list.Add(item);
                        }
                    }
                }

                await _organizationAuthorizationRepository.DeleteAllAsync(a => a.OrganizationId == AuthOrgId);
                await _organizationAuthorizationRepository.SaveAsync();
                await _organizationAuthorizationRepository.InsertAllAsync(list);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrganizationConfig", "SaveOrgAuthsAsync", "", 0, 0, 0);
            }
            return await _organizationAuthorizationRepository.SaveAsync();
        }
        public async Task<AuthApp> GetCompanyAuths(long OrgId, long ComId)
        {
            AuthApp comAuthApp = new AuthApp();
            try
            {
                sqlQuery = string.Format(@"Exec spCompanyAuthorization @OrgId={0},@ComId={1}", OrgId, ComId);
                var comAuth = await _dapperData.SqlQueryListAsync<AuthViewModel>(Database.ControlPanel, sqlQuery);
                // return obj


                if (comAuth.AsList().Count > 0)
                {
                    // app
                    var app = comAuth.Select(s => new { s.ApplicationId, s.ApplicationName }).Distinct().SingleOrDefault();
                    if (app != null)
                    {
                        comAuthApp.AppId = app.ApplicationId;
                        comAuthApp.AppName = app.ApplicationName;
                    }
                    // module
                    comAuthApp.Modules = new List<AuthModule>();
                    var modules = comAuth.Select(s => new { s.ModuleId, s.ModuleName }).Distinct().ToList();

                    foreach (var item in modules)
                    {
                        AuthModule orgAuthModule = new AuthModule()
                        {
                            ModuleId = item.ModuleId,
                            ModuleName = item.ModuleName,
                            MainMenus = comAuth.Where(m => m.ModuleId == item.ModuleId).Select(s => new AuthMainmenu { MainMenuId = s.MMId, MainMenuName = s.MenuName, HasPermision = s.HasPermision }).ToList()
                        };
                        comAuthApp.Modules.Add(orgAuthModule);
                    }
                }

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrganizationConfig", "GetCompanyAuths", "", 0, 0, 0);
            }
            return comAuthApp;
        }
        public async Task<bool> SaveComAuthsAsync(AuthApp orgAuth, long AuthOrgId, long AuthComId, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                List<CompanyAuthorization> list = new List<CompanyAuthorization>();
                foreach (var m in orgAuth.Modules)
                {
                    foreach (var mm in m.MainMenus)
                    {
                        if (mm.HasPermision)
                        {
                            CompanyAuthorization item = new CompanyAuthorization();
                            item.ApplicationId = orgAuth.AppId;
                            item.ModuleId = m.ModuleId;
                            item.MainmenuId = mm.MainMenuId;
                            item.OrganizationId = AuthOrgId;
                            item.CompanyId = AuthComId;
                            item.CreatedBy = UserId;
                            item.CreatedDate = DateTime.Now;
                            list.Add(item);
                        }
                    }
                }
                await _companyAuthorizationRepository.DeleteAllAsync(a => a.OrganizationId == AuthOrgId && a.CompanyId == AuthComId);
                await _companyAuthorizationRepository.SaveAsync();

                await _companyAuthorizationRepository.InsertAllAsync(list);

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrganizationConfig", "SaveComAuthsAsync", "", 0, 0, 0);
            }
            return await _companyAuthorizationRepository.SaveAsync();
        }
        public async Task<IEnumerable<ApplicationRoleViewModel>> GetApplicationRolesAsync(string RoleName, string RoleId, long RoleComId, long RoleOrgId, bool? IsActive, long OrgId, long ComId, long BranchId, string UserId)
        {
            IEnumerable<ApplicationRoleViewModel> viewData = new List<ApplicationRoleViewModel>();
            try
            {
                //var data = (await _applicationRoleRepository.GetAllAsync(role =>
                //    (string.IsNullOrEmpty(RoleName) || string.IsNullOrWhiteSpace(RoleName) || role.Name.Contains(RoleName.ToLower())) &&
                //    (string.IsNullOrEmpty(RoleId) || string.IsNullOrWhiteSpace(RoleId) || (role.Id.ToString() == RoleId)) &&
                //    (RoleComId == 0 || role.CompanyId == RoleComId) &&
                //    (RoleOrgId == 0 || role.OrganizationId == RoleOrgId) &&
                //    (!IsActive.HasValue || role.IsActive == IsActive)
                //));

                //var viewData = _mapper.Map<IEnumerable<ApplicationRoleViewModel>>(data);
                //foreach (var item in viewData) {
                //    item.OrganizationName = (await _organizationRepository.GetSingleAsync(org => org.OrganizationId == item.OrganizationId)).OrganizationName;
                //    item.CompanyName = (await _companyRepository.GetSingleAsync(com => com.CompanyId == item.CompanyId)).CompanyName;
                //}
                sqlQuery = string.Format(@"sp_ApplicationRoles");
                var parameters = new DynamicParameters();
                parameters.Add("RoleId", RoleId ?? "");
                parameters.Add("RoleName", RoleName ?? "");
                parameters.Add("CompanyId", RoleComId);
                parameters.Add("OrganizationId", RoleOrgId);
                parameters.Add("ExecutionFlag", "AllRoleList");
                //var exMsg = "sp_ApplicationRoles @RoleId=" + (RoleId ?? "").ToString() + ",@RoleName=" + (RoleName ?? "").ToString() + ",CompanyId=" + RoleComId.ToString() + ",RoleOrgId=" + RoleOrgId.ToString() + ",ExecutionFlag=AllRoleList";
                //Exception exception = new Exception(exMsg);
                //await _sysLogger.SaveControlPanelException(exception, Database.ControlPanel, "OrganizationConfig", "GetApplicationRolesAsync", "", 0, 0, 0);

                viewData = await _dapperData.SqlQueryListAsync<ApplicationRoleViewModel>(Database.ControlPanel, sqlQuery, parameters, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrganizationConfig", "GetApplicationRolesAsync", "", 0, 0, 0);
            }
            return viewData;
        }

        public async Task<IEnumerable<Select2Dropdown>> GetApplicationRolesExtensionAsync(long OrgId, long ComId)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                sqlQuery = string.Format(@"sp_ApplicationRoles");
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", ComId);
                parameters.Add("OrganizationId", OrgId);
                parameters.Add("ExecutionFlag", "Extension");

                data = await _dapperData.SqlQueryListAsync<Select2Dropdown>(Database.ControlPanel, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrganizationConfig", "GetApplicationRolesExtensionAsync", "", 0, 0, 0);
            }
            return data;
        }
        public async Task<bool> SaveApplicationRoleAsync(ApplicationRole role, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                if (role.Id.ToString() == "00000000-0000-0000-0000-000000000000" || role.Id.ToString() == "{00000000-0000-0000-0000-000000000000}")
                {
                    if (role.OrganizationId != 7)
                    {
                        role.Name = role.Name.Trim() + "#Org_" + role.OrganizationId.ToString() + "#Com_" + role.CompanyId.ToString();
                    }
                    role.CreatedBy = UserId;
                    role.CreatedDate = DateTime.Now;
                    var IsSucceeded = await _roleManager.CreateAsync(role);

                    if (IsSucceeded.Succeeded)
                    {
                        return true;
                    };
                }
                else
                {
                    var roleInDb = await _roleManager.FindByIdAsync(role.Id.ToString());
                    if (roleInDb != null)
                    {
                        if (role.OrganizationId != 7)
                        {
                            roleInDb.Name = role.Name.Trim() + "#Org_" + role.OrganizationId.ToString() + "#Com_" + role.CompanyId.ToString();
                        }
                        roleInDb.CompanyId = role.CompanyId;
                        roleInDb.OrganizationId = role.OrganizationId;
                        roleInDb.UpdatedBy = UserId;
                        roleInDb.UpdatedDate = DateTime.Now;
                        if ((await _roleManager.UpdateAsync(roleInDb)).Succeeded)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrganizationConfig", "SaveApplicationRoleAsync", "", 0, 0, 0);
                return false;
            }
        }
        public async Task<IEnumerable<Select2Dropdown>> GetCompanyAuthMainmenuExtensionAsync(long comId)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var comAuthMainmenu = (await _companyAuthorizationRepository.GetAllAsync(c => c.CompanyId == comId)).Select(s => s.MainmenuId).ToList();
                data = (await _mainMenuRepository.GetAllAsync(m => comAuthMainmenu.Contains(m.MMId))).Select(m => new Select2Dropdown
                {
                    Id = m.MMId.ToString(),
                    Text = m.MenuName
                }).ToList();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrganizationConfig", "GetCompanyAuthMainmenuExtensionAsync", "", 0, 0, 0);
            }
            return data;
        }
        public async Task<IEnumerable<ModuleConfigViewModel>> GetModuleConfigsAsync(long mainemnuId, long moduleId, long branchId, long comId, long orgId)
        {
            IEnumerable<ModuleConfigViewModel> data = new List<ModuleConfigViewModel>();
            try
            {
                sqlQuery = string.Format(@"Exec sp_CompanyModuleConfig @MainmenuId={0},@ModuleId={1},@BranchId={2},@ComId={3},@OrgId={4}", mainemnuId, moduleId, branchId, comId, orgId);
                sqlQuery = Utility.ParamChecker(sqlQuery);
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapperData.SqlQueryListAsync<ModuleConfigViewModel>(Database.ControlPanel, sqlQuery);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrganizationConfig", "GetModuleConfigsAsync", "", 0, 0, 0);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveModuleConfigAsync(List<ModuleConfig> moduleConfigs, string userId)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var newConfig = moduleConfigs.Where(s => s.ModuleConfigId == 0).ToList();
                var updateConfig = moduleConfigs.Where(s => s.ModuleConfigId > 0).ToList();
                foreach (var item in newConfig)
                {
                    if (item.ModuleConfigId == 0)
                    {
                        item.CreatedBy = userId;
                        item.CreatedDate = DateTime.Now;
                        item.ConfigValue = item.ConfigValue == "true" ?
                            Utility.ProperCase(item.ConfigValue) : item.ConfigValue == "false" ? null : item.ConfigValue;
                    }
                }
                foreach (var item in updateConfig)
                {
                    if (item.ModuleConfigId > 0)
                    {
                        item.UpdatedBy = userId;
                        item.UpdatedDate = DateTime.Now;
                        item.ConfigValue = item.ConfigValue == "true" ?
                             Utility.ProperCase(item.ConfigValue) : item.ConfigValue == "false" ? null : item.ConfigValue;
                    }
                }
                if (newConfig.Count > 0)
                {
                    await _moduleConfigReposiitory.InsertAllAsync(newConfig);
                }
                if (updateConfig.Count > 0)
                {
                    await _moduleConfigReposiitory.UpdateAllAsync(updateConfig);
                }
                if (await _moduleConfigReposiitory.SaveAsync())
                {
                    executionStatus = new ExecutionStatus();
                    executionStatus.Status = true;
                    executionStatus.Msg = "Data has been saved successfully";
                }
                else
                {
                    executionStatus = new ExecutionStatus();
                    executionStatus.Status = false;
                    executionStatus.Msg = "Data has not been saved successfully";
                }
                return executionStatus;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "OrganizationConfig", "SaveModuleConfigAsync", "", 0, 0, 0);
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
            }
            return executionStatus = Utility.Invalid();
        }
    }
}
