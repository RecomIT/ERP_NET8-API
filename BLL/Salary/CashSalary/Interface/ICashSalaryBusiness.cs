using System.Data;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.OtherModels.DataService;
using Shared.Payroll.DTO.CashSalary;
using Shared.Payroll.Filter.CashSalary;

namespace BLL.Salary.CashSalary.Interface
{
    public interface ICashSalaryBusiness
    {
        /// <summary>
        /// Report Head & Footer
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="companyId"></param>
        /// <param name="branchId"></param>
        /// <param name="divisionId"></param>
        /// <returns></returns>
        /// 

        Task<ExecutionStatus> UploadCashSalaryHeadExcelAsync(List<CashSalaryHeadDTO> salaryHeadReadDTOs, AppUser user);
        Task<ExecutionStatus> SaveCashSalaryHeadAsync(CashSalaryHeadDTO headDTO, AppUser user);
        Task<IEnumerable<CashSalaryHeadDTO>> GetCashSalaryHeadListAsync(long? cashSalaryHeadId, string cashSalaryHeadName, string cashSalaryHeadCode, string cashSalaryHeadNameInBengali, AppUser user);
        Task<IEnumerable<CashSalaryHeadDTO>> GetCashSalaryHeadByIdAsync(long cashSalaryHeadId, AppUser user);
        //Upload Cash Salary
        Task<IEnumerable<Select2Dropdown>> GetCashSalaryHeadExtensionAsync(long? cashSalaryHeadId, AppUser appUser);
        Task<ExecutionStatus> UploadCashSalaryExcelAync(List<UploadCashSalaryDTO> cashSalaryDTOs, AppUser user);
        Task<IEnumerable<UploadCashSalaryDTO>> UploadCashSalaryListAsync(long? uploadCashSalaryId, long? employeeId, long? cashSalaryHeadId, short salaryMonth, short salaryYear, string stateStatus, AppUser user);
        Task<ExecutionStatus> UpdateUploadCashSalaryAsync(UploadCashSalaryDTO dTO, AppUser user);
        Task<ExecutionStatus> SaveUploadCashSalaryApprovalAsync(long? uploadCashSalaryId, long? employeeId, long? cashSalaryHeadId, string stateStatus, string remarks, AppUser user);
        Task<ExecutionStatus> SaveCashSalariesAync(List<UploadCashSalaryDTO> cashSalaryDTOs, AppUser user);
        Task<ExecutionStatus> CashSalaryProcessAsync(CashSalaryProcessExecutionDTO data, AppUser user);
        Task<IEnumerable<CashSalaryProcessExecutionDTO>> GetCashSalaryProcessInfosAsync(long? cashSalaryProcessId, short? salaryMonth, short? salaryYear, DateTime? salaryDate, string batchNo, AppUser user);

        Task<ExecutionStatus> CashSalaryProcessDisbursedOrUndoAsync(long cashSalaryProcessId, string actionName, AppUser user);
        Task<DataTable> GetCashSalarySheetAsync(CashSalarySheet_Filter filter, AppUser user);
        Task<IEnumerable<CashSalaryProcessExecutionDTO>> GetCashSalaryProcessDetailAsync(long? cashSalaryProcessId, long? cashSalaryDetailId, long? employeeId, short? salaryMonth, short? salaryYear, DateTime? salaryDate, string batchNo, AppUser user);
        Task<DataTable> GetActualCashSalarySheetDetailAsync(CashSalarySheet_Filter filter, AppUser user);

    }
}
