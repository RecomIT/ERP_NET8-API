using BLL.Base.Interface;
using Dapper;
using Shared.OtherModels.Pagination;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;
using DAL.DapperObject.Interface;
using BLL.Expense_Reimbursement.Interface.Request;
using Shared.Expense_Reimbursement.DTO.Request;
using Shared.Expense_Reimbursement.ViewModel.Request;
using Shared.Expense_Reimbursement.Filter.Request;
using System.Net.Mail;
using System.Net;
using Shared.Expense_Reimbursement.ViewModel.Email;
using Shared.Expense_Reimbursement.Services;
using NLog.Filters;
using Shared.Separation.Filter;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BLL.Expense_Reimbursement.Implementation.Request
{
    public class RequestBusiness : IRequestBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public RequestBusiness(
            IDapperData dapper,
            ISysLogger sysLogger)            
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<ExecutionStatus> DeleteRequestAsync(RequestFilter model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Reimburse_Request";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Delete);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "DeleteRequestAsync", user);
            }
            return executionStatus;
        }
        public async Task<DBResponse<RequestViewModel>> GetRequestDataAsync(RequestFilter filter, AppUser user)
        {
            DBResponse<RequestViewModel> data = new DBResponse<RequestViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sp_name = "sp_Reimburse_Request_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                parameters.Add("ExecutionFlag", "List");

                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<RequestViewModel>>(response.JSONData) ?? new List<RequestViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
               
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "GetRequestDataAsync", user);
            }

            return data;
        }
        public async Task<IEnumerable<RequestViewModel>> GetRequestDataListAsync(RequestFilter filter, AppUser user)
        {
            IEnumerable<RequestViewModel> dataList = new List<RequestViewModel>();
            try
            {
                var sp_name = "sp_Reimburse_Request_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                parameters.Add("ExecutionFlag", "DataList");

                dataList = await _dapper.SqlQueryListAsync<RequestViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
           
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "GetRequestDataListAsync", user);
            }
            return dataList;
        }         
        public async Task<IEnumerable<EmailSendViewModel>> EmailSendAsync(EmailDataViewModel request, AppUser user)
        {
            IEnumerable<EmailSendViewModel> emailDtls = new List<EmailSendViewModel>();
            try
            {
                var sp_name = "sp_Reimburse_Request";
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeId", user.EmployeeId);
                parameters.Add("@UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", "Email");

                emailDtls = await _dapper.SqlQueryListAsync<EmailSendViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);

                if (emailDtls != null)
                {
                    var emailSetting = Utility.JsonToObject<IEnumerable<EmailSettingViewModel>>(emailDtls.First().Json).FirstOrDefault();
                    var email = Utility.JsonToObject<IEnumerable<EmailSendViewModel>>(emailDtls.First().JsonEmailCCBCC).FirstOrDefault();
                    var emailDtl = emailDtls.First();

                    if (emailSetting != null)
                    {
                        await EmailSend(email, emailSetting, emailDtl, request);
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "EmailSendAsync", user);
            }
            return emailDtls;
        }
        private async Task EmailSend(EmailSendViewModel email, EmailSettingViewModel emailSetting, EmailSendViewModel emailDtl, EmailDataViewModel request)
        {
            var Subject = "";
            var Titel = request.TransactionType;

            try
            {

                MailMessage message = new MailMessage();
                message.From = new MailAddress(emailSetting.EmailAddress, "Notification of " + Titel + " Request");

                if (!string.IsNullOrEmpty(email.EmailCC1))
                {
                    message.CC.Add(new MailAddress(email.EmailCC1));
                }
                if (!string.IsNullOrEmpty(email.EmailCC2))
                {
                    message.CC.Add(new MailAddress(email.EmailCC2));
                }
                if (!string.IsNullOrEmpty(email.EmailCC3))
                {
                    message.CC.Add(new MailAddress(email.EmailCC3));
                }

                if (!string.IsNullOrEmpty(email.EmailBCC1))
                {
                    message.Bcc.Add(new MailAddress(email.EmailBCC1));
                }
                if (!string.IsNullOrEmpty(email.EmailBCC2))
                {
                    message.Bcc.Add(new MailAddress(email.EmailBCC2));
                }

                if (request.Flag == "Request")
                {
                    if (request.SpendMode == "Advance")
                    {
                        Subject = "Advance Payment Request For " + request.TransactionType + ".";
                    }
                    else
                    {
                        Subject = "Expenses Request " + request.TransactionType + ".";
                    }
                
                    message.To.Add(new MailAddress(emailDtl.EmailTo));
                }
                else if (request.Flag == "Edit")
                {
                    if (request.SpendMode == "Advance")
                    {
                        Subject = "Edited Advance Payment Request For " + request.TransactionType + ".";
                    }
                    else
                    {
                        Subject = "Edited " + request.TransactionType + " Expenses Request";
                    }
        
                    message.To.Add(new MailAddress(emailDtl.EmailTo));
                }
                else if (request.Flag == "Cancel")
                {
                    if (request.SpendMode == "Advance")
                    {
                        Subject = "Canceled Advance Payment Request For " + request.TransactionType + ".";
                    }
                    else
                    {
                        Subject = "Canceled " + request.TransactionType + " Expenses Request";
                    }

                    message.To.Add(new MailAddress(emailDtl.EmailTo));
                }


                message.Subject = Subject;
                message.IsBodyHtml = emailSetting.IsBodyHtml;                
                message.Body = ReimburseEmailTemplate.GetEmailTemplate(request.Flag, emailDtl, request);

                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = emailSetting.EnableSsl;
                smtp.UseDefaultCredentials = emailSetting.UseDefaultCredentials;
                smtp.Port = Convert.ToInt32(emailSetting.Port);
                smtp.Host = emailSetting.Host;
                smtp.Credentials = new NetworkCredential(emailSetting.EmailAddress, emailSetting.EmailPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(message);
            }
            catch (Exception)
            {

            }

        }
        public async Task<long> GetRequestCountEmployeeWiseAsync(long employeeId, string transactionType, AppUser user)
        {
            long data = 0;
            var sp_name = "";
            try
            {
                if (transactionType == "Conveyance")
                {
                    sp_name = $@"SELECT Count(isnull(ConveyanceId,0))+1 as RequestId from Reimburse_Conveyance 
                    WHERE 1=1 AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                }
                if (transactionType == "Travels")
                {
                    sp_name = $@"SELECT Count(isnull(TravelId,0))+1 as RequestId from Reimburse_Travel 
                    WHERE 1=1 AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                }
                if (transactionType == "Entertainment")
                {
                    sp_name = $@"SELECT Count(isnull(EntertainmentId,0))+1 as RequestId from Reimburse_Entertainment 
                    WHERE 1=1 AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                }
                if (transactionType == "Expat")
                {
                    sp_name = $@"SELECT Count(isnull(ExpatId,0))+1 as RequestId from Reimburse_Expat 
                    WHERE 1=1 AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                }
                if (transactionType == "Training")
                {
                    sp_name = $@"SELECT Count(isnull(TrainingId,0))+1 as RequestId from Reimburse_Training
                    WHERE 1=1 AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                }
                if (transactionType == "Purchase")
                {
                    sp_name = $@"SELECT Count(isnull(PurchaseId,0))+1 as RequestId from Reimburse_Purchase
                    WHERE 1=1 AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                }

                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                data = await _dapper.SqlQueryFirstAsync<long>(user.Database, sp_name, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "GetRequestCountEmployeeWiseAsync", user);
            }
            return data;
        }
        public async Task<long> GetRequestIDAsync(RequestFilter filter, AppUser user)
        {
            var sp_name = "";
            long data = 0;
            try
            {                
                if (filter.TransactionType == "Entertainment")
                {
                    sp_name = $@"SELECT EntertainmentId as RequestId from Reimburse_Entertainment 
                    WHERE 1=1 AND RequestDate=@RequestDate AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                }
                
                if (filter.TransactionType == "Purchase")
                {
                    sp_name = $@"SELECT PurchaseId as RequestId from Reimburse_Purchase
                    WHERE 1=1 AND RequestDate=@RequestDate AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                }

                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", filter.EmployeeId);

                string formattedDate = ((DateTime)filter.RequestDate).ToString("yyyy-MM-dd");
                parameters.Add("RequestDate", formattedDate);

                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                data = await _dapper.SqlQueryFirstAsync<long>(user.Database, sp_name, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "GetRequestIDAsync", user);
            }
            return data;
        }

        #region  Entertainment

        public async Task<ExecutionStatus> SaveEntertainmentUploadFileAsync(EntertainmentDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            string file;
            string filePath = null;
            string fileName = null;
            string extenstion = null;
            string fileSize = null;
            string actualFileName = null;
            try
            {

                if (model.RequestId > 0 && model.File != null)
                {
                    //Upload.DeleteFile(string.Format(@"{0}/{1}", Upload.PhysicalDriver, model.FilePath));
                    file = await Upload.SaveFileAsync(model.File, string.Format(@"{0}",user.OrgCode), model.TransactionType, string.Format(@"{0}", model.EmployeeId), model.ReferenceNumber);
                    filePath = file.Substring(0, file.LastIndexOf("/"));
                    fileName = file.Substring(file.LastIndexOf("/") + 1);
                    extenstion = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    fileSize = Math.Round(Convert.ToDecimal(model.File.Length / 1024), 0, MidpointRounding.AwayFromZero).ToString() + "KB";
                    actualFileName = model.File.FileName;
                }
                else if (model.RequestId == 0 && model.File != null)
                {
                    file = await Upload.SaveFileAsync(model.File, string.Format(@"{0}", user.OrgCode), model.TransactionType, string.Format(@"{0}", model.EmployeeId), model.ReferenceNumber);
                    filePath = file.Substring(0, file.LastIndexOf("/"));
                    fileName = file.Substring(file.LastIndexOf("/") + 1);
                    extenstion = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    fileSize = Math.Round(Convert.ToDecimal(model.File.Length / 1024), 0, MidpointRounding.AwayFromZero).ToString() + "KB";
                    actualFileName = model.File.FileName;
                }


                var sp_name = "sp_Reimburse_Request";
                var parameters = new DynamicParameters();
                parameters.Add("TransactionType", model.TransactionType);
                parameters.Add("RequestId", model.RequestId);
                parameters.Add("EmployeeId", model.EmployeeId);
                parameters.Add("RequestDate", model.RequestDate);
                parameters.Add("FileName", fileName);
                parameters.Add("ActualFileName", actualFileName);
                parameters.Add("FileSize", fileSize);
                parameters.Add("FileFormat", extenstion);
                parameters.Add("FilePath", filePath);       

                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", "Upload");               
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
     
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "SaveEntertainmentUploadFileAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidationEntertainmentAsync(List<EntertainmentDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Reimburse_Request";
                var jsonData = Utility.JsonData(model);
                var parameters = new DynamicParameters();
                parameters.Add("JsonData", jsonData);
                parameters.Add("TransactionType", model.FirstOrDefault().TransactionType);
                parameters.Add("EmployeeId", model.FirstOrDefault().EmployeeId);
                parameters.Add("RequestId", model.FirstOrDefault().RequestId);
                parameters.Add("RequestDate", model.FirstOrDefault().RequestDate);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Shared.Services.Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "ValidationEntertainmentAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveEntertainmentAsync(List<EntertainmentDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus(); 
            try
            {

                var sp_name = "sp_Reimburse_Request";
                var jsonData = Utility.JsonData(model);
                var parameters = new DynamicParameters();
                parameters.Add("JsonData", jsonData);

                parameters.Add("TransactionType", model.FirstOrDefault().TransactionType);
                parameters.Add("TransactionDate", model.FirstOrDefault().TransactionDate);

                parameters.Add("RequestId", model.FirstOrDefault().EntertainmentId);
                parameters.Add("EmployeeId", model.FirstOrDefault().EmployeeId);                
                parameters.Add("ReferenceNumber", model.FirstOrDefault().ReferenceNumber);
                parameters.Add("RequestDate", model.FirstOrDefault().RequestDate);
                parameters.Add("Purpose", model.FirstOrDefault().Purpose);              
                parameters.Add("SpendMode", model.FirstOrDefault().SpendMode);
                parameters.Add("Entertainments", model.FirstOrDefault().Entertainments);
                parameters.Add("AdvanceAmount", model.FirstOrDefault().AdvanceAmount);

                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Insert);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "SaveEntertainmentAsync", user);
            }
            return executionStatus;
        }

        #endregion

        #region  Conveyance

        public async Task<ExecutionStatus> ValidationConveyanceAsync(List<ConveyanceDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var TransactionType = model.FirstOrDefault().TransactionType;
                var EmployeeId = model.FirstOrDefault().EmployeeId;
                var RequestId = model.FirstOrDefault().RequestId;
                var RequestDate = model.FirstOrDefault().RequestDate;


                var sp_name = "sp_Reimburse_Request";
                var parameters = new DynamicParameters();
                parameters.Add("TransactionType", TransactionType);
                parameters.Add("EmployeeId", EmployeeId);
                parameters.Add("RequestId", RequestId);
                parameters.Add("RequestDate", RequestDate);
                //parameters.Add("TransactionType", model.FirstOrDefault().TransactionType);
                //parameters.Add("EmployeeId", model.FirstOrDefault().EmployeeId);
                //parameters.Add("RequestId", model.FirstOrDefault().RequestId);
                //parameters.Add("RequestDate", model.FirstOrDefault().RequestDate);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Validate);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Shared.Services.Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "ValidationConveyanceAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveConveyanceAsync(List<ConveyanceDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Reimburse_Request";
                var jsonData = Utility.JsonData(model);
                var parameters = new DynamicParameters();
                parameters.Add("JsonData", jsonData);

                parameters.Add("TransactionType", model.FirstOrDefault().TransactionType);
                parameters.Add("TransactionDate", model.FirstOrDefault().TransactionDate);
                parameters.Add("EmployeeId", model.FirstOrDefault().EmployeeId);
                parameters.Add("RequestId", model.FirstOrDefault().RequestId);
                parameters.Add("ReferenceNumber", model.FirstOrDefault().ReferenceNumber);
                parameters.Add("RequestDate", model.FirstOrDefault().RequestDate);
                parameters.Add("CompanyName", model.FirstOrDefault().CompanyName);
                parameters.Add("Purpose", model.FirstOrDefault().Purpose);
                parameters.Add("Description", model.FirstOrDefault().Description);
                parameters.Add("SpendMode", model.FirstOrDefault().SpendMode);
                parameters.Add("Transportation", model.FirstOrDefault().Transportation);
                parameters.Add("AdvanceAmount", model.FirstOrDefault().AdvanceAmount);

                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Insert);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "SaveConveyanceAsync", user);
            }
            return executionStatus;
        }

        #endregion

        #region  Expat

        public async Task<IEnumerable<ExpatViewModel>> GetCompanyNameAsync(AppUser user)
        {
            IEnumerable<ExpatViewModel> list = new List<ExpatViewModel>();
            try
            {
                var sp_name = $@"SELECT Distinct companyName from Reimburse_Expat_Details 
                WHERE 1=1 AND companyName is not null AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                list = await _dapper.SqlQueryListAsync<ExpatViewModel>(user.Database, sp_name.Trim(), parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "GetCompanyNameAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<ExpatViewModel>> GetBillTypeAsync(AppUser user)
        {
            IEnumerable<ExpatViewModel> list = new List<ExpatViewModel>();
            try
            {
                var sp_name = $@"SELECT Distinct billType from Reimburse_Expat_Details 
                WHERE 1=1 AND billType is not null AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                list = await _dapper.SqlQueryListAsync<ExpatViewModel>(user.Database, sp_name.Trim(), parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "GetBillTypeAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> ValidationExpatAsync(List<ExpatDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Reimburse_Request";
                var parameters = new DynamicParameters();
                parameters.Add("TransactionType", model.FirstOrDefault().TransactionType);
                parameters.Add("EmployeeId", model.FirstOrDefault().EmployeeId);
                parameters.Add("RequestId", model.FirstOrDefault().RequestId);
                parameters.Add("RequestDate", model.FirstOrDefault().RequestDate);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Validate);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                executionStatus = Shared.Services.Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "ValidationExpatAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveExpatAsync(List<ExpatDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus(); 
            try
            {
                var sp_name = "sp_Reimburse_Request";
                var jsonData = Utility.JsonData(model);
                var parameters = new DynamicParameters();
                parameters.Add("JsonData", jsonData);

                parameters.Add("TransactionType", model.FirstOrDefault().TransactionType);
                parameters.Add("TransactionDate", model.FirstOrDefault().TransactionDate);
                parameters.Add("EmployeeId", model.FirstOrDefault().EmployeeId);
                parameters.Add("RequestId", model.FirstOrDefault().RequestId);                               
                parameters.Add("ReferenceNumber", model.FirstOrDefault().ReferenceNumber);
                parameters.Add("RequestDate", model.FirstOrDefault().RequestDate);
                parameters.Add("Expats", model.FirstOrDefault().Expats);
                parameters.Add("SpendMode", model.FirstOrDefault().SpendMode);
                parameters.Add("Description", model.FirstOrDefault().Description);         

                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Insert);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "SaveExpatAsync", user);
            }
            return executionStatus;
        }

        #endregion

        #region Travels

        public async Task<IEnumerable<TravelViewModel>> GetLocationAsync(AppUser user)
        {
            IEnumerable<TravelViewModel> list = new List<TravelViewModel>();
            try
            {
                var sp_name = $@"SELECT Distinct location from Reimburse_Travel 
                WHERE 1=1 AND Location is not null AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                list = await _dapper.SqlQueryListAsync<TravelViewModel>(user.Database, sp_name.Trim(), parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "GetLocationAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> ValidatorTravelAsync(TravelDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Reimburse_Request";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "ValidatorSaveAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveTravelAsync(TravelDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Reimburse_Request";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", model.RequestId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "SaveAsync", user);
            }
            return executionStatus;
        }

        #endregion

        #region Training
     
        public async Task<ExecutionStatus> ValidatorTrainingAsync(TrainingDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Reimburse_Request";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "ValidatorSaveAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveTrainingAsync(TrainingDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Reimburse_Request";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", model.RequestId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestBusiness", "SaveAsync", user);
            }
            return executionStatus;
        }

        #endregion

    }
}
