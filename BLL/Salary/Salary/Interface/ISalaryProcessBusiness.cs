using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Payroll.ViewModel.Salary;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.DTO.Salary;
using Shared.Payroll.Domain.Salary;

namespace BLL.Salary.Salary.Interface
{
    public interface ISalaryProcessBusiness
    {
        Task<ExecutionStatus> SalaryProcessAsync(SalaryProcessViewModel model, AppUser user);
        Task<ExecutionStatus> SalaryProcessAsync(SalaryProcessExecution processExecution, AppUser user);
        Task<IEnumerable<SalaryProcessViewModel>> GetSalaryProcessInfosAsync(long? salaryProcessId, long? fiscalYearId, short? month, short? year, DateTime? salaryDate, long? branchId, string batchNo, AppUser user);
        Task<IEnumerable<SalaryProcessDetailViewModel>> GetSalaryProcessDetailsAsync(long? salaryProcessId, long? salaryProcessDetailId, long? employeeId, long? fiscalYearId, short? month, short? year, long? branchId, string batchNo, AppUser user);
        Task<DBResponse<SalaryProcessDetailViewModel>> GetSalaryProcessDetailsAsync(SalaryProcessDetail_Filter filter, AppUser user);
        Task<ExecutionStatus> SalaryProcessDisbursedOrUndoAsync(SalaryProcessDisbursedOrUndoDTO model, AppUser user);
        Task<IEnumerable<SalaryAllowanceViewModel>> GetEmplyeeSalaryAllowancesAsync(long? employeeId, long? allowanceNameId, long? salaryProcessId, long? salaryProcessDetailId, long? fiscalYearId, short? month, short? year, string batchNo, AppUser user);
        Task<IEnumerable<SalaryDeductionViewModel>> GetEmplyeeSalaryDeductionsAsync(long? employeeId, long? deductionNameId, long? salaryProcessId, long? salaryProcessDetailId, short? month, short? year, string batchNo, AppUser user);
        Task<decimal> TillMonthSalaryAllowanceAmount(long employeeId, long allowanceNameId, string allowanceFlag, string firstDateOfthisMonth, long salaryMonth, long salaryYear, long fiscalYearId, string fiscalYearFrom, string fiscalYearTo, AppUser user);
        Task<decimal> CurrentMonthSalaryAllowanceAmount(long employeeId, long allowanceNameId, string allowanceFlag, string firstDateOfthisMonth, long salaryMonth, long salaryYear, long fiscalYearId, string fiscalYearFrom, string fiscalYearTo, AppUser user);
        Task<IEnumerable<SalaryBatchNoViewModel>> GetSalaryProcessBatchNoAsync(string isDisbursed, AppUser user);
        Task<IEnumerable<Dropdown>> GetSalaryProcessBatchNoDropdownAsync(string isDisbursed, AppUser user);
        Task<List<long>> GetEmployeeLastSalaryProcessedReviewIdsAsync(long employeeId, long? fiscalYearId, AppUser user);
        Task<List<long>> GetEmployeeSalaryProcessedReviewIdsOfaMonthAsync(long employeeId, int month, int year, long? fiscalYearId, AppUser user);
        Task<LastSalaryProcessInfoViewModel> GetLastPendingSalaryProcessInfoAsysnc(string processType, int salaryMonth, int salaryYear, AppUser user);
        Task<SalaryProcess> GetPendingSalaryProcessInfoAsync(string processType, int salaryMonth, int salaryYear, AppUser user);
        Task<string> GenerateBatchNoAysnc(int salaryMonth, int salaryYear, AppUser user);
        Task<ExecutionStatus> DisbursedSalaryProcessAsync(long salaryProcessId, AppUser user);
        Task<ExecutionStatus> UndoSalaryProcessAsync(long salaryProcessId, AppUser user);
        Task<int> GetSalaryReceiptInThisFiscalYearAsync(long employeeId, long fiscalYearId, int month, AppUser user);
        Task<int> GetSalaryReceiptInTotalAsync(long employeeId, AppUser user);
        Task<SalaryProcess> GetSalaryProcessByIdAsync(long salaryProcessId, AppUser user);
        Task<SalaryProcessDetail> GetSalaryProcessDetailByEmployeeIdAsync(long employeeId, int month, int year, AppUser user);
        Task<SalaryProcessDetail> GetSalaryProcessDetailByIdAsync(long salaryProcessDetailId, AppUser user);
        Task<DateTime?> GetEmployeeLastSalaryMonthInAIncomeYear(long employeeId, long fiscalYearId, AppUser user);
    }
}
