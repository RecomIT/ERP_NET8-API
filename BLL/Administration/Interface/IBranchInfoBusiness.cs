

using Shared.Control_Panel.Domain;
using Shared.OtherModels.User;

namespace BLL.Administration.Interface
{
    public interface IBranchInfoBusiness
    {
        Task<IEnumerable<Branch>> GetBranchsAsync(string branchName, AppUser user);
        Task<Branch> GetBranchByIdAsync(long id, AppUser user);
        Task<byte[]> GetLogo(long id, AppUser user);
    }
}
