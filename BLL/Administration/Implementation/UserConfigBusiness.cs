using AutoMapper;
using Microsoft.AspNetCore.Identity;
using DAL.DapperObject;
using Shared.Services;
using Dapper;
using System.Data;
using Shared.OtherModels.Response;
using BLL.Base.Interface;
using DAL.DapperObject.Interface;
using DAL.UnitOfWork.Control_Panel.Interface;
using Shared.Control_Panel.Domain;
using DAL.Repository.Control_Panel;
using Shared.Control_Panel.ViewModels;
using BLL.Administration.Interface;

namespace BLL.Administration.Implementation
{
    public class UserConfigBusiness : IUserConfigBusiness
    {
        private readonly IMapper _mapper;
        private readonly IDapperData _dapper;
        private IControlPanelUnitOfWork _controlPanelDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISysLogger _sysLogger;
        private readonly IClientDatabase _clientDatabase;


        // repository
        private readonly RoleAuthorizationRepository _roleAuthorizationRepository;
        private readonly RoleAuthTabRepository _roleAuthTabRepository;
        private readonly UserAuthorizationRepository _userAuthorizationRepository;
        private readonly UserAuthTabRepository _userAuthTabRepository;
        private readonly ApplicationUserRepository _applicationUserRepository;
        public UserConfigBusiness(IClientDatabase clientDatabase, IControlPanelUnitOfWork controlPanelDbContext, IMapper mapper, IDapperData dapperData, UserManager<ApplicationUser> userManager, ISysLogger sysLogger)
        {
            _controlPanelDbContext = controlPanelDbContext;
            _userManager = userManager;
            _mapper = mapper;
            _dapper = dapperData;
            _sysLogger = sysLogger;
            _clientDatabase = clientDatabase;
            _roleAuthorizationRepository = new RoleAuthorizationRepository(_controlPanelDbContext);
            _roleAuthTabRepository = new RoleAuthTabRepository(_controlPanelDbContext);
            _userAuthorizationRepository = new UserAuthorizationRepository(_controlPanelDbContext);
            _userAuthTabRepository = new UserAuthTabRepository(_controlPanelDbContext);
            _applicationUserRepository = new ApplicationUserRepository(_controlPanelDbContext);
        }

        public async Task<IEnumerable<AppUserMenu>> GetAppMenusAsync(string userId, string roleId, long companyId, long organizationId, string flag)
        {
            IEnumerable<AppUserMenu> list = new List<AppUserMenu>();
            try
            {
                var sp_name = string.Format(@"Exec sp_App_Menus @userId='{0}',@RoleId='{1}',@CompanyId={2},@OrganizationId='{3}',@Flag='{4}'", userId, roleId, companyId, organizationId, flag);
                sp_name = Utility.ParamChecker(sp_name);
                if (!string.IsNullOrEmpty(sp_name))
                {
                    var data = await _dapper.SqlQueryListAsync<AppSubmenus>(Database.ControlPanel, sp_name);
                    var dataList = (List<AppSubmenus>)data;
                    return await GetMenus(dataList);
                }
                else
                {
                    var ex = new Exception("Invalid Sql Query Execution");
                    await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "GetAppMenusAsync", "", 0, 0, 0);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "GetCompanyAuths", "", 0, 0, 0);
            }
            return list;
        }
        private async Task<List<AppUserMenu>> GetMenus(List<AppSubmenus> dataList)
        {
            List<AppUserMenu> appUserMenus = new List<AppUserMenu>();
            try
            {
                var appMainmenus = dataList.OrderBy(i => i.SequenceNo).Select(mm => new { mm.MMId, mm.MenuName, mm.MainmenuIconClass, mm.MainmenuIconColor }).Distinct();

                // Main Obj

                // data manipulation
                //
                foreach (var item in appMainmenus)
                {
                    AppUserMenu appUserMenu = new AppUserMenu()
                    {
                        MainmenuId = item.MMId,
                        MainmenuName = item.MenuName,
                        IconClass = item.MainmenuIconClass,
                        IconColor = item.MainmenuIconColor
                    };

                    // Submenu
                    List<AppUserSubmenu> appUserSubmenus = new List<AppUserSubmenu>();
                    var submenus = dataList.OrderBy(i => i.MenuSequence).Where(s => s.MMId == item.MMId && (s.IsActAsParent || s.ParentSubmenuId == 0)).Select(sub => new { sub.SubmenuId, sub.SubmenuName, sub.IconClass, sub.IconColor, sub.IsActAsParent, sub.Add, sub.Edit, sub.Delete, sub.Detail, sub.Approval, sub.Report, sub.HasTab, sub.Path, sub.Component, sub.Upload }).Distinct();

                    foreach (var submenu in submenus)
                    {
                        AppUserSubmenu appUserSubmenu = new AppUserSubmenu();
                        appUserSubmenu.SubmenuId = submenu.SubmenuId;
                        appUserSubmenu.SubmenuName = submenu.SubmenuName;
                        appUserSubmenu.Path = submenu.Path;
                        appUserSubmenu.Add = submenu.Add;
                        appUserSubmenu.Edit = submenu.Edit;
                        appUserSubmenu.Delete = submenu.Delete;
                        appUserSubmenu.Detail = submenu.Detail;
                        appUserSubmenu.Approval = submenu.Approval;
                        appUserSubmenu.Report = submenu.Report;
                        appUserSubmenu.Upload = submenu.Upload;
                        appUserSubmenu.HasTab = submenu.HasTab;
                        appUserSubmenu.IsActAsParent = submenu.IsActAsParent;
                        appUserSubmenu.IconColor = submenu.IconColor;
                        appUserSubmenu.IconClass = submenu.IconClass;
                        appUserSubmenu.Component = submenu.Component;
                        if (!submenu.IsActAsParent)
                        {
                            List<AppUserPageTab> appUserPageTabs = new List<AppUserPageTab>();
                            if (appUserSubmenu.HasTab)
                            {
                                appUserPageTabs = dataList.OrderBy(i => i.MenuSequence).Where(p => p.SubmenuId == submenu.SubmenuId).Select(pt => new AppUserPageTab
                                {
                                    TabId = pt.TabId,
                                    TabName = pt.TabName,
                                    TabIconClass = pt.TabIconClass,
                                    TabIconColor = pt.TabIconColor,
                                    TabAdd = pt.TabAdd,
                                    TabEdit = pt.TabEdit,
                                    TabDelete = pt.TabDelete,
                                    TabDetail = pt.TabDetail,
                                    TabApproval = pt.TabApproval,
                                    TabReport = pt.TabReport,
                                    TabComponent = pt.Component,
                                    TabUpload = pt.Upload
                                }).ToList();
                                appUserSubmenu.AppUserPageTabs = appUserPageTabs;
                            }// HasTab
                            else
                            {
                                appUserSubmenu.AppUserPageTabs = appUserPageTabs; // empty list
                                appUserSubmenu.AppUserSubSubmenus = new List<AppUserSubSubmenu>(); // empty list
                            }
                        }
                        else
                        { // Subsubmenus

                            var Subsubmenus = dataList.OrderBy(i => i.MenuSequence).Where(s => s.MMId == item.MMId && !s.IsActAsParent && s.ParentSubmenuId == submenu.SubmenuId).Select(sub => new { sub.SubmenuId, sub.SubmenuName, sub.IconClass, sub.IconColor, sub.IsActAsParent, sub.Add, sub.Edit, sub.Delete, sub.Detail, sub.Approval, sub.Report, sub.Upload, sub.HasTab, sub.Path, sub.Component }).Distinct();

                            if (Subsubmenus.AsList().Count > 0)
                            {
                                List<AppUserSubSubmenu> AppUserSubSubmenus = new List<AppUserSubSubmenu>();
                                foreach (var subsubmenu in Subsubmenus)
                                {
                                    // Subsubmenu
                                    AppUserSubSubmenu appUserSubSubmenu = new AppUserSubSubmenu()
                                    {
                                        SubSubmenuId = subsubmenu.SubmenuId,
                                        SubSubmenuName = subsubmenu.SubmenuName,
                                        Path = subsubmenu.Path,
                                        IconClass = subsubmenu.IconClass,
                                        IconColor = subsubmenu.IconColor,
                                        Add = subsubmenu.Add,
                                        Edit = subsubmenu.Edit,
                                        Delete = subsubmenu.Delete,
                                        Detail = subsubmenu.Detail,
                                        Approval = subsubmenu.Approval,
                                        Report = subsubmenu.Report,
                                        Upload = subsubmenu.Upload,
                                        HasTab = subsubmenu.HasTab,
                                        Component = subsubmenu.Component
                                    };

                                    List<AppUserPageTab> appUserPageTabs = new List<AppUserPageTab>();
                                    if (subsubmenu.HasTab)
                                    {
                                        appUserPageTabs = dataList.OrderBy(i => i.MenuSequence).Where(p => p.SubmenuId == subsubmenu.SubmenuId)
                                            .Select(pt => new AppUserPageTab
                                            {
                                                TabId = pt.TabId,
                                                TabName = pt.TabName,
                                                TabIconClass = pt.TabIconClass,
                                                TabIconColor = pt.TabIconColor,
                                                TabAdd = pt.TabAdd,
                                                TabEdit = pt.TabEdit,
                                                TabDelete = pt.TabDelete,
                                                TabDetail = pt.TabDetail,
                                                TabApproval = pt.TabApproval,
                                                TabReport = pt.TabReport,
                                                TabUpload = pt.TabUpload,
                                                TabComponent = pt.Component
                                            }).ToList();
                                        appUserSubSubmenu.AppUserPageTabs = appUserPageTabs;
                                    }
                                    appUserSubSubmenu.AppUserPageTabs = appUserPageTabs; // empty list
                                    AppUserSubSubmenus.Add(appUserSubSubmenu);
                                }
                                appUserSubmenu.AppUserSubSubmenus = AppUserSubSubmenus;
                                appUserSubmenu.AppUserPageTabs = new List<AppUserPageTab>();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        appUserSubmenus.Add(appUserSubmenu);
                    } // submenu

                    appUserMenu.AppUserSubmenus = appUserSubmenus;
                    appUserMenus.Add(appUserMenu);
                } // mainmenu

            }
            catch (Exception ex)
            {
                appUserMenus = new List<AppUserMenu>();
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "GetMenus", "", 0, 0, 0);
            }
            return appUserMenus;
        }
        private async Task<List<AppMainMenuForPermission>> GetMainMenuForPermissions(List<AppSubmenus> dataList)
        {
            List<AppMainMenuForPermission> appMainMenuForPermissions = new List<AppMainMenuForPermission>();
            try
            {
                var appMainmenus = dataList.Select(mm => new { mm.MMId, mm.MenuName, mm.ModuleId }).Distinct();

                foreach (var mm in appMainmenus)
                {
                    AppMainMenuForPermission appMainMenuForPermission = new AppMainMenuForPermission();
                    appMainMenuForPermission.MainmenuId = mm.MMId;
                    appMainMenuForPermission.MainmenuName = mm.MenuName;
                    appMainMenuForPermission.ModuleId = mm.ModuleId;
                    List<AppSubmenuForPermission> appSubmenuForPermissions = new List<AppSubmenuForPermission>();
                    var submenus = dataList.Where(s => s.MMId == mm.MMId && (s.IsActAsParent || s.ParentSubmenuId == 0)).Select(sub => new { sub.SubmenuId, sub.SubmenuName, sub.IsActAsParent, sub.HasTab }).Distinct();
                    foreach (var sub in submenus)
                    {
                        if (!sub.IsActAsParent && sub.HasTab)
                        {
                            var submenusTabs = dataList.Where(s => s.SubmenuId == sub.SubmenuId && s.TabId > 0).ToList();
                            var i = 0;
                            foreach (var item in submenusTabs)
                            {
                                i++;
                                AppSubmenuForPermission appSubmenuForPermission = new AppSubmenuForPermission()
                                {
                                    SubmenuId = item.SubmenuId,
                                    SubmenuName = i == 1 ? item.SubmenuName : "",
                                    SubSubmenuId = 0,
                                    SubSubmenuName = "",
                                    TabId = item.TabId,
                                    TabName = item.TabName,
                                    IsPageTabPermission = true,
                                    IsSubmenuPermission = false,
                                    IsAdd = item.TabAdd,
                                    IsEdit = item.TabEdit,
                                    IsDelete = item.TabDelete,
                                    IsDetail = item.TabDetail,
                                    IsApproval = item.TabApproval,
                                    IsReport = item.TabReport,
                                    IsUpload = item.TabUpload,
                                    IsAll = item.TabAdd && item.TabEdit && item.TabDelete && item.TabDetail && item.TabApproval && item.TabReport && item.TabUpload
                                };
                                appSubmenuForPermissions.Add(appSubmenuForPermission);
                            }
                        } // sub.IsActAsParent && sub.HasTab
                        else if (!sub.IsActAsParent && !sub.HasTab)
                        {
                            var submenu = dataList.FirstOrDefault(s => s.SubmenuId == sub.SubmenuId);
                            if (submenu != null)
                            {
                                AppSubmenuForPermission appSubmenuForPermission = new AppSubmenuForPermission()
                                {
                                    SubmenuId = submenu.SubmenuId,
                                    SubmenuName = submenu.SubmenuName,
                                    SubSubmenuId = 0,
                                    SubSubmenuName = "",
                                    TabId = 0,
                                    TabName = "",
                                    IsPageTabPermission = true,
                                    IsSubmenuPermission = false,
                                    IsAdd = submenu.Add,
                                    IsEdit = submenu.Edit,
                                    IsDelete = submenu.Delete,
                                    IsDetail = submenu.Detail,
                                    IsApproval = submenu.Approval,
                                    IsReport = submenu.Report,
                                    IsUpload = submenu.Upload,
                                    IsAll = submenu.Add && submenu.Edit && submenu.Delete && submenu.Detail && submenu.Approval && submenu.Report && submenu.Upload
                                };
                                appSubmenuForPermissions.Add(appSubmenuForPermission);
                            }
                        }// !sub.IsActAsParent && !sub.HasTab
                        else if (sub.IsActAsParent)
                        {
                            var childSubmenus = dataList.Where(s => s.MMId == mm.MMId && s.ParentSubmenuId ==
                            sub.SubmenuId && !s.IsActAsParent).Select(cs => new { cs.SubmenuId, cs.SubmenuName, cs.HasTab }).Distinct().ToList();
                            var i = 0;
                            foreach (var childSubmenu in childSubmenus)
                            {
                                i++;
                                if (!childSubmenu.HasTab)
                                {
                                    var cs = dataList.FirstOrDefault(s => s.SubmenuId == childSubmenu.SubmenuId && s.HasTab == false);

                                    AppSubmenuForPermission appSubmenuForPermission = new AppSubmenuForPermission()
                                    {
                                        SubmenuId = sub.SubmenuId,
                                        SubmenuName = i == 1 ? sub.SubmenuName : "",
                                        SubSubmenuId = childSubmenu.SubmenuId,
                                        SubSubmenuName = childSubmenu.SubmenuName,
                                        HasParentSubmenu = true,
                                        IsSubmenuPermission = true,
                                        IsPageTabPermission = false,
                                        TabId = 0,
                                        TabName = "",
                                        IsAdd = cs.Add,
                                        IsEdit = cs.Edit,
                                        IsDelete = cs.Delete,
                                        IsDetail = cs.Detail,
                                        IsApproval = cs.Approval,
                                        IsReport = cs.Report,
                                        IsUpload = cs.Upload,
                                        IsAll = cs.Add && cs.Edit && cs.Delete && cs.Detail && cs.Approval && cs.Report && cs.Upload
                                    };
                                    appSubmenuForPermissions.Add(appSubmenuForPermission);
                                } //!childSubmenu.HasTab
                                else
                                {
                                    var submenusTabs = dataList.Where(s => s.SubmenuId == childSubmenu.SubmenuId && s.ParentSubmenuId == sub.SubmenuId && s.TabId > 0).ToList();
                                    var j = 0;
                                    foreach (var submenusTab in submenusTabs)
                                    {
                                        j++;
                                        AppSubmenuForPermission appSubmenuForPermission = new AppSubmenuForPermission()
                                        {
                                            SubmenuId = sub.SubmenuId,
                                            SubmenuName = i == 1 ? sub.SubmenuName : "",
                                            SubSubmenuId = submenusTab.SubmenuId,
                                            SubSubmenuName = j == 1 ? submenusTab.SubmenuName : "",
                                            HasParentSubmenu = true,
                                            IsSubmenuPermission = false,
                                            IsPageTabPermission = true,
                                            TabId = submenusTab.TabId,
                                            TabName = submenusTab.TabName,
                                            IsAdd = submenusTab.TabAdd,
                                            IsEdit = submenusTab.TabEdit,
                                            IsDelete = submenusTab.TabDelete,
                                            IsDetail = submenusTab.TabDetail,
                                            IsApproval = submenusTab.TabApproval,
                                            IsReport = submenusTab.TabReport,
                                            IsUpload = submenusTab.TabUpload,
                                            IsAll = submenusTab.TabAdd && submenusTab.TabEdit && submenusTab.TabDelete && submenusTab.TabDetail && submenusTab.TabApproval && submenusTab.TabReport && submenusTab.TabUpload
                                        };
                                        appSubmenuForPermissions.Add(appSubmenuForPermission);
                                    }
                                }
                            }
                        }// sub.IsActAsParent
                    }
                    appMainMenuForPermission.AppSubmenuForPermissions = appSubmenuForPermissions;
                    appMainMenuForPermissions.Add(appMainMenuForPermission);
                }

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "GetMainMenuForPermissions", "", 0, 0, 0);
            }
            return appMainMenuForPermissions;
        }
        public async Task<IEnumerable<AppMainMenuForPermission>> GetAppMenusForPermissionAsync(string userId, string roleId, long companyId, long organizationId, string flag)
        {
            IEnumerable<AppMainMenuForPermission> data = new List<AppMainMenuForPermission>();
            try
            {
                var sp_name = "sp_App_Menus";
                var parameters = new DynamicParameters();
                parameters.Add("UserId", userId);
                parameters.Add("RoleId", roleId);
                parameters.Add("CompanyId", companyId);
                parameters.Add("OrganizationId", organizationId);
                parameters.Add("Flag", flag);
                if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                {
                    var submenu = await _dapper.SqlQueryListAsync<AppSubmenus>(Database.ControlPanel, sp_name, parameters, CommandType.StoredProcedure);
                    var dataList = (List<AppSubmenus>)submenu;
                    data = await GetMainMenuForPermissions(dataList);
                }
                else
                {
                    var ex = new Exception("Invalid Sql Query Execution");
                    await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "GetAppMenusForPermissionAsync", "", 0, 0, 0);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "GetAppMenusForPermissionAsync", "", 0, 0, 0);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveUserInfo(UserInfoData userInfo, long BranchId, long ComId, long OrgId, string UserId)
        {
            try
            {
                var validator = await UserValidator(userInfo.AppUserInfo);
                if (validator != null && !validator.Status)
                {
                    return validator;
                }
                string errorMsg = "";
                var appUser = _mapper.Map<ApplicationUser>(userInfo.AppUserInfo);
                if (appUser != null)
                {
                    if (appUser.Id.ToString() == null || appUser.Id.ToString() == "00000000-0000-0000-0000-000000000000")
                    {
                        appUser.CreatedBy = UserId;
                        appUser.CreatedDate = DateTime.Now;
                        appUser.IsDefaultPassword = true;
                        appUser.PasswordChangedCount = 0;
                        appUser.UserName = appUser.UserName.Trim();
                        userInfo.AppUserInfo.Password = Utility.RandomPassword();
                        appUser.DefaultCode = userInfo.AppUserInfo.Password;
                        appUser.PasswordExpiredDate = DateTime.Now.AddDays(30);

                        var result = await _userManager.CreateAsync(appUser, userInfo.AppUserInfo.Password);
                        if (result.Succeeded)
                        {
                            if (!appUser.IsRoleActive)
                            {
                                if (await AssignUserMenuAsync(userInfo.AppUserMenuPermission, appUser, BranchId, ComId, OrgId, UserId))
                                {
                                    return new ExecutionStatus()
                                    {
                                        Status = true,
                                        Msg = "Data has been has been saved Successfully"
                                    };
                                }
                            }
                            return new ExecutionStatus()
                            {
                                Status = true,
                                Msg = "Only User Information has been saved Successfully"
                            };
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                // Log or inspect the error messages
                                Console.WriteLine($"Error: {error.Description}");
                                errorMsg += error.Description;
                            }
                        }
                    }
                    else
                    {
                        var appUserInDb = await _userManager.FindByIdAsync(appUser.Id.ToString());
                        if (appUserInDb != null)
                        {
                            appUserInDb.RoleId = appUser.RoleId;
                            appUserInDb.FullName = appUser.FullName;
                            appUserInDb.Email = appUser.Email;
                            appUserInDb.UserName = appUser.UserName;
                            appUserInDb.PhoneNumber = appUser.PhoneNumber;
                            appUserInDb.Address = appUser.Address;
                            appUserInDb.IsActive = appUser.IsActive;
                            appUserInDb.IsRoleActive = appUser.IsRoleActive;
                            appUserInDb.UpdatedBy = UserId;
                            appUserInDb.UpdatedDate = DateTime.Now;
                            if ((await _userManager.UpdateAsync(appUserInDb)).Succeeded)
                            {
                                if (!appUser.IsRoleActive)
                                {
                                    if (await AssignUserMenuAsync(userInfo.AppUserMenuPermission, appUserInDb, BranchId, ComId, OrgId, UserId))
                                    {
                                        return new ExecutionStatus()
                                        {
                                            Status = true,
                                            Msg = "Data has been has been saved Successfully"
                                        };
                                    }
                                }
                                return new ExecutionStatus()
                                {
                                    Status = true,
                                    Msg = "Only User Information has been saved Successfully"
                                };
                            }
                        }
                    }
                }

                return new ExecutionStatus()
                {
                    Status = false,
                    Msg = "Something went wrong " + errorMsg
                };
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "SaveUserInfo", "", 0, 0, 0);
                return new ExecutionStatus()
                {
                    Status = false,
                    Msg = "Something went wrong"
                };
            }
        }
        public async Task<bool> AssignUserMenuAsync(List<AppMainMenuForPermission> appMainMenuForPermissions, ApplicationUser appUser, long BranchId, long ComId, long OrgId, string UserId)
        {
            try
            {
                List<UserAuthorization> userAuthorizationList = new List<UserAuthorization>();
                List<UserAuthTab> userAuthTabList = new List<UserAuthTab>();
                foreach (var mainmenu in appMainMenuForPermissions)
                {
                    foreach (var sub in mainmenu.AppSubmenuForPermissions)
                    {
                        if (sub.IsAdd || sub.IsDelete || sub.IsEdit || sub.IsDetail
                            || sub.IsApproval || sub.IsReport)
                        {
                            var submenuId = sub.HasParentSubmenu ? sub.SubSubmenuId : sub.SubmenuId;
                            if (sub.IsPageTabPermission)
                            {
                                UserAuthTab userAuthTab = new UserAuthTab()
                                {
                                    UserId = appUser.Id.ToString(),
                                    SubmenuId = submenuId,
                                    TabId = sub.TabId,
                                    TabName = sub.TabName,
                                    Add = sub.IsAdd,
                                    Edit = sub.IsEdit,
                                    Delete = sub.IsDelete,
                                    Detail = sub.IsDetail,
                                    Approval = sub.IsApproval,
                                    Report = sub.IsReport,
                                    Upload = sub.IsUpload,
                                    CreatedBy = UserId,
                                    CreatedDate = DateTime.Now,
                                    OrganizationId = appUser.OrganizationId,
                                    CompanyId = appUser.CompanyId,
                                    BranchId = appUser.BranchId,
                                };
                                userAuthTabList.Add(userAuthTab);
                            }

                            if (userAuthorizationList.Count() == 0 || userAuthorizationList.LastOrDefault(s => s.SubmenuId == submenuId) == null)
                            {
                                UserAuthorization userAuthorization = new UserAuthorization()
                                {
                                    UserId = appUser.Id.ToString(),
                                    RoleId = appUser.RoleId,
                                    ModuleId = mainmenu.ModuleId,
                                    MainmenuId = mainmenu.MainmenuId,
                                    SubmenuId = submenuId,
                                    ParentSubmenuId = sub.HasParentSubmenu ? sub.SubmenuId : 0,
                                    IsPageTabPermission = sub.IsPageTabPermission,
                                    IsSubmenuPermission = sub.IsSubmenuPermission,
                                    HasTab = sub.IsPageTabPermission,
                                    Add = sub.IsAdd,
                                    Edit = sub.IsEdit,
                                    Delete = sub.IsDelete,
                                    Detail = sub.IsDetail,
                                    Approval = sub.IsApproval,
                                    Report = sub.IsReport,
                                    Upload = sub.IsUpload,
                                    CreatedBy = UserId,
                                    CreatedDate = DateTime.Now,
                                    OrganizationId = appUser.OrganizationId,
                                    CompanyId = appUser.CompanyId,
                                    BranchId = appUser.BranchId,
                                    DivisionId = appUser.DivisionId,
                                };
                                userAuthorizationList.Add(userAuthorization);
                            }
                        }
                    }
                }
                if (appUser.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                {

                    await _userAuthTabRepository.DeleteAllAsync(user => user.UserId == appUser.Id.ToString() && user.OrganizationId == appUser.OrganizationId && user.CompanyId == appUser.CompanyId);

                    await _userAuthorizationRepository.DeleteAllAsync(user => user.UserId == appUser.Id.ToString() && user.OrganizationId == appUser.OrganizationId && user.CompanyId == appUser.CompanyId);

                    foreach (var item in userAuthorizationList)
                    {
                        if (item.HasTab)
                        {
                            item.UserAuthTabs = userAuthTabList.Where(s => s.SubmenuId == item.SubmenuId).ToList();
                        }
                    }
                    await _userAuthorizationRepository.InsertAllAsync(userAuthorizationList);
                    return await _userAuthorizationRepository.SaveAsync();
                }
                return false;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "AssignUserMenuAsync", "", OrgId, ComId, 0);
                return false;
            }
        }
        public async Task<IEnumerable<ApplicationUserViewModel>> GetApplicationUsers(string fullName, string userName, string roleId, string address, string email, string phoneNumber, long branchId, long divisionId, long companyId, long organizationId, string fromDate, string toDate)
        {
            IEnumerable<ApplicationUserViewModel> data = new List<ApplicationUserViewModel>();
            try
            {
                var query = $@"Select Convert(nvarchar(50),u.Id) as 'Id',
	u.FullName,u.IsActive,u.IsRoleActive,u.RoleId,SUBSTRING(r.[Name],1,(CASE WHEN CHARINDEX('#Org',r.[Name]) > 0 THEN CHARINDEX('#Org',r.[Name])-1 ELSE LEN([NAME]) END)) as 'RoleName',u.[Address],u.UserName,u.Email,u.PhoneNumber,
	u.OrganizationId,org.OrganizationName,u.CompanyId,com.CompanyName,u.DivisionId,div.DivisionName,u.BranchId,br.BranchName,
	org.OrganizationName,u.CreatedDate,u.DefaultCode,EmployeeId=ISNULL(u.EmployeeId,0),u.FullName,u.PhoneNumber
	From AspNetUsers u
	Inner Join AspNetRoles r on u.RoleId = Convert(nvarchar(50),r.Id)
	--Left Join [HRMS].dbo.HR_EmployeeInformation emp on Convert(bigint,u.EmployeeId) = emp.EmployeeId
	Left Join tblOrganizations org on u.OrganizationId = org.OrganizationId
	Left Join tblCompanies com on u.CompanyId = com.CompanyId
	Left Join tblDivisions div on u.DivisionId = div.DivisionId
	Left Join tblBranches br on u.BranchId = br.BranchId
	Where 1=1
	AND (@FullName IS NULL OR @FullName='' OR u.FullName LIKE '%'+@FullName+'%')
	AND (@UserName IS NULL OR @UserName='' OR u.UserName LIKE '%'+@UserName+'%')
	AND (@Email IS NULL OR @Email='' OR u.Email LIKE '%'+@Email+'%')
	AND (@Address IS NULL OR @Address='' OR u.[Address] LIKE '%'+@Address+'%')
	AND (@PhoneNumber IS NULL OR @PhoneNumber='' OR u.PhoneNumber LIKE '%'+@PhoneNumber+'%')
	AND (@BranchId IS NULL OR @BranchId=0 OR u.BranchId = @BranchId)
	AND (u.DivisionId IS NULL OR @DivisionId=0 OR u.DivisionId = @DivisionId)
	AND (@CompanyId IS NULL OR @CompanyId=0 OR u.CompanyId = @CompanyId)
	AND (@OrganizationId IS NULL OR @OrganizationId=0 OR u.OrganizationId = @OrganizationId)
	AND(
		(@CreatedDateFrom IS NULL AND @CreatedDateTo IS NULL)
		OR
		(@CreatedDateFrom ='' AND @CreatedDateTo ='')
		OR
		(@CreatedDateFrom != '' AND @CreatedDateTo !='' 
		AND (u.CreatedDate between Convert(date,@CreatedDateFrom) and  Convert(date,@CreatedDateTo)))
		OR
		(@CreatedDateFrom != ''
		AND (u.CreatedDate = Convert(date,@CreatedDateFrom)))
		OR
		(@CreatedDateTo != ''
		AND (u.CreatedDate = Convert(date,@CreatedDateTo)))
	)";
                data = await _dapper.SqlQueryListAsync<ApplicationUserViewModel>(Database.ControlPanel, query, new
                {
                    FullName = fullName,
                    UserName = userName,
                    Email = email,
                    Address = address,
                    PhoneNumber = phoneNumber,
                    BranchId = branchId,
                    DivisionId = divisionId,
                    CompanyId = companyId,
                    OrganizationId = organizationId,
                    CreatedDateFrom = fromDate,
                    CreatedDateTo = toDate
                });

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "GetApplicationUsers", "", organizationId, companyId, 0);
            }
            return data;
        }
        public async Task<ExecutionStatus> UserValidator(ApplicationUserViewModel AppUserInfo)
        {
            ExecutionStatus executionStatus = null;
            try
            {

                var duplicateUsername = await _applicationUserRepository.GetSingleAsync(u => u.Id.ToString() != AppUserInfo.Id && u.UserName == AppUserInfo.UserName) != null;
                var duplicateEmployee = await _applicationUserRepository.GetSingleAsync(u => u.Id.ToString() != AppUserInfo.Id && u.EmployeeId == AppUserInfo.EmployeeId) != null;
                var duplicateUserEmail = await _applicationUserRepository.GetSingleAsync(u => u.Id.ToString() != AppUserInfo.Id && u.Email == AppUserInfo.Email) != null;
                var duplicateUserPhone = await _applicationUserRepository.GetSingleAsync(u => u.Id.ToString() != AppUserInfo.Id && u.PhoneNumber == AppUserInfo.PhoneNumber) != null;

                if (duplicateUsername || duplicateUserEmail || duplicateUserPhone)
                {
                    executionStatus = new ExecutionStatus();
                    executionStatus.Status = false;
                    executionStatus.Msg = "Validation Error";
                    executionStatus.Errors = new Dictionary<string, string>();
                    if (duplicateUsername)
                    {
                        executionStatus.Errors.Add("duplicateUsername", "Duplicate Username");
                    }
                    if (duplicateEmployee)
                    {
                        executionStatus.Errors.Add("duplicateEmployee", "Duplicate Employee");
                    }
                    if (duplicateUserEmail)
                    {
                        executionStatus.Errors.Add("duplicateUserEmail", "Duplicate Email");
                    }
                    if (duplicateUserPhone)
                    {
                        executionStatus.Errors.Add("duplicatePhone", "Duplicate Phonenumber");
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "UserValidator", "", 0, 0, 0);
                executionStatus = new ExecutionStatus()
                {
                    Status = false,
                    Msg = "Something went wrong",
                    Errors = null
                };
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveRoleAuthAsync(List<AppMainMenuForPermission> appMainMenus, string roleId, long branchId, long companyId, long organizationId, string userId)
        {
            try
            {
                if (await AssignRoleMenuAsync(appMainMenus, roleId, branchId, companyId, organizationId, userId))
                {
                    return new ExecutionStatus()
                    {
                        Status = true,
                        Msg = "Data has been saved Successfully"
                    };
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "UserValidator", "", 0, 0, 0);
            }
            return new ExecutionStatus()
            {
                Status = true,
                Msg = "Data has not been saved Successfully"
            };
        }
        public async Task<bool> AssignRoleMenuAsync(List<AppMainMenuForPermission> appMainMenus, string roleId, long branchId, long companyId, long organizationId, string userId)
        {
            try
            {
                List<RoleAuthorization> roleAuthorizationList = new List<RoleAuthorization>();
                List<RoleAuthTab> roleAuthTabList = new List<RoleAuthTab>();
                foreach (var mainmenu in appMainMenus)
                {
                    foreach (var sub in mainmenu.AppSubmenuForPermissions)
                    {
                        if (sub.IsAdd || sub.IsDelete || sub.IsEdit || sub.IsDetail
                            || sub.IsApproval || sub.IsReport)
                        {
                            var submenuId = sub.HasParentSubmenu ? sub.SubSubmenuId : sub.SubmenuId;
                            if (sub.IsPageTabPermission)
                            {
                                RoleAuthTab roleAuthTab = new RoleAuthTab()
                                {
                                    RoleId = roleId.ToString(),
                                    SubmenuId = submenuId,
                                    TabId = sub.TabId,
                                    TabName = sub.TabName,
                                    Add = sub.IsAdd,
                                    Edit = sub.IsEdit,
                                    Delete = sub.IsDelete,
                                    Detail = sub.IsDetail,
                                    Approval = sub.IsApproval,
                                    Report = sub.IsReport,
                                    Upload = sub.IsUpload,
                                    CreatedBy = userId,
                                    CreatedDate = DateTime.Now,
                                    OrganizationId = organizationId,
                                    CompanyId = companyId,
                                    BranchId = 0,
                                };
                                roleAuthTabList.Add(roleAuthTab);
                            }

                            if (roleAuthorizationList.Count() == 0 || roleAuthorizationList.LastOrDefault(s => s.SubmenuId == submenuId) == null)
                            {
                                RoleAuthorization roleAuthorization = new RoleAuthorization()
                                {
                                    RoleId = roleId.ToString(),
                                    ModuleId = mainmenu.ModuleId,
                                    MainmenuId = mainmenu.MainmenuId,
                                    SubmenuId = submenuId,
                                    ParentSubmenuId = sub.HasParentSubmenu ? sub.SubmenuId : 0,
                                    IsPageTabPermission = sub.IsPageTabPermission,
                                    IsSubmenuPermission = sub.IsSubmenuPermission,
                                    HasTab = sub.IsPageTabPermission,
                                    Add = sub.IsAdd,
                                    Edit = sub.IsEdit,
                                    Delete = sub.IsDelete,
                                    Detail = sub.IsDetail,
                                    Approval = sub.IsApproval,
                                    Report = sub.IsReport,
                                    Upload = sub.IsUpload,
                                    CreatedBy = userId,
                                    CreatedDate = DateTime.Now,
                                    OrganizationId = organizationId,
                                    CompanyId = companyId,
                                    BranchId = 0

                                };
                                roleAuthorizationList.Add(roleAuthorization);
                            }
                        }
                    }
                }

                foreach (var item in roleAuthorizationList)
                {
                    if (item.HasTab)
                    {
                        item.RoleAuthTabs = roleAuthTabList.Where(s => s.SubmenuId == item.SubmenuId).ToList();
                    }
                }
                await _roleAuthTabRepository.DeleteAllAsync(i => i.RoleId == roleId && i.CompanyId == companyId && i.OrganizationId == organizationId);
                await _roleAuthorizationRepository.DeleteAllAsync(i => i.RoleId == roleId && i.CompanyId == companyId && i.OrganizationId == organizationId);
                await _roleAuthorizationRepository.InsertAllAsync(roleAuthorizationList);
                return await _roleAuthorizationRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "AssignRoleMenuAsync", "", organizationId, companyId, branchId);
            }
            return false;
        }
        public async Task<IEnumerable<AppUserMenu>> GetAppUserMenusAsync(string username)
        {
            IEnumerable<AppUserMenu> menuData = new List<AppUserMenu>();
            try
            {
                var sp_name = "sp_AppUserMenu";
                var parameters = new DynamicParameters();
                parameters.Add("Username", username);
                if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                {
                    var data = await _dapper.SqlQueryListAsync<AppSubmenus>(Database.ControlPanel, sp_name, parameters, CommandType.StoredProcedure);
                    var dataList = (List<AppSubmenus>)data;
                    menuData = await GetMenus(dataList);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "GetAppUserMenusAsync", "", 0, 0, 0);
            }
            return menuData;
        }
        public async Task<UsermenuWithComponent> GetAppUsermenuWithComponentAsync(string username)
        {
            UsermenuWithComponent usermenuWithComponent = new UsermenuWithComponent();
            IEnumerable<AppUserMenu> menuData = new List<AppUserMenu>();
            try
            {
                var sp_name = "sp_AppUserMenu";
                var parameters = new DynamicParameters();
                parameters.Add("Username", username);
                if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                {
                    var data = await _dapper.SqlQueryListAsync<AppSubmenus>(Database.ControlPanel, sp_name, parameters, CommandType.StoredProcedure);
                    var dataList = (List<AppSubmenus>)data;
                    menuData = await GetMenus(dataList);

                    usermenuWithComponent.AppUserMenus = menuData;
                    var components = dataList.Where(s => s.Component != "N/A" && s.Component != "" && s.Component != null).Select(sub => new Component
                    {
                        SubmenuId = sub.SubmenuId,
                        Submenu = sub.SubmenuName,
                        Name = sub.Component,
                        Add = sub.Add,
                        Edit = sub.Edit,
                        Detail = sub.Detail,
                        Delete = sub.Delete,
                        Approval = sub.Approval,
                        Report = sub.Report,
                        Upload = sub.Upload
                    }).Distinct();
                    var submenus = dataList.Select(s => s.SubmenuId).Distinct().ToList();
                    foreach (var item in components)
                    {
                        item.TabsOfComponents = new List<TabsOfComponent>();
                        var tabsInSubmenu = dataList.Where(s => s.SubmenuId == Convert.ToInt64(item.SubmenuId)).ToList();
                        if (tabsInSubmenu.Count > 0)
                        {
                            foreach (var tab in tabsInSubmenu)
                            {
                                TabsOfComponent tabsOfComponent = new TabsOfComponent();
                                tabsOfComponent.TabName = tab.TabName;
                                tabsOfComponent.TabAdd = tab.Add;
                                tabsOfComponent.TabEdit = tab.Edit;
                                tabsOfComponent.TabDetail = tab.Detail;
                                tabsOfComponent.TabDelete = tab.Delete;
                                tabsOfComponent.TabApproval = tab.Approval;
                                tabsOfComponent.TabReport = tab.Report;
                                tabsOfComponent.TabUpload = tab.Upload;
                                item.TabsOfComponents.Add(tabsOfComponent);
                            }
                        }
                    }

                    usermenuWithComponent.Components = components.ToList();
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "GetAppUsermenuWithComponentAsync", "", 0, 0, 0);
            }
            return usermenuWithComponent;
        }
        public async Task<ExecutionStatus> ChangeUserDefaultPasswordAsync(ChangePasswordViewModel model)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var userInfo = await _userManager.FindByIdAsync(model.UserId);
                if (userInfo != null)
                {
                    model.CurrentPassword = model.CurrentPassword;
                    model.NewPassword = model.NewPassword;
                    model.ConfirmPassword = model.ConfirmPassword;

                    var currentPasswordHash = userInfo.PasswordHash;
                    var securityStamp = userInfo.DefaultSecurityStamp;
                    var changePassword = await _userManager.ChangePasswordAsync(userInfo, model.CurrentPassword, model.NewPassword);
                    if (changePassword.Succeeded)
                    {
                        userInfo.DefaultPasswordHash = currentPasswordHash;
                        userInfo.IsDefaultPassword = false;
                        userInfo.PasswordChangedCount = userInfo.PasswordChangedCount + 1;
                        userInfo.DefaultSecurityStamp = securityStamp;
                        userInfo.PasswordExpiredDate = DateTime.Now.AddDays(30);
                        var updateUserInfo = await _userManager.UpdateAsync(userInfo);
                        executionStatus = new ExecutionStatus();
                        executionStatus.Status = true;
                        executionStatus.Msg = "Password changed successfully";
                    }
                    else
                    {
                        executionStatus = new ExecutionStatus();
                        executionStatus.Status = false;
                        executionStatus.Msg = "Password change failed";
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "ChangeUserDefaultPasswordAsync", "", 0, 0, 0);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ChangeUserPasswordAsync(ChangePasswordViewModel model)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var userInfo = await _userManager.FindByIdAsync(model.UserId);
                if (userInfo != null)
                {
                    model.CurrentPassword = model.CurrentPassword;
                    model.NewPassword = model.NewPassword;
                    model.ConfirmPassword = model.ConfirmPassword;
                    var changePassword = await _userManager.ChangePasswordAsync(userInfo, model.CurrentPassword, model.NewPassword);
                    if (changePassword.Succeeded)
                    {
                        userInfo.PasswordChangedCount = userInfo.PasswordChangedCount + 1;
                        userInfo.PasswordExpiredDate = DateTime.Now.AddDays(30);
                        var updateUserInfo = await _userManager.UpdateAsync(userInfo);
                        executionStatus = new ExecutionStatus();
                        executionStatus.Status = true;
                        executionStatus.Msg = "Password changed successfully";
                    }
                    else
                    {
                        executionStatus = new ExecutionStatus();
                        executionStatus.Status = false;
                        executionStatus.Msg = "Password change failed";
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "ChangeUserPasswordAsync", "", 0, 0, 0);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> CheckUserAsync(string userId, string currentPassword)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var userInfo = await _userManager.FindByIdAsync(userId);
                if (userInfo != null)
                {
                    var checkUser = await _userManager.CheckPasswordAsync(userInfo, currentPassword);
                    executionStatus = new ExecutionStatus();
                    if (checkUser)
                    {
                        executionStatus.Status = true;
                        executionStatus.Msg = "Valid Password";
                    }
                    else
                    {
                        executionStatus.Status = true;
                        executionStatus.Msg = "Invalid Password";
                    }
                }
                else
                {
                    executionStatus = new ExecutionStatus();
                    executionStatus.Status = false;
                    executionStatus.Msg = "Invalid User";
                }
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "CheckUserAsync", "", 0, 0, 0);
            }
            return executionStatus;
        }
        public async Task<Component> CheckUserprivilegeAsync(Usercomponentprivilege usercomponentprivilege)
        {
            Component component = new Component();
            try
            {
                var sp_name = "sp_UserComponentPrivilege";
                var parameters = new DynamicParameters();
                parameters.Add("UserId", usercomponentprivilege.UserId);
                parameters.Add("Component", usercomponentprivilege.Component);
                parameters.Add("CompanyId", usercomponentprivilege.CompanyId);
                parameters.Add("OrganizationId", usercomponentprivilege.OrganizationId);
                component = await _dapper.SqlQueryFirstAsync<Component>(Database.ControlPanel, sp_name, parameters, CommandType.StoredProcedure);
                if (component != null)
                {
                    if (component.SubmenuId > 0)
                    {
                        if (!Utility.IsNullEmptyOrWhiteSpace(component.Tabs))
                        {
                            component.TabsOfComponents = Utility.JsonToObject<IEnumerable<TabsOfComponent>>(component.Tabs).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserConfigBusiness", "CheckUserprivilegeAsync", "", 0, 0, 0);
            }
            return component;
        }
        public async Task GetUserInfosAsync(long companyId, long organizationId)
        {
            IEnumerable<ApplicationUserViewModel> list = new List<ApplicationUserViewModel>();
            List<ExecutionStatus> listOfExecution = new List<ExecutionStatus>();
            try
            {
                var query = $@"SELECT EMP.FullName,EMP.EmployeeId,EMP.EmployeeCode,IsActive=1,IsRoleActive=1,EMP.OrganizationId,EMP.CompanyId,EMP.BranchId,
                Email=EMP.OfficeEmail,Username=EMP.OfficeEmail,
                PhoneNumber=EMP.OfficeEmail
                FROM HR_EmployeeInformation EMP
                INNER JOIN HR_EmployeeHierarchy EH ON EMP.EmployeeId= EH.EmployeeId
                INNER JOIN HR_EmployeeDetail ED ON EMP.EmployeeId = ED.EmployeeId
                Where 1=1
                AND EMP.CompanyId=@CompanyId AND EMP.OrganizationId=@OrganizationId
                AND EH.EmployeeId NOT IN (Select EmployeeId FROM ControlPanel.dbo.AspNetUsers Where CompanyId=@CompanyId)";

                var database = _clientDatabase.GetDatabaseName(organizationId);

                list = await _dapper.SqlQueryListAsync<ApplicationUserViewModel>(database, query, new { CompanyId = companyId, OrganizationId = organizationId });

                foreach (var item in list)
                {
                    ExecutionStatus executionStatus = await SaveUserInfo(new UserInfoData()
                    {
                        AppUserInfo = item,
                        AppUserMenuPermission = new List<AppMainMenuForPermission>()
                    }, item.BranchId, item.CompanyId, item.OrganizationId, "sysadmin");

                    executionStatus.Msg = "EmployeeId=" + item.EmployeeId.ToString() + ",Employee Code=" + item.EmployeeCode + ", Employee Name=" + item.FullName;

                    if (executionStatus.Status == false)
                    {
                        listOfExecution.Add(executionStatus);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<ApplicationUser> GetUserInfoByIdAysc(string id)
        {
            ApplicationUser user = null;
            try
            {
                string query = $@"SELECT * FROM AspNetUsers Where Id=@Id";
                user = await _dapper.SqlQueryFirstAsync<ApplicationUser>(Database.ControlPanel, query, new { Id = id });
            }
            catch (Exception ex)
            {
            }
            return user;

        }
    }
}
