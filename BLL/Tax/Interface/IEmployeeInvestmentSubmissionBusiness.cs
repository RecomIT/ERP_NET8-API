using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.DTO.Tax;
using Shared.Payroll.Filter.Tax;
using Shared.Payroll.ViewModel.Tax;

namespace BLL.Tax.Interface
{
    public interface IEmployeeInvestmentSubmissionBusiness
    {
        Task<ExecutionStatus> SaveEmployeeYearlyInvestmentAsync(EmployeeYearlyInvestmentDTO model, AppUser user);
        Task<DBResponse<EmployeeYearlyInvestmentViewModel>> GetEmployeeYearlyInvestmentsAsync(EmployeeYearInvestment_Filter filter, AppUser user);
        Task<decimal?> GetYearlInvestmentAmountInTaxProcessAsync(long employeeId, long fiscalYearId, AppUser user);
        Task<IEnumerable<EmployeeYearlyInvestmentViewModel>> GetEmployeeYearlyInvestmentByIdAsync(long id, AppUser user);
        Task<ExecutionStatus> UploadEmployeeYearlyInvestmentExcelAsync(List<EmployeeYearlyInvestmentViewModel> viewModels, AppUser user);
    }
}
