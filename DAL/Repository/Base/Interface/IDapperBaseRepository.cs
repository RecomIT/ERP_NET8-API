using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;

namespace DAL.Repository.Base.Interface
{
    public interface IDapperBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(long id, AppUser user);
        Task<IEnumerable<T>> GetAllAsync(AppUser user);
        Task<int> DeleteByIdAsync(long id, AppUser user);
        Task<IEnumerable<T>> GetAllAsync(object filter, AppUser user);
    }
}
