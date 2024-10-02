using Shared.OtherModels.User;
using Shared.OtherModels.DataService;

namespace BLL.Employee.Interface.Miscellaneous
{
    public interface IDataLabelBusiness
    {
        Task<IEnumerable<Dropdown>> GetDataByLabelAsync(string label, AppUser user);
    }
}
