using DAL.Repository.Employee.Implementation;
using DAL.Repository.Employee.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.Repository.Employee
{
    public static class DALEmployeeModuleInjector
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IJobCategoryRepository, JobCategoryRepository>();
            services.AddScoped<IEducationDegreeRepository, EducationDegreeRepository>();
            services.AddScoped<IEmployeeHierarchyRepository, EmployeeHierarchyRepository>();
            services.AddScoped<IEmailSendingConfigRepository, EmailSendingConfigRepository>();
            services.AddScoped<IEmployeePFActivationRepository, EmployeePFActivationRepository>();
            services.AddScoped<IEmploymentConfirmationRepository, EmploymentConfirmationRepository>();
            services.AddScoped<IDiscontinuedEmployeeRepository, DiscontinuedEmployeeRepository>();
            services.AddScoped<IEmployeeEducationRepository, EmployeeEducationRepository>();
            services.AddScoped<IEmployeePromotionProposalRepository, EmployeePromotionProposalRepository>();
            services.AddScoped<IDesignationRepository, DesignationRepository>();
        }
    }
}
