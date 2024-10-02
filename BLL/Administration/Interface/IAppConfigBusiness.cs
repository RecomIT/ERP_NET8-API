using Shared.Control_Panel.Domain;
using Shared.Control_Panel.ViewModels;
using Shared.OtherModels.DataService;

namespace BLL.Administration.Interface
{
    public interface IAppConfigBusiness
    {
        // Application
        Task<IEnumerable<ApplicationViewModel>> GetApplicationsAsync(string ApplicationName, string ApplicationType, bool? IsActive, long OrgId, long ComId, long BranchId);
        Task<bool> SaveApplicationAsync(Application application, long OrgId, long ComId, long BranchId, string UserId);
        Task<bool> DeleteApplicationAsync(long ApplicationId, long OrgId, long ComId, long BranchId, string UserId);

        // Module
        Task<IEnumerable<ModuleViewModel>> GetModuleAsync(string ModuleName, long? ApplicationId, bool? IsActive, long OrgId, long ComId, long BranchId);
        Task<bool> SaveModuleAsync(Module module, long OrgId, long ComId, long BranchId, string UserId);
        Task<bool> DeleteModuleAsync(long ModuleId, long OrgId, long ComId, long BranchId, string UserId);
        Task<IEnumerable<Select2Dropdown>> GetModuleExtensionAsync(long applicationId);

        // Mainmenu
        Task<IEnumerable<MainMenuViewModel>> GetMainMenusAsync(string MainMenuName, long? ModuleId, long? ApplicationId, bool? IsActive, long OrgId, long ComId, long BranchId);
        Task<bool> SaveMainmenuAsync(MainMenu mainMenu, long OrgId, long ComId, long BranchId, string UserId);
        Task<bool> DeleteMainmenuAsync(long MainmenuId, long OrgId, long ComId, long BranchId, string UserId);

        // Submenu
        Task<IEnumerable<SubmenuViewModel>> GetSubmenusAsync(string SubmenuName, long? MainmenuId, long? ParentSubmenuId, long? ModuleId, long? ApplicationId);
        Task<bool> SaveSubmenuAsync(SubMenu subMenu, long OrgId, long ComId, long BranchId, string UserId);
        Task<bool> DeleteSubmenuAsync(long submenuId, long OrgId, long ComId, long BranchId, string UserId);

        // Page tab
        Task<IEnumerable<PageTabViewModel>> GetPageTabAsync(string TabName, long? SubmenuId, long? MainmenuId);
        Task<bool> SavePageTabAsync(PageTab pageTab, long OrgId, long ComId, long BranchId, string UserId);
        Task<bool> DeletePageTabAsync(long tabId, long OrgId, long ComId, long BranchId, string UserId);
    }
}
