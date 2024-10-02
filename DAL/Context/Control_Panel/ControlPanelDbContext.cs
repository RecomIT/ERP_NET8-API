using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Shared.Control_Panel.Domain;

namespace DAL.Context.Control_Panel
{
    public class ControlPanelDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ControlPanelDbContext(DbContextOptions<ControlPanelDbContext> options) : base(options)
        {
            //this.Database.Migrate();
        }

        // Logger 
        public DbSet<ActivityLogger> tblActivityLogger { get; set; }
        public DbSet<ExceptionLogger> tblExceptionLogger { get; set; }

        // App Config
        public DbSet<Application> tblApplications { get; set; }
        public DbSet<Module> tblModules { get; set; }
        public DbSet<MainMenu> tblMainMenus { get; set; }
        public DbSet<SubMenu> tblSubMenus { get; set; }
        public DbSet<PageTab> tblPageTabs { get; set; }

        // Org Config
        public DbSet<Organization> tblOrganizations { get; set; }
        public DbSet<Company> tblCompanies { get; set; }
        public DbSet<Division> tblDivisions { get; set; }

        public DbSet<Branch> tblBranches { get; set; }
        public DbSet<OrganizationAuthorization> tblOrganizationAuthorization { get; set; }
        public DbSet<CompanyAuthorization> tblCompanyAuthorization { get; set; }

        // User Config
        public DbSet<UserAuthorization> tblUserAuthorization { get; set; }
        public DbSet<RoleAuthorization> tblRoleAuthorization { get; set; }
        public DbSet<UserAuthTab> tblUserAuthTab { get; set; }
        public DbSet<RoleAuthTab> tblRoleAuthTab { get; set; }

        // Module Config..
        public DbSet<HRModuleConfig> tblHRModuleConfig { get; set; }
        public DbSet<PayrollModuleConfig> tblPayrollModuleConfig { get; set; }
        public DbSet<PFModuleConfig> tblPFModuleConfig { get; set; }
        public DbSet<Configurable> tblConfigurable { get; set; }
        public DbSet<ModuleConfig> tblModuleConfig { get; set; }

        //...
        public DbSet<OTPRequests> tblOTPRequests { get; set; }
        public DbSet<EmailSetting> tblEmailSetting { get; set; }

        // Report
        public DbSet<ReportConfig> tblReportConfig { get; set; }
        public DbSet<ReportCategory> tblReportCategory { get; set; }
        public DbSet<ReportSubCategory> tblReportSubCategory { get; set; }
        public DbSet<ReportAuthorization> ReportAuthorization { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUserTokens<string>>()
                .HasKey(t => new { t.UserId, t.LoginProvider, t.Name, t.SessionId });
        }

    }
}
