

using BLL.Expense_Reimbursement.Implementation.Reimbursement;
using BLL.Expense_Reimbursement.Implementation.Request;
using BLL.Expense_Reimbursement.Interface.ReqReimbursementuest;
using BLL.Expense_Reimbursement.Interface.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Expense_Reimbursement
{
    public static class Expense_ReimbursementModuleInjector
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)        {

            services.AddScoped<IRequestBusiness, RequestBusiness>();
            services.AddScoped<IApprovalBusiness, ApprovalBusiness>();
            services.AddScoped<IReimbursementBusiness, ReimbursementBusiness>();

        }
    }
}
