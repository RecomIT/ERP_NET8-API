using AutoMapper;
using BLL.Base.Interface;
using DAL.DapperObject;
using Shared.OtherModels.DataService;
using DAL.Repository.Control_Panel;
using DAL.UnitOfWork.Control_Panel.Interface;
using Shared.Control_Panel.ViewModels;
using Shared.Control_Panel.Domain;
using BLL.Administration.Interface;

namespace BLL.Administration.Implementation
{
    public class AppConfigBusiness : IAppConfigBusiness
    {
        private readonly IMapper _mapper;
        private readonly ISysLogger _sysLogger;
        private readonly ModuleRepository _moduleRepository;
        private readonly SubMenuRepository _subMenuRepository;
        private readonly PageTabRepository _pageTabRepository;
        private readonly MainMenuRepository _mainMenuRepository;
        private readonly ApplicationRepository _applicationRepository;
        private readonly IControlPanelUnitOfWork _controlPanelDbContext;
        public AppConfigBusiness(IControlPanelUnitOfWork controlPanelDbContext, IMapper mapper, ISysLogger sysLogger)
        {
            _controlPanelDbContext = controlPanelDbContext;
            _mapper = mapper;
            _sysLogger = sysLogger;
            _applicationRepository = new ApplicationRepository(_controlPanelDbContext);
            _moduleRepository = new ModuleRepository(_controlPanelDbContext);
            _mainMenuRepository = new MainMenuRepository(_controlPanelDbContext);
            _subMenuRepository = new SubMenuRepository(_controlPanelDbContext);
            _pageTabRepository = new PageTabRepository(_controlPanelDbContext);
        }
        // Application
        public async Task<IEnumerable<ApplicationViewModel>> GetApplicationsAsync(string ApplicationName, string ApplicationType, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            IEnumerable<ApplicationViewModel> list = new List<ApplicationViewModel>();
            try
            {
                var appsInDomain = (await _applicationRepository.GetAllAsync(app =>
                   (string.IsNullOrEmpty(ApplicationName) || app.ApplicationName == ApplicationName.ToLower()) &&
                   (string.IsNullOrEmpty(ApplicationType) || app.ApplicationType == ApplicationType.ToLower()) &&
                   (!IsActive.HasValue || app.IsActive == IsActive)
                )).ToList();
                list = _mapper.Map<List<ApplicationViewModel>>(appsInDomain);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "AppConfigBusiness", "GetApplicationsAsync", null);
            }
            return list;
        }
        public async Task<bool> SaveApplicationAsync(Application application, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                if (application.ApplicationId == 0)
                {
                    application.CreatedBy = UserId;
                    application.CreatedDate = DateTime.Now;
                    await _applicationRepository.InsertAsync(application);
                }
                else
                {
                    var applicationInDb = await _applicationRepository.GetSingleAsync(app => app.ApplicationId == application.ApplicationId);
                    if (applicationInDb != null)
                    {

                        applicationInDb.ApplicationName = application.ApplicationName;
                        applicationInDb.ApplicationType = application.ApplicationType;
                        applicationInDb.IsActive = application.IsActive;
                        applicationInDb.UpdatedBy = UserId;
                        applicationInDb.UpdatedDate = DateTime.Now;
                        await _applicationRepository.UpdateAsync(applicationInDb);
                    }
                }
                return await _applicationRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "AppConfigBusiness", "SaveApplicationAsync", null);
            }
            return false;
        }
        public async Task<bool> DeleteApplicationAsync(long ApplicationId, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                await _applicationRepository.DeleteSingleAsync(app => app.ApplicationId == ApplicationId);
                var status = await _applicationRepository.SaveAsync();
                return status;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "", "", null);
                return false;
            }
        }
        // Module
        public async Task<IEnumerable<ModuleViewModel>> GetModuleAsync(string ModuleName, long? ApplicationId, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            IEnumerable<ModuleViewModel> list = new List<ModuleViewModel>();
            try
            {
                var moduleInDb = await _moduleRepository.GetAllAsync(module =>
                (string.IsNullOrEmpty(ModuleName) || module.ModuleName == ModuleName.ToLower()) &&
           (!ApplicationId.HasValue || ApplicationId.Value == 0 || module.ApplicationId == ApplicationId) &&
           (!IsActive.HasValue || module.IsActive == IsActive));
                list = _mapper.Map<IEnumerable<ModuleViewModel>>(moduleInDb);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "", "", null);
            }
            return list;
        }
        public async Task<bool> SaveModuleAsync(Module module, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                if (module.ModuleId == 0)
                {
                    module.CreatedBy = UserId;
                    module.CreatedDate = DateTime.Now;
                    await _moduleRepository.InsertAsync(module);
                }
                else
                {
                    var moduleInDb = await _moduleRepository.GetSingleAsync(m => m.ModuleId == module.ModuleId);
                    if (moduleInDb != null)
                    {
                        moduleInDb.ModuleName = module.ModuleName;
                        moduleInDb.ApplicationId = module.ApplicationId;
                        moduleInDb.IsActive = module.IsActive;
                        moduleInDb.UpdatedBy = UserId;
                        moduleInDb.UpdatedDate = DateTime.Now;
                        await _moduleRepository.UpdateAsync(moduleInDb);
                    }
                }
                return await _moduleRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "", "SaveModuleAsync", null);
                return false;
            }
        }
        public async Task<bool> DeleteModuleAsync(long ModuleId, long OrgId, long ComId, long BranchId, string UserId)
        {
            // _controlPanelDbContext.Database.()
            try
            {
                await _moduleRepository.DeleteSingleAsync(module => module.ModuleId == ModuleId);
                var status = await _moduleRepository.SaveAsync();
                return status;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "", "DeleteModuleAsync", null);
                return false;
            }
        }
        public async Task<IEnumerable<Select2Dropdown>> GetModuleExtensionAsync(long applicationId)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                data = (await _moduleRepository.GetAllAsync(m => applicationId <= 0 || m.ApplicationId == applicationId)).Select(m => new Select2Dropdown
                {
                    Id = m.ModuleId.ToString(),
                    Text = m.ModuleName
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "", "GetModuleExtensionAsync", null);
            }
            return data;
        }

        // Mainmenu
        public async Task<IEnumerable<MainMenuViewModel>> GetMainMenusAsync(string MainMenuName, long? ModuleId, long? ApplicationId, bool? IsActive, long OrgId, long ComId, long BranchId)
        {
            IEnumerable<MainMenuViewModel> list = new List<MainMenuViewModel>();
            try
            {
                var mainInDb = await _mainMenuRepository.GetAllAsync(
                        mm => (string.IsNullOrEmpty(MainMenuName) || mm.MenuName == MainMenuName.ToLower()) &&
                   (!ApplicationId.HasValue || ApplicationId.Value == 0 || mm.ApplicationId == ApplicationId) &&
                   (!IsActive.HasValue || mm.IsActive == IsActive)
                    );

                list = mainInDb.ToList().Select(mm => new MainMenuViewModel()
                {
                    MMId = mm.MMId,
                    MId = mm.MId,
                    MenuName = mm.MenuName,
                    ShortName = mm.ShortName,
                    ModuleName = _moduleRepository.GetSingle(m => m.ModuleId == mm.MId).ModuleName,
                    ApplicationId = mm.ApplicationId,
                    ApplicationName = _applicationRepository.GetSingle(app => app.ApplicationId == mm.ApplicationId).ApplicationName,
                    IconClass = mm.IconClass,
                    IconColor = mm.IconColor,
                    CreatedBy = mm.CreatedBy,
                    CreatedDate = mm.CreatedDate,
                    UpdatedBy = mm.UpdatedBy,
                    UpdatedDate = mm.UpdatedDate,
                    IsActive = mm.IsActive,
                    SequenceNo = mm.SequenceNo ?? 0
                }).OrderBy(item => item.SequenceNo).ToList();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "", "GetMainMenusAsync", null);
            }
            return list;
        }
        public async Task<bool> SaveMainmenuAsync(MainMenu mainMenu, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                var appId = (await _moduleRepository.GetSingleAsync(mm => mm.ModuleId == mainMenu.MId)).ApplicationId;
                if (mainMenu.MMId == 0)
                {
                    mainMenu.ApplicationId = appId;
                    mainMenu.CreatedBy = UserId;
                    mainMenu.CreatedDate = DateTime.Now;
                    await _mainMenuRepository.InsertAsync(mainMenu);
                }
                else
                {
                    var menuInDb = await _mainMenuRepository.GetSingleAsync(m => m.MMId == mainMenu.MMId);
                    if (menuInDb != null)
                    {
                        menuInDb.ApplicationId = mainMenu.ApplicationId;
                        menuInDb.MenuName = mainMenu.MenuName;
                        menuInDb.IconClass = mainMenu.IconClass;
                        menuInDb.IconColor = mainMenu.IconColor;
                        menuInDb.MId = mainMenu.MId;
                        menuInDb.IsActive = mainMenu.IsActive;
                        menuInDb.UpdatedBy = UserId;
                        menuInDb.UpdatedDate = DateTime.Now;
                        await _mainMenuRepository.UpdateAsync(menuInDb);
                    }
                }
                return await _mainMenuRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "", "SaveMainmenuAsync", null);
                return false;
            }
        }
        public async Task<bool> DeleteMainmenuAsync(long MainmenuId, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                await _mainMenuRepository.DeleteSingleAsync(m => m.MMId == MainmenuId);
                var status = await _mainMenuRepository.SaveAsync();
                return status;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "", "DeleteMainmenuAsync", null);
                return false;
            }
        }

        // Submenu
        public async Task<IEnumerable<SubmenuViewModel>> GetSubmenusAsync(string SubmenuName, long? MainmenuId, long? ParentSubmenuId, long? ModuleId, long? ApplicationId)
        {
            IEnumerable<SubmenuViewModel> submenus = new List<SubmenuViewModel>();
            try
            {
                var submenuInDb = await _subMenuRepository.GetAllAsync(sub =>
                (string.IsNullOrEmpty(SubmenuName) || sub.SubmenuName.Contains(SubmenuName)) &&
                (!MainmenuId.HasValue || MainmenuId.Value == 0 || sub.MMId == MainmenuId) &&
                (!ParentSubmenuId.HasValue || ParentSubmenuId.Value == 0 || sub.ParentSubMenuId == ParentSubmenuId) &&
                (!ModuleId.HasValue || ModuleId.Value == 0 || sub.ModuleId == ModuleId) &&
                (!ApplicationId.HasValue || ApplicationId.Value == 0 || sub.ApplicationId == ApplicationId));

                submenus = _mapper.Map<List<SubmenuViewModel>>(submenuInDb);
                foreach (var item in submenus)
                {
                    item.MenuName = (await _mainMenuRepository.GetSingleAsync(mm => mm.MMId == item.MMId)).MenuName;
                    item.ModuleName = (await _moduleRepository.GetSingleAsync(m => m.ModuleId == item.ModuleId)).ModuleName;
                    item.ApplicationName = (await _applicationRepository.GetSingleAsync(a => a.ApplicationId == item.ApplicationId)).ApplicationName;
                    item.MenuSequence = item.MenuSequence ?? 0;
                }

                if (submenus.Any())
                {
                    submenus = submenus.OrderBy(x => x.MenuSequence).ToList();
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "", "", null);
            }
            return submenus;
        }
        public async Task<bool> SaveSubmenuAsync(SubMenu subMenu, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                var mainmenuInDb = await _mainMenuRepository.GetSingleAsync(mm => mm.MMId == subMenu.MMId);
                if (subMenu.SubmenuId == 0)
                {
                    subMenu.CreatedBy = UserId;
                    subMenu.CreatedDate = DateTime.Now;
                    subMenu.ModuleId = mainmenuInDb.MId;
                    //subMenu.ComId = ComId;
                    subMenu.ApplicationId = mainmenuInDb.ApplicationId;
                    await _subMenuRepository.InsertAsync(subMenu);
                }
                else
                {
                    var submenuInDb = await _subMenuRepository.GetSingleAsync(s => s.SubmenuId == subMenu.SubmenuId);
                    if (submenuInDb != null)
                    {
                        submenuInDb.SubmenuName = subMenu.SubmenuName;
                        submenuInDb.ControllerName = subMenu.ControllerName;
                        submenuInDb.ActionName = subMenu.ActionName;
                        submenuInDb.Path = subMenu.Path;
                        submenuInDb.Component = subMenu.Component;
                        submenuInDb.MMId = subMenu.MMId;
                        submenuInDb.ModuleId = mainmenuInDb.MId;
                        submenuInDb.ApplicationId = mainmenuInDb.ApplicationId;
                        submenuInDb.IconClass = subMenu.IconClass;
                        submenuInDb.IconColor = subMenu.IconColor;
                        submenuInDb.IsViewable = subMenu.IsViewable;
                        submenuInDb.ParentSubMenuId = subMenu.ParentSubMenuId;
                        submenuInDb.IsActAsParent = subMenu.IsActAsParent;
                        submenuInDb.HasTab = subMenu.HasTab;
                        submenuInDb.MenuSequence = subMenu.MenuSequence;
                        subMenu.UpdatedBy = UserId;
                        subMenu.UpdatedDate = DateTime.Now;
                        await _subMenuRepository.UpdateAsync(submenuInDb);
                    }
                }
                return await _subMenuRepository.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteSubmenuAsync(long submenuId, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                await _subMenuRepository.DeleteSingleAsync(s => s.SubmenuId == submenuId);
                var status = await _subMenuRepository.SaveAsync();
                return status;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<IEnumerable<PageTabViewModel>> GetPageTabAsync(string TabName, long? SubmenuId, long? MainmenuId)
        {
            try
            {
                var pageTabInDb = await _pageTabRepository.GetAllAsync(t =>
                    (string.IsNullOrEmpty(TabName) || t.TabName.Contains(TabName)) &&
                    (!MainmenuId.HasValue || MainmenuId.Value == 0 || t.MMId == MainmenuId) &&
                    (!SubmenuId.HasValue || SubmenuId.Value == 0 || t.SubmenuId == SubmenuId));
                var pageTabs = _mapper.Map<List<PageTabViewModel>>(pageTabInDb);

                foreach (var item in pageTabs)
                {
                    item.SubmenuName = (await _subMenuRepository.GetSingleAsync(s => s.SubmenuId == item.SubmenuId)).SubmenuName;
                    item.MenuName = (await _mainMenuRepository.GetSingleAsync(s => s.MMId == item.MMId)).MenuName;
                }
                return pageTabs;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> SavePageTabAsync(PageTab pageTab, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                var submenuInDb = await _subMenuRepository.GetSingleAsync(s => s.SubmenuId == pageTab.SubmenuId);
                if (pageTab.TabId == 0)
                {
                    pageTab.CreatedBy = UserId;
                    pageTab.CreatedDate = DateTime.Now;
                    pageTab.OrganizationId = OrgId;
                    pageTab.BranchId = BranchId;
                    pageTab.ComId = ComId;
                    pageTab.MMId = submenuInDb.MMId;
                    await _pageTabRepository.InsertAsync(pageTab);
                }
                else
                {
                    var pageTabInDb = await _pageTabRepository.GetSingleAsync(t => t.TabId == pageTab.TabId);
                    if (pageTabInDb != null)
                    {
                        pageTabInDb.TabName = pageTab.TabName;
                        pageTabInDb.SubmenuId = pageTab.SubmenuId;
                        pageTabInDb.IconClass = pageTab.IconClass;
                        pageTabInDb.IconColor = pageTab.IconColor;
                        pageTabInDb.IsActive = pageTab.IsActive;
                        pageTabInDb.MMId = pageTab.MMId;
                        pageTabInDb.UpdatedBy = UserId;
                        pageTabInDb.UpdatedDate = DateTime.Now;
                        await _pageTabRepository.UpdateAsync(pageTabInDb);
                    }
                }
                return await _pageTabRepository.SaveAsync();
            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<bool> DeletePageTabAsync(long tabId, long OrgId, long ComId, long BranchId, string UserId)
        {
            try
            {
                await _pageTabRepository.DeleteSingleAsync(t => t.TabId == tabId);
                var status = await _pageTabRepository.SaveAsync();
                return status;
            }
            catch (Exception)
            {

                return false;
            }
        }


    }
}
