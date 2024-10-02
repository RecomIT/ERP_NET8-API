using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.ViewModel.Locational;
using Shared.Employee.Filter.Locational;
using Shared.Employee.DTO.Locational;

namespace BLL.Employee.Interface.Locational
{
    public interface IPoliceStationBusiness
    {
        Task<IEnumerable<PoliceStationViewModel>> GetPoliceStationsAsync(PoliceStation_Filter filter, AppUser user);
        Task<ExecutionStatus> SavePoliceStationAsync(PoliceStationDTO model, AppUser user);
        Task<ExecutionStatus> ValidatePoliceStationAsync(PoliceStationDTO model, AppUser user);
    }
}
