using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.DTO.Locational;
using Shared.Employee.Filter.Locational;
using Shared.Employee.ViewModel.Locational;

namespace BLL.Employee.Interface.Locational
{
    public interface ILocationBusiness
    {
        Task<IEnumerable<LocationViewModel>> GetLocationsAsync(Location_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveLocationAsync(LocationDTO model, AppUser user);
        Task<ExecutionStatus> ValidateLocationAsync(LocationDTO model, AppUser user);
    }
}
