using Shared.OtherModels.User;


namespace BLL.Separation.Interface.User
{
    public interface IUserResignationBusiness
    {
        Task<IEnumerable<object>> GetUserResignationRequestsAsync(dynamic filter, AppUser user);
    }
}
