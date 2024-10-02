using Shared.OtherModels.DataService;
using Shared.OtherModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.PF.Interface
{
    public interface IPFServiceBusiness
    {
        Task<IEnumerable<Select2Dropdown>> GetPFEmployeesAsync(long? notEmployee, long designationId, long departmentId, long sectionId, long subsectionId, AppUser user);
    }
}
