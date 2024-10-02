using Shared.OtherModels.User;

namespace BLL.Employee.Interface.Training
{
    public interface ITrainingBusiness
    {
        Task<IEnumerable<object>> GetAllTrainingAsync(dynamic filter, AppUser user);
        Task<object> SaveTrainingRequestAsync(dynamic filter, AppUser user);
        Task<IEnumerable<object>> GetTrainingRequestsAsync(dynamic filter, AppUser user);
    }
}
