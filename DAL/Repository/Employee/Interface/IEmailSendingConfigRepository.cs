using Shared.OtherModels.User;
using Shared.Employee.Domain.Setup;
using DAL.Repository.Base.Interface;

namespace DAL.Repository.Employee.Interface
{
    public interface IEmailSendingConfigRepository : IDapperBaseRepository<EmailSendingConfiguration>
    {
        Task<EmailSendingConfiguration> GetEmailSendingConfiguration(string moduleName, string emailStage, AppUser user);
    }
}
