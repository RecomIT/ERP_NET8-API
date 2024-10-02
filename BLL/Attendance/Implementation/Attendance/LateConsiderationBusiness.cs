using AutoMapper;
using DAL.DapperObject;
using Dapper;
using Newtonsoft.Json;
using Shared.Services;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.Pagination;
using BLL.Base.Interface;
using Shared.OtherModels.DataService;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.Attendance;
using Shared.Attendance.ViewModel.Attendance.EarlyDeparture;
using Shared.Attendance.ViewModel.Attendance.LateConsideration;

namespace BLL.Attendance.Implementation.Attendance
{
    public class LateConsiderationBusiness : ILateConsiderationBusiness
    {
        private readonly IDapperData _dapper;
        private readonly IMapper _mapper;
        private readonly ISysLogger _sysLogger;
        private readonly IClientDatabase _clientDatabase;
        private string sqlQuery;
        public LateConsiderationBusiness(IDapperData dapper, IMapper mapper, ISysLogger sysLogger, IClientDatabase clientDatabase)
        {
            _dapper = dapper;
            _mapper = mapper;
            _sysLogger = sysLogger;
            _clientDatabase = clientDatabase;
        }


        public async Task<IEnumerable<Select2Dropdown>> GetLateTransactionDateAsync(AppUser user)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {


                sqlQuery = string.Format(@"sp_HR_ddl_GetLateTransactionDate");
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("EmployeeId", user.EmployeeId);
                parameters.Add("Flag", Data.Extension);

                //sqlQuery = Utility.ParamChecker(sqlQuery);
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<Select2Dropdown>(Database.HRMS, sqlQuery, parameters, CommandType.StoredProcedure);
                }
                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        public async Task<IEnumerable<Select2Dropdown>> GetLateReasonsAsync(long lateReasonId, AppUser user)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {


                sqlQuery = string.Format(@"sp_HR_ddl_GetLateReasons");
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("LateReasonId", lateReasonId);
                parameters.Add("Flag", Data.Extension);

                //sqlQuery = Utility.ParamChecker(sqlQuery);
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<Select2Dropdown>(Database.HRMS, sqlQuery, parameters, CommandType.StoredProcedure);
                }
                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        public async Task<IEnumerable<Select2Dropdown>> GetSupervisorAsync(AppUser user)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {


                sqlQuery = string.Format(@"sp_HR_ddl_GetSupervisor");
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("EmployeeId", user.EmployeeId);
                parameters.Add("Flag", Data.Extension);

                //sqlQuery = Utility.ParamChecker(sqlQuery);
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<Select2Dropdown>(Database.HRMS, sqlQuery, parameters, CommandType.StoredProcedure);
                }
                return data;
            }
            catch (Exception ex)
            {
                return data;
            }


        }



        public async Task<ExecutionStatus> SaveLateRequestAsync(List<LateRequestViewModel> modal, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var jsonData = Utility.JsonData(modal);
                sqlQuery = string.Format(@"sp_HR_LateConsiderationRequest02");
                var parameters = new DynamicParameters();
                parameters.Add("JSONData", jsonData);
                parameters.Add("EmployeeId", user.EmployeeId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("Flag", Data.Insert);

                // Save late requests
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(Database.HRMS, sqlQuery, parameters, CommandType.StoredProcedure);

                // If late requests are saved successfully, send email
                if (executionStatus != null && executionStatus.Status)
                {
                    await SendLateRequestEmailAsync(modal, user);
                }
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
            }
            return executionStatus;
        }

        private async Task SendLateRequestEmailAsync(List<LateRequestViewModel> modal, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

            try
            {
                foreach (var lateRequest in modal)
                {
                    if (user.HasBoth)
                    {
                        var detail = await GetEmployeeDetailsById(user.EmployeeId, user);

                        MailMessage mail = new MailMessage();

                        // Set the 'From' address
                        mail.From = new MailAddress("mr.nurul.islam023@gmail.com"); // Replace with your email address

                        // Set the 'To' recipient
                        mail.To.Add(new MailAddress(detail.SupervisorEmail));

                        // Set the 'CC' recipient (supervisor)
                        mail.CC.Add(new MailAddress(detail.OfficeEmail));

                        // Set the subject of the email
                        mail.Subject = "Employee Late Consideration Request";

                        // Set the email body content
                        mail.Body = GenerateHtmlTableForLateRequestSave(detail, lateRequest); // Use the HTML table generated by GenerateHtmlTable

                        // Specify that the email body is in HTML format
                        mail.IsBodyHtml = true;

                        try
                        {
                            // Send the email
                            smtpClient.Port = 587; // Specify the SMTP port (e.g., 587 for TLS)
                            smtpClient.Credentials = new NetworkCredential("mr.nurul.islam023@gmail.com", "upbm sesk okgq ovlz"); // Specify your SMTP credentials
                            smtpClient.EnableSsl = true; // Enable SSL/TLS (if required)
                            smtpClient.Send(mail);

                            // Update the status or do other things if needed
                        }
                        catch (Exception ex)
                        {
                            // Handle any exceptions here for individual email sending
                            Console.WriteLine($"Error sending email to {lateRequest.FullName}: {ex.Message}");
                        }
                        finally
                        {
                            // Dispose of the MailMessage
                            mail.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
            finally
            {
                smtpClient.Dispose();
            }
        }


        private string GenerateHtmlTableForLateRequestSave(EmployeeDTO data, LateRequestViewModel query)
        {
            string emailBody = $@"<!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Employee Attendance Notice</title>
    </head>
    <body style='font-family: Arial, sans-serif;'>

        <div style='background-color: #f4f4f4; padding: 10px; text-align: center;'>
            <h2>Attendance Notice</h2>
        </div>

        <div style='padding: 20px;'>
            <p>Dear, <strong>{data.SupervisorName}</strong>,</p>

            <p>We hope this message finds you well. We wanted to bring to your attention some concerns regarding your recent Late Consideration request from <strong>Mr./Mrs {data.Name}</strong>.</p>
            <div style='text-align: left;'>
                <b>Requested Date  {query.RequestedForDate:dd MMMM yyyy}</b>
                
            </div>

           

           
        </div>

        <div style='background-color: #f4f4f4; padding: 6px; text-align: center;'>
            <p style='font-size: 12px;'>This is an automated mail. Please do not reply to this email.</p>
            <p><b>Powered By ReCom Consulting Limited.<b></p>
        </div>
    </body>
    </html>";



            return emailBody;
        }

        public async Task<DBResponse<LateRequestViewModel>> GetLateConsiderationMasterAsync(LateRequestFilter query, AppUser User)
        {
            string flagValue = "master";
            DBResponse<LateRequestViewModel> data = new DBResponse<LateRequestViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                sqlQuery = string.Format(@"sp_HR_GetLateConsiderationRequest");
                //var parameters = Utility.DappperParams(flagValue, User, addBaseProperty: true);
                // parameters.Add("@Flag", flagValue);
                var parameters = new DynamicParameters();

                parameters.Add("UserId", User.UserId);
                parameters.Add("CompanyId", User.CompanyId);
                parameters.Add("OrganizationId", User.OrganizationId);
                parameters.Add("BranchId", User.BranchId);
                parameters.Add("Flag", flagValue);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(Database.HRMS, sqlQuery, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<LateRequestViewModel>>(response.JSONData) ?? new List<LateRequestViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, User.Database, "TaxBusiness", "SaveEmployeeTaxDocumentAsync", User.Username, User.OrganizationId, User.CompanyId, User.BranchId);
            }
            return data;
        }

        public async Task<DBResponse<LateRequestsDetailViewModel>> GetLateConsiderationDetailAsync(long lateRequestsId, AppUser User)
        {
            string flagValue = "Detail";
            DBResponse<LateRequestsDetailViewModel> data = new DBResponse<LateRequestsDetailViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                sqlQuery = string.Format(@"sp_HR_GetLateConsiderationRequest");
                //var parameters = Utility.DappperParams(flagValue, User, addBaseProperty: true);
                // parameters.Add("@Flag", flagValue);
                var parameters = new DynamicParameters();
                parameters.Add("LateRequestsId", lateRequestsId);
                parameters.Add("UserId", User.UserId);
                parameters.Add("CompanyId", User.CompanyId);
                parameters.Add("OrganizationId", User.OrganizationId);
                parameters.Add("BranchId", User.BranchId);
                parameters.Add("Flag", flagValue);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(Database.HRMS, sqlQuery, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<LateRequestsDetailViewModel>>(response.JSONData) ?? new List<LateRequestsDetailViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, User.Database, "TaxBusiness", "SaveEmployeeTaxDocumentAsync", User.Username, User.OrganizationId, User.CompanyId, User.BranchId);
            }
            return data;
        }

        public async Task<ExecutionStatus> UpdateStatusLateRequestDetaileAsync(long lateRequestsDetailId, string comment, string flag, long attendanceId, long lateRequestsId, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                //var jsonData = Utility.JsonData(modal);
                sqlQuery = string.Format(@"sp_HR_LateConsiderationApproveRejectRecheck");
                var parameters = new DynamicParameters();
                //parameters.Add("JSONData", jsonData);
                parameters.Add("LateRequestsDetailId", lateRequestsDetailId);
                parameters.Add("AttendanceId", attendanceId);
                parameters.Add("LateRequestsId", lateRequestsId);
                parameters.Add("EmployeeId", user.EmployeeId);
                parameters.Add("Comment", comment);
                parameters.Add("UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("Flag", flag);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(Database.HRMS, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
            }
            return executionStatus;

        }

        public async Task<DBResponse<LateRequestViewModel>> GetLateConsiderationMasterByIdAsync(LateRequestFilter query, AppUser User)
        {
            string flagValue = "master";
            DBResponse<LateRequestViewModel> data = new DBResponse<LateRequestViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                sqlQuery = string.Format(@"sp_HR_GetLateConsiderationRequestById");
                //var parameters = Utility.DappperParams(flagValue, User, addBaseProperty: true);
                // parameters.Add("@Flag", flagValue);
                var parameters = new DynamicParameters();

                parameters.Add("UserId", User.UserId);
                parameters.Add("CompanyId", User.CompanyId);
                parameters.Add("OrganizationId", User.OrganizationId);
                parameters.Add("BranchId", User.BranchId);
                parameters.Add("EmployeeId", User.EmployeeId);
                parameters.Add("Flag", flagValue);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(Database.HRMS, sqlQuery, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<LateRequestViewModel>>(response.JSONData) ?? new List<LateRequestViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, User.Database, "TaxBusiness", "SaveEmployeeTaxDocumentAsync", User.Username, User.OrganizationId, User.CompanyId, User.BranchId);
            }
            return data;
        }


        public async Task<ExecutionStatus> feedbackEmailLateRequestAsync(List<feedbackdata> dataList, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

            try
            {
                if (user.HasBoth)
                {
                    foreach (var data in dataList)
                    {
                        MailMessage mail = new MailMessage();

                        // Set the 'From' address
                        mail.From = new MailAddress("mr.nurul.islam023@gmail.com"); // Replace with your email address

                        // Set the 'To' recipient
                        mail.To.Add(new MailAddress(data.OfficeEmail));

                        // Set the 'CC' recipient (supervisor)
                        mail.CC.Add(new MailAddress(data.SupervisorEmail));

                        // Set the subject of the email
                        mail.Subject = "Employee Late Consideration Request Feedback";

                        // Set the email body content
                        mail.Body = GenerateHtmlTable(data); // Use the HTML table generated by GenerateHtmlTable

                        // Specify that the email body is in HTML format
                        mail.IsBodyHtml = true;

                        try
                        {
                            // Send the email
                            smtpClient.Port = 587; // Specify the SMTP port (e.g., 587 for TLS)
                            smtpClient.Credentials = new NetworkCredential("mr.nurul.islam023@gmail.com", "upbm sesk okgq ovlz"); // Specify your SMTP credentials
                            smtpClient.EnableSsl = true; // Enable SSL/TLS (if required)
                            smtpClient.Send(mail);
                            var parameters = new DynamicParameters();

                            parameters.Add("emailNotificationStatus", "Notified");

                            parameters.Add("LateRequestsId", data.LateRequestsId);
                            parameters.Add("CompanyId", user.CompanyId);
                            parameters.Add("OrganizationId", user.OrganizationId);
                            //parameters.Add("BranchId", user.BranchId);

                            string sql = @$"UPDATE HR_LateRequests SET EmailNotificationStatus = @emailNotificationStatus
                                           WHERE LateRequestsId = @LateRequestsId AND CompanyId = @CompanyId AND OrganizationId = @OrganizationId";


                            //string sql = "UPDATE HR_LateRequests " +
                            //    "SET EmailNotificationStatus = 'Notified' " +
                            //    "WHERE LateRequestsId = @LateRequestsId, " +
                            //    "CompanyId=@CompanyId, " +
                            //    "OrganizationId=@OrganizationId";

                            var rawCount = await _dapper.SqlExecuteNonQueryStatus(user.Database, sql, parameters, CommandType.Text);

                            if (rawCount > 0)
                            {
                                executionStatus.Status = true;
                                executionStatus.Msg = "E-Mail Send Successfully!";

                            }
                            else
                            {
                                executionStatus.Status = false;
                                executionStatus.ErrorMsg = "Some Problem on Database";
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle any exceptions here for individual email sending
                            Console.WriteLine($"Error sending email to {data.FullName}: {ex.Message}");
                        }
                        finally
                        {
                            // Dispose of the MailMessage
                            mail.Dispose();
                        }
                    }
                }

                return executionStatus;
            }
            catch (Exception ex)
            {
                throw; // Re-throw the exception to be handled elsewhere
            }
            finally
            {
                smtpClient.Dispose();
            }

        }
        private string GenerateHtmlTable(feedbackdata data)
        {
            string emailBody = $@"<!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Employee Attendance Notice</title>
    </head>
    <body style='font-family: Arial, sans-serif;'>

        <div style='background-color: #f4f4f4; padding: 10px; text-align: center;'>
            <h2>Attendance Notice</h2>
        </div>

        <div style='padding: 20px;'>
            <p>Dear, <strong>{data.FullName}</strong>,</p>

            <p>We hope this message finds you well. We wanted to bring to your attention some concerns regarding your recent late consideration request feedback.</p>
            <div style='text-align: left;'>
                <b>Applied Date  {data.AppliedDate:dd MMMM yyyy}</b>
                
            </div>

            [AttendanceTable]

           
        </div>

        <div style='background-color: #f4f4f4; padding: 6px; text-align: center;'>
            <p style='font-size: 12px;'>This is an automated mail. Please do not reply to this email.</p>
            <p><b>Powered By ReCom Consulting Limited.<b></p>
        </div>
    </body>
    </html>";

            // Populate attendance records in the table
            string attendanceTable = @"<div style='text-align: center;'>
        <table style='margin: 0 auto; border-collapse: collapse; width: 80%; font-family: Arial, sans-serif; border: 1px solid #dddddd;'>
            <thead>
                <tr style='background-color: #f4f4f4;'>
                    <th style='text-align: center; padding: 10px; border: 1px solid #dddddd;'>SL</th>
                    <th style='text-align: center; padding: 10px; border: 1px solid #dddddd;'>Requested Date</th>
                    <th style='text-align: center; padding: 10px; border: 1px solid #dddddd;'>Comment</th>
                    <th style='text-align: center; padding: 10px; border: 1px solid #dddddd;'>Status</th>
                </tr>
            </thead>
            <tbody>";

            int rowIndex = 1; // Initialize the row index with 1 for the serial number

            foreach (var item in data.feedBackDetails)
            {
                string rowClass = rowIndex % 2 == 0 ? "even-row" : "odd-row"; // Determine the row class

                string row = $@"
            <tr class='{rowClass}'>
                <td style='text-align: center; padding: 5px; border: 1px solid #dddddd;'>{rowIndex}</td>
                <td style='text-align: center; padding: 5px; border: 1px solid #dddddd;'>{item.RequestedForDate:dd-MMM-yyyy}</td>
                <td style='text-align: center; padding: 5px; border: 1px solid #dddddd;'>{item.Comment}</td>
                <td style='text-align: center; padding: 5px; border: 1px solid #dddddd;'>{item.Flag}</td>
            </tr>";

                attendanceTable += row;
                rowIndex++; // Increment the row index
            }

            attendanceTable += @"
            </tbody>
        </table>
    </div>";

            emailBody = emailBody.Replace("[AttendanceTable]", attendanceTable);

            return emailBody;
        }


        public async Task<ExecutionStatus> SaveEarlyDepartureAsync(EarlyDepartureViewModel earlyDeparture, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                sqlQuery = string.Format(@"sp_HR_SaveEarlyDepartureRequest");

                var parameters = new DynamicParameters();

                parameters.Add("EmployeeId", earlyDeparture.EmployeeId);
                parameters.Add("RequestedForDate", earlyDeparture.RequestedForDate);
                parameters.Add("AppliedTime", earlyDeparture.AppliedTime);
                parameters.Add("Comment", earlyDeparture.Comment);
                parameters.Add("LateReasonId", earlyDeparture.Reason);
                parameters.Add("StateStatus", earlyDeparture.Status);
                parameters.Add("OtherReason", earlyDeparture.OtherReason);
                parameters.Add("SupervisorId", earlyDeparture.SupervisorId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("ExecutionFlag", earlyDeparture.EarlyDepartureId > 0 ? Data.Update : Data.Insert);

                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
                    if (executionStatus != null && executionStatus.Status == true)
                    {
                        // Log before sending email
                        Console.WriteLine("Before sending email");

                        // The Early Departure request is saved successfully, now send an email
                        await SendEarlyDepartureEmailAsync(earlyDeparture, user);

                        // Log after sending email
                        Console.WriteLine("After sending email");

                    }

                }
            }
            catch (SqlException ex)
            {
                // SQL Server specific error
                executionStatus = Utility.Invalid(ex.Message); // Populate the error message from the SqlException
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
            }
            return executionStatus;
        }
        private async Task<ExecutionStatus> SendEarlyDepartureEmailAsync(EarlyDepartureViewModel earlyDeparture, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

            try
            {
                if (user.HasBoth)
                {
                    var detail = await GetEmployeeDetailsById(earlyDeparture.EmployeeId, user);

                    MailMessage mail = new MailMessage();

                    // Set the 'From' address
                    mail.From = new MailAddress("mr.nurul.islam023@gmail.com"); // Replace with your email address

                    // Set the 'To' recipient
                    mail.To.Add(new MailAddress(detail.SupervisorEmail));

                    // Set the 'CC' recipient (supervisor)
                    mail.CC.Add(new MailAddress(detail.OfficeEmail));

                    // Set the subject of the email
                    mail.Subject = "Employee Early Departure Request";

                    // Set the email body content
                    mail.Body = GenerateHtmlTableForEarlyDepartureSave(detail, earlyDeparture); // Use the HTML table generated by GenerateHtmlTable

                    // Specify that the email body is in HTML format
                    mail.IsBodyHtml = true;

                    try
                    {
                        // Send the email
                        smtpClient.Port = 587;
                        smtpClient.Credentials = new NetworkCredential("mr.nurul.islam023@gmail.com", "upbm sesk okgq ovlz");
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(mail);

                        // Update the executionStatus
                        executionStatus.Status = true;
                        executionStatus.Msg = "E-Mail Sent Successfully!";
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions here for individual email sending
                        Console.WriteLine($"Error sending email to {detail.Name}: {ex.Message}");

                        // Update the executionStatus for failure
                        executionStatus.Status = false;
                        executionStatus.ErrorMsg = $"Error sending email: {ex.Message}";
                    }
                    finally
                    {
                        // Dispose of the MailMessage
                        mail.Dispose();
                    }


                }

                return executionStatus;
            }
            catch (Exception ex)
            {
                throw; // Re-throw the exception to be handled elsewhere
            }
            finally
            {
                smtpClient.Dispose();
            }

        }

        public async Task<IEnumerable<EarlyDepartureViewModel>> GetEarlyDepartureAsync(long? earlyDepartureId, long? employeeId, string flag, AppUser user)
        {
            IEnumerable<EarlyDepartureViewModel> data = new List<EarlyDepartureViewModel>();
            try
            {
                sqlQuery = string.Format(@"sp_HR_EarlyDepartureRequestList");

                var parameters = new DynamicParameters();
                parameters.Add("earlyDepartureId", earlyDepartureId ?? 0);
                parameters.Add("EmployeeId", employeeId ?? 0);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("Flag", flag); // Supply the @Flag parameter


                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<EarlyDepartureViewModel>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                //.. Exception logger service
            }
            return data;
        }

        public async Task<DBResponse<EarlyDepartureViewModel>> GetEarlyMasterAsync(LateRequestFilter query, AppUser User)
        {
            string flagValue = "Master";
            DBResponse<EarlyDepartureViewModel> data = new DBResponse<EarlyDepartureViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                sqlQuery = string.Format(@"sp_HR_EarlyDepartureRequestList");
                //var parameters = Utility.DappperParams(flagValue, User, addBaseProperty: true);
                // parameters.Add("@Flag", flagValue);
                var parameters = new DynamicParameters();

                parameters.Add("UserId", User.UserId);
                parameters.Add("CompanyId", User.CompanyId);
                parameters.Add("OrganizationId", User.OrganizationId);
                parameters.Add("BranchId", User.BranchId);
                parameters.Add("Flag", flagValue);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(User.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<EarlyDepartureViewModel>>(response.JSONData) ?? new List<EarlyDepartureViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, User.Database, "EarlyDepartur", "SaveEarlyDepartur", User.Username, User.OrganizationId, User.CompanyId, User.BranchId);
            }
            return data;
        }


        public async Task<IEnumerable<EarlyDepartureViewModel>> GetEarlyDepartureByIdAsync(long earlyDepartureId, AppUser user)
        {
            string flagValue = "Detail";
            IEnumerable<EarlyDepartureViewModel> data = new List<EarlyDepartureViewModel>();
            try
            {
                sqlQuery = string.Format(@"sp_HR_EarlyDepartureRequestList");

                // Prepare the parameters for the stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("EarlyDepartureId", earlyDepartureId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("Flag", flagValue);

                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    // Execute the stored procedure and get the JSON data
                    var jsonData = await _dapper.SqlQueryFirstAsync<string>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);

                    // Deserialize the JSON data into a collection of EarlyDepartureViewModel
                    data = JsonConvert.DeserializeObject<IEnumerable<EarlyDepartureViewModel>>(jsonData);
                }
            }
            catch (Exception ex)
            {
            }
            return data;
        }

        public async Task<ExecutionStatus> UpdateEarlyDepartureAsync(long earlyDepartureId, string comment, string flag, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                sqlQuery = string.Format(@"sp_HR_SaveEarlyDepartureRequest");

                var parameters = new DynamicParameters();

                parameters.Add("EarlyDepartureId", earlyDepartureId);
                parameters.Add("Comment", comment);
                parameters.Add("StateStatus", flag);
                parameters.Add("UserId", user.UserId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("ExecutionFlag", Data.Checking);

                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                // SQL Server specific error
                executionStatus = Utility.Invalid(ex.Message); // Populate the error message from the SqlException
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
            }
            return executionStatus;
        }


        public async Task<ExecutionStatus> feedbackEmailEarlyDepartureAsync(List<EarlyDepartureFeedbackdata> dataList, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

            try
            {
                if (user.HasBoth)
                {
                    foreach (var data in dataList)
                    {
                        MailMessage mail = new MailMessage();

                        // Set the 'From' address
                        mail.From = new MailAddress("mr.nurul.islam023@gmail.com"); // Replace with your email address

                        // Set the 'To' recipient
                        mail.To.Add(new MailAddress(data.OfficeEmail));

                        // Set the 'CC' recipient (supervisor)
                        mail.CC.Add(new MailAddress(data.SupervisorEmail));

                        // Set the subject of the email
                        mail.Subject = "Employee Early Departure Request Feedback";

                        // Set the email body content
                        mail.Body = GenerateHtmlTableForEarlyDeparture(data); // Use the HTML table generated by GenerateHtmlTable

                        // Specify that the email body is in HTML format
                        mail.IsBodyHtml = true;

                        try
                        {
                            // Send the email
                            smtpClient.Port = 587; // Specify the SMTP port (e.g., 587 for TLS)
                            smtpClient.Credentials = new NetworkCredential("mr.nurul.islam023@gmail.com", "upbm sesk okgq ovlz"); // Specify your SMTP credentials
                            smtpClient.EnableSsl = true; // Enable SSL/TLS (if required)
                            smtpClient.Send(mail);
                            var parameters = new DynamicParameters();

                            parameters.Add("emailNotificationStatus", "Notified");

                            parameters.Add("EarlyDepartureId", data.EarlyDepartureId);
                            parameters.Add("CompanyId", user.CompanyId);
                            parameters.Add("OrganizationId", user.OrganizationId);
                            //parameters.Add("BranchId", user.BranchId);

                            string sql = @$"UPDATE HR_EarlyDeparture SET EmpEmailNotificationStatus = @emailNotificationStatus
                                           WHERE EarlyDepartureId = @EarlyDepartureId AND CompanyId = @CompanyId AND OrganizationId = @OrganizationId";


                            //string sql = "UPDATE HR_LateRequests " +
                            //    "SET EmailNotificationStatus = 'Notified' " +
                            //    "WHERE LateRequestsId = @LateRequestsId, " +
                            //    "CompanyId=@CompanyId, " +
                            //    "OrganizationId=@OrganizationId";

                            var rawCount = await _dapper.SqlExecuteNonQueryStatus(user.Database, sql, parameters, CommandType.Text);

                            if (rawCount > 0)
                            {
                                executionStatus.Status = true;
                                executionStatus.Msg = "E-Mail Send Successfully!";

                            }
                            else
                            {
                                executionStatus.Status = false;
                                executionStatus.ErrorMsg = "Some Problem on Database";
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle any exceptions here for individual email sending
                            Console.WriteLine($"Error sending email to {data.FullName}: {ex.Message}");
                        }
                        finally
                        {
                            // Dispose of the MailMessage
                            mail.Dispose();
                        }
                    }
                }

                return executionStatus;
            }
            catch (Exception ex)
            {
                throw; // Re-throw the exception to be handled elsewhere
            }
            finally
            {
                smtpClient.Dispose();
            }

        }
        private string GenerateHtmlTableForEarlyDeparture(EarlyDepartureFeedbackdata data)
        {
            string emailBody = $@"<!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Employee Attendance Notice</title>
    </head>
    <body style='font-family: Arial, sans-serif;'>

        <div style='background-color: #f4f4f4; padding: 10px; text-align: center;'>
            <h2>Attendance Notice</h2>
        </div>

        <div style='padding: 20px;'>
            <p>Dear, <strong>{data.FullName}</strong>,</p>

            <p>We hope this message finds you well. We wanted to bring to your attention some concerns regarding your recent Early Departur request feedback.</p>
            <div style='text-align: left;'>
                <b>Applied Date  {data.AppliedDate:dd MMMM yyyy}</b>
                
            </div>

            [AttendanceTable]

           
        </div>

        <div style='background-color: #f4f4f4; padding: 6px; text-align: center;'>
            <p style='font-size: 12px;'>This is an automated mail. Please do not reply to this email.</p>
            <p><b>Powered By ReCom Consulting Limited.<b></p>
        </div>
    </body>
    </html>";

            // Populate attendance records in the table
            string attendanceTable = @"<div style='text-align: center;'>
        <table style='margin: 0 auto; border-collapse: collapse; width: 80%; font-family: Arial, sans-serif; border: 1px solid #dddddd;'>
            <thead>
                <tr style='background-color: #f4f4f4;'>
                    <th style='text-align: center; padding: 10px; border: 1px solid #dddddd;'>SL</th>
                    <th style='text-align: center; padding: 10px; border: 1px solid #dddddd;'>Requested Date</th>
                    <th style='text-align: center; padding: 10px; border: 1px solid #dddddd;'>Comment</th>
                    <th style='text-align: center; padding: 10px; border: 1px solid #dddddd;'>Status</th>
                </tr>
            </thead>
            <tbody>";

            int rowIndex = 1; // Initialize the row index with 1 for the serial number

            foreach (var item in data.EarlyDepartureFeedBackDetail)
            {
                string rowClass = rowIndex % 2 == 0 ? "even-row" : "odd-row"; // Determine the row class

                string row = $@"
            <tr class='{rowClass}'>
                <td style='text-align: center; padding: 5px; border: 1px solid #dddddd;'>{rowIndex}</td>
                <td style='text-align: center; padding: 5px; border: 1px solid #dddddd;'>{item.RequestedForDate:dd-MMM-yyyy}</td>
                <td style='text-align: center; padding: 5px; border: 1px solid #dddddd;'>{item.Comment}</td>
                <td style='text-align: center; padding: 5px; border: 1px solid #dddddd;'>{item.Flag}</td>
            </tr>";

                attendanceTable += row;
                rowIndex++; // Increment the row index
            }

            attendanceTable += @"
            </tbody>
        </table>
    </div>";

            emailBody = emailBody.Replace("[AttendanceTable]", attendanceTable);

            return emailBody;
        }




        public async Task<DBResponse<EarlyDepartureViewModel>> GetEarlyMasterByIdAsync(LateRequestFilter query, AppUser User)
        {
            string flagValue = "Master";
            DBResponse<EarlyDepartureViewModel> data = new DBResponse<EarlyDepartureViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                sqlQuery = string.Format(@"sp_HR_EarlyDepartureRequestList");
                //var parameters = Utility.DappperParams(flagValue, User, addBaseProperty: true);
                // parameters.Add("@Flag", flagValue);
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", User.EmployeeId);
                parameters.Add("UserId", User.UserId);
                parameters.Add("CompanyId", User.CompanyId);
                parameters.Add("OrganizationId", User.OrganizationId);
                parameters.Add("BranchId", User.BranchId);
                parameters.Add("Flag", flagValue);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(User.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<EarlyDepartureViewModel>>(response.JSONData) ?? new List<EarlyDepartureViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, User.Database, "EarlyDepartur", "SaveEarlyDepartur", User.Username, User.OrganizationId, User.CompanyId, User.BranchId);
            }
            return data;
        }




        public async Task<EmployeeDTO> GetEmployeeDetailsById(long? empId, AppUser user)
        {
            EmployeeDTO data = new EmployeeDTO();

            try
            {
                string sqlQuery = EmployeeDetails() + " WHERE e.EmployeeId = @EmployeeId";
                data = await _dapper.SqlQueryFirstAsync<EmployeeDTO>(user.Database, sqlQuery, new { EmployeeId = empId }, CommandType.Text);
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
        private string GenerateHtmlTableForEarlyDepartureSave(EmployeeDTO data, EarlyDepartureViewModel query)
        {
            string emailBody = $@"<!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Employee Attendance Notice</title>
    </head>
    <body style='font-family: Arial, sans-serif;'>

        <div style='background-color: #f4f4f4; padding: 10px; text-align: center;'>
            <h2>Attendance Notice</h2>
        </div>

        <div style='padding: 20px;'>
            <p>Dear, <strong>{data.SupervisorName}</strong>,</p>

            <p>We hope this message finds you well. We wanted to bring to your attention some concerns regarding your recent Early Departur request from <strong>Mr./Mrs {data.Name}</strong>.</p>
            <div style='text-align: left;'>
                <b>Requested Date  {query.RequestedForDate:dd MMMM yyyy}</b>
                
            </div>

           

           
        </div>

        <div style='background-color: #f4f4f4; padding: 6px; text-align: center;'>
            <p style='font-size: 12px;'>This is an automated mail. Please do not reply to this email.</p>
            <p><b>Powered By ReCom Consulting Limited.<b></p>
        </div>
    </body>
    </html>";

            return emailBody;
        }

    }
}

