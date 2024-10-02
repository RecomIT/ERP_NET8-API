
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.DTO.Organizational.InternalDesignation;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.ViewModel.Organizational.InternalDesignation;

namespace BLL.Employee.Interface.Organizational
{
    public interface IInternalDesignationBusiness
    {
        // Internal Designation
        Task<ExecutionStatus> SaveInternalDesignationAsync(InternalDesignationDTO designationDTO, AppUser user);
        Task<DBResponse<InternalDesignationViewModel>> GetInternalDesignationsAsync(InternalDesignation_Filter filter, AppUser user);
        Task<IEnumerable<InternalDesignationViewModel>> GetInternalDesignationByIdAync(long internalDesignationId, AppUser user);
        Task<ExecutionStatus> UploadInternalDesignationExcelAsync(List<InternalDesignationDTO> internalDesignationDTOs, AppUser user);
        Task<IEnumerable<InternalDesignationViewModel>> GetInternalDesignationListAsync(InternalDesignation_Filter filter, AppUser user);
    }
}
