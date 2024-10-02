using System.Data;
using Shared.Leave.Report;
using Shared.OtherModels.User;
using Shared.Leave.Filter.Report;
using Shared.OtherModels.DataService;

namespace BLL.Leave.Interface.Report
{
    public interface ILeaveReportBusiness
    {
        Task<IEnumerable<EmployeeInfoInLeaveBalance>> EmployeeWiseLeaveBalanceSummaryAsync(LeaveQuery_Filter filter, AppUser user);
        Task<DataTable> MonthlyLeaveReportAsync(LeaveQuery_Filter filter, AppUser user);
        Task<DataTable> IndividualYearlyStatusAsync(LeaveQuery_Filter filter, AppUser user);
        Task<DataTable> DateRangeWiseLeaveReportAsync(LeaveQuery_Filter filter, AppUser user);
        Task<DataTable> YearlyLeaveReportAsync(LeaveQuery_Filter filter, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetLeaveYearDropdownAsync(AppUser user);
        Task<IEnumerable<LeaveCardEmployeeInformation>> GetEmployeeInfoForLeaveCardAsync(LeaveCardFilter filter, AppUser user);
        Task<IEnumerable<LeaveCardLeaveBalanceSummary>> GetEmployeeLeaveBalanceSummaryForLeaveCardAsync(LeaveCardFilter filter, AppUser user);
        Task<IEnumerable<LeaveCardAppliedLeaveInformation>> GetAppliedLeaveInformationForLeaveCardAsync(LeaveCardFilter filter, AppUser user);
        Task<IEnumerable<LeaveCardLeaveBalanceSummary>> GetEmployeeLeaveBalanceSummaryWithApplicableLeaveAsync(LeaveCardFilter filter, AppUser user);
    }
}
