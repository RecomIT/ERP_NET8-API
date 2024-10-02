using Shared.OtherModels.User;
using Shared.Employee.Domain.Info;
using DAL.Repository.Base.Interface;

namespace DAL.Repository.Employee.Interface
{
    public interface IDocumentRepository : IDapperBaseRepository<EmployeeDocument>
    {
        Task<EmployeeDocument> GetDocumentByNameAsync(string name, AppUser user);
        Task<IEnumerable<EmployeeDocument>> GetDocumentsByEmployeeIdAsync(long employeeId, AppUser user);
        Task<EmployeeDocument> GetDocumentByEmployeeIdAsync(long employeeId, string documentName, AppUser user);
        Task<bool> IsDocumentAlreadyExistInAnotherEmployeeAsync(long employee, string documentName, string documentNo, AppUser user);
    }
}
