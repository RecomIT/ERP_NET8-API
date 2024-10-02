using DAL.Database_Context.Database_Connection.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Leave.Domain.Balance;
using Shared.Leave.Domain.History;
using Shared.Leave.Domain.Request;
using Shared.Leave.Domain.Setup;


namespace DAL.Context.Leave
{
    public class LeaveModuleDbContext : DbContext
    {
        public LeaveModuleDbContext(DbContextOptions<LeaveModuleDbContext> options) : base(options)
        {
        }


        public DbSet<LeaveType> HR_LeaveTypes { get; set; }
        public DbSet<LeaveSetting> HR_LeaveSettings { get; set; }
        public DbSet<EmployeeLeaveBalance> HR_EmployeeLeaveBalance { get; set; }
        public DbSet<EmployeeLeaveRequest> HR_EmployeeLeaveRequest { get; set; }
        public DbSet<EmployeeLeaveHistory> HR_EmployeeLeaveHistory { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
