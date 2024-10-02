using Shared.Employee.Domain.Info;
using Shared.Employee.Domain.Setup;
using Shared.Employee.Domain.Stage;
using Shared.Employee.Domain.Logger;
using Microsoft.EntityFrameworkCore;
using Shared.Employee.Domain.Account;
using Shared.Employee.Domain.Education;
using Shared.Employee.Domain.Locational;
using Shared.Employee.Domain.Termination;
using Shared.Employee.Domain.Miscellaneous;
using Shared.Employee.Domain.Organizational;
using Shared.Models.Dashboard.CommonDashboard.Domain;
using Shared.Employee.Domain.Integration;

namespace DAL.Context.Employee
{
    public class EmployeeModuleDbContext : DbContext
    {
        public EmployeeModuleDbContext(DbContextOptions<EmployeeModuleDbContext> options) : base(options)
        {
            //this.Database.Migrate();
        }

        public DbSet<CompanyAccountInfo> HR_CompanyAccountInfo { get; set; }
        public DbSet<TableConfig> HR_TableConfig { get; set; }

        // Logger
        public DbSet<HRExceptionLogger> HR_ExceptionLogger { get; set; }
        public DbSet<HRActivityLogger> HR_ActivityLogger { get; set; }

        // Common Dashboard

        public DbSet<CompanyEventsCalendar> HR_CompanyEventsCalendar { get; set; }
        public DbSet<CompanyEvents> HR_CompanyEvents { get; set; }

        // Organizational
        public DbSet<Grade> HR_Grades { get; set; }
        public DbSet<Designation> HR_Designations { get; set; }
        public DbSet<Department> HR_Departments { get; set; }
        public DbSet<DepartmentalDivision> HR_DepartmentalDivision { get; set; } // Departmental Division
        public DbSet<Section> HR_Sections { get; set; }
        public DbSet<SubSection> HR_SubSections { get; set; }
        public DbSet<Unit> HR_Units { get; set; }
        public DbSet<CostCenter> HR_Costcenter { get; set; }
        public DbSet<FunctionalDivision> HR_FunctionalDivision { get; set; } // Parent Division

        // Locational
        public DbSet<Country> HR_Countries { get; set; }
        public DbSet<Division> HR_Divisions { get; set; }
        public DbSet<District> HR_Districts { get; set; }
        public DbSet<PoliceStation> HR_PoliceStations { get; set; }
        public DbSet<Location> HR_JobLocations { get; set; }

        // Miscellaneous
        public DbSet<Jobtype> HR_Jobtypes { get; set; } // Permanent / Contractual / Probation / Intrean/Trainee / Partime
        public DbSet<Level> HR_Levels { get; set; } //
        public DbSet<Line> HR_Line { get; set; } // 
        public DbSet<JobCategory> HR_JobCategory { get; set; }
        public DbSet<EmployeeType> HR_EmployeeType { get; set; }

        // New Added 25 May, 2024
        public DbSet<DataLabel> HR_DataLabel { get; set; }

        // Account 
        public DbSet<Bank> HR_Banks { get; set; }
        public DbSet<BankBranch> HR_BankBranchs { get; set; }
        public DbSet<EmployeeAccountInfo> HR_EmployeeAccountInfo { get; set; }

        // Education
        public DbSet<LevelOfEducation> HR_LevelOfEducation { get; set; }
        public DbSet<EducatioalDegree> HR_EducationalDegrees { get; set; }
        public DbSet<EmployeeEducation> HR_EmployeeEducation { get; set; }

        //Email Config
        public DbSet<EmailSendingConfiguration> HR_EmailSendingConfiguration { get; set; }

        //Info
        public DbSet<EmployeeInformation> HR_EmployeeInformation { get; set; }
        public DbSet<EmployeeDetail> HR_EmployeeDetail { get; set; }
        public DbSet<EmployeeDocument> HR_EmployeeDocument { get; set; }
        public DbSet<EmployeeExperience> HR_EmployeeExperience { get; set; }
        public DbSet<EmployeeFamilyInfo> HR_EmployeeFamilyInfo { get; set; }
        public DbSet<EmployeeHierarchy> HR_EmployeeHierarchy { get; set; }
        public DbSet<EmployeeHistory> HR_EmployeeHistory { get; set; }
        public DbSet<EmployeeNomineeInfo> HR_EmployeeNomineeInfo { get; set; }
        public DbSet<EmployeeSkill> HR_EmployeeSkill { get; set; }
        public DbSet<EmployeeSalaryBreakDown> HR_EmployeeCurrentSalaryBreakDown { get; set; }

        // Stage
        public DbSet<EmployeeConfirmationProposal> HR_EmployeeConfirmationProposal { get; set; }
        public DbSet<EmployeePFActivation> HR_EmployeePFActivation { get; set; }
        public DbSet<EmployeeProbationaryExtensionProposal> HR_EmployeeProbationaryExtensionProposal { get; set; }
        public DbSet<EmployeePromotionProposal> HR_EmployeePromotionProposal { get; set; }
        public DbSet<EmployeeTransferProposal> HR_EmployeeTransferProposal { get; set; }
        public DbSet<EmploymentConfirmationOrProbation> HR_EmploymentConfirmationOrProbotion { get; set; }
        public DbSet<DiscontinuedEmployee> HR_DiscontinuedEmployee { get; set; }
        public DbSet<ContractualEmployment> HR_ContractualEmployment { get; set; }

        // Integration // Workday - SAP - AZURE
        public DbSet<IntegrationConfig> HR_IntegrationConfig { get; set; }
        public DbSet<IntegrationModule> HR_IntegrationModule { get; set; }
        public DbSet<IntegrationColumnMapping> HR_IntegrationColumnMapping { get; set; }

        public DbSet<LunchRate> HR_LunchRate { get; set; }
        public DbSet<LunchRequest> HR_LunchRequest { get; set; }

    }
}
