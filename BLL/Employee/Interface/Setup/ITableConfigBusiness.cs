using Shared.Employee.ViewModel.Setup;
using Shared.OtherModels.User;

namespace BLL.Employee.Interface.Setup
{
    public interface ITableConfigBusiness
    {
        Task<IEnumerable<TableConfigViewModel>> GetColumnsAsync(string table, string purpose, AppUser user);
    }
}
