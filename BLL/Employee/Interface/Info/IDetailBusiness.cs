using Shared.OtherModels.User;
using Shared.Employee.ViewModel.Info;
using Shared.Employee.Filter.Info;

namespace BLL.Employee.Interface.Info
{
    public interface IDetailBusiness
    {
        Task<EmployeePersonalInfoList> GetEmployeePersonalInfoByIdAsync(EmployeePersonalInfoQuery query, AppUser User);
    }
}
