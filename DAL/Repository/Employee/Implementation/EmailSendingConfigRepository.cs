using DAL.Logger.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.Employee.Domain.Setup;
using DAL.Repository.Employee.Interface;

namespace DAL.Repository.Employee.Implementation
{
    public class EmailSendingConfigRepository : IEmailSendingConfigRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;

        public EmailSendingConfigRepository(IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmailSendingConfiguration>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmailSendingConfiguration>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<EmailSendingConfiguration> GetByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<EmailSendingConfiguration> GetEmailSendingConfiguration(string moduleName, string emailStage, AppUser user)
        {
            EmailSendingConfiguration emailSending = null;
            try
            {
                var query = $@"Select * from HR_EmailSendingConfiguration Where 1=1	and ModuleName=@ModuleName and EmailStage=@EmailStage and IsActive=1
				and CompanyId = @CompanyId and OrganizationId = @OrganizationId";
                emailSending = await _dapper.SqlQueryFirstAsync<EmailSendingConfiguration>(user.Database, query, new { ModuleName = moduleName, EmailStage = emailStage, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmailSendingConfigRepository", "GetEmailSendingConfiguration", user);
            }
            return emailSending;
        }
    }
}
