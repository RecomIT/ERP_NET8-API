
using BLL.Asset.Implementation.Approval;
using BLL.Asset.Implementation.Assigning;
using BLL.Asset.Implementation.Create;
using BLL.Asset.Implementation.Dashboard;
using BLL.Asset.Implementation.IT;
using BLL.Asset.Implementation.Resignation;
using BLL.Asset.Implementation.Setting;
using BLL.Asset.Implementation.Support;
using BLL.Asset.Interface.Approval;
using BLL.Asset.Interface.Assigning;
using BLL.Asset.Interface.Create;
using BLL.Asset.Interface.Dashboard;
using BLL.Asset.Interface.IT;
using BLL.Asset.Interface.Resignation;
using BLL.Asset.Interface.Setting;
using BLL.Asset.Interface.Support;
using BLL.Asset.Report.Implementation;
using BLL.Asset.Report.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Asset
{
    public static class AssetModuleInjector
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICategoryBusiness, CategoryBusiness>();
            services.AddScoped<ISubCategoryBusiness, SubCategoryBusiness>();
            services.AddScoped<IBrandBusiness, BrandBusiness>();
            services.AddScoped<IStoreBusiness, StoreBusiness>();
            services.AddScoped<IVendorBusiness, VendorBusiness>();
            services.AddScoped<ICreateBusiness, CreateBusiness>();
            services.AddScoped<IAssigningBusiness, AssigningBusiness>();
            services.AddScoped<IApprovalBusiness, ApprovalBusiness>();
            services.AddScoped<IITSupportBusiness, ITSupportBusiness>();
            services.AddScoped<IResignationBusiness, ResignationBusiness>();
            services.AddScoped<IReplacementBusiness, ReplacementBusiness>();
            services.AddScoped<IHandoverBusiness, HandoverBusiness>();
            services.AddScoped<IServicingBusiness, ServicingBusiness>();
            services.AddScoped<IRepairedBusiness, RepairedBusiness>();
            services.AddScoped<IReportBusiness, ReportBusiness>();
            services.AddScoped<IEmployeeBusiness, EmployeeBusiness>();
            services.AddScoped<IAdminBusiness, AdminBusiness>();
        }
    }
}
