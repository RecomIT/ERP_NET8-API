using Shared.OtherModels.User;
using Shared.Payroll.DTO.Salary;

namespace BLL.Salary.Salary.Interface
{
    public interface ISalaryComponentHistoriesBusiness
    {
        Task<IEnumerable<SalaryComponentHistoryIdDTO>> GetComponentHistoryIds(long salaryProcessId, AppUser user);
    }
}
