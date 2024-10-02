using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.ViewModel.Bonus;
using Shared.Payroll.Domain.Bonus;
using Shared.Payroll.ViewModel.Salary;
using Shared.Payroll.ViewModel.Setup;
using Shared.Payroll.Filter.Bonus;
using Shared.Payroll.DTO.Bonus;

namespace BLL.Salary.Bonus.Interface
{
    public interface IBonusProcessBusiness
    {
        Task<ExecutionStatus> ExecuteBonusProcessAsync(ExecuteBonusProcess bonusProcess, AppUser user);
        Task<IEnumerable<EligibleEmployeesForBonus>> GetEligibleEmployeesAsync(ExecuteBonusProcess bonusProcess, AppUser user);
        Task<BonusTaxAmount> BonusTaxProcessAsync(BonusTaxProcess taxProcess, EligibleEmployeesForBonus employee, FiscalYearViewModel fiscalYearInfo, SalaryReviewInfoViewModel currentSalaryReview, IEnumerable<SalaryReviewDetailViewModel> currentSalaryReviewDetails, AppUser user);
        Task<DBResponse<BonusProcessViewModel>> GetBonusProcessesInfoAsync(BonusProcessInfo_Filter filter, AppUser user);
        Task<DBResponse<BonusProcessDetailViewModel>> GetBonusProcessDetailAsync(BonusProcessDetail_Filter filter, AppUser user);
        Task<ExecutionStatus> DisbursedBonusAsync(DisbursedUndoBonusDTO model, AppUser user);
        Task<ExecutionStatus> UndoBonusAsync(DisbursedUndoBonusDTO model, AppUser user);
        Task<ExecutionStatus> UndoEmployeeBonusAsync(UndoEmployeeBonus model, AppUser user);
        Task<ExecutionStatus> SaveExcludeEmployeeFromBonusAsync(EmployeeExcludedFromBonusDTO model, AppUser user);
        Task<IEnumerable<EmployeeExcludedFromBonusViewModel>> GetExcludedEmployeesFromBonusAsync(ExcludeEmployeedFromBonus_Filter filter, AppUser user);
        Task<ExecutionStatus> DeleteEmployeeFromExcludeListAsync(EmployeeExcludedFromBonusDTO model, AppUser user);

    }
}
