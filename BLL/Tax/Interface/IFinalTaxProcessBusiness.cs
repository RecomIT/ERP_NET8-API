using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.Domain.Setup;
using Shared.Payroll.Domain.Tax;
using Shared.Payroll.DTO.Tax;
using Shared.Payroll.Process.Tax;
using Shared.Payroll.ViewModel.Tax;
using System.Data;

namespace BLL.Tax.Interface
{
    public interface IFinalTaxProcessBusiness
    {
        Task<ExecutionStatus> FinalTaxProcessAsync(FinalTaxProcessDTO model, AppUser user);
        Task<ExecutionStatus> SaveAsync(List<FinalTaxProcessedInfo> infos, AppUser user);
        Task<ExecutionStatus> DeleteAsync(long employeeId, long fiscalYearId, AppUser user);
        Task<IEnumerable<EmployeeInfoForFinalTaxProcessViewModel>> GetEmployeesAsync(long fiscalYearId, string flag, long branchId, AppUser user);
        Task<EligibleEmployeeForTaxType> GetEligibleEmployeeForTaxProcess(long employeeId, FiscalYear fiscalYear, AppUser user);
        Task<IEnumerable<TaxProcessSummeryInfoViewModel>> GetFinalTaxProcessSummaryAsync(long fiscalYearId, long branchId, AppUser user);
        Task<IEnumerable<EmployeeTaxProcesDetailInfosViewModel>> GetFinalTaxProcesSummariesAsync(long employeeId, long fiscalYearId, long branchId, AppUser user);
        Task<DataTable> Download108Report(long fiscalYearId, long branchId, AppUser user);
        IEnumerable<EmployeeTaxProcessDetail> GetTaxProcessDetails(long employeeId, long fiscalYearId, int year, int month, AppUser user);
    }
}
