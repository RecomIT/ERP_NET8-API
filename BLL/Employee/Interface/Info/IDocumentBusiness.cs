using Shared.OtherModels.User;
using Shared.Employee.DTO.Info;
using Shared.OtherModels.Response;
using Shared.Employee.Filter.Info;
using Shared.Employee.ViewModel.Info;


namespace BLL.Employee.Interface.Info
{
    public interface IDocumentBusiness
    {
        Task<ExecutionStatus> SaveEmployeeDocumentAsync(EmployeeDocumentDTO model, AppUser user);
        Task<IEnumerable<EmployeeDocumentViewModel>> GetEmployeeDocumentsAsync(EmployeeDocument_Filter filter, AppUser user);
    }
}
