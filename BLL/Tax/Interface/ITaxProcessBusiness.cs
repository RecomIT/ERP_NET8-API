using System.Data;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.ViewModel.Tax;
using Shared.Payroll.DTO.Tax;

namespace BLL.Tax.Interface
{
    public interface ITaxProcessBusiness
    {
        Task<ExecutionStatus> TaxProcessAsync(TaxProcessViewModel model, AppUser user);
        Task<ExecutionStatus> DeleteTaxProcessAsync(DeleteTaxInfoDTO model, AppUser user);
        Task<IEnumerable<TaxProcessSummeryInfoViewModel>> GetTaxProcessSummeryInfosAsync(long? fiscalYearId, short month, short year, AppUser user);
        Task<IEnumerable<EmployeeTaxProcesDetailInfosViewModel>> GetEmployeeTaxProcesDetailInfosAsync(long? employeeId, long fiscalYearId, long? branchId, long? salaryProcessId, short month, short year, AppUser user);
        Task<DataTable> GetTaxSheetData(long fiscalYearId, short month, short year, AppUser user);
        Task<IEnumerable<TaxDetailInTaxProcess>> GetTaxableAllowanceIncomeDetail(long employeeId, long fiscalYearId, string firstDateOfThisMonth, short salaryMonth, short salaryYear, string fiscalYearFrom, string fiscalYearTo, short remainProjectionMonthForThisEmployee, AppUser user);

        Task<IEnumerable<EmployeeRamainFestivalBonusViewModal>> GetEmployeeRamainFestivalBonusAsync(long fiscalYearId, string religionName, string dateOfConfirmation, string taxProcessDate, AppUser user);

        //Added by Monzur 07-Jan-24
        Task<ExecutionStatus> UploadTaxChallanAsync(List<TaxChallanDTO> taxChallanDTOs, AppUser user);
    }
}
