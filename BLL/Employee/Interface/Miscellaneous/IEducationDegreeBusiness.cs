using Shared.OtherModels.User;
using Shared.OtherModels.DataService;

namespace BLL.Employee.Interface.Miscellaneous
{
    public interface IEducationalDegreeBusiness
    {
        Task<IEnumerable<Dropdown>> GetEducationDegreeDropdownAsync(AppUser user);
        Task<IEnumerable<Dropdown>> GetEducationDegreeDropdownAsync(long leaveOfEductionId, AppUser user);
    }
}
