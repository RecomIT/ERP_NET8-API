
using System.Data;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Payroll.ViewModel.Incentive.QuarterlyIncentive;
using Shared.Payroll.Filter.Incentive.QuarterlyIncentive;

namespace BLL.Salary.Incentive.Interface
{
    public interface IQuarterlyIncentiveBusiness
    {
        Task<ExecutionStatus> UploadQuarterlyIncentiveAsync(List<QuarterlyIncentiveProcessDetailsViewModel> quarterlyIncentiveProcessDetails, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetQuarterlyIncenitveYearAsync(AppUser appUser);
        Task<IEnumerable<Select2Dropdown>> GetQuarterlyIncenitveQuarterAsync(Quarter_Filter filter, AppUser appUser);
        Task<IEnumerable<Select2Dropdown>> GetQuarterlyIncenitveNumberAsync(Quarter_Filter filter, AppUser appUser);
        Task<IEnumerable<Select2Dropdown>> GetQuarterlyIncenitveEmployees(QuarterlyIncentiveEmployee_Filter filter, AppUser appUser);
        Task<IEnumerable<QuarterlyIncentiveProcessDetailsViewModel>> GetQuarterlyIncentiveDetailAsync(QuarterlyIncentiveDetail_Filter filter, AppUser user);
        Task<ExecutionStatus> UpdateQuarterlyIncentiveDetailAsync(QuarterlyIncentiveProcessDetailsUpdate detailsUpdate, AppUser user);
        Task<ExecutionStatus> DeleteQuarterlyIncentiveProcessAsync(DeleteQuarterlyIncentiveProcess_Filter filter, AppUser user);
        Task<ExecutionStatus> UndoOrDisbursedQuarterlyIncentiveProcessAsync(UndoOrDisbursed_Filter filter, AppUser user);
        Task<DataTable> GetQuarterlyIncentiveReportAsync(QuarterlyIncentiveReport_Filter filter, AppUser user);
        Task<DataTable> GetQuarterlyIncenitveExcel(DownloadExcel_Filter filter, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetQuarterNumberExtensionAsync(short incentiveYear, AppUser appUser);
        Task<IEnumerable<Select2Dropdown>> GetBatchNoExtensionAsync(short incentiveYear, long? incentiveQuarterNoId, AppUser appUser);
        Task<DBResponse<QuarterlyIncentiveProcessViewModel>> GetQuarterlyIncentiveAsync(QuarterlyIncentive_Filter filter, AppUser user);
    }
}
