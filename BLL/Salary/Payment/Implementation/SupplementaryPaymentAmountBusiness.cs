using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.Payroll.DTO.Payment;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using BLL.Salary.Payment.Interface;
using Shared.OtherModels.Pagination;
using Shared.Payroll.Filter.Payment;
using DAL.Payroll.Repository.Interface;
using Shared.Payroll.Process.Allowance;
using Shared.Payroll.ViewModel.Payment;

namespace BLL.Salary.Payment.Implementation
{
    public class SupplementaryPaymentAmountBusiness : ISupplementaryPaymentAmountBusiness
    {
        private IDapperData _dapper;
        private ISysLogger _sysLogger;
        private readonly ISupplementaryPaymentProcessInfoRepository _supplementaryPaymentProcessInfoRepository;

        public SupplementaryPaymentAmountBusiness(IDapperData dapper, ISysLogger sysLogger, ISupplementaryPaymentProcessInfoRepository supplementaryPaymentProcessInfoRepository)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _supplementaryPaymentProcessInfoRepository = supplementaryPaymentProcessInfoRepository;
        }
        public async Task<DBResponse<SupplementaryPaymentAmountViewModel>> GetSupplementaryPaymentAmountInfosAsync(SupplementaryPaymentAmount_Filter filter, AppUser user)
        {
            DBResponse<SupplementaryPaymentAmountViewModel> data = new DBResponse<SupplementaryPaymentAmountViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sp_name = "sp_Payroll_SupplementaryPaymentAmount_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<SupplementaryPaymentAmountViewModel>>(response.JSONData) ?? new List<SupplementaryPaymentAmountViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentAmountBusiness", "GetSupplementaryPaymentAmountInfos", user);
            }
            return data;
        }
        public async Task<IEnumerable<SupplementaryPaymentAmountViewModel>> GetSupplementaryPaymentAmountsForProcessAsync(SupplementaryPaymentAmount_Filter filter, AppUser user)
        {
            IEnumerable<SupplementaryPaymentAmountViewModel> list = new List<SupplementaryPaymentAmountViewModel>();
            try
            {
                var sp_name = "sp_Payroll_SupplementaryPaymentAmountsForProcess";
                var parameters = DapperParam.AddParams(filter, user, new string[] { "PaymentAmountId", "Status" });
                list = await _dapper.SqlQueryListAsync<SupplementaryPaymentAmountViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentAmountBusiness", "GetSupplementaryPaymentAmountsForProcessAsync", user);
            }
            return list;
        }
        public async Task<DBResponse<SupplementaryPaymentProcessInfoViewModel>> GetSupplementaryPaymentProcessInfosAsync(SupplementaryPaymentProcessInfo_Filter filter, AppUser user)
        {
            DBResponse<SupplementaryPaymentProcessInfoViewModel> data = new DBResponse<SupplementaryPaymentProcessInfoViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sp_name = "sp_Payroll_SupplementaryPaymentProcessInfo_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<SupplementaryPaymentProcessInfoViewModel>>(response.JSONData) ?? new List<SupplementaryPaymentProcessInfoViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentAmountBusiness", "GetSupplementaryPaymentProcessInfosAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveBulkSupplementaryPaymentAmountAsync(List<SupplementaryPaymentAmountDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_SupplementaryPaymentAmount_Insert_Update";
                var jsonData = JsonReverseConverter.JsonData(model);
                var parameters = new DynamicParameters();
                parameters.Add("JsonData", jsonData);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Insert);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentAmountBusiness", "SaveBulkSupplementaryPaymentAmountAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveSupplementaryPaymentAmountAsync(SupplementaryPaymentAmountDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_Payroll_SupplementaryPaymentAmount_Insert_Update";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", model.PaymentAmountId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentAmountBusiness", "SaveSupplementaryPaymentAmountAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveSupplementaryProcessAsync(SupplementaryProcessDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_SupplementaryPayment_Tax_FY_23_24";
                var parameters = DapperParam.AddParams(model, user);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentAmountBusiness", "SaveSupplementaryProcessAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidatePaymentAsync(List<SupplementaryPaymentAmountDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            executionStatus.Status = true;
            executionStatus.Errors = new Dictionary<string, string>();
            try
            {
                var sp_name = "sp_Payroll_SupplementaryPaymentAmount_Insert_Update";
                string duplicate = string.Empty;
                int itemCount = 0;
                foreach (var item in model)
                {
                    var parameters = DapperParam.AddParams(item, user, new string[] { "EmployeeCode" });
                    parameters.Add("CompanyId", user.CompanyId);
                    parameters.Add("OrganizationId", user.OrganizationId);
                    parameters.Add("ExecutionFlag", Data.Validate);

                    var value = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                    if (value.Status == false)
                    {
                        executionStatus.Status = false;
                        itemCount = itemCount + 1;
                        executionStatus.Errors.Add(item.EmployeeCode, value.ErrorMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentAmountBusiness", "ValidatePaymentAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UploadSupplementaryPaymentAmountAsync(List<SupplementaryProcessExcelReadDTO> upload, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var model = new SupplementaryProcessExcelReadDTO();
                foreach (var item in upload)
                {
                    model.PaymentMode = item.PaymentMode;
                    model.WithCOC = item.WithCOC;
                }
                var sp_name = "sp_Payroll_UploadExcelSupplementaryPaymentAmount";
                var jsonData = Utility.JsonData(upload);
                var parameters = new DynamicParameters();
                parameters.Add("JsonData", jsonData);
                parameters.Add("PaymentMode", model.PaymentMode);
                parameters.Add("WithCOC", model.WithCOC);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("ExecutionFlag", "XL_Upload");

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentAmountBusiness", "UploadSupplementaryPaymentAmountAsync", user);
            }
            return executionStatus;
        }
        public async Task<DataTable> GetSupplementaryTaxSheetDetailsReport(long paymentProcessInfoId, long employeeId, long fiscalYearId, short paymentMonth, short paymentYear, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_Supplementary_Tax_Sheet_With_Calculation";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("PaymentProcessInfoId", paymentProcessInfoId.ToString());
                keyValuePairs.Add("EmployeeId", employeeId.ToString());
                keyValuePairs.Add("FiscalYearId", fiscalYearId.ToString());
                keyValuePairs.Add("PaymentMonth", paymentMonth.ToString());
                keyValuePairs.Add("PaymentYear", paymentYear.ToString());
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());

                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveSystemException(ex, user.Database, "SupplementaryPaymentAmountBusiness", "GetSupplementaryTaxSheetDetailsReport", user.UserId, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return dataTable;
        }
        public async Task<DataTable> GetSupplementaryPaymentReport(long paymentProcessInfoId, long employeeId, long fiscalYearId, short paymentMonth, short paymentYear, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_SupplementaryPaymentSheet";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("PaymentProcessInfoId", paymentProcessInfoId.ToString());
                keyValuePairs.Add("EmployeeId", employeeId.ToString());
                keyValuePairs.Add("FiscalYearId", fiscalYearId.ToString());
                keyValuePairs.Add("PaymentMonth", paymentMonth.ToString());
                keyValuePairs.Add("PaymentYear", paymentYear.ToString());
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());

                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveSystemException(ex, user.Database, "SupplementaryPaymentAmountBusiness", "GetSupplementaryPaymentReport", user.UserId, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return dataTable;
        }
        public async Task<IEnumerable<AllowanceDisbursedAmount>> GetThisMonthSupplementaryAmountInTaxProcess(long employeeId, long fiscalYearId, short paymentMonth, short paymentYear, AppUser user)
        {
            IEnumerable<AllowanceDisbursedAmount> list = new List<AllowanceDisbursedAmount>();
            try
            {
                var query = $@"SELECT ALW.AllowanceNameId,AllowanceName=ALW.[Name],ALW.Flag,PA.Amount FROM Payroll_SupplementaryPaymentAmount PA
                INNER JOIN Payroll_SupplementaryPaymentProcessInfo PPI ON PA.PaymentProcessInfoId = PPI.PaymentProcessInfoId
                INNER JOIN Payroll_AllowanceName ALW ON PA.AllowanceNameId= ALW.AllowanceNameId
                WHERE PA.EmployeeId=@EmployeeId AND PA.FiscalYearId=@FiscalYearId AND PA.PaymentMonth=@PaymentMonth AND PA.PaymentYear=@PaymentYear AND PA.StateStatus='Disbursed'
                AND PA.CompanyId=@CompanyId AND PA.OrganizationId=@OrganizationId";

                list = await _dapper.SqlQueryListAsync<AllowanceDisbursedAmount>(user.Database, query, new
                {
                    EmployeeId = employeeId,
                    FiscalYearId = fiscalYearId,
                    PaymentMonth = paymentMonth,
                    PaymentYear = paymentYear,
                    user.CompanyId,
                    user.OrganizationId
                }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentAmountBusiness", "GetThisMonthSupplementaryAmountInTaxProcess", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> DeleteSupplementaryAmountAsync(DeletePaymentDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var itemInDb = await _supplementaryPaymentProcessInfoRepository.GetByIdAsync(model.ProcessId, user);
                if (itemInDb != null && itemInDb.StateStatus == StateStatus.Pending)
                {
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                    {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                var query = $@"DELETE FROM Payroll_SupplementaryPaymentTaxSlab Where PaymentAmountId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                var rawAffected = await connection.ExecuteAsync(query, new
                                {
                                    Id = model.PaymentId,
                                    user.CompanyId,
                                    user.OrganizationId
                                }, transaction);

                                query = $@"DELETE FROM Payroll_SupplementaryPaymentTaxDetail Where PaymentAmountId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                rawAffected = await connection.ExecuteAsync(query, new
                                {
                                    Id = model.PaymentId,
                                    user.CompanyId,
                                    user.OrganizationId
                                }, transaction);

                                query = $@"DELETE FROM Payroll_SupplementaryPaymentTaxInfo Where PaymentAmountId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                rawAffected = await connection.ExecuteAsync(query, new
                                {
                                    Id = model.PaymentId,
                                    user.CompanyId,
                                    user.OrganizationId
                                }, transaction);

                                query = $@"DELETE FROM Payroll_SupplementaryPaymentAmount Where PaymentAmountId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                rawAffected = await connection.ExecuteAsync(query, new
                                {
                                    Id = model.PaymentId,
                                    user.CompanyId,
                                    user.OrganizationId
                                }, transaction);

                                if (rawAffected > 0)
                                {
                                    rawAffected = 0;
                                    query = $@"Update Payroll_SupplementaryPaymentProcessInfo
                                    SET TotalAmount=TotalAmount-@amount,TotalOnceOffAmount=TotalOnceOffAmount-@tax,TotalEmployees=TotalEmployees-1
                                    Where PaymentProcessInfoId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId
                                    AND StateStatus='Pending'";

                                    rawAffected = await connection.ExecuteAsync(query, new
                                    {
                                        Id = model.ProcessId,
                                        Amount = model.amount,
                                        tax = model.taxAmount,
                                        user.CompanyId,
                                        user.OrganizationId
                                    }, transaction);
                                    if (rawAffected > 0)
                                    {
                                        transaction.Commit();
                                        executionStatus = ResponseMessage.Message(true, ResponseMessage.Saved);
                                    }
                                    else
                                    {
                                        throw new Exception("Process Info is failed to update");
                                    }
                                }
                                else
                                {
                                    executionStatus = ResponseMessage.Invalid("Unfortunately, Payment can not be undo");
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                if (ex.InnerException != null)
                                {
                                    await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentAmountBusiness", "DeleteSupplementaryAmountAsync", user);
                                    executionStatus = ResponseMessage.Invalid("Something went worng");
                                }
                                else
                                {
                                    executionStatus = ResponseMessage.Invalid(ex.Message);
                                }
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Invalid("Payment has been already disbursed");
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentAmountBusiness", "DeleteSupplementaryAmountAsync", user);
            }
            return executionStatus;
        }
        public async Task<SupplementaryPaymentAmountViewModel> GetSupplementaryAmountByIdAsync(long paymentAmountId, AppUser user)
        {
            SupplementaryPaymentAmountViewModel data = new SupplementaryPaymentAmountViewModel();
            try
            {
                var query = $@"SELECT PA.PaymentAmountId,PA.AllowanceNameId,AllowanceName=ALW.[Name],EMP.EmployeeCode,EmployeeName=EMP.FullName,
                PA.PaymentMonth,PA.PaymentYear,PA.Amount,PA.BaseOfPayment,PA.[Percentage],PA.StateStatus,PA.OnceOffAmount,PA.TaxAmount,PA.PaymentProcessInfoId,
                EMP.EmployeeId
                FROM Payroll_SupplementaryPaymentAmount PA
                INNER JOIN HR_EmployeeInformation EMP ON PA.EmployeeId = EMP.EmployeeId
                INNER JOIN Payroll_AllowanceName ALW ON ALW.AllowanceNameId = PA.AllowanceNameId
                Where 1=1 
                AND PA.PaymentAmountId=@PaymentAmountId";
                data = await _dapper.SqlQueryFirstAsync<SupplementaryPaymentAmountViewModel>(user.Database, query, new
                {
                    PaymentAmountId = paymentAmountId
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentAmountBusiness", "GetSupplementaryAmountById", user);
            }
            return data;
        }
    }
}
