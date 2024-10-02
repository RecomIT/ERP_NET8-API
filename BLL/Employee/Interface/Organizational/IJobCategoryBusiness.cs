using Shared.OtherModels.User;
using Shared.OtherModels.DataService;

namespace BLL.Employee.Interface.Organizational
{
    public interface IJobCategoryBusiness
    {
        Task<IEnumerable<Dropdown>> GetJobCategoryDropdownAsync(AppUser user);
    }
}
