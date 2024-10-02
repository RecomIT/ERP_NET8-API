using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.Domain.Salary;
using Shared.Payroll.Process.Salary;
using Shared.Payroll.ViewModel.Salary;

namespace BLL.Salary.Salary.Interface
{
    public interface IExecuteSalaryProcess
    {
        Task<ExecutionStatus> ExecuteProcess(SalaryProcessExecution execution, AppUser user);
        Task<IEnumerable<EligibleEmployeeForSalaryType>> GetEligibleEmployees(string process, EligibleEmployeeForSalary_Filter filter, AppUser user);
        Task<List<EmployeeSalaryProcessedInfo>> RunSystematically(SalaryProcessExecution execution, List<EligibleEmployeeForSalaryType> employees, AppUser user);
        Task<EmployeeSalaryProcessedInfo> ReProcessSystematically(SalaryProcessExecution execution, EligibleEmployeeForSalaryType employee, SalaryProcessDetail salaryProcessDetail, AppUser user);
        Task<ExecutionStatus> ReProcess(SalaryReprocess reprocess, AppUser user);
        Task<ExecutionStatus> DeleteSingleEmployeeSalaryAsync(SalaryReprocess reprocess, AppUser user);
        Task<int> GetPresentCountBetweenSalaryDates(PresentCountBetweenSalaryDates_Filter parameters, AppUser user);
        Task<int> GetPresentCountBetweenSalaryDatesWhenSalaryWasHold(PresentCountBetweenSalaryDates_Filter parameters, AppUser user);
        Task<int> GetEmployeeHoldDays(PresentCountBetweenSalaryDates_Filter parameters, AppUser user);
        Task<ExecutionStatus> SaveSalaryAsync(string processType, int salaryMonth, int salaryYear, bool IsMargeProcess, List<EmployeeSalaryProcessedInfo> salaryItems, AppUser user);
        Task<ExecutionStatus> UpdateSalaryAsync(string processType, long salaryProcessId, long salaryProcessDetailId, int salaryMonth, int salaryYear, EmployeeSalaryProcessedInfo salaryItem, AppUser user);


    }
}
