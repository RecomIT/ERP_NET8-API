using System;
using System.Text;
using Shared.Employee.ViewModel.Info;
using Shared.Expense_Reimbursement.DTO.Request;
using Shared.Expense_Reimbursement.ViewModel.Email;
using Shared.Leave.ViewModel.Request;

namespace Shared.Services
{
    public static class ReimburseEmailTemplate
    {
        public static string GetEmailTemplate(string flag, EmailSendViewModel emailDtl, EmailDataViewModel request)
        {
            if (flag == "Request") {
               
                if (request.TransactionType == "Conveyance")
                {
                    return ReimburseEmailTemplate.ConveyanceRequest(emailDtl, request);
                }

                if (request.TransactionType == "Travels")
                {
                    return ReimburseEmailTemplate.TravelRequest(emailDtl, request);
                }

                if (request.TransactionType == "Entertainment")
                {
                    return ReimburseEmailTemplate.EntertainmentRequest(emailDtl, request);
                }

                if (request.TransactionType == "Expat")
                {
                    return ReimburseEmailTemplate.ExpatRequest(emailDtl, request);
                }
            }
            else if (flag == "Cancel")
            {
                if (request.TransactionType == "Conveyance")
                {
                    return ReimburseEmailTemplate.ConveyanceRequestCancel(emailDtl, request);
                }

                if (request.TransactionType == "Travels")
                {
                    return ReimburseEmailTemplate.TravelRequestCancel(emailDtl, request);
                }

                if (request.TransactionType == "Entertainment")
                {
                    return ReimburseEmailTemplate.EntertainmentRequestCancel(emailDtl, request);
                }

                if (request.TransactionType == "Expat")
                {
                    return ReimburseEmailTemplate.ExpatRequestCancel(emailDtl, request);
                }
            }
            else if (flag == "Edit")
            {
                if (request.TransactionType == "Conveyance")
                {
                    return ReimburseEmailTemplate.ConveyanceRequestEdit(emailDtl, request);
                }

                if (request.TransactionType == "Travels")
                {
                    return ReimburseEmailTemplate.TravelRequestEdit(emailDtl, request);
                }

                if (request.TransactionType == "Entertainment")
                {
                    return ReimburseEmailTemplate.EntertainmentRequestEdit(emailDtl, request);
                }

                if (request.TransactionType == "Expat")
                {
                    return ReimburseEmailTemplate.ExpatRequestEdit(emailDtl, request);
                }
            }
            //else if (flag == "Approved")
            //{
            //    return ReimburseEmailTemplate.RequestApproved(emailDtl, request);
            //}
            //else if (flag == "Rejected")
            //{
            //    return ReimburseEmailTemplate.RequestRejected(emailDtl, request);
            //}
            return "";
        }


        public static string ConveyanceRequest(EmailSendViewModel emailDtl, EmailDataViewModel request)
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

            sb.AppendFormat(@"<p>Dear {0},</p>", emailDtl.EmailToName);
            sb.Append(@"<p>You have a new <b>" + request.TransactionType + "</b> request from <b>" + emailDtl.EmpDtls + "</b> that requires your attention:</p>");

            sb.Append(@"<ul>");
            sb.AppendFormat(@"<li><strong>Request Date:</strong> {0} </li>", request.TransactionDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Type:</strong> {0} </li>", request.TransactionType);
            sb.AppendFormat(@"<li><strong>Expense Date:</strong> {0} </li>", request.RequestDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Company Name:</strong> {0} </li>", request.CompanyName);
            sb.AppendFormat(@"<li><strong>Purpose:</strong> {0} </li>", request.Purpose);
            sb.AppendFormat(@"<li><strong>Spend Mode:</strong> {0} days</li>", request.SpendMode);    
            sb.AppendFormat(@"<li><strong>Description:</strong> {0} </li>", request.Description);
            sb.AppendFormat(@"</ul>");

            //sb.AppendFormat(@"<p>Please review and take action on the request:</p>");
            //if(AppSettings.App_environment == "Local") {
            //    sb.AppendFormat(@"<button class='approve-button'><a href='{0}' style='text - decoration: none; color: white;'>Approve</a></button>", AppSettings.ClientOrigin);
            //    sb.AppendFormat(@"<button class='reject-button'><a href='{0}' style='text - decoration: none; color: white;'>Reject</a></button>", AppSettings.ClientOrigin);
            //    sb.AppendFormat(@"<br/><br/>");
            //}
       
            sb.AppendFormat(@"<p><p>If you log in to our portal, please <a href='{0}'>Login</a> to view and manage expense requests.</p>", AppSettings.ClientOrigin);
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");


            string template = sb.ToString();
            return template;
        }
        public static string ConveyanceRequestEdit(EmailSendViewModel emailDtl, EmailDataViewModel request)
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

            sb.AppendFormat(@"<p>Dear {0},</p>", emailDtl.EmailToName);
            sb.Append(@"<p>You have a edited <b>" + request.TransactionType + "</b> request from <b>" + emailDtl.EmpDtls + "</b> that requires your attention:</p>");

            sb.Append(@"<ul>");
            sb.AppendFormat(@"<li><strong>Request Date:</strong> {0} </li>", request.TransactionDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Type:</strong> {0} </li>", request.TransactionType);
            sb.AppendFormat(@"<li><strong>Expense Date:</strong> {0} </li>", request.RequestDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Company Name:</strong> {0} </li>", request.CompanyName);
            sb.AppendFormat(@"<li><strong>Purpose:</strong> {0} </li>", request.Purpose);
            sb.AppendFormat(@"<li><strong>Spend Mode:</strong> {0} days</li>", request.SpendMode);
            sb.AppendFormat(@"<li><strong>Description:</strong> {0} </li>", request.Description);
            sb.AppendFormat(@"</ul>");

            //sb.AppendFormat(@"<p>Please review and take action on the request:</p>");
            //if(AppSettings.App_environment == "Local") {
            //    sb.AppendFormat(@"<button class='approve-button'><a href='{0}' style='text - decoration: none; color: white;'>Approve</a></button>", AppSettings.ClientOrigin);
            //    sb.AppendFormat(@"<button class='reject-button'><a href='{0}' style='text - decoration: none; color: white;'>Reject</a></button>", AppSettings.ClientOrigin);
            //    sb.AppendFormat(@"<br/><br/>");
            //}

            sb.AppendFormat(@"<p><p>If you log in to our portal, please <a href='{0}'>Login</a> to view and manage expense requests.</p>", AppSettings.ClientOrigin);
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");


            string template = sb.ToString();
            return template;
        }
        public static string ConveyanceRequestCancel(EmailSendViewModel emailDtl, EmailDataViewModel request)
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
            sb.AppendFormat(@"<p>Dear {0},</p>", emailDtl.EmailToName);
            sb.AppendFormat(@"<p>Your team member's <b>" + emailDtl.EmpDtls + "</b> canceled his requested expense request.</p>");

            sb.Append(@"<ul>");
            sb.AppendFormat(@"<li><strong>Request Date:</strong> {0} </li>", request.TransactionDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Type:</strong> {0} </li>", request.TransactionType);
            sb.AppendFormat(@"<li><strong>Expense Date:</strong> {0} </li>", request.RequestDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Company Name:</strong> {0} </li>", request.CompanyName);
            sb.AppendFormat(@"<li><strong>Purpose:</strong> {0} </li>", request.Purpose);
            sb.AppendFormat(@"<li><strong>Spend Mode:</strong> {0} days</li>", request.SpendMode);
            sb.AppendFormat(@"<li><strong>Description:</strong> {0} </li>", request.Description);
            sb.AppendFormat(@"</ul>");

            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");

            string template = sb.ToString();
            return template;
        }

        public static string EntertainmentRequest(EmailSendViewModel emailDtl, EmailDataViewModel request)
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

            sb.AppendFormat(@"<p>Dear {0},</p>", emailDtl.EmailToName);
            sb.Append(@"<p>You have a new <b>" + request.TransactionType + "</b> request from <b>" + emailDtl.EmpDtls + "</b> that requires your attention:</p>");

            sb.Append(@"<ul>");
            sb.AppendFormat(@"<li><strong>Request Date:</strong> {0} </li>", request.TransactionDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Type:</strong> {0} </li>", request.TransactionType);
            sb.AppendFormat(@"<li><strong>Expense Date:</strong> {0} </li>", request.RequestDate.Value.ToString("dd-MMM-yyyy"));            
            sb.AppendFormat(@"<li><strong>Purpose:</strong> {0} </li>", request.Purpose);
            sb.AppendFormat(@"<li><strong>Spend Mode:</strong> {0} days</li>", request.SpendMode);
            sb.AppendFormat(@"<li><strong>Description:</strong> {0} </li>", request.Description);
            sb.AppendFormat(@"</ul>");

            sb.AppendFormat(@"<p><p>If you log in to our portal, please <a href='{0}'>Login</a> to view and manage expense requests.</p>", AppSettings.ClientOrigin);
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");

            string template = sb.ToString();
            return template;
        }
        public static string EntertainmentRequestEdit(EmailSendViewModel emailDtl, EmailDataViewModel request)
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

            sb.AppendFormat(@"<p>Dear {0},</p>", emailDtl.EmailToName);
            sb.Append(@"<p>You have a edited <b>" + request.TransactionType + "</b> request from <b>" + emailDtl.EmpDtls + "</b> that requires your attention:</p>");

            sb.Append(@"<ul>");
            sb.AppendFormat(@"<li><strong>Request Date:</strong> {0} </li>", request.TransactionDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Type:</strong> {0} </li>", request.TransactionType);
            sb.AppendFormat(@"<li><strong>Expense Date:</strong> {0} </li>", request.RequestDate.Value.ToString("dd-MMM-yyyy"));            
            sb.AppendFormat(@"<li><strong>Purpose:</strong> {0} </li>", request.Purpose);
            sb.AppendFormat(@"<li><strong>Spend Mode:</strong> {0} days</li>", request.SpendMode);
            sb.AppendFormat(@"<li><strong>Description:</strong> {0} </li>", request.Description);
            sb.AppendFormat(@"</ul>");

            //sb.AppendFormat(@"<p>Please review and take action on the request:</p>");
            //if(AppSettings.App_environment == "Local") {
            //    sb.AppendFormat(@"<button class='approve-button'><a href='{0}' style='text - decoration: none; color: white;'>Approve</a></button>", AppSettings.ClientOrigin);
            //    sb.AppendFormat(@"<button class='reject-button'><a href='{0}' style='text - decoration: none; color: white;'>Reject</a></button>", AppSettings.ClientOrigin);
            //    sb.AppendFormat(@"<br/><br/>");
            //}

            sb.AppendFormat(@"<p><p>If you log in to our portal, please <a href='{0}'>Login</a> to view and manage expense requests.</p>", AppSettings.ClientOrigin);
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");


            string template = sb.ToString();
            return template;
        }
        public static string EntertainmentRequestCancel(EmailSendViewModel emailDtl, EmailDataViewModel request)
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
            sb.AppendFormat(@"<p>Dear {0},</p>", emailDtl.EmailToName);
            sb.AppendFormat(@"<p>Your team member's <b>" + emailDtl.EmpDtls + "</b> canceled his requested expense request.</p>");

            sb.Append(@"<ul>");
            sb.AppendFormat(@"<li><strong>Request Date:</strong> {0} </li>", request.TransactionDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Type:</strong> {0} </li>", request.TransactionType);
            sb.AppendFormat(@"<li><strong>Expense Date:</strong> {0} </li>", request.RequestDate.Value.ToString("dd-MMM-yyyy"));            
            sb.AppendFormat(@"<li><strong>Purpose:</strong> {0} </li>", request.Purpose);
            sb.AppendFormat(@"<li><strong>Spend Mode:</strong> {0} days</li>", request.SpendMode);
            sb.AppendFormat(@"<li><strong>Description:</strong> {0} </li>", request.Description);
            sb.AppendFormat(@"</ul>");

            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");

            string template = sb.ToString();
            return template;
        }

        public static string TravelRequest(EmailSendViewModel emailDtl, EmailDataViewModel request)
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

            sb.AppendFormat(@"<p>Dear {0},</p>", emailDtl.EmailToName);
            sb.Append(@"<p>You have a new <b>" + request.TransactionType + "</b> expenses request from <b>" + emailDtl.EmpDtls + "</b> that requires your attention:</p>");

            sb.Append(@"<ul>");
            sb.AppendFormat(@"<li><strong>Request Date:</strong> {0} </li>", request.TransactionDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Type:</strong> {0} </li>", request.TransactionType);
            sb.AppendFormat(@"<li><strong>From Date:</strong> {0} </li>", request.FromDate);
            sb.AppendFormat(@"<li><strong>To Date:</strong> {0} </li>", request.ToDate);
            sb.AppendFormat(@"<li><strong>Spend Mode:</strong> {0} days</li>", request.SpendMode);
            sb.AppendFormat(@"<li><strong>Destination City:</strong> {0} </li>", request.Location);
            sb.AppendFormat(@"<li><strong>Purpose:</strong> {0} </li>", request.Purpose);
            sb.AppendFormat(@"<li><strong>Transportation:</strong> {0} </li>", request.Transportation);
            sb.AppendFormat(@"<li><strong>Transportation Costs:</strong> {0} </li>", request.TransportationCosts);
            sb.AppendFormat(@"<li><strong>Accommodation Costs:</strong> {0} </li>", request.AccommodationCosts);
            sb.AppendFormat(@"<li><strong>Subsistence Costs:</strong> {0} </li>", request.SubsistenceCosts);
            sb.AppendFormat(@"<li><strong>Other Costs:</strong> {0} </li>", request.OtherCosts);
            sb.AppendFormat(@"<li><strong>Description:</strong> {0} </li>", request.Description);
            sb.AppendFormat(@"</ul>");

            sb.AppendFormat(@"<p><p>If you log in to our portal, please <a href='{0}'>Login</a> to view and manage expense requests.</p>", AppSettings.ClientOrigin);
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");


            string template = sb.ToString();
            return template;
        }
        public static string TravelRequestEdit(EmailSendViewModel emailDtl, EmailDataViewModel request)
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

            sb.AppendFormat(@"<p>Dear {0},</p>", emailDtl.EmailToName);
            sb.Append(@"<p>You have a new edited <b>" + request.TransactionType + "</b> expenses request from <b>" + emailDtl.EmpDtls + "</b> that requires your attention:</p>");

            sb.Append(@"<ul>");
            sb.AppendFormat(@"<li><strong>Request Date:</strong> {0} </li>", request.TransactionDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Type:</strong> {0} </li>", request.TransactionType);
            sb.AppendFormat(@"<li><strong>From Date:</strong> {0} </li>", request.FromDate);
            sb.AppendFormat(@"<li><strong>To Date:</strong> {0} </li>", request.ToDate);
            sb.AppendFormat(@"<li><strong>Spend Mode:</strong> {0} days</li>", request.SpendMode);
            sb.AppendFormat(@"<li><strong>Destination City:</strong> {0} </li>", request.Location);
            sb.AppendFormat(@"<li><strong>Purpose:</strong> {0} </li>", request.Purpose);
            sb.AppendFormat(@"<li><strong>Transportation:</strong> {0} </li>", request.Transportation);
            sb.AppendFormat(@"<li><strong>Transportation Costs:</strong> {0} </li>", request.TransportationCosts);
            sb.AppendFormat(@"<li><strong>Accommodation Costs:</strong> {0} </li>", request.AccommodationCosts);
            sb.AppendFormat(@"<li><strong>Subsistence Costs:</strong> {0} </li>", request.SubsistenceCosts);
            sb.AppendFormat(@"<li><strong>Other Costs:</strong> {0} </li>", request.OtherCosts);
            sb.AppendFormat(@"<li><strong>Description:</strong> {0} </li>", request.Description);
            sb.AppendFormat(@"</ul>");

            sb.AppendFormat(@"<p><p>If you log in to our portal, please <a href='{0}'>Login</a> to view and manage expense requests.</p>", AppSettings.ClientOrigin);
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");


            string template = sb.ToString();
            return template;
        }
        public static string TravelRequestCancel(EmailSendViewModel emailDtl, EmailDataViewModel request)
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

            sb.AppendFormat(@"<p>Dear {0},</p>", emailDtl.EmailToName);            
            sb.AppendFormat(@"<p>Your team member's <b>" + emailDtl.EmpDtls + "</b> canceled his requested <b>" + request.TransactionType + "</b> expense request.</p>");

            sb.Append(@"<ul>");
            sb.AppendFormat(@"<li><strong>Request Date:</strong> {0} </li>", request.TransactionDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Type:</strong> {0} </li>", request.TransactionType);
            sb.AppendFormat(@"<li><strong>From Date:</strong> {0} </li>", request.FromDate);
            sb.AppendFormat(@"<li><strong>To Date:</strong> {0} </li>", request.ToDate);
            sb.AppendFormat(@"<li><strong>Spend Mode:</strong> {0} days</li>", request.SpendMode);
            sb.AppendFormat(@"<li><strong>Destination City:</strong> {0} </li>", request.Location);
            sb.AppendFormat(@"<li><strong>Purpose:</strong> {0} </li>", request.Purpose);
            sb.AppendFormat(@"<li><strong>Transportation:</strong> {0} </li>", request.Transportation);
            sb.AppendFormat(@"<li><strong>Transportation Costs:</strong> {0} </li>", request.TransportationCosts);
            sb.AppendFormat(@"<li><strong>Accommodation Costs:</strong> {0} </li>", request.AccommodationCosts);
            sb.AppendFormat(@"<li><strong>Subsistence Costs:</strong> {0} </li>", request.SubsistenceCosts);
            sb.AppendFormat(@"<li><strong>Other Costs:</strong> {0} </li>", request.OtherCosts);
            sb.AppendFormat(@"<li><strong>Description:</strong> {0} </li>", request.Description);
            sb.AppendFormat(@"</ul>");

            sb.AppendFormat(@"<p><p>If you log in to our portal, please <a href='{0}'>Login</a> to view and manage expense requests.</p>", AppSettings.ClientOrigin);
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");


            string template = sb.ToString();
            return template;
        }

        public static string ExpatRequest(EmailSendViewModel emailDtl, EmailDataViewModel request)
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

            sb.AppendFormat(@"<p>Dear {0},</p>", emailDtl.EmailToName);
            sb.Append(@"<p>You have a new <b>" + request.TransactionType + "</b> request from <b>" + emailDtl.EmpDtls + "</b> that requires your attention:</p>");

            sb.Append(@"<ul>");
            sb.AppendFormat(@"<li><strong>Request Date:</strong> {0} </li>", request.TransactionDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Type:</strong> {0} </li>", request.TransactionType);
            sb.AppendFormat(@"<li><strong>Expense Date:</strong> {0} </li>", request.RequestDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Purpose:</strong> {0} </li>", request.Expats);
            sb.AppendFormat(@"<li><strong>Spend Mode:</strong> {0} days</li>", request.SpendMode);
            sb.AppendFormat(@"<li><strong>Description:</strong> {0} </li>", request.Expats);
            sb.AppendFormat(@"</ul>");

            sb.AppendFormat(@"<p><p>If you log in to our portal, please <a href='{0}'>Login</a> to view and manage expense requests.</p>", AppSettings.ClientOrigin);
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");

            string template = sb.ToString();
            return template;
        }
        public static string ExpatRequestEdit(EmailSendViewModel emailDtl, EmailDataViewModel request)
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

            sb.AppendFormat(@"<p>Dear {0},</p>", emailDtl.EmailToName);
            sb.Append(@"<p>You have a edited <b>" + request.TransactionType + "</b> request from <b>" + emailDtl.EmpDtls + "</b> that requires your attention:</p>");

            sb.Append(@"<ul>");
            sb.AppendFormat(@"<li><strong>Request Date:</strong> {0} </li>", request.TransactionDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Type:</strong> {0} </li>", request.TransactionType);
            sb.AppendFormat(@"<li><strong>Expense Date:</strong> {0} </li>", request.RequestDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Purpose:</strong> {0} </li>", request.Expats);
            sb.AppendFormat(@"<li><strong>Spend Mode:</strong> {0} days</li>", request.SpendMode);
            sb.AppendFormat(@"<li><strong>Description:</strong> {0} </li>", request.Expats);
            sb.AppendFormat(@"</ul>");


            sb.AppendFormat(@"<p><p>If you log in to our portal, please <a href='{0}'>Login</a> to view and manage expense requests.</p>", AppSettings.ClientOrigin);
            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");


            string template = sb.ToString();
            return template;
        }
        public static string ExpatRequestCancel(EmailSendViewModel emailDtl, EmailDataViewModel request)
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
            sb.AppendFormat(@"<p>Dear {0},</p>", emailDtl.EmailToName);
            sb.AppendFormat(@"<p>Your team member's <b>" + emailDtl.EmpDtls + "</b> canceled his requested expense request.</p>");

            sb.Append(@"<ul>");
            sb.AppendFormat(@"<li><strong>Request Date:</strong> {0} </li>", request.TransactionDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Type:</strong> {0} </li>", request.TransactionType);
            sb.AppendFormat(@"<li><strong>Expense Date:</strong> {0} </li>", request.RequestDate.Value.ToString("dd-MMM-yyyy"));
            sb.AppendFormat(@"<li><strong>Purpose:</strong> {0} </li>", request.Expats);
            sb.AppendFormat(@"<li><strong>Spend Mode:</strong> {0} days</li>", request.SpendMode);
            sb.AppendFormat(@"<li><strong>Description:</strong> {0} </li>", request.Expats);
            sb.AppendFormat(@"</ul>");

            sb.AppendFormat(@"<p style='color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            sb.AppendFormat(@"</body></html>");

            string template = sb.ToString();
            return template;
        }



        public static string RequestApproved(string recipientName, EmployeeLeaveRequestViewModel presentleaveInfo, EmployeeLeaveRequestViewModel previousleaveInfo, EmployeeServiceDataViewModel employeeInfo, EmployeeServiceDataViewModel approvedBy)
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
        public static string RequestRejected(string recipientName, EmployeeLeaveRequestViewModel presentleaveInfo, EmployeeLeaveRequestViewModel previousleaveInfo, EmployeeServiceDataViewModel employeeInfo, EmployeeServiceDataViewModel rejectedBy)
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
        public static string ApprovedRequestCancelled(string recipientName, EmployeeLeaveRequestViewModel presentleaveInfo, EmployeeLeaveRequestViewModel previousleaveInfo, EmployeeServiceDataViewModel employeeInfo, EmployeeServiceDataViewModel cancelledBy)
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
   
        

    }

}
