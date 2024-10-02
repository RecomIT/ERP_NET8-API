using Dapper;
using System.Data;
using Shared.Services;
using DAL.DapperObject;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.ViewModel.Setup;
using BLL.Employee.Interface.Organizational;
using DAL.DapperObject.Interface;

namespace BLL.Employee.Implementation.Organizational
{
    public class EmailSendingConfigurationBusiness : IEmailSendingConfigurationBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapperData;
        public EmailSendingConfigurationBusiness(ISysLogger sysLogger, IDapperData dapperData)
        {
            _dapperData = dapperData;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<EmailSendingConfigurationViewModel>> GetEmailSendingConfigurationAsync(int Id, AppUser user)
        {
            IEnumerable<EmailSendingConfigurationViewModel> data = new List<EmailSendingConfigurationViewModel>();
            try
            {
                var query = $@"Select * From HR_EmailSendingConfiguration 
				Where 1=1
				And (@Id IS NULL OR @Id = 0 OR Id = @Id)				
				And (CompanyId = @CompanyId)
				And (OrganizationId = @OrganizationId)";
                var parameters = new DynamicParameters();
                parameters.Add("Id", Id);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                data = await _dapperData.SqlQueryListAsync<EmailSendingConfigurationViewModel>(user.Database, query, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmailSendingConfigurationBusiness", "GetEmailSendingConfigurationAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveEmailSendingConfigurationAsync(EmailSendingConfigurationViewModel EmailSendingConfiguration, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var query = $@"sp_HR_EmailSendingConfiguration";
                var parameters = new DynamicParameters();
                parameters.Add("Id", EmailSendingConfiguration.Id);
                parameters.Add("ModuleName", EmailSendingConfiguration.ModuleName);
                parameters.Add("EmailStage", EmailSendingConfiguration.EmailStage);
                parameters.Add("EmailCC1", EmailSendingConfiguration.EmailCC1);
                parameters.Add("EmailCC2", EmailSendingConfiguration.EmailCC2);
                parameters.Add("EmailCC3", EmailSendingConfiguration.EmailCC3);
                parameters.Add("EmailBCC1", EmailSendingConfiguration.EmailBCC1);
                parameters.Add("EmailBCC2", EmailSendingConfiguration.EmailBCC2);
                parameters.Add("EmailTo", EmailSendingConfiguration.EmailTo);
                parameters.Add("IsActive", EmailSendingConfiguration.IsActive);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrgId", user.OrganizationId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("Flag", EmailSendingConfiguration.Id > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapperData.SqlQueryFirstAsync<ExecutionStatus>(user.Database, query, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmailSendingConfigurationBusiness", "SaveEmailSendingConfigurationAsync", user);
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> EmailSendingConfigurationValidatorAsync(EmailSendingConfigurationViewModel EmailSendingConfiguration, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var query = $@"sp_HR_EmailSendingConfiguration";
                var parameters = new DynamicParameters();
                parameters.Add("Id", EmailSendingConfiguration.Id);
                parameters.Add("ModuleName", EmailSendingConfiguration.ModuleName);
                parameters.Add("EmailStage", EmailSendingConfiguration.EmailStage);
                parameters.Add("EmailCC1", EmailSendingConfiguration.EmailCC1);
                parameters.Add("EmailCC2", EmailSendingConfiguration.EmailCC2);
                parameters.Add("EmailCC3", EmailSendingConfiguration.EmailCC3);
                parameters.Add("EmailBCC1", EmailSendingConfiguration.EmailBCC1);
                parameters.Add("EmailBCC2", EmailSendingConfiguration.EmailBCC2);
                parameters.Add("EmailTo", EmailSendingConfiguration.EmailTo);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrgId", user.OrganizationId);
                parameters.Add("Flag", Data.Validate);
                executionStatus = await _dapperData.SqlQueryFirstAsync<ExecutionStatus>(user.Database, query, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmailSendingConfigurationBusiness", "EmailSendingConfigurationValidatorAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<Select2Dropdown>> LoadModuleNameAsync(AppUser user)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var query = $@"SELECT mm.MMId 'Id',mm.MenuName as 'Text' FROM tblMainMenus mm
				Inner Join tblCompanyAuthorization ca On mm.MMId = ca.MainmenuId
				Where ca.CompanyId=@CompanyId";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                data = await _dapperData.SqlQueryListAsync<Select2Dropdown>(Database.ControlPanel, query, null, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmailSendingConfigurationBusiness", "LoadModuleNameAsync", user);
            }
            return data;
        }
    }
}
