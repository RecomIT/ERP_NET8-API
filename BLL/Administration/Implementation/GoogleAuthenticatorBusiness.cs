using AutoMapper;
using DAL.DapperObject;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Shared.Services;
using System.Data;
using System.Text;
using BLL.Base.Interface;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.Control_Panel.Domain;
using Google.Authenticator;
using static Shared.Services.Utility;
using BLL.Administration.Interface;

namespace BLL.Administration.Implementation
{
    public class GoogleAuthenticatorBusiness : IGoogleAuthenticatorBusiness
    {
        private string sqlQuery;
        private IDapperData _dapperData;
        private IMapper _mapper;
        private readonly ISysLogger _sysLogger;
        private UserManager<ApplicationUser> _userManager;
        private readonly IClientDatabase _clientDatabase;
        private SignInManager<ApplicationUser> _signInManager;
        private ILoginManager _loginManager;
        private IEmailFor2FABusiness _emailFor2FABusiness;
        // private readonly ConcurrentDictionary<string, Tuple<string, DateTime>> _otpStorage = new();
        public GoogleAuthenticatorBusiness(IDapperData dapperData,
            IMapper mapper, ISysLogger sysLogger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoginManager loginManager,
            IEmailFor2FABusiness emailFor2FABusiness,
            IClientDatabase clientDatabase)
        {
            _dapperData = dapperData;
            _mapper = mapper;
            _sysLogger = sysLogger;
            _userManager = userManager;
            _signInManager = signInManager;
            _clientDatabase = clientDatabase;
            _loginManager = loginManager;
            _emailFor2FABusiness = emailFor2FABusiness;
        }

        public async Task<object> GenerateQRcodeAsync(string sendToEmail, AppUser appUser)
        {

            try
            {

                if (appUser != null)
                {


                    bool IsEnabled = await IsEnabledAsync(appUser);


                    TwoFactorAuthenticator TwoFacAuth = new TwoFactorAuthenticator();
                    string googleAuthKey = GoogleAuthKeys.GoogleAuthKey;
                    long employeeId = appUser.EmployeeId;
                    string userId = appUser.Username;

                    if (sendToEmail == "sendToEmail")
                    {
                        var empDetail = await _emailFor2FABusiness.GetEmployeeDetailsById(employeeId, appUser);
                        if (empDetail == null)
                        {

                            if (empDetail == null)
                            {
                                return new
                                {
                                    Status = false,
                                    message = "Employee details not found"
                                };
                            }
                        }
                        var otp = GenerateRandomDigits(5, numericCharacters);
                        DateTime dateTime = DateTime.Now;
                        string email = empDetail.OfficeEmail;
                        string secretKey = googleAuthKey;
                        bool secretIsBase32 = false;
                        DateTime now = DateTime.UtcNow;
                        var pin = TwoFacAuth.GetCurrentPIN(secretKey, secretIsBase32);
                        var emailSend = await _emailFor2FABusiness.SendEmailAsync(email, pin, appUser);

                        return new
                        {
                            email,
                            message = emailSend.Message,
                            Status = true
                        };

                    }
                    else
                    {

                        if (IsEnabled == false)
                        {
                            string userUniqueKey = (userId + googleAuthKey).Substring(0, 5);
                            var setupInfo = TwoFacAuth.GenerateSetupCode("ReCom-HRIS App", userId, ConvertSecretToBytes(userUniqueKey, false), 4);

                            return new
                            {
                                UserUniqueKey = userUniqueKey,
                                BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl,
                                SetupCode = setupInfo.ManualEntryKey,
                                Status = true
                            };
                        }
                        return new
                        {
                            UserUniqueKey = string.Empty,
                            BarcodeImageUrl = string.Empty,
                            SetupCode = string.Empty,
                            Status = false
                        };

                    }




                }


                else
                {
                    throw new ArgumentNullException(nameof(appUser.Username), "User Not Find");
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "GoogleAuthenticatorBusiness", "GenerateQRcodeAsync", "", 0, 0, 0);
                return new { Status = false, Message = "An error occurred while processing the request" };
            }

        }
        private static byte[] ConvertSecretToBytes(string secret, bool secretIsBase32) =>
         secretIsBase32 ? Base32Encoding.ToBytes(secret) : Encoding.UTF8.GetBytes(secret);
        public async Task<ExecutionStatus> TwoFactorAuthenticate(string token, bool sendToEmail, AppUser appUser)
        {
            ExecutionStatus executionStatus = null;

            try
            {
                if (appUser == null)
                {
                    executionStatus = new ExecutionStatus()
                    {
                        Status = false,
                        Msg = "User is not found!",
                        Errors = null
                    };
                    return executionStatus;
                }



                bool IsEnabled = await IsEnabledAsync(appUser);

                TwoFactorAuthenticator twoFacAuth = new TwoFactorAuthenticator();
                string userId = appUser.Username;
                string googleAuthKey = GoogleAuthKeys.GoogleAuthKey;
                string userUniqueKey = (userId + googleAuthKey).Substring(0, 5);

                if (sendToEmail)
                {
                    string secretKey = googleAuthKey;
                    bool secretIsBase32 = false;

                    bool isValid = twoFacAuth.ValidateTwoFactorPIN(secretKey, token, secretIsBase32);
                    if (isValid)
                    {

                        executionStatus = new ExecutionStatus()
                        {
                            Status = true,
                            Msg = "Google Two Factor PIN is Correct!",
                            Errors = null
                        };
                    }
                    else
                    {

                        executionStatus = new ExecutionStatus()
                        {
                            Status = false,
                            Msg = "Google Two Factor PIN is expired or wrong",
                            Errors = null
                        };


                    }
                }
                else
                {
                    bool isValid = twoFacAuth.ValidateTwoFactorPIN(userUniqueKey, token, false);
                    if (isValid)
                    {


                        if (IsEnabled == false)
                        {

                            int insert = await InsertTwoFactorAuthAsync(appUser);

                        }

                        executionStatus = new ExecutionStatus()
                        {
                            Status = true,
                            Msg = "Google Two Factor PIN is Correct!",
                            Errors = null
                        };
                    }
                    else
                    {
                        executionStatus = new ExecutionStatus()
                        {
                            Status = false,
                            Msg = "Google Two Factor PIN is expired or wrong",
                            Errors = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = new ExecutionStatus()
                {
                    Status = false,
                    Msg = ex.Message,
                    Errors = null
                };
            }

            return executionStatus;
        }
        private async Task<bool> IsEnabledAsync(AppUser appUser)
        {
            try
            {
                if (appUser == null)
                {
                    return false;
                }

                var parameters = new DynamicParameters();
                parameters.Add("@employeeId", appUser.EmployeeId);
                parameters.Add("@companyId", appUser.CompanyId);
                parameters.Add("@organizationId", appUser.OrganizationId);

                string sqlQuery = @"
                SELECT [Enabled] 
                FROM [ControlPanel].[dbo].[tblTwoFactorAuth] 
                WHERE [EmployeeId] = @employeeId 
                AND [OrganizationId] = @organizationId 
                AND [CompanyId] = @companyId";

                bool isEnabled = await _dapperData.SqlQueryFirstAsync<bool>(Database.ControlPanel, sqlQuery, parameters, CommandType.Text);
                return isEnabled;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "GoogleAuthenticatorBusiness", "GenerateQRcodeAsync", "", 0, 0, 0);
                return false;
            }
        }
        private async Task<int> InsertTwoFactorAuthAsync(AppUser appUser)
        {
            try
            {
                string method = "Google Authenticator";
                bool enabled = true;
                string createdBy = appUser.UserId;
                DateTime createdDate = DateTime.Now;

                var parameters = new DynamicParameters();
                parameters.Add("@employeeId", appUser.EmployeeId);
                parameters.Add("@companyId", appUser.CompanyId);
                parameters.Add("@organizationId", appUser.OrganizationId);
                parameters.Add("@method", method);
                parameters.Add("@enabled", enabled);
                parameters.Add("@createdBy", createdBy);
                parameters.Add("@createdDate", createdDate);

                string insertQuery = @"
            INSERT INTO [ControlPanel].[dbo].[tblTwoFactorAuth] 
            (
                [Id],
                [EmployeeId],
                [Method],
                [Enabled],
                [CreatedBy],
                [CreatedDate],
                [UpdatedBy],
                [UpdatedDate],
                [OrganizationId],
                [CompanyId]
            ) 
            VALUES 
            (
                NEWID(),  -- Generate a new GUID for the Id column
                @employeeId,
                @method,
                @enabled,
                @createdBy,
                @createdDate,
                '', -- Placeholder for UpdatedBy (assuming it can be empty initially)
                GETDATE(), -- Placeholder for UpdatedDate (assuming it's the current date/time)
                @organizationId,
                @companyId
            );
            SELECT CAST(@@ROWCOUNT as int)";

                int rowsAffected = await _dapperData.SqlQueryFirstAsync<int>(Database.ControlPanel, insertQuery, parameters, CommandType.Text);
                return rowsAffected;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "GoogleAuthenticatorBusiness", "GenerateQRcodeAsync", "", 0, 0, 0);
                throw;
            }
        }

    }
}
