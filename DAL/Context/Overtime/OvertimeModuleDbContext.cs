using Microsoft.EntityFrameworkCore;
using Shared.Overtime.Domain;

namespace DAL.Context.Overtime
{
    public class OvertimeModuleDbContext : DbContext
    {
        public OvertimeModuleDbContext(DbContextOptions<OvertimeModuleDbContext> options) : base(options)
        {
        }

        public DbSet<OvertimePolicy> PayrollOvertime { get; set; }
        public DbSet<OvertimeApprovalLevel> OvertimeApprovalLevel { get; set; }
        public DbSet<OvertimeApprover> OvertimeApprover { get; set; }
        public DbSet<OvertimeTeamApprovalMapping> OvertimeTeamApprovalMapping { get; set; }
        public DbSet<OvertimeRequest> OvertimeRequest { get; set; }
        public DbSet<OvertimeRequestDetails> OvertimeRequestDetails { get; set; }

        public DbSet<OvertimeProcess> OvertimeProcess { get; set; }
        public DbSet<OvertimeProcessDetails> OvertimeProcessDetails { get; set; }
        public DbSet<OvertimeAllowances> OvertimeAllowances { get; set; }
        public DbSet<UploadOvertimeAllowances> UploadOvertimeAllowances { get; set; }
    }
}
