
using Microsoft.EntityFrameworkCore;
using Shared.Asset_Module.Models.Domain.Creation;
using Shared.Asset_Module.Models.Domain.Resignation;
using Shared.Asset_Module.Models.Domain.Setting;
using Shared.Asset_Module.Models.Domain.Support;

namespace DAL.Context.Asset
{
    public class AssetModuleDbContext : DbContext
    {
        public AssetModuleDbContext(DbContextOptions<AssetModuleDbContext> options) : base(options)
        {
            //this.Database.Migrate();
        }

        public DbSet<Brand> Asset_Brand { get; set; }
        public DbSet<Category> Asset_Category { get; set; }
        public DbSet<SubCategory> Asset_SubCategory { get; set; }
        public DbSet<Vendor> Asset_Vendor { get; set; }
        public DbSet<Store> Asset_Store { get; set; }
        public DbSet<Assets> Asset_Create { get; set; }
        public DbSet<Product> Asset_Product { get; set; }
        public DbSet<Assigning> Asset_Assigning { get; set; }
        public DbSet<Resignation> Asset_Resignation { get; set; }
        public DbSet<Damaged> Asset_Damaged { get; set; }
        public DbSet<Replacement> Asset_Replacement { get; set; }
        public DbSet<Received> Asset_Received { get; set; }
        public DbSet<Handover> Asset_Handover { get; set; }
        public DbSet<Servicing> Asset_Servicing { get; set; }
        public DbSet<Repaired> Asset_Repaired { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
