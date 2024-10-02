using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.Filter.Incentive.MonthlyIncentive;
using Shared.Payroll.ViewModel.Incentive.MonthlyIncentive;
using System.Data;


namespace BLL.Salary.Incentive.Interface
{
    public interface IMonthlyIncentiveBusiness
    {
        Task<IEnumerable<Select2Dropdown>> GetBatchNoExtensionAsync(short incentiveYear, long incentiveMonth, AppUser appUser);
        Task<IEnumerable<Select2Dropdown>> GetMonthlyIncentiveYearExtensionAsync(short incentiveYear, AppUser appUser);
        Task<IEnumerable<Select2Dropdown>> GetMonthlyIncentiveMonthExtensionAsync(short incentiveYear, short incentiveMonth, AppUser appUser);
        Task<ExecutionStatus> UploadMonthlyIncentiveExcelAsync(List<MonthlyIncentiveProcessDetailViewModel> models, AppUser user);
        Task<DBResponse<MonthlyIncentiveProcessViewModel>> GetMonthlyIncentiveListAsync(MonthlyIncentiveProcess_Filter incentiveProcess_Filter, AppUser user);
        Task<IEnumerable<MonthlyIncentiveProcessDetailViewModel>> GetMonthlyIncentiveDetailAsync(MonthlyIncentiveDetail_Filter filter, AppUser user);
        Task<ExecutionStatus> UpdateMonthlyIncentiveDetailAsync(UpdateMonthlyIncentiveProcessDetails_Filter filter, AppUser user);
        Task<ExecutionStatus> DeleteMonthlyIncentiveDetailAsync(DeleteMonthlyIncentiveProcess_Filter filter, AppUser user);
        Task<ExecutionStatus> UndoOrDisbursedMonthlyIncentiveProcessAsync(MonthlyIncentiveUndoOrDisbursed_Filter filter, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetMonthlyIncentiveEmployeesExtensionAsync(short incentiveYear, short incentiveMonth, long? employeeIdForSearch, AppUser appUser);
        //Task<IEnumerable<MonthlyIncentiveProcessDetailViewModel>> GetMonthlyIncentiveReportAsync(MonthlyIncentiveReport_Filter filter, AppUser user);

        Task<DataTable> GetMonthlyIncentiveReportAsync(MonthlyIncentiveReport_Filter filter, AppUser user);

        Task<DataTable> GetMonthlyIncenitveExcelAsync(MonthlyIncentiveDownloadExcel_Filter filter, AppUser user);
    }
}
