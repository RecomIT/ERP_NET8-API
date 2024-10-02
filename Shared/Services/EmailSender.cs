using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shared.OtherModels.EmailService;
using Shared.Control_Panel.ViewModels;

namespace Shared.Services
{
    public static class EmailSender
    {
        public static void Send(EmailSettingObject setting, EmailReceiverObject receiver)
        {
            using (var smtpClient = new SmtpClient(setting.Host, Convert.ToInt16(setting.Port))) {
                smtpClient.EnableSsl = setting.EnableSsl;
                smtpClient.UseDefaultCredentials = setting.UseDefaultCredentials;
                smtpClient.Credentials = new NetworkCredential(setting.EmailAddress, setting.EmailPassword);
                using (var mailMessage = new MailMessage(setting.EmailAddress, receiver.MailAddress, receiver.Subject, setting.EmailHtmlBody)) {
                    if(receiver.Files != null)
                    {
                        for (int i = 0; i < receiver.Files.Count; i++)
                        {
                            var attachment = new Attachment(new MemoryStream(receiver.Files[i]), receiver.AttachmentNames[i]);
                            mailMessage.Attachments.Add(attachment);
                        }
                    }
                    
                    if (AppSettings.EmailService) {
                        smtpClient.Send(mailMessage);
                    }
                }
            }
        }
        public static async Task<bool> SendMail(EmailSettingObject setting, EmailReceiverObject receiver)
        {
            try {
                using (var smtpClient = new SmtpClient(setting.Host, Convert.ToInt16(setting.Port))) {
                    smtpClient.EnableSsl = setting.EnableSsl;
                    smtpClient.UseDefaultCredentials = setting.UseDefaultCredentials;
                    smtpClient.Credentials = new NetworkCredential(setting.EmailAddress, setting.EmailPassword);

                    MailAddress fromAddress = new MailAddress(setting.EmailAddress, setting.DisplayName);

                    using (var mailMessage = new MailMessage()) {
                        mailMessage.From = fromAddress;
                        mailMessage.To.Add(new MailAddress(receiver.MailAddress, receiver.RecipientName));
                        mailMessage.Subject = receiver.Subject;
                        mailMessage.Body = setting.EmailHtmlBody;
                        mailMessage.IsBodyHtml = setting.IsBodyHtml;

                        if (receiver.MailAddressList != null) {
                            foreach (var to in receiver.MailAddressList) {
                                mailMessage.To.Add(new MailAddress(to.EmailAddress, to.DisplayName));
                            }
                        }
                        if (receiver.CCList != null) {
                            foreach (var cc in receiver.CCList) {
                                mailMessage.CC.Add(new MailAddress(cc.EmailAddress, cc.DisplayName));
                            }
                        }
                        if (receiver.BCCList != null) {
                            foreach (var bcc in receiver.BCCList) {
                                mailMessage.Bcc.Add(new MailAddress(bcc.EmailAddress, bcc.DisplayName));
                            }
                        }
                        for (int i = 0; i < receiver.Files.Count; i++) {
                            var attachment = new Attachment(new MemoryStream(receiver.Files[i]), receiver.AttachmentNames[i]);
                            mailMessage.Attachments.Add(attachment);
                        }
                        if (AppSettings.EmailService) {
                            await smtpClient.SendMailAsync(mailMessage);
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception) {
                return false;
            }
        }
        public static void Send(EmailSettingObject setting, List<EmailReceiverObject> receivers)
        {
            using (var smtpClient = new SmtpClient(setting.Host, Convert.ToInt16(setting.Port))) {
                smtpClient.EnableSsl = setting.EnableSsl;
                smtpClient.UseDefaultCredentials = setting.UseDefaultCredentials;
                smtpClient.Credentials = new NetworkCredential(setting.EmailAddress, setting.EmailPassword);
                foreach (var receiver in receivers) {
                    using (var mailMessage = new MailMessage(setting.EmailAddress, receiver.MailAddress, receiver.Subject, null)) {
                        for (int i = 0; i < receiver.Files.Count; i++) {
                            var attachment = new Attachment(new MemoryStream(receiver.Files[i]), receiver.AttachmentNames[i]);
                            mailMessage.Attachments.Add(attachment);
                        }
                        if (AppSettings.EmailService) {
                            smtpClient.Send(mailMessage);
                        }
                    }
                }
            }
        }


    }
}
