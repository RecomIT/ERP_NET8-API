using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;

namespace BLL.Dashboard.DataService.Interface
{
    public interface IDataGetService
    {
        // ------------------------------------------ >>> GetDataAsync
        Task<object> GetDataWithoutEmployeeAsync<T>(AppUser user, string sqlQuery, string businessClassName, string methodName);
        // ------------------------------------------ >>> GetDataAsync
        Task<object> GetDataAsync<T>(AppUser user, string sqlQuery, string businessClassName, string methodName);

        // ------------------------------------------ >>> GetDataAsync with Filter
        Task<IEnumerable<T>> GetDataAsync<T>(AppUser user, string sqlQuery, string businessClassName, string methodName, object filter = null);




        Task<object> GetDataAsync<T>(AppUser user, string sqlQuery);

        // ------------------------------------------ >>> GetDataAsync with Filter
        Task<IEnumerable<T>> GetDataAsync<T>(AppUser user, string sqlQuery, object filter = null);
    }
}
