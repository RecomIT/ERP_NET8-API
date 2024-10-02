
using BLL.Base.Interface;
using Dapper;
using Shared.OtherModels.Pagination;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;
using System.Net.Mail;
using System.Net;
using Shared.OtherModels.DataService;
using BLL.Asset.Interface.Assigning;
using DAL.DapperObject.Interface;
using Shared.Asset.Filter.Create;
using Shared.Asset.ViewModel.Create;
using Shared.Asset.ViewModel.Assigning;
using Shared.Asset.DTO.Assigning;
using Shared.Asset.ViewModel.Email;
using Shared.Asset.Filter.Assigning;

namespace BLL.Asset.Implementation.Assigning
{

    public class AssigningBusiness : IAssigningBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public AssigningBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<Dropdown>> GetProductDropdownAsync(Product_Filter filter, AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try {
                var list = await this.ProductDropdownAsync(filter, user);
                foreach (var item in list) {
                    dropdowns.Add(new Dropdown() {
                        Id = item.AssetId,
                        Value = item.ProductId.ToString(),
                        Text = item.ProductId.ToString()
                    });
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportBusiness", "GetProductDropdownAsync", user);
            }
            return dropdowns;
        }
        public async Task<IEnumerable<ProductViewModel>> ProductDropdownAsync(Product_Filter filter, AppUser user)
        {
            IEnumerable<ProductViewModel> list = new List<ProductViewModel>();
            try {
                var sp_name = $@"Select ProductId,ProductId From Asset_Product
                Where 1=1 AND (CompanyId = @CompanyId) AND (OrganizationId = @OrganizationId)";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<ProductViewModel>(user.Database, sp_name, parameters, CommandType.Text);

            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CreateBusiness", "ProductAsync", user);
            }
            return list;
        }
        public async Task<DBResponse<AssigningViewModel>> GetAssignedDataAsync(Assigning_Filter filter, AppUser user)
        {
            DBResponse<AssigningViewModel> data = new DBResponse<AssigningViewModel>();
            DBResponse response = new DBResponse();
            try {
                var sp_name = "sp_Asset_Assigning_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<AssigningViewModel>>(response.JSONData) ?? new List<AssigningViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssetCreateBusiness", "GetAssignedDataAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveAssetAssigningAsync(Assigning_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Assigning_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", model.AssigningId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssigningBusiness", "SaveAssetAssigningAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidatorAssetAssigningAsync(Assigning_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Assigning_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Shared.Services.Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssigningBusiness", "ValidatorAssetAssigningAsync", user);
            }
            return executionStatus;
        }      
        public async Task<ExecutionStatus> ApprovedAssetAsync(long assetId, long assigningId, string activeTab, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {

                if (activeTab == "assetCreateApproval") {
                    var sp_name = @"UPDATE Asset_Create SET Approved = 1 WHERE AssetId = @AssetId AND CompanyId = @CompanyId AND OrganizationId = @OrganizationId";
                    var parameters = new DynamicParameters();
                    parameters.Add("@AssetId", assetId);
                    parameters.Add("@CompanyId", user.CompanyId);
                    parameters.Add("@OrganizationId", user.OrganizationId);
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.Text);
                }

                if (activeTab == "assetAssigningApproval") {
                    var sp_name = @"UPDATE Asset_Assigning SET Approved = 1 WHERE AssigningId = @AssigningId AND AssetId = @AssetId AND CompanyId = @CompanyId AND OrganizationId = @OrganizationId";
                    var parameters = new DynamicParameters();
                    parameters.Add("@AssigningId", assigningId);
                    parameters.Add("@AssetId", assetId);
                    parameters.Add("@CompanyId", user.CompanyId);
                    parameters.Add("@OrganizationId", user.OrganizationId);
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.Text);

                    var sp_name1 = @"UPDATE Asset_Product SET Assigned = 1 WHERE AssigningId = @AssigningId AND AssetId = @AssetId AND CompanyId = @CompanyId AND OrganizationId = @OrganizationId";
                    var parameters1 = new DynamicParameters();
                    parameters1.Add("@AssigningId", assigningId);
                    parameters1.Add("@AssetId", assetId);
                    parameters1.Add("@CompanyId", user.CompanyId);
                    parameters1.Add("@OrganizationId", user.OrganizationId);
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name1, parameters1, CommandType.Text);

                }

                if (executionStatus == null) {
                    executionStatus = new ExecutionStatus { Status = true, Msg = "Asset approved successfully." };
                }


            }
            catch (Exception ex) {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssigningBusiness", "ApprovedAssetAsync", user);
            }
            return executionStatus;
        }
        public async Task<DBResponse<CreateViewModel>> GetAssetAsync(Create_Filter filter, AppUser user)
        {
            DBResponse<CreateViewModel> data = new DBResponse<CreateViewModel>();
            DBResponse response = new DBResponse();
            try {
                var sp_name = "sp_Asset_Create_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                parameters.Add("ExecutionFlag", "Show");
               response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<CreateViewModel>>(response.JSONData) ?? new List<CreateViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssigningBusiness", "GetAssetAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<ProductViewModel>> GetProductAsync(Product_Filter filter, AppUser user)
        {
            IEnumerable<ProductViewModel> list = new List<ProductViewModel>();
            try {
                var sp_name = "sp_Asset_Product_List";
                var parameters = Utility.DappperParams(filter, user);
                if (filter.Type == "Create") {
                    parameters.Add("ExecutionFlag", "List");
                }
                if (filter.Type == "Assigning") {
                    parameters.Add("ExecutionFlag", "Assigning");
                }
                if (filter.Type == "Approved") {
                    parameters.Add("ExecutionFlag", "Show");
                }

                list = await _dapper.SqlQueryListAsync<ProductViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssigningBusiness", "GetProductAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<EmailSendViewModel>> EmailSendAsync(long employeeId, AppUser user)
        {
            IEnumerable<EmailSendViewModel> emailDtls = new List<EmailSendViewModel>();
            try {
                var sp_name = "sp_Asset_EmailSend";
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", user.UserId);
                parameters.Add("@EmployeeId", employeeId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", "Email");

                emailDtls = await _dapper.SqlQueryListAsync<EmailSendViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);

                if (emailDtls != null) {
                    var emailSetting = Utility.JsonToObject<IEnumerable<EmailSettingViewModel>>(emailDtls.First().Json).FirstOrDefault();
                    var email = Utility.JsonToObject<IEnumerable<EmailSendViewModel>>(emailDtls.First().JsonEmailCCBCC).FirstOrDefault();
                    if (emailSetting != null) {
                        await EmailSend(emailDtls.First().EmailTo, email, emailSetting, emailDtls.First().EmpDtls, emailDtls.First().UserEmail, emailDtls.First().UserEmail, "");
                    }
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssigningBusiness", "EmailSendAsync", user);
            }
            return emailDtls;
        }
        private async Task<bool> EmailSend(string toEmail, EmailSendViewModel email, EmailSettingViewModel emailSetting, string empDtls,string userEmail, string userName, string flag)
        {
            var Subject = "";
            try {

                flag = "Asset Assigning";
                MailMessage message = new MailMessage();
                message.From = new MailAddress(emailSetting.EmailAddress, "Asset Notification");
                Subject = "Asset assigning approval request";
                message.To.Add(new MailAddress(toEmail));

                message.Subject = Subject;
                message.IsBodyHtml = emailSetting.IsBodyHtml;
                message.Body = AssetEmailTemplate.GetAssetEmailTemplate(flag, empDtls, userName);

                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = emailSetting.EnableSsl;
                smtp.UseDefaultCredentials = emailSetting.UseDefaultCredentials;
                smtp.Port = Convert.ToInt32(emailSetting.Port);
                smtp.Host = emailSetting.Host;
                smtp.Credentials = new NetworkCredential(emailSetting.EmailAddress, emailSetting.EmailPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(message);

                return true;

            }
            catch (Exception ex) {
                return false;
            }
        }
    }
}
