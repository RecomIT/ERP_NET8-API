using Shared.Control_Panel.ViewModels;

namespace BLL.Administration.Interface
{
    public interface IReportConfigBusiness
    {
        Task<IEnumerable<ReportConfigViewModel>> GetReportConfigsAsync(string reportCategory, string fiscalYearRange, long companyId, long organizationId);
        Task<ReportConfigViewModel> ReportConfigAsync(string reportCategory, string fiscalYearRange, long companyId, long organizationId);
    }
}
