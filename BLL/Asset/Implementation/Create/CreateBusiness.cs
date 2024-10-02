
using BLL.Base.Interface;
using Dapper;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Pagination;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;
using System.Net.Mail;
using System.Net;
using BLL.Asset.Interface.Create;
using Shared.Asset.ViewModel.Create;
using Shared.Asset.Filter.Create;
using Shared.Asset.DTO.Create;
using Shared.Asset.ViewModel.Email;
using DAL.DapperObject.Interface;


namespace BLL.Asset.Implementation.Create{

    public class CreateBusiness : ICreateBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public CreateBusiness (
            IDapperData dapper,
            ISysLogger sysLogger)            
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<DBResponse<CreateViewModel>> GetAssetAsync(Create_Filter filter, AppUser user)
        {
            DBResponse<CreateViewModel> data = new DBResponse<CreateViewModel>();
            DBResponse response = new DBResponse();
            try {
                var sp_name = "sp_Asset_Create_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                parameters.Add("ExecutionFlag", "List");
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<CreateViewModel>>(response.JSONData) ?? new List<CreateViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CreateBusiness", "GetAssetAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveAssetAsync(Create_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Create_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", model.AssetId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "CreateBusiness", "SaveAssetAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidatorAssetAsync(Create_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Create_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Shared.Services.Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "CreateBusiness", "ValidatorAssetAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<Dropdown>> GetAssetDropdownAsync(Create_Filter filter, AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try {
                var list = await this.AssetDropdownAsync(filter, user);
                foreach (var item in list) {
                    dropdowns.Add(new Dropdown() {
                        Id = item.AssetId,
                        Value = item.AssetId.ToString(),
                        Text = item.AssetName.ToString()
                    });
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CreateBusiness", "GetAssetDropdownAsync", user);
            }
            return dropdowns;
        }
        public async Task<IEnumerable<CreateViewModel>> AssetDropdownAsync(Create_Filter filter, AppUser user)
        {
            IEnumerable<CreateViewModel> list = new List<CreateViewModel>();
            try {
                var sp_name = $@"Select AssetId,AssetName From Asset_Create
                Where 1=1 AND (CompanyId = @CompanyId) AND (OrganizationId = @OrganizationId)";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<CreateViewModel>(user.Database, sp_name, parameters, CommandType.Text);

            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CreateBusiness", "AssetDropdownAsync", user);
            }
            return list;
        }


      



        public async Task<ExecutionStatus> ValidatorUploadExcelAsync(List<UploadFile_DTO> upload, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Product_Insert";
                var jsonData = Utility.JsonData(upload);
                var parameters = new DynamicParameters();
                parameters.Add("JsonData", jsonData);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Shared.Services.Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "CreateBusiness", "ValidatorUploadExcelAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UploadExcelAsync(List<UploadFile_DTO> upload, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try {

                var sp_name = "sp_Asset_Product_Insert";
                var jsonData = Utility.JsonData(upload);      
                var parameters = new DynamicParameters();
                parameters.Add("JsonData", jsonData);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Insert);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CreateBusiness", "UploadExcelAsync", user);
            }
            return executionStatus;
        }

        public async Task<IEnumerable<ProductViewModel>> GetProductAsync(Product_Filter filter, AppUser user)
        {
            IEnumerable<ProductViewModel> list = new List<ProductViewModel>();
            try {
                var sp_name = "sp_Asset_Product_List";
                var parameters = Utility.DappperParams(filter, user);
                parameters.Add("ExecutionFlag", filter.Approved == true ? "Show" : "List");
                list = await _dapper.SqlQueryListAsync<ProductViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CreateBusiness", "GetProductAsync", user);
            }
            return list;
        }           
        public async Task<DBResponse<CreateViewModel>> GetAssetDetailsAsync(Create_Filter filter, AppUser user)
        {
            DBResponse<CreateViewModel> data = new DBResponse<CreateViewModel>();
            DBResponse response = new DBResponse();
            try {
                var sp_name = "sp_Asset_Create_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                parameters.Add("ExecutionFlag", "List");
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<CreateViewModel>>(response.JSONData) ?? new List<CreateViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };

                foreach (var item in data.ListOfObject) { 
                    item.DurationDays = (DateTime.Now.Date - Convert.ToDateTime(item.TransactionDate)).Days + 1;
                }

            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CreateBusiness", "GetAssetDetailsAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<EmailSendViewModel>> EmailSendAsync(AppUser user)
        {
            IEnumerable<EmailSendViewModel> emailDtls = new List<EmailSendViewModel>();
            try {
                var sp_name = "sp_Asset_EmailSend";
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", "Email");

                emailDtls = await _dapper.SqlQueryListAsync<EmailSendViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);

                if (emailDtls != null) {
                    var emailSetting = Utility.JsonToObject<IEnumerable<EmailSettingViewModel>>(emailDtls.First().Json).FirstOrDefault();
                    var email = Utility.JsonToObject<IEnumerable<EmailSendViewModel>>(emailDtls.First().JsonEmailCCBCC).FirstOrDefault();
                    if (emailSetting != null) {
                        await EmailSend(emailDtls.First().EmailTo, email, emailSetting, emailDtls.First().EmpDtls, emailDtls.First().UserEmail, emailDtls.First().UserName, "");
                    }
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CreateBusiness", "EmailSendAsync", user);
            }
            return emailDtls;
        }
        private async Task EmailSend(string toEmail, EmailSendViewModel email, EmailSettingViewModel emailSetting, string empDtls, string userEmail, string userName, string flag)
        {
            var Subject = "";
            try {

                flag = "Asset Create";
                MailMessage message = new MailMessage();
                message.From = new MailAddress(emailSetting.EmailAddress, "Asset Notification"); 
                Subject = "Asset approval request";
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
            }
            catch (Exception) {

            }
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "CreateBusiness", "ApprovedAssetAsync", user);
            }
            return executionStatus;
        }

    }
}
