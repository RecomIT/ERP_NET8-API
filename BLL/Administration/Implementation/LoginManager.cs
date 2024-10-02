using Dapper;
using System.Data;
using System.Net;
using Shared.Helpers;
using System.Net.Mail;
using Shared.Services;
using DAL.DapperObject;
using BLL.Base.Interface;
using Shared.OtherModels.Response;
using Microsoft.AspNetCore.Identity;
using DAL.DapperObject.Interface;
using Shared.Control_Panel.Domain;
using Shared.Control_Panel.ViewModels;
using BLL.Administration.Interface;

namespace BLL.Administration.Implementation
{
    public class LoginManager : ILoginManager
    {
        private IDapperData _dapperData;
        private readonly ISysLogger _sysLogger;
        private UserManager<ApplicationUser> _userManager;
        private readonly IClientDatabase _clientDatabase;
        private readonly IOrgInitBusiness _orgInitBusiness;

        public LoginManager(IDapperData dapperData, ISysLogger sysLogger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IClientDatabase clientDatabase, IOrgInitBusiness orgInitBusiness)
        {
            _dapperData = dapperData;
            _sysLogger = sysLogger;
            _userManager = userManager;
            _clientDatabase = clientDatabase;
            _orgInitBusiness = orgInitBusiness;
        }
        public async Task<AppUserLoggedInfo> GetAppUserLoggedInfoAsync(string username)
        {
            AppUserLoggedInfo data = new AppUserLoggedInfo();
            var sp_name = "sp_AppUserLoggedInfo";
            var parameters = new DynamicParameters();
            try
            {
                parameters.Add("Username", username);
                if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                {
                    var database = _clientDatabase.GetDatabaseName(username);
                    data = await _dapperData.SqlQueryFirstAsync<AppUserLoggedInfo>(database, sp_name, parameters, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "LoginManager", "GetAppUserLoggedInfoAsync", "", 0, 0, 0);
            }
            return data;
        }
        public async Task<bool> IsEmailExistAsync(string email)
        {
            bool IsExist = false;
            try
            {
                IsExist = await _userManager.FindByEmailAsync(email) != null;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "LoginManager", "IsEmailExistAsync", "", 0, 0, 0);
            }

            return IsExist;
        }
        public async Task<ExecutionStatus> UserForgetPasswordOTPResquestAsync(OTPRequestsViewModel model)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var appUser = await _userManager.FindByEmailAsync(model.Email);
                if (appUser != null)
                {
                    var sp_name = "sp_OTPRequests";

                    var otp = Utility.GenerateRandomDigits(5, Utility.numericCharacters);
                    var parameters = new DynamicParameters();
                    parameters.Add("Email", model.Email);
                    parameters.Add("PublicIP", model.PublicIP);
                    parameters.Add("PrivateIP", model.PrivateIP);
                    parameters.Add("DeviceType", model.DeviceType);
                    parameters.Add("OS", model.OS);
                    parameters.Add("OSVersion", model.OSVersion);
                    parameters.Add("Browser", model.Browser);
                    parameters.Add("BrowserVersion", model.BrowserVersion);
                    parameters.Add("OTP", otp);
                    parameters.Add("ExecutionFlag", "Request");

                    executionStatus = await _dapperData.SqlQueryFirstAsync<ExecutionStatus>(Database.ControlPanel, sp_name, parameters, CommandType.StoredProcedure);
                    if (executionStatus != null && executionStatus.Status)
                    {
                        var emailSetting = Utility.JsonToObject<IEnumerable<EmailSettingObject>>(executionStatus.Json).FirstOrDefault();
                        if (emailSetting != null)
                        {
                            await OTPEmailService(model.Email, otp, emailSetting, EmailTemplateFlag.ForgetPassword);
                            executionStatus.token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
                        }
                        else
                        {
                            executionStatus = ResponseMessage.Invalid("Can not find user with this email");

                        }
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Invalid("Can not find user with this email");
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "LoginManager", "UserForgetPasswordOTPResquestAsync", "", 0, 0, 0);
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
            }
            return executionStatus;
        }
        private async Task OTPEmailService(string toEmail, string password, EmailSettingObject emailSetting, string flag = "")
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(emailSetting.EmailAddress, emailSetting.DisplayName);
                message.To.Add(new MailAddress(toEmail));
                message.Subject = emailSetting.Subject;
                message.IsBodyHtml = emailSetting.IsBodyHtml;
                message.Body = Utility.GetEmailTemplate(flag, password);

                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = emailSetting.EnableSsl;
                smtp.UseDefaultCredentials = emailSetting.UseDefaultCredentials;
                smtp.Port = Convert.ToInt32(emailSetting.Port);
                smtp.Host = emailSetting.Host;
                smtp.Credentials = new NetworkCredential(emailSetting.EmailAddress, emailSetting.EmailPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "LoginManager", "OTPEmailService", "", 0, 0, 0);
            }
        }
        public async Task<ExecutionStatus> UserForgetPasswordOTPVerificationAsync(OTPVerificationViewModel model)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_OTPRequests";
                var parameters = new DynamicParameters();
                parameters.Add("Email", model.Email);
                parameters.Add("OTP", model.OTP);
                parameters.Add("ExecutionFlag", "Verification");

                executionStatus = await _dapperData.SqlQueryFirstAsync<ExecutionStatus>(Database.ControlPanel, sp_name, parameters, CommandType.StoredProcedure);
                if (executionStatus.Status)
                {
                    var appUser = await _userManager.FindByEmailAsync(model.Email);
                    var newPassword = Utility.RandomPassword();
                    var result = await _userManager.ResetPasswordAsync(appUser, model.Token, newPassword);
                    if (result.Succeeded)
                    {
                        executionStatus.Msg = "OTP Varified Successfully.<br> Defualt Password has been sent to your Email. <br> Please login with this password";

                        var emailSetting = Utility.JsonToObject<IEnumerable<EmailSettingObject>>(executionStatus.Json).FirstOrDefault();

                        if (emailSetting != null)
                        {
                            await OTPEmailService(model.Email, newPassword, emailSetting, EmailTemplateFlag.DefaultPassword);
                        }
                        else
                        {
                            executionStatus.Status = false;
                            executionStatus.ErrorMsg = "Can not find user with this email";
                        }
                    }
                    else
                    {
                        executionStatus.Status = false;
                        executionStatus.Msg = "Reset is failed due to invalid token";
                    }
                }

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "LoginManager", "UserForgetPasswordOTPVerificationAsync", "", 0, 0, 0);
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
            }
            return executionStatus;
        }
        public async Task<EmailSettingObject> EmailSettings(string EmaliFor)
        {
            EmailSettingObject emailSetting = null;
            try
            {
                var sp_name = "sp_EmailSettingInformation";
                var parameters = new DynamicParameters();
                parameters.Add("EmailFor", EmaliFor);
                emailSetting = await _dapperData.SqlQueryFirstAsync<EmailSettingObject>(Database.ControlPanel, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "LoginManager", "EmailSettings", null);
            }
            return emailSetting;
        }
        public async Task<AppUserLoggedInfo> GetAppUserEmployeeInfoAsync(long employeeId, long companyId, long organizationId, string database)
        {
            AppUserLoggedInfo info = new AppUserLoggedInfo();
            try
            {
                var query = $@"Select emp.*,EmployeeName=emp.FullName,grade.GradeName,desig.DesignationName,dept.DepartmentName,
                sec.SectionName,subsec.SubSectionName,JobStatusName=(CASE WHEN ISNULL(emp.IsActive,0)=0 THEN 'Inactive' 
                WHEN ISNULL(emp.IsActive,0)=1 THEN 'Active' ELSE 'Inactive' END),
                (ws.Title+'#'+ws.[Name]) 'WorkShiftName',emp.WorkShiftId,
                PhotoPath= ISNULL(SUBSTRING((ED.PhotoPath+'/'+ED.Photo),CHARINDEX('/',(ED.PhotoPath+'/'+ED.Photo))+1,200),'default'),emp.TerminationDate
                From HR_EmployeeInformation emp
                LEFT Join HR_Designations Desig on emp.DesignationId = desig.DesignationId
                LEFT Join HR_Grades grade on Desig.GradeId = grade.GradeId
                Left Join HR_Departments dept on emp.DepartmentId = dept.DepartmentId
                Left Join HR_Sections sec on emp.SectionId = sec.SectionId
                Left Join HR_SubSections subsec on emp.SubSectionId = subsec.SubSectionId
                Left Join HR_WorkShifts ws on emp.WorkShiftId = ws.WorkShiftId
                LEFT Join HR_EmployeeDetail ED ON emp.EmployeeId = ed.EmployeeId
                Where 1=1
                AND emp.EmployeeId=@EmployeeId 
                AND emp.CompanyId=@CompanyId 
                AND emp.OrganizationId=@OrganizationId";
                var parameters = new { EmployeeId = employeeId, CompanyId = companyId, OrganizationId = organizationId };
                info = await _dapperData.SqlQueryFirstAsync<AppUserLoggedInfo>(database, query, parameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "", "", null);
            }
            return info;
        }
        public async Task<AppUserLoggedInfo> GetAppUserLoggedInfo2Async(string username)
        {
            var userInfo = new AppUserLoggedInfo();
            try
            {
                var query = $@"Select
                Cast(u.Id as Nvarchar(50)) 'UserId',u.UserName 'Username',Cast(r.Id as Nvarchar(50)) 'RoleId',
                SUBSTRING(r.[Name],1,(CASE WHEN CHARINDEX('#Org',r.[Name]) > 0 
                THEN CHARINDEX('#Org',r.[Name])-1 ELSE LEN(r.[NAME]) END)) as 'RoleName',
                (Case 
                When PasswordExpiredDate IS NULL then 0
                When PasswordExpiredDate < Cast(GETDATE() as date) then 0
                When PasswordExpiredDate > Cast(GETDATE() as date) then DATEDIFF(DAY,GETDATE(),PasswordExpiredDate)
                When PasswordExpiredDate = Cast(GETDATE() as date) then 0
                Else 0 End) 'RemainExpireDays',u.IsActive,
                [SiteThumbnailPath]=SUBSTRING(com.CompanyLogoPath,CHARINDEX('/',com.CompanyLogoPath)+1,500), 
                SiteShortName=Com.ShortName,Com.CompanyName,Org.OrganizationName,u.EmployeeId,u.CompanyId,u.OrganizationId,u.IsDefaultPassword
                From AspNetUsers u
                Inner Join AspNetRoles r On u.RoleId = r.Id
                Inner Join tblOrganizations Org On Org.OrganizationId = u.OrganizationId
                Inner Join tblCompanies Com On Com.CompanyId = U.CompanyId
                Where u.UserName = @UserName";
                userInfo = await _dapperData.SqlQueryFirstAsync<AppUserLoggedInfo>(Database.ControlPanel, query, new { UserName = username });
                if (userInfo.EmployeeId > 0)
                {
                    var database = _clientDatabase.GetDatabaseName(userInfo.Username);
                    if (database.IsNullEmptyOrWhiteSpace() == false && userInfo.EmployeeId > 0)
                    {
                        var employeeInfo = await GetAppUserEmployeeInfoAsync(userInfo.EmployeeId, userInfo.CompanyId, userInfo.OrganizationId, database);
                        if (employeeInfo != null && employeeInfo.EmployeeId > 0)
                        {
                            var branchInfo = await _orgInitBusiness.GetBranchById(employeeInfo.BranchId, userInfo.CompanyId, userInfo.OrganizationId);
                            if (branchInfo != null && branchInfo.BranchId > 0)
                            {
                                userInfo.BranchId = branchInfo.BranchId;
                                userInfo.BranchName = branchInfo.BranchName;
                            }
                            else
                            {
                                userInfo.BranchId = 0;
                                userInfo.BranchName = "";
                            }
                            userInfo.EmployeeId = employeeInfo.EmployeeId;
                            userInfo.EmployeeCode = employeeInfo.EmployeeCode;
                            userInfo.EmployeeName = employeeInfo.EmployeeName.IsNullEmptyOrWhiteSpace() ? userInfo.Username : employeeInfo.EmployeeName;
                            userInfo.GradeId = employeeInfo.GradeId;
                            userInfo.GradeName = employeeInfo.GradeName;
                            userInfo.DesignationId = employeeInfo.DesignationId;
                            userInfo.DesignationName = employeeInfo.DesignationName;
                            userInfo.DepartmentId = employeeInfo.DepartmentId;
                            userInfo.DepartmentName = employeeInfo.DepartmentName;
                            userInfo.SectionId = employeeInfo.SectionId;
                            userInfo.SectionName = employeeInfo.SectionName;
                            userInfo.SubSectionId = employeeInfo.SubSectionId;
                            userInfo.SubSectionName = employeeInfo.SubSectionName;
                            userInfo.SubSectionId = employeeInfo.SubSectionId;
                            userInfo.SubSectionName = employeeInfo.SubSectionName;
                            userInfo.PhotoPath = employeeInfo.PhotoPath;
                            userInfo.WorkShiftId = employeeInfo.WorkShiftId;
                            userInfo.WorkShiftName = employeeInfo.WorkShiftName;
                            userInfo.TerminationDate = employeeInfo.TerminationDate;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "LoginManager", "GetAppUserLoggedInfo2Async", null);
            }
            return userInfo;
        }
        public async Task<IEnumerable<loginViewModel>> GetLoginInfosAsync(long companyId)
        {
            IEnumerable<loginViewModel> list = new List<loginViewModel>();
            try
            {
                var query = $@"SELECT UserName,[Password]='Ye@s!n202@' FROM ControlPanel.dbo.AspNetUsers
                Where CompanyId=@CompanyId";
                list = await _dapperData.SqlQueryListAsync<loginViewModel>(Database.ControlPanel, query, new { CompanyId = companyId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "LoginManager", "GetLoginInfosAsync", null);
            }
            return list;
        }
    }
}
