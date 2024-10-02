using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.DTO.Payment;
using Shared.Payroll.Domain.Setup;
using Shared.Control_Panel.Domain;
using Shared.Payroll.Process.Allowance;
using Shared.Payroll.Process.Tax;
using Shared.Payroll.ViewModel.Payment;

namespace BLL.Salary.Payment.Interface
{
    public interface ISupplementaryPaymentProcessBusiness
    {
        Task<ExecutionStatus> ProcessAsync(SupplementaryProcessDTO model, AppUser user);
        Task<ExecutionStatus> SaveAsync(SupplementaryProcessDTO model, List<SupplementaryPaymentProcessSaveDTO> list, FiscalYear fiscalYearInfo, AppUser user);
        Task<SupplementaryPaymentProcessSaveDTO> TaxProcessAsync(
            FiscalYear fiscalYearInfo,
            PayrollModuleConfig payrollModuleConfig,
            AllowanceInfo allowanceInfo,
            SupplementaryAmountDTO model,
            AppUser user);
        Task<EligibleEmployeeForTaxType> GetEmployeeInfoAsync(long employeeId, short month, short year, AppUser user);
        Task<UndisbursedSupplementaryPaymentInfoViewModel> UndisbursedSupplementaryPaymentInfoAsync(long id, AppUser user);
        Task<ExecutionStatus> DisbursedOrUndoPaymentAsync(DisbursedOrUndoPaymentDTO model, AppUser user);
        Task<SupplementaryPaymentInfoAndDetailViewModel> GetSupplementaryPaymentInfoAndDetailAsync(long id, AppUser user);
        Task<List<SupplementaryPaymentProcessSaveDTO>> ProcessData(SupplementaryProcessDTO model, FiscalYear fiscalYearInfo, AppUser user);
        Task<ExecutionStatus> UpdatePaymentAmountAsync(SupplementaryPaymentAmountDTO item, AppUser user);
        Task<ExecutionStatus> UpdateSupplementaryTaxAmountAsync(long processId,List<UpdateSupplementaryTaxAmountDTO> employees, AppUser user);
    }
}
