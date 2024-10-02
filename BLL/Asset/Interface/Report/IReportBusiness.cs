
using Shared.Asset.Filter.Report;
using Shared.OtherModels.User;
using System.Data;


namespace BLL.Asset.Report.Interface
{
    public interface IReportBusiness
    {        
        Task<DataTable> GetAssetReport(Report_Filter filter, AppUser user);
        Task<DataTable> GetAssetAssigningReport(Report_Filter filter, AppUser user);
        Task<DataTable> GetServicingReport(Report_Filter filter, AppUser user);
        Task<DataTable> GetReplacementReport(Report_Filter filter, AppUser user);
        Task<DataTable> GetHandoverReport(Report_Filter filter, AppUser user);
        Task<DataTable> GetDamageReport(Report_Filter filter, AppUser user);
        Task<DataTable> GetStockReport(Report_Filter filter, AppUser user);
        Task<DataTable> GetRepairedReport(Report_Filter filter, AppUser user);
    }
}
