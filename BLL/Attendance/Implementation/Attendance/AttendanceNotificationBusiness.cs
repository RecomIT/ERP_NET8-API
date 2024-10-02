using Shared.OtherModels.User;
using Dapper;
using Shared.Services;
using System.Net.Mail;
using System.Net;
using System.Data;
using BLL.Base.Interface;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.Attendance;
using Shared.Attendance.ViewModel.Attendance;
using BLL.Administration.Interface;

namespace BLL.Attendance.Implementation.Attendance
{
    public class AttendanceNotificationBusiness : IAttendanceNotificationBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly IModuleConfigBusiness _moduleConfigBusiness;
        private readonly ILoginManager _loginManager;
        public AttendanceNotificationBusiness(ISysLogger sysLogger, IDapperData dapper, IModuleConfigBusiness moduleConfigBusiness, ILoginManager loginManager)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _moduleConfigBusiness = moduleConfigBusiness;
            _loginManager = loginManager;
        }
        public async Task AttendanceEmailNotificationProcess(AppUser user)
        {
            List<string> listOfLateSet = new List<string>();
            try
            {
                var moduleConfig = (await _moduleConfigBusiness.GetHRModuleConfigsAsync(user.CompanyId, user.OrganizationId)).FirstOrDefault();
                if (moduleConfig != null)
                {
                    if (moduleConfig.EnableMaxLateWarning != null && moduleConfig.EnableMaxLateWarning == true)
                    {
                        // Attendance list
                        var attendanceData = await EmployeeLateEntryEmailNotificationProcess(user);
                        if (attendanceData.ToList().Count > 0)
                        {
                            //Email Config 
                            var emailSetting = await _loginManager.EmailSettings("Attendance");
                            if (emailSetting != null)
                            {
                                var uniqueLateId = attendanceData.Select(item => item.LateSetId).Distinct();
                                foreach (var lateId in uniqueLateId)
                                {
                                    // late list
                                    var lateList = attendanceData.Where(item => item.LateSetId == lateId).ToList();

                                    var records = lateList.Select(item => new EmployeeAttendance
                                    {
                                        Date = item.TransactionDate,
                                        Day = item.DayName,
                                        Shift_Time = item.StartTime + "-" + item.EndTime,
                                        In_Time = item.InTime,
                                        Out_Time = item.OutTime,
                                        Total_Late_Time = item.TotalLateTime,
                                        Total_Working_Hours = item.TotalWorkingHours
                                    });

                                    var first_item = lateList.FirstOrDefault();
                                    var supervisorEmail = "";
                                    var employeeEmail = "";
                                    var supervisorName = "";
                                    var employeeName = "";
                                    var employeeCode = "";
                                    if (first_item != null)
                                    {
                                        supervisorEmail = first_item.SupervisorEmail;
                                        employeeEmail = first_item.OfficeEmail;
                                        supervisorName = first_item.SupervisorName;
                                        employeeName = first_item.FullName;
                                        employeeCode = first_item.EmployeeCode;
                                    }

                                    if (!string.IsNullOrEmpty(supervisorEmail))
                                    {
                                        var lateHtmlTable = records.ToHtmlTable();

                                        MailMessage message = new MailMessage();
                                        message.From = new MailAddress(emailSetting.EmailAddress, emailSetting.DisplayName);
                                        if (moduleConfig.EnableMaxLateWarning == true)
                                        {
                                            message.To.Add(new MailAddress(supervisorEmail));
                                        }


                                        message.Subject = emailSetting.Subject;
                                        message.IsBodyHtml = emailSetting.IsBodyHtml;
                                        message.Body = EmailTemplate.SendEmployeeLateNotification(supervisorName, employeeName, employeeCode, lateHtmlTable);//lateHtmlTable;//Utility.GetEmailTemplate(flag, password);

                                        SmtpClient smtp = new SmtpClient();
                                        smtp.EnableSsl = emailSetting.EnableSsl;
                                        smtp.UseDefaultCredentials = emailSetting.UseDefaultCredentials;
                                        smtp.Port = Convert.ToInt32(emailSetting.Port);
                                        smtp.Host = emailSetting.Host;
                                        smtp.Credentials = new NetworkCredential(emailSetting.EmailAddress, emailSetting.EmailPassword);
                                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                        await smtp.SendMailAsync(message);
                                        listOfLateSet.Add(lateId);
                                    }

                                }

                                // Change Mail Status 
                                if (listOfLateSet.Count > 0)
                                {
                                    var LateSetIdS = string.Join(',', listOfLateSet);
                                    var sp_name = string.Format(@"sp_HR_EmployeeLateAttendanceEmailStatusUpdate");
                                    var parameters = new DynamicParameters();
                                    parameters.Add("@LateSetIdS", LateSetIdS);
                                    parameters.Add("@CompanyId", user.CompanyId);
                                    parameters.Add("@OrganizationId", user.OrganizationId);
                                    await _dapper.SqlExecuteNonQuery(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceNotificationBusiness", "AttendanceEmailNotificationProcess", user);
            }
        }
        public Task<IEnumerable<EmployeeLateEntryEmailNotification>> EmployeeLateEmailAlertProcess(AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<EmployeeLateEntryEmailNotification>> EmployeeLateEntryEmailNotificationProcess(AppUser user)
        {
            IEnumerable<EmployeeLateEntryEmailNotification> list = new List<EmployeeLateEntryEmailNotification>();
            try
            {
                var sp_name = "sp_HR_EmployeeLateEntryEmailNotification";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);

                list = await _dapper.SqlQueryListAsync<EmployeeLateEntryEmailNotification>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceNotificationBusiness", "EmployeeLateEntryEmailNotificationProcess", user);
            }
            return list;
        }
    }
}
