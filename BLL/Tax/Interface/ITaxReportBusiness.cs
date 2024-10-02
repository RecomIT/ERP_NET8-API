using System.Data;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.DTO.Tax;
using Shared.Payroll.Report;

namespace BLL.Tax.Interface
{
    public interface ITaxReportBusiness
    {
        Task<TaxCardMaster> TaxCardAsync(long employeeId, long taxProcessId, long fiscalYearId, short month, short year, bool? isDisbursed, AppUser user);
        Task<DataTable> TaxCardInfoAsync(long employeeId, long taxProcessId, long fiscalYearId, short month, short year, bool? isDisbursed, AppUser user);
        Task<DataTable> TaxCardDetailAsync(long employeeId, long taxProcessId, long fiscalYearId, short month, short year, AppUser user);
        Task<DataTable> TaxCardSlabAsync(long employeeId, long taxProcessId, long fiscalYearId, short month, short year, AppUser user);
        Task<DataTable> TaxChallanAsync(long employeeId, long fiscalYearId, AppUser user);
        Task<DataTable> TaxCardInfoFY22_23(AppUser user);
        Task<DataTable> TaxChallanFY22_23(AppUser user);
        Task<DataTable> TaxDetailFY22_23(string employeeCode, AppUser user);
        Task<DataTable> GetTaxSheetDetailsReport(long employeeId, long fiscalYearId, short salaryMonth, short salaryYear, long salaryProcessId, long salaryProcessDetailId, AppUser user);
        Task<ExecutionStatus> UploadActaulTaxDeductionValidatorAsync(List<ActualTaxDeductionDTO> model, AppUser user);
        Task<ExecutionStatus> UploadActaulTaxDeductionAsync(List<ActualTaxDeductionDTO> model, AppUser user);
        Task<ExecutionStatus> UpdateActaulTaxDeductionInSalaryAndTaxAsync(UpdateActaulTaxDeductedDTO model, AppUser user);
        Task<DataTable> FinalTaxCardInfoAsync(long employeeId, long fiscalYearId, AppUser user);
        Task<DataTable> FinalTaxCardDetailAsync(long employeeId, long fiscalYearId, AppUser user);
        Task<DataTable> FinalTaxCardSlabAsync(long employeeId, long fiscalYearId, AppUser user);

        Task<DataTable> SupplementaryTaxCardInfoAsync(long employeeId, long fiscalYearId, long paymenAmountId, AppUser user);
        Task<DataTable> SupplementaryTaxCardDetailAsync(long employeeId, long fiscalYearId, long paymenAmountId, AppUser user);
        Task<DataTable> SupplementaryTaxCardSlabAsync(long employeeId, long fiscalYearId, long paymenAmountId, AppUser user);
    }
}
