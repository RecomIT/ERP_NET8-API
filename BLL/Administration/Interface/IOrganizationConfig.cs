using Shared.Control_Panel.Domain;
using Shared.Control_Panel.ViewModels;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;

namespace BLL.Administration.Interface
{
    public interface IOrganizationConfig
    {
        // Org - Authorization Config
        Task<AuthApp> GetOrganizationAuths(long OrgId);
        Task<bool> SaveOrgAuthsAsync(AuthApp orgAuth, long AuthOrgId, long OrgId, long ComId, long BranchId, string UserId);

        // Com - Authorization Config
        Task<AuthApp> GetCompanyAuths(long OrgId, long ComId);
        Task<bool> SaveComAuthsAsync(AuthApp orgAuth, long AuthOrgId, long AuthComId, long OrgId, long ComId, long BranchId, string UserId);
        Task<IEnumerable<Select2Dropdown>> GetCompanyAuthMainmenuExtensionAsync(long comId);

        // Role
        Task<IEnumerable<ApplicationRoleViewModel>> GetApplicationRolesAsync(string RoleName, string RoleId, long RoleComId, long RoleOrgId, bool? IsActive, long OrgId, long ComId, long BranchId, string UserId);
        Task<bool> SaveApplicationRoleAsync(ApplicationRole role, long OrgId, long ComId, long BranchId, string UserId);
        Task<IEnumerable<Select2Dropdown>> GetApplicationRolesExtensionAsync(long OrgId, long ComId);

        // Module - Config
        Task<IEnumerable<ModuleConfigViewModel>> GetModuleConfigsAsync(long mainemnuId, long moduleId, long branchId, long comId, long orgId);
        Task<ExecutionStatus> SaveModuleConfigAsync(List<ModuleConfig> moduleConfigs, string userId);

    }
}
