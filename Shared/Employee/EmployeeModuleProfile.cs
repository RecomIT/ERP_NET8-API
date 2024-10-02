using AutoMapper;
using Shared.Employee.Domain.Account;
using Shared.Employee.Domain.Education;
using Shared.Employee.Domain.Info;
using Shared.Employee.Domain.Stage;
using Shared.Employee.DTO.Info;
using Shared.Employee.ViewModel.Account;
using Shared.Employee.ViewModel.Education;
using Shared.Employee.ViewModel.Info;
using Shared.Employee.ViewModel.Stage;

namespace Shared.Employee
{
    public class EmployeeModuleProfile : Profile
    {
        public EmployeeModuleProfile()
        {
            CreateMap<EmployeeAccountInfo, EmployeeAccountInfoViewModel>().ReverseMap();
            CreateMap<EmployeeEducation, EmployeeEducationVM>().ReverseMap();
            CreateMap<EmployeeExperience, EmployeeExperienceVM>().ReverseMap();
            CreateMap<EmployeeSkill, EmployeeSkillVM>().ReverseMap();
            CreateMap<EmploymentStageInfo, EmploymentStageInfoVM>().ReverseMap();
            CreateMap<EmploymentStageDetails, EmploymentStageDetailsVM>().ReverseMap();
            CreateMap<EmploymentConfirmationOrProbation, EmploymentConfirmationOrProbationVM>().ReverseMap();
            CreateMap<EmployeeInformation, EmployeeInformationViewModel>().ReverseMap();
            CreateMap<EmployeeInformation, EmployeeHistory>().ReverseMap(); // History table
            CreateMap<EmployeeUploaderDTO, EmployeeUploadInformation>().ReverseMap(); // History table
        }
    }
}
