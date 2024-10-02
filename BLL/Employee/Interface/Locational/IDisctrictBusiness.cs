using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.Filter.Locational;
using Shared.Employee.DTO.Locational;
using Shared.Employee.ViewModel.Locational;

namespace BLL.Employee.Interface.Locational
{
    public interface IDistrictBusiness
    {
        Task<IEnumerable<DistrictViewModel>> GetDistrictsAsync(District_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveDistrictAsync(DistrictDTO model, AppUser user);
        Task<ExecutionStatus> ValidateDistrictAsync(DistrictDTO model, AppUser user);
    }
}
