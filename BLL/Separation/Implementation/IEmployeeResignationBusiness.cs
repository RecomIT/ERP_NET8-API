using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Separation.Models.DTO.Resignation;
using Shared.Separation.Models.Filter.Resignation;
using Shared.Separation.Models.ViewModel.Resignation;
using System.Threading.Tasks;

namespace BLL.Separation.Implementation
{
    public interface IEmployeeResignationRequestBusiness
    {
        Task<DBResponse<EmployeeResignationRequestViewModel>> GetEmployeeResignationsAsync(ResignationRequest_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveResignationRequestAsync(EmployeeResignationDTO model, AppUser user);
        Task<ExecutionStatus> ValidateResignationAsync(EmployeeResignationDTO model, AppUser user);
    }
}
