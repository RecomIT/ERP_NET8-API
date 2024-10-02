using AutoMapper;
using Shared.Control_Panel.Domain;
using Shared.Control_Panel.ViewModels;

namespace BLL.AutomapperProfile.ControlPanel.Administration
{
    public class AdministrationProfile : Profile
    {
        public AdministrationProfile()
        {
            CreateMap<Application, ApplicationViewModel>().ReverseMap();
            CreateMap<Module, ModuleViewModel>().ReverseMap();
            CreateMap<MainMenu, MainMenuViewModel>().ReverseMap();
            CreateMap<SubMenu, SubmenuViewModel>().ReverseMap();
            CreateMap<PageTab, PageTabViewModel>().ReverseMap();
            CreateMap<Organization, OrganizationViewModel>().ReverseMap();
            CreateMap<Company, CompanyViewModel>().ReverseMap();
            CreateMap<Division, DivisionViewModel>().ReverseMap();
            CreateMap<District, DistrictViewModel>().ReverseMap();
            CreateMap<Zone, ZoneViewModel>().ReverseMap();
            CreateMap<Branch, BranchViewModel>().ReverseMap();
            CreateMap<ApplicationRole, ApplicationRoleViewModel>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();
            CreateMap<ModuleConfig, ModuleConfigViewModel>().ReverseMap();
            CreateMap<OTPRequests, OTPRequestsViewModel>().ReverseMap();
            CreateMap<HRModuleConfig, HRModuleConfigViewModel>().ReverseMap();
            CreateMap<PayrollModuleConfig, PayrollModuleConfigViewModel>().ReverseMap();
            CreateMap<PFModuleConfig, PFModuleConfigViewModel>().ReverseMap();
        }
    }
}
