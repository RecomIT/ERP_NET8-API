using Microsoft.EntityFrameworkCore;
using Shared.Separation.Models.Domain;
using Shared.Separation.Models.Domain.Resignation;
using Shared.Separation.Models.Domain.Settlement;

namespace DAL.Context.Separation
{
    public class SeparationModuleDbContext : DbContext
    {
        public SeparationModuleDbContext(DbContextOptions<SeparationModuleDbContext> options) : base(options)
        {
        }

        // Resignation
        public DbSet<ResignationCategory> HR_ResignationCategory { get; set; }
        public DbSet<ResignationSubCategory> HR_ResignationSubCategory { get; set; }
        public DbSet<ResignationCategoryConfig> HR_ResignationCategoryConfig { get; set; }
        public DbSet<EmployeeResignationRequest> HR_EmployeeResignationRequest { get; set; }
        public DbSet<SupervisorResignationRequestApproval> HR_SupervisorResignationRequestApproval { get; set; }
        public DbSet<AdminResignationRequestApproval> HR_AdminResignationRequestApproval { get; set; }
        public DbSet<EmployeeResignationDetail> HR_EmployeeResignationDetail { get; set; }
        public DbSet<EmployeeResignationFile> HR_EmployeeResignationFiles { get; set; }
        public DbSet<ResignationInterviewQuestion> HR_ResignationInterviewQuestions { get; set; }

        // Settlement

        public DbSet<EmployeeSettlementSetup> Payroll_EmployeeSettlementSetup { get; set; }
        public DbSet<EmployeeSettlementAllowance> Payroll_EmployeeSettlementAllowance { get; set; }
        public DbSet<EmployeeSettlementDeduction> Payroll_EmployeeSettlementDeduction { get; set; }
        public DbSet<EmployeeSettlementProcess> Payroll_EmployeeSettlementProcess { get; set; }
    }
}
