using System;
using System.Text;
using Shared.Employee.ViewModel.Info;
using Shared.Leave.ViewModel.Request;

namespace Shared.Services
{
    public static class EmailTemplate
    {
        public static string GetLeaveEmailTemplate(string flag, string recipientName, EmployeeLeaveRequestViewModel presentleaveInfo, EmployeeLeaveRequestViewModel previousleaveInfo, EmployeeServiceDataViewModel employeeInfo, EmployeeServiceDataViewModel supervisorInfo, EmployeeServiceDataViewModel headOfDepartment, EmployeeServiceDataViewModel actionUser)
        {
            if (flag == "Request") {
                return EmailTemplate.Request(recipientName, presentleaveInfo, employeeInfo);
            }
            else if (flag == "Modified") {
                return EmailTemplate.Modified(recipientName, presentleaveInfo, previousleaveInfo, employeeInfo);
            }
            else if (flag == "Recommended") {
                return EmailTemplate.Recommended(recipientName, presentleaveInfo, previousleaveInfo, employeeInfo, actionUser);
            }
            else if (flag == "Approved") {
                return EmailTemplate.Approved(recipientName, presentleaveInfo, previousleaveInfo, employeeInfo, actionUser);
            }
            else if (flag == "Rejected") {
                return EmailTemplate.Rejected(recipientName, presentleaveInfo, previousleaveInfo, employeeInfo, actionUser);
            }
            else if (flag == "Cancelled") {
                return EmailTemplate.Cancelled(recipientName, presentleaveInfo, previousleaveInfo, employeeInfo, actionUser);
            }
            else if (flag == "ApprovedLeaveCancelled") {
                return EmailTemplate.ApprovedLeaveCancelled(recipientName, presentleaveInfo, previousleaveInfo, employeeInfo, actionUser);
            }
            return "";
        }
        public static string GetEmailTemplate(string flag, string empDtls, string leaveTypeName, string userName, string remarks, string status)
        {
            if (flag == "Request") {
                return EmailTemplate.Request(empDtls, leaveTypeName);
            }
            else if (flag == "Modified") {
                return EmailTemplate.Modified(empDtls, leaveTypeName);
            }
            else if (flag == "Cancelled") {
                return EmailTemplate.Cancelled(empDtls, leaveTypeName);
            }
            else if (flag == "Approved") {
                if (status == "Approved") {
                    return EmailTemplate.Approved(leaveTypeName, userName, remarks);
                }
                else
                if (status == "Recheck") {
                    return EmailTemplate.Recheck(leaveTypeName, userName, remarks);
                }
                else
                if (status == "Cancelled") {
                    return EmailTemplate.Cancelled(leaveTypeName, userName, remarks);
                }
            }
            return string.Empty;
        }
        public static string ForgetPasswordOTP(string otp)
        {
            return String.Format(@"<div style='font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2'>
  <div style='margin:50px auto;width:70%;padding:20px 0'>
    <div style='border-bottom:1px solid #eee'>
      <a href='' style='font-size:1.4em;color: #00466a;text-decoration:none;font-weight:600'>Forget Password Email Verification</a>
    </div>
    <p style='font-size:1.1em'>Hi,</p>
    <p>Use the following OTP to verify your email address. OTP is valid for 5 minutes</p>
    <h2 style='background: #00466a;margin: 0 auto;width: max-content;padding: 0 10px;color: #fff;border-radius: 4px;'>{0}</h2>
    <hr style='border:none;border-top:1px solid #eee' />
    <p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>
    <div style='float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300'>
      <p>Developed & Maintained By Recom Consulting Limited</p>
    </div>
  </div>
</div>", otp.Trim());

            
        }
        public static string SendDefaultPasswordWhenUserForgetPassword(string password)
        {
            return String.Format(@"<div style='font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2'>
  <div style='margin:50px auto;width:70%;padding:20px 0'>
    <div style='border-bottom:1px solid #eee'>
      <a href='' style='font-size:1.4em;color: #00466a;text-decoration:none;font-weight:600'>Default Password After Verify Forget Password OTP</a>
    </div>
    <p style='font-size:1.1em'>Hi,</p>
    <p>Use the following Password to login your account. After logged into your account. You must change this default password. Please do not share your Password.</p>
    <h2 style='background: #00466a;margin: 0 auto;width: max-content;padding: 0 10px;color: #fff;border-radius: 4px;'>{0}</h2>
    <hr style='border:none;border-top:1px solid #eee' />
    <p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>
    <div style='float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300'>
      <p>Developed & Maintained By Recom Consulting Limited</p>
    </div>
  </div>
</div>", password.Trim());
        }
        public static string SendEmployeeLateNotification(string to, string employeeName, string employeeCode, string table)
        {
            return string.Format(@"
    <h5>Dear Mr./Mrs {0},<h5>
<p style='font-weight:normal'>This is to inform you that one of your subordinates Mr./Mrs {1} [ID : {2}] is consistently late in coming to work.<p/>
<p style='font-weight:normal'>Here is his/her attendance late history:</p>
{3}", to, employeeName, employeeCode, table);

        }

        public static string OnceoffPaymentPayslip(string recipientName, string monthName, string year, string allowanceName)
        {
            return string.Format(@"<p style='font-family: Georgia, serif; font-size: 13px;'>Dear Mr/Ms {0},</p>
    <p style='font-family: Georgia, serif; font-size: 13px;'>Attached is your {3} payslip for the month of {1} {2}.The attached files are password protected, please use your employee ID number to open these files.</p>
    <p style='font-family: Georgia, serif; font-size: 13px;'><span>In case of any queries, please reach out to</span> <a href='mailto: sumit.sazed@pwc.com'>sumit.sazed@pwc.com</a> / <a href='mailto: nafees.ali@pwc.com'>nafees.ali@pwc.com</a></p>
    <p style='font-family: Georgia, serif; font-size: 13px;color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>
    <br/>
    <br/>
    <p style='font-family: Georgia, serif; font-size: 13px;'>Regards</p>
    <p style='font-family: Georgia, serif; font-size: 13px;'>ReCom Consulting Ltd.</p>
    <p style='font-family: Georgia, serif; font-size: 13px;'>On behalf of PwC Bangladesh Private Limited.</p>", recipientName, monthName, year, allowanceName);
        }
        public static string SendPayslipAndTaxCard(string recipientName, string monthName, string year)
        {
            return string.Format(@"<p style='font-family: Georgia, serif; font-size: 13px;'>Dear Mr/Ms {0},</p>
    <p style='font-family: Georgia, serif; font-size: 13px;'>Attached is your payslip and tax certificate for the month of {1} {2}.The attached files are password protected, please use your employee ID number to open these files.</p>
    <p style='font-family: Georgia, serif; font-size: 13px;'><span>In case of any queries, please reach out to</span> <a href='mailto: sumit.sazed@pwc.com'>sumit.sazed@pwc.com</a> / <a href='mailto: nafees.ali@pwc.com'>nafees.ali@pwc.com</a></p>
    <p style='font-family: Georgia, serif; font-size: 13px;color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>
    <br/>
    <br/>
    <p style='font-family: Georgia, serif; font-size: 13px;'>Regards</p>
    <p style='font-family: Georgia, serif; font-size: 13px;'>ReCom Consulting Ltd.</p>
    <p style='font-family: Georgia, serif; font-size: 13px;'>On behalf of PwC Bangladesh Private Limited.</p>", recipientName, monthName, year);
        }
        public static string SendTaxCard(string recipientName, string fiscalYearRange)
        {
            return string.Format(@"<p style='font-family: Georgia, serif; font-size: 13px;'>Dear Mr/Ms {0},</p>
    <p style='font-family: Georgia, serif; font-size: 13px;'>Attached is your tax certificate for for FY {1}. The attached file is password protected, please use your employee ID number to open these files.</p>
    <p style='font-family: Georgia, serif; font-size: 13px;'><span>In case of any queries, please reach out to</span> <a href='mailto: sumit.sazed@pwc.com'>sumit.sazed@pwc.com</a> / <a href='mailto: nafees.ali@pwc.com'>nafees.ali@pwc.com</a></p>
    <p style='font-family: Georgia, serif; font-size: 13px;color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>
    <br/>
    <br/>
    <p style='font-family: Georgia, serif; font-size: 13px;'>Regards</p>
    <p style='font-family: Georgia, serif; font-size: 13px;'>ReCom Consulting Ltd.</p>
    <p style='font-family: Georgia, serif; font-size: 13px;'>On behalf of PwC Bangladesh Private Limited.</p>", recipientName, fiscalYearRange);
        }
        public static string Request(string empDtls, string leaveTypeName)
        {

            var template = string.Format(@"<p style='font-family: Century Gothic, serif; font-size: 14px;'>Dear Mr/Ms Recipient,</p>
    <p style='font-family: Century Gothic, serif; font-size: 14px;'><b>{0}</b> has requested <b>{1}</b>.</p>
    <p style='font-family: Century Gothic, serif; font-size: 14px;'>You can track its status through our ERP system by clicking <a href='{3}'>here</a>.</p>
    <br/>
    <p style='font-family: Century Gothic, serif; font-size: 13px;'>Best Regrads</p>
    <p style='font-family: Century Gothic, serif; font-size: 13px;'><b>Recom Consulting Limited</b></p>
    <p style='font-family: Century Gothic, serif; font-size: 10px;color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>", empDtls, leaveTypeName, AppSettings.ClientOrigin);
            return template;
        }
        public static string Request(string recipientName, EmployeeLeaveRequestViewModel presentleaveInfo, EmployeeServiceDataViewModel employeeInfo)
        {

            var sb = new StringBuilder();

            sb.Append(@"<html>
    <head>
        <style>
            body{
                font-family: Century Gothic, serif; font-size: 10px;
            }
            button {
                background-color: #4CAF50;
                border: none;
                color: white;
                padding: 10px 10px;
                text-align: center;
                text-decoration: none;
                display: inline-block;
                font-size: 14px;
                margin: 2px 2px;
                cursor: pointer;
                border-radius: 5px;
            }
            body{
                font-family: Century Gothic, serif; font-size: 12px;
            }

            .approve-button {
                background-color: #4CAF50;
                border: none;
                color: white;
            }

            .reject-button {
                background-color: #FF0000;
                border: none;
                color: white;
            }

            p {
                margin-bottom: 10px;
                font-weight: bold;
                color: #000000;
            }

            li {
                color: #000000;
            }
        </style>
    </head>
    <body>");

            sb.AppendFormat(@"<p>Dear {0},</p>", recipientName);
            sb.Append(@"<p>You have a new leave request that requires your attention:</p>");
            sb.Append(@"<ul>");
            sb.AppendFormat(@"<li><strong>Name:</strong> {0} </li>", employeeInfo.EmployeeName);
            sb.AppendFormat(@"<li><strong>Designation:</strong> {0} </li>", employeeInfo.DesignationName);
            sb.AppendFormat(@"<li><strong>Leave Type:</strong> {0} </li>", presentleaveInfo.LeaveTypeName);
            sb.AppendFormat(@"<li><strong>Leave Balance:</strong> {0} </li>", presentleaveInfo.LeaveBalance);
            sb.AppendFormat(@"<li><strong>Leave Applied For:</strong> {0} days</li>", presentleaveInfo.AppliedTotalDays);
            sb.AppendFormat(@"<li><strong>Leave Dates:</strong> {0} </li>",
                (presentleaveInfo.AppliedFromDate.Value.ToString("dd-MMM-yyyy") + " To " +
                 presentleaveInfo.AppliedToDate.Value.ToString("dd-MMM-yyyy")));
            sb.AppendFormat(@"<li><strong>Reason:</strong> {0} </li>", presentleaveInfo.LeavePurpose);
            sb.AppendFormat(@"<li><strong>Applied Date:</strong> {0} </li>", presentleaveInfo.CreatedDate.Value.ToString("dd MMM yyyy hh:mm:ss tt"));
            sb.AppendFormat(@"</ul>");
            sb.AppendFormat(@"<p>Please review and take action on the request:</p>");
            //sb.AppendFormat(@"<button class='approve-button'><a href='{0}' style='text - decoration: none; color: white;'>Approve</a></button>",AppSettings.ClientOrigin+"/approve/"+ presentleaveInfo.EmployeeLeaveRequestId);
            //sb.AppendFormat(@"<button class='reject-button'><a href='{0}' style='text - decoration: none; color: white;'>Reject</a></button>", AppSettings.ClientOrigin + "/reject/" + presentleaveInfo.EmployeeLeaveRequestId);
            if(AppSettings.App_environment == "Local") {
                sb.AppendFormat(@"<button class='approve-button'><a href='{0}' style='text - decoration: none; color: white;'>Approve</a></button>", AppSettings.ClientOrigin);
                sb.AppendFormat(@"<button class='reject-button'><a href='{0}' style='text - decoration: none; color: white;'>Reject</a></button>", AppSettings.ClientOrigin);
                sb.AppendFormat(@"<br/><br/>");
            }
       
            sb.AppendFormat(@"<p><p>If you log in to our portal, please <a href='{0}'>Login</a> to view and manage leave requests.</p>", AppSettings.ClientOrigin);
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");


            string template = sb.ToString();
            return template;
        }
        public static string Modified(string recipientName, EmployeeLeaveRequestViewModel presentleaveInfo, EmployeeLeaveRequestViewModel previousleaveInfo, EmployeeServiceDataViewModel employeeInfo)
        {

            var sb = new StringBuilder();

            sb.Append(@"<html>
    <head>
        <style>
            button {
                background-color: #4CAF50;
                border: none;
                color: white;
                padding: 10px 10px;
                text-align: center;
                text-decoration: none;
                display: inline-block;
                font-size: 14px;
                margin: 2px 2px;
                cursor: pointer;
                border-radius: 5px;
            }
            body{
                font-family: Century Gothic, serif; font-size: 12px;
            }

            .approve-button {
                background-color: #4CAF50;
                border: none;
                color: white;
            }

            .reject-button {
                background-color: #FF0000;
                border: none;
                color: white;
            }

            p {
                margin-bottom: 10px;
                font-weight: bold;
                color: #000000;
            }

            li {
                color: #000000;
            }
        </style>
    </head>
    <body>");

            sb.AppendFormat(@"<p>Dear {0},</p>", recipientName);
            sb.Append(@"<p>You have a new leave request that requires your attention:</p>
        <ul>");
            sb.AppendFormat(@"<li><strong>Name:</strong> {0} </li>", employeeInfo.EmployeeName);
            sb.AppendFormat(@"<li><strong>Designation:</strong> {0} </li>", employeeInfo.DesignationName);
            sb.AppendFormat(@"<li><strong>Leave Type:</strong> {0} </li>", presentleaveInfo.LeaveTypeName);
            sb.AppendFormat(@"<li><strong>Leave Balance:</strong> {0} </li>", presentleaveInfo.LeaveBalance);
            sb.AppendFormat(@"<li><strong>Leave Applied For:</strong> {0} days</li>", presentleaveInfo.AppliedTotalDays);
            sb.AppendFormat(@"<li><strong>Leave Dates:</strong> {0} </li>",
                (presentleaveInfo.AppliedFromDate.Value.ToString("dd-MMM-yyyy") + " To " +
                 presentleaveInfo.AppliedToDate.Value.ToString("dd-MMM-yyyy")));
            sb.AppendFormat(@"<li><strong>Reason:</strong> {0} </li>", presentleaveInfo.LeavePurpose);
            sb.AppendFormat(@"<li><strong>Applied Date:</strong> {0} </li>", presentleaveInfo.CreatedDate.Value.ToString("dd MMM yyyy hh:mm:ss tt"));


            sb.AppendFormat(@"</ul>");
            sb.AppendFormat(@"<b>Modified Informations:</b>");
            sb.AppendFormat(@"<ul>");
            sb.AppendFormat(@"<li><strong>Modified Leave Type:</strong>{0} (Modified to: {1})</li>", previousleaveInfo.LeaveTypeName, presentleaveInfo.LeaveTypeName);
            sb.AppendFormat(@"<li><strong>Modified Leave Applied For:</strong> {0}  days (Modified to: {1})</li>", previousleaveInfo.AppliedTotalDays, presentleaveInfo.AppliedTotalDays);
            sb.AppendFormat(@"<li><strong>Modified Leave Dates:</strong>{0} (Modified to: {1})</li>", (previousleaveInfo.AppliedFromDate.Value.ToString("dd-MMM-yyyy") + " To " +
                 previousleaveInfo.AppliedToDate.Value.ToString("dd-MMM-yyyy")), (presentleaveInfo.AppliedFromDate.Value.ToString("dd-MMM-yyyy") + " To " +
                 presentleaveInfo.AppliedToDate.Value.ToString("dd-MMM-yyyy")));
            sb.AppendFormat(@"<li><strong>Modified Reason:</strong>{0} (Modified to: {1})</li>", previousleaveInfo.LeavePurpose, presentleaveInfo.LeavePurpose);
            sb.AppendFormat(@"</ul>");
            sb.AppendFormat(@"<p>Please review and take action on the request:</p>");
            if (AppSettings.App_environment == "Local") {
                sb.AppendFormat(@"<button class='approve-button'><a href='{0}' style='text - decoration: none; color: white;'>Approve</a></button>", AppSettings.ClientOrigin);
                sb.AppendFormat(@"<button class='reject-button'><a href='{0}' style='text - decoration: none; color: white;'>Reject</a></button>", AppSettings.ClientOrigin);
                sb.AppendFormat(@"<br/><br/>");
            }
            sb.AppendFormat(@"<p><p>If you log in to our portal, please <a href='{0}'>Login</a> to view and manage leave requests.</p>", AppSettings.ClientOrigin);
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");

            string template = sb.ToString();
            return template;
        }
        public static string Recommended(string recipientName, EmployeeLeaveRequestViewModel presentleaveInfo, EmployeeLeaveRequestViewModel previousleaveInfo, EmployeeServiceDataViewModel employeeInfo, EmployeeServiceDataViewModel recommendedBy)
        {

            var sb = new StringBuilder();

            sb.Append(@"<html>
    <head>
        <style>
            button {
                background-color: #4CAF50;
                border: none;
                color: white;
                padding: 10px 10px;
                text-align: center;
                text-decoration: none;
                display: inline-block;
                font-size: 14px;
                margin: 2px 2px;
                cursor: pointer;
                border-radius: 5px;
            }
            body{
                font-family: Century Gothic, serif; font-size: 12px;
            }

            .approve-button {
                background-color: #4CAF50;
                border: none;
                color: white;
            }

            .reject-button {
                background-color: #FF0000;
                border: none;
                color: white;
            }

            p {
                margin-bottom: 10px;
                font-weight: bold;
                color: #000000;
            }

            li {
                color: #000000;
            }
            h5{
                font-weight: normal;
            }
        </style>
    </head>
    <body>");

            sb.AppendFormat(@"<p>Dear {0},</p>", recipientName);
            sb.AppendFormat(@"<h5>You have a new leave request <b>recommended by: {0}</b>  that requires your attention:</h5>", (recommendedBy.EmployeeName + " (" + recommendedBy.EmployeeCode + ")"));
            sb.Append("<ul>");
            sb.AppendFormat(@"<li><strong>Name:</strong> {0} </li>", employeeInfo.EmployeeName);
            sb.AppendFormat(@"<li><strong>Designation:</strong> {0} </li>", employeeInfo.DesignationName);
            sb.AppendFormat(@"<li><strong>Leave Type:</strong> {0} </li>", presentleaveInfo.LeaveTypeName);
            sb.AppendFormat(@"<li><strong>Leave Balance:</strong> {0} </li>", presentleaveInfo.LeaveBalance);
            sb.AppendFormat(@"<li><strong>Leave Applied For:</strong> {0} days</li>", presentleaveInfo.AppliedTotalDays);
            sb.AppendFormat(@"<li><strong>Leave Dates:</strong> {0} </li>",
                (presentleaveInfo.AppliedFromDate.Value.ToString("dd-MMM-yyyy") + " To " +
                 presentleaveInfo.AppliedToDate.Value.ToString("dd-MMM-yyyy")));
            sb.AppendFormat(@"<li><strong>Reason:</strong> {0} </li>", presentleaveInfo.LeavePurpose);
            sb.AppendFormat(@"<li><strong>Applied Date:</strong> {0} </li>", presentleaveInfo.CreatedDate.Value.ToString("dd MMM yyyy hh:mm:ss tt"));
            sb.AppendFormat(@"<li><strong>Recommender's Comments:</strong> {0} </li>", presentleaveInfo.CheckRemarks);
            sb.Append(@"</ul>");
            sb.AppendFormat(@"<p>Please review and take action on the request:</p>");
            if (AppSettings.App_environment == "Local") {
                sb.AppendFormat(@"<button class='approve-button'><a href='{0}' style='text - decoration: none; color: white;'>Approve</a></button>", AppSettings.ClientOrigin);
                sb.AppendFormat(@"<button class='reject-button'><a href='{0}' style='text - decoration: none; color: white;'>Reject</a></button>", AppSettings.ClientOrigin);
                sb.AppendFormat(@"<br/><br/>");
            }
            sb.AppendFormat(@"<p><p>If you log in to our portal, please <a href='{0}'>Login</a> to view and manage leave requests.</p>", AppSettings.ClientOrigin);
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");

            string template = sb.ToString();
            return template;
        }
        public static string Approved(string recipientName, EmployeeLeaveRequestViewModel presentleaveInfo, EmployeeLeaveRequestViewModel previousleaveInfo, EmployeeServiceDataViewModel employeeInfo, EmployeeServiceDataViewModel approvedBy)
        {
            var sb = new StringBuilder();

            sb.Append(@"<html>
    <head>
        <style>
            button {
                background-color: #4CAF50;
                border: none;
                color: white;
                padding: 10px 10px;
                text-align: center;
                text-decoration: none;
                display: inline-block;
                font-size: 14px;
                margin: 2px 2px;
                cursor: pointer;
                border-radius: 5px;
            }
            body{
                font-family: Century Gothic, serif; font-size: 12px;
            }

            .approve-button {
                background-color: #4CAF50;
                border: none;
                color: white;
            }

            .reject-button {
                background-color: #FF0000;
                border: none;
                color: white;
            }

            p {
                margin-bottom: 10px;
                font-weight: bold;
                color: #000000;
            }

            li {
                color: #000000;
            }
            h5{
                font-weight: normal;
            }
        </style>
    </head>
    <body>");

            sb.AppendFormat(@"<p>Dear {0},</p>", recipientName);
            sb.AppendFormat(@"<h5>Your leave request has been <b>approved by: {0}</b>:</h5>
        <ul>", (approvedBy.EmployeeName + " (" + approvedBy.EmployeeCode + ")"));
            sb.AppendFormat(@"<li><strong>Name:</strong> {0} </li>", employeeInfo.EmployeeName);
            sb.AppendFormat(@"<li><strong>Designation:</strong> {0} </li>", employeeInfo.DesignationName);
            sb.AppendFormat(@"<li><strong>Leave Type:</strong> {0} </li>", presentleaveInfo.LeaveTypeName);
            sb.AppendFormat(@"<li><strong>Leave Balance:</strong> {0} </li>", presentleaveInfo.LeaveBalance);
            sb.AppendFormat(@"<li><strong>Leave Applied For:</strong> {0} days</li>", presentleaveInfo.AppliedTotalDays);
            sb.AppendFormat(@"<li><strong>Leave Dates:</strong> {0} </li>",
                (presentleaveInfo.AppliedFromDate.Value.ToString("dd-MMM-yyyy") + " To " +
                 presentleaveInfo.AppliedToDate.Value.ToString("dd-MMM-yyyy")));
            sb.AppendFormat(@"<li><strong>Reason:</strong> {0} </li>", presentleaveInfo.LeavePurpose);
            sb.AppendFormat(@"<li><strong>Applied Date:</strong> {0} </li>", presentleaveInfo.CreatedDate.Value.ToString("dd MMM yyyy hh:mm:ss tt"));
            sb.AppendFormat(@"<li><strong>Approver's Comments:</strong> {0} </li>", presentleaveInfo.ApprovalRemarks);
            sb.AppendFormat(@"</ul>");
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");

            string template = sb.ToString();
            return template;
        }
        public static string Rejected(string recipientName, EmployeeLeaveRequestViewModel presentleaveInfo, EmployeeLeaveRequestViewModel previousleaveInfo, EmployeeServiceDataViewModel employeeInfo, EmployeeServiceDataViewModel rejectedBy)
        {
            var sb = new StringBuilder();

            sb.Append(@"<html>
    <head>
        <style>
            button {
                background-color: #4CAF50;
                border: none;
                color: white;
                padding: 10px 10px;
                text-align: center;
                text-decoration: none;
                display: inline-block;
                font-size: 14px;
                margin: 2px 2px;
                cursor: pointer;
                border-radius: 5px;
            }
            body{
                font-family: Century Gothic, serif; font-size: 12px;
            }

            .approve-button {
                background-color: #4CAF50;
                border: none;
                color: white;
            }

            .reject-button {
                background-color: #FF0000;
                border: none;
                color: white;
            }

            p {
                margin-bottom: 10px;
                font-weight: bold;
                color: #000000;
            }

            li {
                color: #000000;
            }
            h5{
                font-weight: normal;
            }
        </style>
    </head>
    <body>");

            sb.AppendFormat(@"<p>Dear {0},</p>", recipientName);
            sb.AppendFormat(@"<h5>Your leave request has been <b> rejected by: {0}</b>:</h5>
        <ul>", (rejectedBy.EmployeeName + " (" + rejectedBy.EmployeeCode + ")"));
            sb.AppendFormat(@"<li><strong>Name:</strong> {0} </li>", employeeInfo.EmployeeName);
            sb.AppendFormat(@"<li><strong>Designation:</strong> {0} </li>", employeeInfo.DesignationName);
            sb.AppendFormat(@"<li><strong>Leave Type:</strong> {0} </li>", presentleaveInfo.LeaveTypeName);
            sb.AppendFormat(@"<li><strong>Leave Balance:</strong> {0} </li>", presentleaveInfo.LeaveBalance);
            sb.AppendFormat(@"<li><strong>Leave Applied For:</strong> {0} days</li>", presentleaveInfo.AppliedTotalDays);
            sb.AppendFormat(@"<li><strong>Leave Dates:</strong> {0} </li>",
                (presentleaveInfo.AppliedFromDate.Value.ToString("dd-MMM-yyyy") + " To " +
                 presentleaveInfo.AppliedToDate.Value.ToString("dd-MMM-yyyy")));
            sb.AppendFormat(@"<li><strong>Reason:</strong> {0} </li>", presentleaveInfo.LeavePurpose);
            sb.AppendFormat(@"<li><strong>Applied Date:</strong> {0} </li>", presentleaveInfo.CreatedDate.Value.ToString("dd MMM yyyy hh:mm:ss tt"));
            sb.AppendFormat(@"<li><strong>Comments:</strong> {0} </li>", presentleaveInfo.RejectedRemarks);
            sb.AppendFormat(@"</ul>");
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");

            string template = sb.ToString();
            return template;
        }
        public static string Cancelled(string recipientName, EmployeeLeaveRequestViewModel presentleaveInfo, EmployeeLeaveRequestViewModel previousleaveInfo, EmployeeServiceDataViewModel employeeInfo, EmployeeServiceDataViewModel cancelledBy)
        {
            var sb = new StringBuilder();

            sb.Append(@"<html>
    <head>
        <style>
            button {
                background-color: #4CAF50;
                border: none;
                color: white;
                padding: 10px 10px;
                text-align: center;
                text-decoration: none;
                display: inline-block;
                font-size: 14px;
                margin: 2px 2px;
                cursor: pointer;
                border-radius: 5px;
            }
            body{
                font-family: Century Gothic, serif; font-size: 12px;
            }

            .approve-button {
                background-color: #4CAF50;
                border: none;
                color: white;
            }

            .reject-button {
                background-color: #FF0000;
                border: none;
                color: white;
            }

            p {
                margin-bottom: 10px;
                font-weight: bold;
                color: #000000;
            }

            li {
                color: #000000;
            }
            h5{
                font-weight: normal;
            }
        </style>
    </head>
    <body>");
            sb.AppendFormat(@"<p>Dear {0},</p>", recipientName);
            sb.AppendFormat(@"<h5>Your team member's leave request has been <b> cancelled by: {0}</b>:</h5>", (cancelledBy.EmployeeName + " (" + cancelledBy.EmployeeCode + ")"));
            sb.AppendFormat(@"<ul>");
            sb.AppendFormat(@"<li><strong>Name:</strong> {0} </li>", employeeInfo.EmployeeName);
            sb.AppendFormat(@"<li><strong>Designation:</strong> {0} </li>", employeeInfo.DesignationName);
            sb.AppendFormat(@"<li><strong>Leave Type:</strong> {0} </li>", presentleaveInfo.LeaveTypeName);
            sb.AppendFormat(@"<li><strong>Leave Balance:</strong> {0} </li>", presentleaveInfo.LeaveBalance);
            sb.AppendFormat(@"<li><strong>Leave Applied For:</strong> {0} days</li>", presentleaveInfo.AppliedTotalDays);
            sb.AppendFormat(@"<li><strong>Leave Dates:</strong> {0} </li>",
                (presentleaveInfo.AppliedFromDate.Value.ToString("dd-MMM-yyyy") + " To " +
                 presentleaveInfo.AppliedToDate.Value.ToString("dd-MMM-yyyy")));
            sb.AppendFormat(@"<li><strong>Reason:</strong> {0} </li>", presentleaveInfo.LeavePurpose);
            sb.AppendFormat(@"<li><strong>Applied Date:</strong> {0} </li>", presentleaveInfo.CreatedDate.Value.ToString("dd MMM yyyy hh:mm:ss tt"));
            sb.AppendFormat(@"</ul>");
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");

            string template = sb.ToString();
            return template;
        }

        public static string ApprovedLeaveCancelled(string recipientName, EmployeeLeaveRequestViewModel presentleaveInfo, EmployeeLeaveRequestViewModel previousleaveInfo, EmployeeServiceDataViewModel employeeInfo, EmployeeServiceDataViewModel cancelledBy)
        {
            var sb = new StringBuilder();
            sb.Append(@"<html>
    <head>
        <style>
            button {
                background-color: #4CAF50;
                border: none;
                color: white;
                padding: 10px 10px;
                text-align: center;
                text-decoration: none;
                display: inline-block;
                font-size: 14px;
                margin: 2px 2px;
                cursor: pointer;
                border-radius: 5px;
            }
            body{
                font-family: Century Gothic, serif; font-size: 12px;
            }

            .approve-button {
                background-color: #4CAF50;
                border: none;
                color: white;
            }

            .reject-button {
                background-color: #FF0000;
                border: none;
                color: white;
            }

            p {
                margin-bottom: 10px;
                font-weight: bold;
                color: #000000;
            }

            li {
                color: #000000;
            }
            h5{
                font-weight: normal;
            }
        </style>
    </head>
    <body>");
            sb.AppendFormat(@"<p>Dear {0},</p>", recipientName);
            sb.AppendFormat(@"<h5>Your team member's leave request has been <b> cancelled by: {0}</b>:</h5>", (cancelledBy.EmployeeName + " (" + cancelledBy.EmployeeCode + ")"));
            sb.AppendFormat(@"<ul>");
            sb.AppendFormat(@"<li><strong>Name:</strong> {0} </li>", employeeInfo.EmployeeName);
            sb.AppendFormat(@"<li><strong>Designation:</strong> {0} </li>", employeeInfo.DesignationName);
            sb.AppendFormat(@"<li><strong>Leave Type:</strong> {0} </li>", presentleaveInfo.LeaveTypeName);
            sb.AppendFormat(@"<li><strong>Leave Balance:</strong> {0} </li>", presentleaveInfo.LeaveBalance);
            sb.AppendFormat(@"<li><strong>Leave Applied For:</strong> {0} days</li>", presentleaveInfo.AppliedTotalDays);
            sb.AppendFormat(@"<li><strong>Leave Dates:</strong> {0} </li>",
                (presentleaveInfo.AppliedFromDate.Value.ToString("dd-MMM-yyyy") + " To " +
                 presentleaveInfo.AppliedToDate.Value.ToString("dd-MMM-yyyy")));
            sb.AppendFormat(@"<li><strong>Reason:</strong> {0} </li>", presentleaveInfo.LeavePurpose);
            sb.AppendFormat(@"<li><strong>Applied Date:</strong> {0} </li>", presentleaveInfo.CreatedDate.Value.ToString("dd MMM yyyy hh:mm:ss tt"));
            sb.AppendFormat(@"<li><strong>Recommended By:</strong> {0} </li>", presentleaveInfo.CheckedBy);
            sb.AppendFormat(@"<li><strong>Approved By:</strong> {0} </li>", presentleaveInfo.ApprovedBy);
            sb.AppendFormat(@"<li><strong style='color:#FF0000'>Cancel Reason:</strong> {0} </li>", presentleaveInfo.CancelRemarks);
            sb.AppendFormat(@"</ul>");
            sb.AppendFormat(@"<h5>Leave balance has been restored.</h5>");
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");

            string template = sb.ToString();
            return template;
        }
        public static string Modified(string empDtls, string leaveTypeName)
        {

            var template = string.Format(@"<p style='font-family: Century Gothic, serif; font-size: 14px;'>Dear Mr/Ms Recipient,</p>
    <p style='font-family: Century Gothic, serif; font-size: 14px;'><b>{0}</b> has modified his/her requested <b>{1}</b>.</p>
    <p style='font-family: Century Gothic, serif; font-size: 14px;'>You can track its status through our ERP system by clicking <a href='{0}login'>here</a>.</p>
    <br/>
    <p style='font-family: Century Gothic, serif; font-size: 13px;'>Best Regrads</p>
    <p style='font-family: Century Gothic, serif; font-size: 13px;'><b>Recom Consulting Limited</b></p>
    <p style='font-family: Century Gothic, serif; font-size: 10px;color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>", empDtls, leaveTypeName);
            return template;
        }
        public static string Cancelled(string empDtls, string leaveTypeName)
        {

            var template = string.Format(@"<p style='font-family: Century Gothic, serif; font-size: 14px;'>Dear Mr/Ms Recipient,</p>
    <p style='font-family: Century Gothic, serif; font-size: 14px;'><b>{0}</b> has cancelled his/her requested <b>{1}</b>.</p>
    <p style='font-family: Century Gothic, serif; font-size: 14px;'>You can track its status through our ERP system by clicking <a href='{0}login'>here</a>.</p>
    <br/>
    <p style='font-family: Century Gothic, serif; font-size: 13px;'>Best Regrads</p>
    <p style='font-family: Century Gothic, serif; font-size: 13px;'><b>Recom Consulting Limited</b></p>
    <p style='font-family: Century Gothic, serif; font-size: 10px;color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>", empDtls, leaveTypeName);
            return template;
        }
        public static string Approved(string leaveTypeName, string userName, string remarks)
        {

            var template = string.Format(@"<p style='font-family: Century Gothic, serif; font-size: 14px;'>Dear Mr/Ms {0}</p>
    <p style='font-family: Century Gothic, serif; font-size: 14px;'><b>Your {1}</b> request has been approved.</p>
    <p style='font-family: Century Gothic, serif; font-size: 14px;'>You can track its status through our ERP system by clicking <a href='{0}login'>here</a>.</p>
    <br/>
    <p style='font-family: Century Gothic, serif; font-size: 13px;'>Best Regrads</p>
    <p style='font-family: Century Gothic, serif; font-size: 13px;'><b>Recom Consulting Limited</b></p>
    <p style='font-family: Century Gothic, serif; font-size: 10px;color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>", userName, leaveTypeName);

            return template;
        }
        public static string Recheck(string leaveTypeName, string userName, string remarks)
        {
            return $@"<div style='font-family: Helvetica, Arial, sans-serif; min-width: 1000px; overflow: auto; line-height: 2;'>        
                <p style='font-size: 1.1em;'>Dear {userName},</p>
                <p>Your <b>{leaveTypeName}</b> request is requested to be re-examined with the following comment(s):</p>
                <p><b>{remarks}</b>.</p>    
                <br />
                <p style='font-size: 0.9em;'>Best Regards,<br />Recom Consulting Limited</p>
                <br />
                <p><span style='color: red;'>Note: This is a system-generated mail, please do not reply.</span></p>
            </div>";
        }
        public static string Cancelled(string leaveTypeName, string userName, string remarks)
        {
            return $@"<div style='font-family: Helvetica, Arial, sans-serif; min-width: 1000px; overflow: auto; line-height: 2;'>        
                <p style='font-size: 1.1em;'>Dear {userName},</p>
                <p>Your <b>{leaveTypeName}</b> request has been declined with the following comment(s):</p>
                <p><b>{remarks}</b>.</p>                   
                <br />
                <p style='font-size: 0.9em;'>Best Regards,<br />Recom Consulting Limited</p>
                <br />
                <p><span style='color: red;'>Note: This is a system-generated mail, please do not reply.</span></p>
            </div>";
        }
    }

}
