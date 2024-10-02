using DAL.UnitOfWork.Control_Panel.Interface;
using Shared.Control_Panel.Domain;

namespace DAL.Repository.Control_Panel
{
    public class ActivityLoggerRepository : ControlPanelBaseRepository<ActivityLogger>
    {
        public ActivityLoggerRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class ApplicationRepository : ControlPanelBaseRepository<Application>
    {
        public ApplicationRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class ApplicationRoleRepository : ControlPanelBaseRepository<ApplicationRole>
    {
        public ApplicationRoleRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class ApplicationUserRepository : ControlPanelBaseRepository<ApplicationUser>
    {
        public ApplicationUserRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class BranchRepository : ControlPanelBaseRepository<Branch>
    {
        public BranchRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class CompanyRepository : ControlPanelBaseRepository<Company>
    {
        public CompanyRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class CompanyAuthorizationRepository : ControlPanelBaseRepository<CompanyAuthorization>
    {
        public CompanyAuthorizationRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class DistrictRepository : ControlPanelBaseRepository<District>
    {
        public DistrictRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class DivisionRepository : ControlPanelBaseRepository<Division>
    {
        public DivisionRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class ExceptionLoggerRepository : ControlPanelBaseRepository<ExceptionLogger>
    {
        public ExceptionLoggerRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class MainMenuRepository : ControlPanelBaseRepository<MainMenu>
    {
        public MainMenuRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class ModuleRepository : ControlPanelBaseRepository<Module>
    {
        public ModuleRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class OrganizationRepository : ControlPanelBaseRepository<Organization>
    {
        public OrganizationRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class OrganizationAuthorizationRepository : ControlPanelBaseRepository<OrganizationAuthorization>
    {
        public OrganizationAuthorizationRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class PageTabRepository : ControlPanelBaseRepository<PageTab>
    {
        public PageTabRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class RoleAuthorizationRepository : ControlPanelBaseRepository<RoleAuthorization>
    {
        public RoleAuthorizationRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class RoleAuthTabRepository : ControlPanelBaseRepository<RoleAuthTab>
    {
        public RoleAuthTabRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class SubMenuRepository : ControlPanelBaseRepository<SubMenu>
    {
        public SubMenuRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class UserAuthorizationRepository : ControlPanelBaseRepository<UserAuthorization>
    {
        public UserAuthorizationRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class UserAuthTabRepository : ControlPanelBaseRepository<UserAuthTab>
    {
        public UserAuthTabRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class ZoneRepository : ControlPanelBaseRepository<Zone>
    {
        public ZoneRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }

    public class ModuleConfigReposiitory : ControlPanelBaseRepository<ModuleConfig>
    {
        public ModuleConfigReposiitory(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }

    public class EmailSettingReposiitory : ControlPanelBaseRepository<EmailSetting>
    {
        public EmailSettingReposiitory(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }

    public class PayrollModuleConfigReposiitory : ControlPanelBaseRepository<PayrollModuleConfig>
    {
        public PayrollModuleConfigReposiitory(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }


}
