using Microsoft.Extensions.Configuration;
using BLL.Employee.Interface.Info;
using BLL.Employee.Interface.Education;
using BLL.Employee.Interface.Termination;
using BLL.Employee.Interface.Organizational;
using BLL.Employee.Interface.Stage;
using BLL.Employee.Interface.Account;
using BLL.Employee.Implementation.Info;
using BLL.Employee.Implementation.Stage;
using BLL.Employee.Implementation.Locational;
using BLL.Employee.Interface.Locational;
using BLL.Employee.Implementation.Organizational;
using BLL.Employee.Implementation.Report;
using BLL.Employee.Interface.Report;
using BLL.Employee.Implementation.Termination;
using BLL.Employee.Implementation.Account;
using BLL.Employee.Interface.Training;
using BLL.Employee.Interface.Setup;
using BLL.Employee.Implementation.Setup;
using BLL.Employee.Implementation.Training;
using BLL.Employee.Interface.Miscellaneous;
using Microsoft.Extensions.DependencyInjection;
using BLL.Employee.Implementation.Miscellaneous;

namespace BLL.Employee
{
    public static class EmployeeModuleInjector
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Locational
            services.AddScoped<ICountryBusiness, CountryBusiness>();
            services.AddScoped<IDivisionBusiness, DivisionBusiness>();
            services.AddScoped<IDistrictBusiness, DistrictBusiness>();
            services.AddScoped<IPoliceStationBusiness, PoliceStationBusiness>();
            services.AddScoped<ILocationBusiness, LocationBusiness>();

            // Organizational 
            services.AddScoped<IGradeBusiness, GradeBusiness>();
            services.AddScoped<IDesignationBusiness, DesignationBusiness>();
            services.AddScoped<IDepartmentBusiness, DepartmentBusiness>();
            services.AddScoped<ISectionBusiness, SectionBusiness>();
            services.AddScoped<ISubSectionBusiness, SubSectionBusiness>();
            services.AddScoped<IUnitBusiness, UnitBusiness>();
            services.AddScoped<ILineBusiness, LineBusiness>();
            services.AddScoped<ICostCenterBusiness, CostCenterBusiness>();
            services.AddScoped<IInternalDesignationBusiness, InternalDesignationBusiness>();
            services.AddScoped<IEmployeeTypeBusiness, EmployeeTypeBusiness>();

            // Miscellaneous
            services.AddScoped<ILevelOfEducationBusiness, LevelOfEducationBusiness>();
            services.AddScoped<IEducationalDegreeBusiness, EducationalDegreeBusiness>();

            // Account
            services.AddScoped<IBankBusiness, BankBusiness>();
            services.AddScoped<IBankBranchBusiness, BankBranchBusiness>();
            services.AddScoped<IAccountInfoBusiness, AccountInfoBusiness>();

            // Employee Info 
            services.AddScoped<IInfoBusiness, InfoBusiness>();
            services.AddScoped<IDetailBusiness, DetailBusiness>();
            services.AddScoped<ITableConfigBusiness, TableConfigBusiness>();
            services.AddScoped<IUploaderBusiness, UploaderBusiness>();
            services.AddScoped<IExperienceBusiness, ExperienceBusiness>();
            services.AddScoped<IEducationBusiness, EducationBusiness>();
            services.AddScoped<ISkillBusiness, SkillBusiness>();
            services.AddScoped<IDocumentBusiness, DocumentBusiness>();
            services.AddScoped<IEmploymentConfirmationBusiness, EmploymentConfirmationBusiness>();
            services.AddScoped<IEmployeePromotionBusiness, EmployeePromotionBusiness>();
            services.AddScoped<IEmployeePFActivationBusiness, EmployeePFActivationBusiness>();
            services.AddScoped<IEmployeeTransferBusiness, EmployeeTransferBusiness>();
            services.AddScoped<IContractualEmploymentBusiness, ContractualEmploymentBusiness>();
            services.AddScoped<IHierarchyBusiness, HierarchyBusiness>();
            services.AddScoped<IJobCategoryBusiness, JobCategoryBusiness>();
            services.AddScoped<IEmployeeLoggerBusiness, EmployeeLoggerBusiness>();
            services.AddScoped<IDataLabelBusiness, DataLabelBusiness>();
            services.AddScoped<ILunchRequestService, LunchRequestservice>();

            // Termination
            services.AddScoped<IDiscontinuedEmployeeBusiness, DiscontinuedEmployeeBusiness>();
            // Trianing
            services.AddScoped<ITrainingBusiness, TrainingBusiness>();
            // HR Letters
            services.AddScoped<IHRLetterBusiness, HRLetterBusiness>();
        }
    }
}
