
using Microsoft.EntityFrameworkCore;
using Shared.Employee.Domain.Training;

namespace DAL.Context.Training
{
    public class TrainingModuleDbContext: DbContext
    {
        public TrainingModuleDbContext(DbContextOptions<TrainingModuleDbContext> options): base(options) 
        {
            
        }
        public DbSet<HR_Training> HR_Training { get; set; }
        public DbSet<HR_TrainingRequest> HR_TrainingRequest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
