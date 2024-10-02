
using System.Data;
using Shared.Employee.ViewModel.Report;
using Shared.OtherModels.User;

namespace BLL.Employee.Interface.Report
{
    public interface IHRLetterBusiness
    {
        Task<HRLetterEmployeeInfoViewModel> GetEmployeeInfoAsync(long id, AppUser user);
        Task<DataTable> SalaryBreakdownAsync(long id, AppUser user);
        Task<DataTable> SalaryHeadsAysnc(long id, AppUser user);
    }
}
