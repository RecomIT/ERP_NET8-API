using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using System.Data;
using Shared.Employee.DTO.Info;
using Shared.Employee.Filter.Info;

namespace BLL.Employee.Interface.Info
{
    public interface IEmployeeLoggerBusiness
    {
        Task<List<EmployeeLoggerDTO>> GetEmployeeLogReportAsync(EmployeeLogReport_Filter filter, AppUser user);
        Task<bool> IsEmployeeApprovedBeforeStartDate(long employeeId, string startDate, AppUser user);
        Task<ExecutionStatus> IsEmployeeApprovedBetweenStartAndEndDate(long employeeId, string startDate, string endDate, AppUser user);
        Task<DataTable> GetUserLogInfoAysnc(string userId, AppUser user);
    }
}
