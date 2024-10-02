using System.Data;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.ViewModel.Payment;
using Shared.Payroll.Filter.Payment;
using Shared.Payroll.DTO.Payment;
using Shared.Payroll.Process.Allowance;

namespace BLL.Salary.Payment.Interface
{
    public interface ISupplementaryPaymentAmountBusiness
    {
        Task<DBResponse<SupplementaryPaymentAmountViewModel>> GetSupplementaryPaymentAmountInfosAsync(SupplementaryPaymentAmount_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveSupplementaryPaymentAmountAsync(SupplementaryPaymentAmountDTO model, AppUser user);
        Task<ExecutionStatus> SaveBulkSupplementaryPaymentAmountAsync(List<SupplementaryPaymentAmountDTO> model, AppUser user);
        Task<ExecutionStatus> ValidatePaymentAsync(List<SupplementaryPaymentAmountDTO> model, AppUser user);
        Task<IEnumerable<SupplementaryPaymentAmountViewModel>> GetSupplementaryPaymentAmountsForProcessAsync(SupplementaryPaymentAmount_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveSupplementaryProcessAsync(SupplementaryProcessDTO model, AppUser user);
        Task<DBResponse<SupplementaryPaymentProcessInfoViewModel>> GetSupplementaryPaymentProcessInfosAsync(SupplementaryPaymentProcessInfo_Filter filter, AppUser user);
        Task<ExecutionStatus> UploadSupplementaryPaymentAmountAsync(List<SupplementaryProcessExcelReadDTO> upload, AppUser user);
        Task<DataTable> GetSupplementaryTaxSheetDetailsReport(long paymentProcessInfoId, long employeeId, long fiscalYearId, short paymentMonth, short paymentYear, AppUser user);
        Task<DataTable> GetSupplementaryPaymentReport(long paymentProcessInfoId, long employeeId, long fiscalYearId, short paymentMonth, short paymentYear, AppUser user);
        Task<IEnumerable<AllowanceDisbursedAmount>> GetThisMonthSupplementaryAmountInTaxProcess(long employeeId, long fiscalYearId, short paymentMonth, short paymentYear, AppUser user);
        Task<ExecutionStatus> DeleteSupplementaryAmountAsync(DeletePaymentDTO model, AppUser user);
        Task<SupplementaryPaymentAmountViewModel> GetSupplementaryAmountByIdAsync(long paymentAmountId, AppUser user);
    }
}
