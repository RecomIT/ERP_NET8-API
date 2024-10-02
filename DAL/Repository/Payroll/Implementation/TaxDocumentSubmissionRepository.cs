using Dapper;
using Shared.Services;
using DAL.Logger.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using DAL.Payroll.Repository.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.Domain.AIT;

namespace DAL.Payroll.Repository.Implementation
{
    public class TaxDocumentSubmissionRepository : ITaxDocumentSubmissionRepository
    {
        private readonly IDapperData _dapper;
        private readonly IDALSysLogger _sysLogger;
        public TaxDocumentSubmissionRepository(IDapperData dapper, IDALSysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        #region NotImplemented
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaxDocumentSubmission>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaxDocumentSubmission>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<TaxDocumentSubmission> GetByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region AIT
        public async Task<TaxDocumentSubmission> GetAITByIdAsync(long id, AppUser user)
        {
            TaxDocumentSubmission submission = new TaxDocumentSubmission();
            try {
                var query = $@"Select * From Payroll_TaxDocumentSubmission Where SubmissionId=@Id AND CertificateType='AIT' AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                submission = await _dapper.SqlQueryFirstAsync<TaxDocumentSubmission>(user.Database, query, new {
                    Id = id,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxDocumentSubmissionRepository", "GetAITByIdAsync", user);
            }
            return submission;
        }
        public async Task<ExecutionStatus> SaveAITAsync(TaxDocumentSubmission model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try {
                if (model.SubmissionId == 0) {
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database)) {
                        connection.Open();
                        {
                            using (var transaction = connection.BeginTransaction()) {
                                model.CertificateType = "AIT";
                                model.StateStatus = StateStatus.Pending;
                                model.CreatedBy = user.ActionUserId;
                                
                                model.CreatedDate = DateTime.Now;
                                model.CompanyId = user.CompanyId;
                                model.OrganizationId = user.OrganizationId;
                                var parameters = Utility.DappperParamsInKeyValuePairs(model, user, addBaseProperty: true, addUserId: false, addCompany: false, addOrganization: false);
                                parameters.Remove("SubmissionId");
                                string query = Utility.GenerateInsertQuery(tableName: "Payroll_TaxDocumentSubmission", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                var rowAffected = await connection.ExecuteAsync(query, parameters, transaction);

                                if (rowAffected > 0) {
                                    transaction.Commit();
                                    executionStatus = new ExecutionStatus();
                                    executionStatus.Status = true;
                                    executionStatus.Msg = ResponseMessage.Saved;
                                }
                                else {
                                    transaction.Rollback();
                                    executionStatus = ResponseMessage.Invalid("Data has been failed to save");
                                }
                            }
                        }
                    }
                }
                else {
                    var AITInDb = await this.GetAITByIdAsync(model.SubmissionId, user);
                    if (AITInDb != null && AITInDb.SubmissionId > 0) {
                        if(AITInDb.StateStatus == StateStatus.Pending) {
                            using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database)) {
                                connection.Open();
                                {
                                    using (var transaction = connection.BeginTransaction()) {
                                        AITInDb.Amount = model.Amount;
                                        AITInDb.NumberOfCar = model.NumberOfCar;
                                        AITInDb.FileName = string.IsNullOrEmpty(model.FileName) || string.IsNullOrWhiteSpace(model.FileName) ? AITInDb.FileName : model.FileName;
                                        AITInDb.FilePath = string.IsNullOrEmpty(model.FilePath) || string.IsNullOrWhiteSpace(model.FilePath) ? AITInDb.FilePath : model.FilePath;
                                        AITInDb.FileSize = string.IsNullOrEmpty(model.FileSize) || string.IsNullOrWhiteSpace(model.FileSize) ? AITInDb.FileSize : model.FileSize;
                                        AITInDb.FileFormat = string.IsNullOrEmpty(model.FileFormat) || string.IsNullOrWhiteSpace(model.FileFormat) ? AITInDb.FilePath : model.FileFormat;
                                        AITInDb.ActualFileName = string.IsNullOrEmpty(model.ActualFileName) || string.IsNullOrWhiteSpace(model.ActualFileName) ? AITInDb.ActualFileName : model.ActualFileName;
                                        AITInDb.UpdatedBy = user.ActionUserId;
                                        AITInDb.UpdatedDate = DateTime.Now;

                                        var parameters = DapperParam.GetKeyValuePairsDynamic(AITInDb, addBaseProperty: true);
                                        parameters.Remove("SubmissionId");
                                        string query = Utility.GenerateUpdateQuery(tableName: "Payroll_TaxDocumentSubmission", paramkeys: parameters.Select(x => x.Key).ToList());
                                        parameters.Add("SubmissionId", AITInDb.SubmissionId);
                                        query += $@"Where SubmissionId=@SubmissionId";

                                        var rowAffected = await connection.ExecuteAsync(query, parameters, transaction);
                                        if (rowAffected > 0) {
                                            transaction.Commit();
                                            executionStatus = new ExecutionStatus();
                                            executionStatus.Status = true;
                                            executionStatus.Msg = ResponseMessage.Updated;
                                        }
                                        else {
                                            transaction.Rollback();
                                            executionStatus = ResponseMessage.Invalid("Data has been failed to save");
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            executionStatus = ResponseMessage.Invalid("AIT status is not in pending");
                        }
                        
                    }
                    else {
                        executionStatus = ResponseMessage.Invalid("AIT not found");
                    }
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxDocumentSubmissionRepository", "SaveAITAsync", user);
            }
            return executionStatus;
        }
        public async Task<bool> IsAITApprovedAsync(long id, long employeeId, AppUser user)
        {
            bool isApproved = false;
            try {
                var query = $@"SELECT (CASE WHEN StateStatus= 'Approved' THEN 1 ELSE 0 END) 
                FROM Payroll_TaxDocumentSubmission Where CertificateType='AIT' 
                AND SubmissionId=@Id AND EmployeeId=@EmployeeId
                AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                isApproved = await _dapper.SqlQueryFirstAsync<bool>(user.Database, query, new {
                    Id = id,
                    EmployeeId = employeeId,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxDocumentSubmissionRepository", "IsAITApprovedAsync", user);
            }
            return isApproved;
        }
        public async Task<bool> IsAITExistAsync(long fiscalYearId, long employeeId, AppUser user)
        {
            bool isExist = false;
            try {
                var query = $@"SELECT COUNT(*) FROM Payroll_TaxDocumentSubmission 
                Where EmployeeId=@EmployeeId AND CertificateType='AIT' AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId
                AND StateStatus IN ('Pending','Approved')";

                var count = await _dapper.SqlQueryFirstAsync<int>(user.Database, query, new {
                    FiscalYearId = fiscalYearId,
                    EmployeeId = employeeId,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });

                if (count > 0) {
                    isExist = true;
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxDocumentSubmissionRepository", "IsAITApprovedAsync", user);
            }
            return isExist;
        }
        #endregion

        #region Tax-Refund
        public async Task<TaxDocumentSubmission> GetTaxRefundByIdAsync(long id, AppUser user)
        {
            TaxDocumentSubmission submission = new TaxDocumentSubmission();
            try {
                var query = $@"Select * From Payroll_TaxDocumentSubmission Where SubmissionId=@Id AND CertificateType='CET' AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                submission = await _dapper.SqlQueryFirstAsync<TaxDocumentSubmission>(user.Database, query, new {
                    Id = id,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxDocumentSubmissionRepository", "GetTaxRefundByIdAsync", user);
            }
            return submission;
        }
        public async Task<bool> IsTaxRefundApprovedAsync(long id, long employeeId, AppUser user)
        {
            bool isApproved = false;
            try {
                var query = $@"SELECT (CASE WHEN StateStatus= 'Approved' THEN 1 ELSE 0 END) 
                FROM Payroll_TaxDocumentSubmission Where CertificateType='CET' 
                AND SubmissionId=@Id AND EmployeeId=@EmployeeId
                AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                isApproved = await _dapper.SqlQueryFirstAsync<bool>(user.Database, query, new {
                    Id = id,
                    EmployeeId = employeeId,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxDocumentSubmissionRepository", "IsTaxRefundApprovedAsync", user);
            }
            return isApproved;
        }
        public async Task<bool> IsTaxRefundExistAsync(long fiscalYearId, long employeeId, AppUser user)
        {
            bool isExist = false;
            try {
                var query = $@"SELECT COUNT(*) FROM Payroll_TaxDocumentSubmission 
                Where EmployeeId=@EmployeeId AND CertificateType='CET' AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId
                AND StateStatus IN ('Pending','Approved')";

                var count = await _dapper.SqlQueryFirstAsync<int>(user.Database, query, new {
                    FiscalYearId = fiscalYearId,
                    EmployeeId = employeeId,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });

                if (count > 0) {
                    isExist = true;
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxDocumentSubmissionRepository", "IsTaxRefundExistAsync", user);
            }
            return isExist;
        }
        public async Task<ExecutionStatus> SaveTaxRefundAsync(TaxDocumentSubmission model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try {
                if (model.SubmissionId == 0) {
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database)) {
                        connection.Open();
                        {
                            using (var transaction = connection.BeginTransaction()) {
                                model.CertificateType = "CET";
                                model.StateStatus = StateStatus.Pending;
                                model.CreatedBy = user.ActionUserId;
                                model.CreatedDate = DateTime.Now;
                                model.CompanyId = user.CompanyId;
                                model.OrganizationId = user.OrganizationId;
                                var parameters = Utility.DappperParamsInKeyValuePairs(model, user, addBaseProperty: true, addUserId: false, addCompany: false, addOrganization: false);
                                parameters.Remove("SubmissionId");
                                string query = Utility.GenerateInsertQuery(tableName: "Payroll_TaxDocumentSubmission", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                var rowAffected = await connection.ExecuteAsync(query, parameters, transaction);

                                if (rowAffected > 0) {
                                    transaction.Commit();
                                    executionStatus = new ExecutionStatus();
                                    executionStatus.Status = true;
                                    executionStatus.Msg = ResponseMessage.Saved;
                                }
                                else {
                                    transaction.Rollback();
                                    executionStatus = ResponseMessage.Invalid("Data has been failed to save");
                                }
                            }
                        }
                    }
                }
                else {
                    var TaxRefundInDb = await this.GetTaxRefundByIdAsync(model.SubmissionId, user);
                    if (TaxRefundInDb != null && TaxRefundInDb.SubmissionId > 0) {
                        //if(TaxRefundInDb.StateStatus == StateStatus.Pending) {

                        //else {
                        //    executionStatus = ResponseMessage.Invalid("Tax Refund is not in pending");
                        //}
                        using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                        {
                            connection.Open();
                            {
                                using (var transaction = connection.BeginTransaction())
                                {
                                    TaxRefundInDb.Amount = model.Amount;
                                    TaxRefundInDb.FilePath = string.IsNullOrEmpty(model.FilePath) || string.IsNullOrWhiteSpace(model.FilePath) ? TaxRefundInDb.FilePath : model.FilePath;
                                    TaxRefundInDb.FileSize = string.IsNullOrEmpty(model.FileSize) || string.IsNullOrWhiteSpace(model.FileSize) ? TaxRefundInDb.FileSize : model.FileSize;
                                    TaxRefundInDb.FileFormat = string.IsNullOrEmpty(model.FileFormat) || string.IsNullOrWhiteSpace(model.FileFormat) ? TaxRefundInDb.FileFormat : model.FileFormat;
                                    TaxRefundInDb.ActualFileName = string.IsNullOrEmpty(model.ActualFileName) || string.IsNullOrWhiteSpace(model.ActualFileName) ? TaxRefundInDb.ActualFileName : model.ActualFileName;
                                    TaxRefundInDb.FileName = string.IsNullOrEmpty(model.FileName) || string.IsNullOrWhiteSpace(model.FileName) ? TaxRefundInDb.FileName : model.FileName;
                                    TaxRefundInDb.UpdatedBy = user.ActionUserId;
                                    TaxRefundInDb.UpdatedDate = DateTime.Now;

                                    var parameters = DapperParam.GetKeyValuePairsDynamic(TaxRefundInDb, addBaseProperty: true);
                                    parameters.Remove("SubmissionId");
                                    string query = Utility.GenerateUpdateQuery(tableName: "Payroll_TaxDocumentSubmission", paramkeys: parameters.Select(x => x.Key).ToList());
                                    parameters.Add("SubmissionId", TaxRefundInDb.SubmissionId);
                                    query += $@"Where SubmissionId=@SubmissionId";

                                    var rowAffected = await connection.ExecuteAsync(query, parameters, transaction);
                                    if (rowAffected > 0)
                                    {
                                        transaction.Commit();
                                        executionStatus = new ExecutionStatus();
                                        executionStatus.Status = true;
                                        executionStatus.Msg = ResponseMessage.Updated;
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        executionStatus = ResponseMessage.Invalid("Data has been failed to save");
                                    }
                                }
                            }

                        }
                    }
                    else {
                        executionStatus = ResponseMessage.Invalid("Tax Refund is not found");
                    }
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxDocumentSubmissionRepository", "SaveAITAsync", user);
            }
            return executionStatus;
        }
        #endregion
    }
}
