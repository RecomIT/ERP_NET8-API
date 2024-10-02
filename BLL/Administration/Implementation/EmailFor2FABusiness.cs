using AutoMapper;
using DAL.DapperObject;
using Microsoft.AspNetCore.Identity;
using System.Collections.Concurrent;
using System.Data;
using System.Net.Mail;
using Dapper;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.Services;
using DAL.DapperObject.Interface;
using Shared.Control_Panel.Domain;
using Shared.Control_Panel.DTO.GoogleAuthenticator;
using Shared.Control_Panel.ViewModels;
using BLL.Administration.Interface;

namespace BLL.Administration.Implementation
{
    public class EmailFor2FABusiness : IEmailFor2FABusiness
    {
        private string sqlQuery;
        private IDapperData _dapperData;
        private IMapper _mapper;
        private readonly ISysLogger _sysLogger;
        private UserManager<ApplicationUser> _userManager;
        private readonly IClientDatabase _clientDatabase;
        private SignInManager<ApplicationUser> _signInManager;

        private ILoginManager _loginManager;
        private readonly ConcurrentDictionary<string, Tuple<string, DateTime>> _otpStorage = new();
        public EmailFor2FABusiness(IDapperData dapperData, IMapper mapper, ISysLogger sysLogger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILoginManager loginManager, IClientDatabase clientDatabase)
        {
            _dapperData = dapperData;
            _mapper = mapper;
            _sysLogger = sysLogger;
            _userManager = userManager;
            _signInManager = signInManager;
            _clientDatabase = clientDatabase;
            _loginManager = loginManager;
        }

        //public async Task<bool> SendEmailAsync(long employeeId, string code, AppUser appUser)
        //{

        //    var empDetail = await  GetEmployeeDetailsById(employeeId, appUser);

        //    var mail = new MailMessage();

        //    // Set the 'From' address
        //    mail.From = new MailAddress("mr.nurul.islam023@gmail.com"); // Replace with your email address

        //    // Set the 'To' recipient
        //    mail.To.Add(new MailAddress(toEmail));

        //    // Set the subject of the email
        //    mail.Subject = "Your Two-Factor Authentication Code";

        //    // Set the email body content
        //    mail.Body = $"Your authentication code is: {code}";

        //    // Specify that the email body is plain text (or HTML if you prefer)
        //    mail.IsBodyHtml = false; // Set to true if the body is HTML

        //    try {
        //        await Task.Run(() =>
        //        {
        //            using (var smtpClient = new SmtpClient()) {
        //                // Specify the SMTP server and port
        //                smtpClient.Host = "smtp.gmail.com";
        //                smtpClient.Port = 587;

        //                // Enable SSL/TLS (required for Gmail)
        //                smtpClient.EnableSsl = true;

        //                // Specify your SMTP credentials
        //                smtpClient.Credentials = new NetworkCredential("mr.nurul.islam023@gmail.com", "upbm sesk okgq ovlz");

        //                // Send the email
        //                smtpClient.Send(mail);
        //            }
        //        });
        //    }
        //    catch (Exception ex) {
        //        Console.WriteLine($"Error sending email: {ex.Message}");
        //        throw; // Rethrow the exception to propagate it
        //    }
        //    finally {
        //        // Dispose of the MailMessage
        //        mail.Dispose();
        //    }
        //}
        public async Task<(bool Success, string Message)> SendEmailAsync(string email, string code, AppUser appUser)
        {

            var mailConfig = await _loginManager.EmailSettings("Send");

            // Placeholder: Retrieve the email address based on employeeId and appUser
            string toEmail = email; // Replace with actual logic to retrieve email address
            int port = Convert.ToInt32(mailConfig.Port);
            var mail = new MailMessage();

            // Set the 'From' address
            mail.From = new MailAddress(mailConfig.EmailAddress); // Replace with your email address

            // Set the 'To' recipient
            mail.To.Add(new MailAddress(toEmail));

            // Set the subject of the email
            mail.Subject = "Your Authentication Code";

            // Set the email body content
            mail.Body = $"Your authentication code is: {code}";

            // Specify that the email body is plain text (or HTML if you prefer)
            mail.IsBodyHtml = false; // Set to true if the body is HTML

            mailConfig.EmailHtmlBody = mail.Body;

            try
            {
                EmailSender.Send(mailConfig, new Shared.OtherModels.EmailService.EmailReceiverObject()
                {
                    MailAddress = toEmail,
                    Subject = mail.Subject,

                });
                //using (var smtpClient = new SmtpClient(mailConfig.Host,port)) {
                //    // Enable SSL/TLS (required for Gmail)
                //    smtpClient.EnableSsl = true;

                //    // Specify your SMTP credentials
                //    smtpClient.Credentials = new NetworkCredential(mailConfig.EmailAddress, mailConfig.EmailPassword);

                //    // Send the email asynchronously
                //   await smtpClient.SendMailAsync(mail);
                //}

                string obscuredEmail = ObscureEmail(email);

                //// Email sent successfully
                return (true, $"Your authentication code has been sent successfully to your email: {obscuredEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                // Email sending failed
                return (false, $"Error sending email: {ex.Message}");
            }
            finally
            {
                // Dispose of the MailMessage
                mail.Dispose();
            }
        }
        private string ObscureEmail(string email)
        {
            int indexOfAt = email.IndexOf('@');
            if (indexOfAt > 0)
            {
                string prefix = email.Substring(0, indexOfAt);
                string obscured = prefix.Substring(0, Math.Min(5, prefix.Length)); // Show the first 5 characters
                obscured += new string('*', prefix.Length - 5); // Replace the rest with asterisks
                return $"{obscured}@{email.Substring(indexOfAt + 1)}";
            }
            return email;
        }

        public async Task<EmployeeDTO> GetEmployeeDetailsById(long? empId, AppUser user)
        {
            EmployeeDTO data = new EmployeeDTO();

            try
            {
                string sqlQuery = EmployeeDetails() + " WHERE e.EmployeeId = @EmployeeId";
                data = await _dapperData.SqlQueryFirstAsync<EmployeeDTO>(user.Database, sqlQuery, new { EmployeeId = empId }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "Employee", "GetEmployeeDetailsById", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }

            return data;
        }



        public static string EmployeeDetails()
        {
            return
                $@"SELECT    e.EmployeeId
				, e.EmployeeCode
				, e.FullName AS 'Name'
				, ISNULL(desig.DesignationName,'-') AS 'Designation'
				, ISNULL(dpt.DepartmentName,'-') AS 'Department'
				, ISNULL(dv.DivisionName ,'-') AS 'Division'
				, ISNULL(br.BranchName  ,'-')  AS 'Branch'
				,e.OfficeEmail
				,Subquery.SupervisorName
				,Subquery.SupervisorEmail
				,Subquery.SupervisorId
                FROM  dbo.HR_EmployeeInformation e
                LEFT JOIN dbo.HR_Designations desig
                ON e.DepartmentId = desig.DesignationId
                LEFT JOIN dbo.HR_Departments dpt
                ON e.DepartmentId = dpt.DepartmentId
                LEFT JOIN dbo.HR_Divisions dv
                ON e.DivisionId = dv.DivisionId
                LEFT JOIN [ControlPanel].dbo.tblBranches br 
                ON  br.CompanyId = e.CompanyId AND e.BranchId = br.BranchId 
				LEFT JOIN (
				SELECT EMPH.EmployeeId, EMPI.EmployeeId AS SupervisorId, EMPI.FullName AS SupervisorName, EMPI.OfficeEmail AS SupervisorEmail,
					EMPH.ActivationDate AS LastActivationDate
				FROM HR_EmployeeHierarchy EMPH
				INNER JOIN HR_EmployeeInformation EMPI ON EMPH.SupervisorId = EMPI.EmployeeId
				WHERE EMPH.IsActive = 1 AND EMPH.ActivationDate = (SELECT MAX(ActivationDate) FROM HR_EmployeeHierarchy WHERE EmployeeId = EMPH.EmployeeId)
			) Subquery ON e.EmployeeId = Subquery.EmployeeId";

        }



        public async Task<EmailSettingObject> EmailSettings(string EmaliFor)
        {
            EmailSettingObject emailSetting = null;
            try
            {
                sqlQuery = string.Format(@"sp_EmailSettingInformation");
                var parameters = new DynamicParameters();
                parameters.Add("EmailFor", EmaliFor);
                emailSetting = await _dapperData.SqlQueryFirstAsync<EmailSettingObject>(Database.ControlPanel, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return emailSetting;
        }


    }
}
