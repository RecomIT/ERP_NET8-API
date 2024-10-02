
using Microsoft.EntityFrameworkCore;
using Shared.Expense_Reimbursement.Domain.Request;



namespace DAL.Context.Expense_Reimbursement
{
    public class Expense_ReimbursementModuleDbContext : DbContext
    {
        public Expense_ReimbursementModuleDbContext(DbContextOptions<Expense_ReimbursementModuleDbContext> options) : base(options)
        {
            this.Database.Migrate();
        }

        public DbSet<Conveyance> Reimburse_Conveyance { get; set; }
        public DbSet<Conveyance_Details> Reimburse_Conveyance_Details { get; set; }
        public DbSet<Travel> Reimburse_Travel { get; set; }
        public DbSet<Entertainment> Reimburse_Entertainment { get; set; }
        public DbSet<Entertainment_Details> Reimburse_Entertainment_Details { get; set; }
        public DbSet<Expat> Reimburse_Expat { get; set; }
        public DbSet<Expat_Details> Reimburse_Expat_Details { get; set; }
        public DbSet<Purchase> Reimburse_Purchase { get; set; }
        public DbSet<Purchase_Details> Reimburse_Purchase_Details { get; set; }
        public DbSet<Reimburse_Training> Reimburse_Training { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}
