using DAL.Logger.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.Employee.Domain.Info;
using DAL.Repository.Employee.Interface;
using DAL.Context.Employee;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Employee.Implementation
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly EmployeeModuleDbContext _employeeModuleDbContext;

        public DocumentRepository(IDALSysLogger sysLogger, EmployeeModuleDbContext employeeModuleDbContext, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _employeeModuleDbContext = employeeModuleDbContext;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<EmployeeDocument>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<EmployeeDocument>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<EmployeeDocument> GetByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<EmployeeDocument> GetDocumentByEmployeeIdAsync(long employeeId, string documentName, AppUser user)
        {
            EmployeeDocument employeeDocument = new EmployeeDocument();
            try
            {
                var query = $@"SELECT * FROM HR_EmployeeDocument Where DocumentName=@Name AND EmployeeId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                employeeDocument = await _dapper.SqlQueryFirstAsync<EmployeeDocument>(user.Database, query, new { Id = employeeId, Name = documentName, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DiscontinuedEmployeeRepository", "GetDocumentByEmployeeIdAsync", user);
            }
            return employeeDocument;
        }
        public Task<EmployeeDocument> GetDocumentByNameAsync(string name, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<EmployeeDocument>> GetDocumentsByEmployeeIdAsync(long employeeId, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsDocumentAlreadyExistInAnotherEmployeeAsync(long employee, string documentName, string documentNo, AppUser user)
        {
            EmployeeDocument employeeDocument = null;
            try
            {
                employeeDocument = await _employeeModuleDbContext.HR_EmployeeDocument.FirstOrDefaultAsync(i =>
                i.EmployeeId != employee
                && i.DocumentName == documentName
                && i.DocumentNumber == documentNo
                && i.CompanyId == user.CompanyId
                && i.OrganizationId == user.OrganizationId);
            }
            catch (Exception)
            {
            }
            return employeeDocument !=null;
        }
    }
}
