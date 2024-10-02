using Shared.Employee.Filter.Info;
using Shared.OtherModels.User;
using System.Threading.Tasks;

namespace BLL.Asset.Implementation.Create
{
    internal interface IEmployeeRepository
    {
        Task GetEmployeeServiceDataAsync(EmployeeService_Filter employeeService_Filter, AppUser user);
    }
}