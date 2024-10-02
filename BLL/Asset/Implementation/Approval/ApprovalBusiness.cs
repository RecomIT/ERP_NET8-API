

using BLL.Base.Interface;
using DAL.DapperObject;
using Dapper;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;
using System.Net.Mail;
using System.Net;
using DAL.DapperObject.Interface;
using Shared.Asset.ViewModel.Email;
using BLL.Asset.Interface.Approval;

namespace BLL.Asset.Implementation.Approval
{

    public class ApprovalBusiness : IApprovalBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public ApprovalBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
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

                    //var sp_name1 = @"UPDATE Asset_Product SET Assigned = 1 WHERE AssigningId = @AssigningId AND AssetId = @AssetId AND CompanyId = @CompanyId AND OrganizationId = @OrganizationId";
                    //var parameters1 = new DynamicParameters();
                    //parameters1.Add("@AssigningId", assigningId);
                    //parameters1.Add("@AssetId", assetId);
                    //parameters1.Add("@CompanyId", user.CompanyId);
                    //parameters1.Add("@OrganizationId", user.OrganizationId);
                    //executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name1, parameters1, CommandType.Text);

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
        public async Task<IEnumerable<EmailSendViewModel>> EmailSendAsync(long employeeId, string sendingType, AppUser user)
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
                        await EmailSend(emailDtls.First().EmailTo, email, emailSetting, emailDtls.First().EmpDtls, emailDtls.First().UserEmail, emailDtls.First().UserEmail, "", sendingType);
                    }
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssigningBusiness", "EmailSendAsync", user);
            }
            return emailDtls;
        }
        private async Task<bool> EmailSend(string toEmail, EmailSendViewModel email, EmailSettingViewModel emailSetting, string empDtls,string userEmail, string userName, string flag, string sendingType)
        {
            var Subject = "";
            try {

                if (sendingType == "assetAssigningApproval") {
                    flag = "Asset User";
                    Subject = "Asset assigning";
                }
                else {
                    flag = "Asset Approved";
                    Subject = "Asset approved";
                }


                MailMessage message = new MailMessage();
                message.From = new MailAddress(emailSetting.EmailAddress, "Asset Notification");
                //Subject = "Asset assigning";
                toEmail = "md_nur@live.com";
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
