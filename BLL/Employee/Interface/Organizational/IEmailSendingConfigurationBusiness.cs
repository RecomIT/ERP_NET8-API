using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.ViewModel.Setup;

namespace BLL.Employee.Interface.Organizational
{
    public interface IEmailSendingConfigurationBusiness
    {
        Task<IEnumerable<EmailSendingConfigurationViewModel>> GetEmailSendingConfigurationAsync(int Id, AppUser user);
        Task<ExecutionStatus> SaveEmailSendingConfigurationAsync(EmailSendingConfigurationViewModel EmailSendingConfiguration, AppUser user);
        Task<ExecutionStatus> EmailSendingConfigurationValidatorAsync(EmailSendingConfigurationViewModel EmailSendingConfiguration, AppUser user);
        Task<IEnumerable<Select2Dropdown>> LoadModuleNameAsync(AppUser user);
    }
}
