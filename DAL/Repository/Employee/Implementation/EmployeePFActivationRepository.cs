using Dapper;
using Shared.Services;
using DAL.Logger.Interface;
using Shared.OtherModels.User;
using Shared.Employee.DTO.Info;
using Shared.Employee.DTO.Stage;
using Shared.OtherModels.Response;
using Shared.Employee.Domain.Stage;
using DAL.DapperObject.Interface;
using DAL.Repository.Employee.Interface;

namespace DAL.Repository.Employee.Implementation
{
    public class EmployeePFActivationRepository : IEmployeePFActivationRepository
    {
        private readonly IDapperData _dapper;
        private readonly IDALSysLogger _sysLogger;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeePFActivationRepository(IDapperData dapper, IDALSysLogger sysLogger, IEmployeeRepository employeeRepository)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _employeeRepository = employeeRepository;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<EmployeePFActivation>> GetAllAsync(AppUser user)
        {
            IEnumerable<EmployeePFActivation> list = new List<EmployeePFActivation>();
            try
            {
                var query = $@"SELECT * FROM HR_EmployeePFActivation Where 1=1 AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                list = await _dapper.SqlQueryListAsync<EmployeePFActivation>(user.Database, query, new { user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePFActivationRepository", "GetAllAsync", user);
            }
            return list;
        }
        public Task<IEnumerable<EmployeePFActivation>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<EmployeePFActivation> GetByIdAsync(long id, AppUser user)
        {
            EmployeePFActivation employeePFActivation = null;
            try
            {
                var query = $@"SELECT * FROM HR_EmployeePFActivation Where PFActivationId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                employeePFActivation = await _dapper.SqlQueryFirstAsync<EmployeePFActivation>(user.Database, query, new { Id = id, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePFActivationRepository", "GetByIdAsync", user);
            }
            return employeePFActivation;
        }
        public async Task<EmployeePFActivation> GetEmployeePFActivationByConfirmationId(long confirmationId, AppUser user)
        {
            EmployeePFActivation employeePFActivation = null;
            try
            {
                var query = $@"Select * From HR_EmployeePFActivation Where ConfirmationProposalId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                employeePFActivation = await _dapper.SqlQueryFirstAsync<EmployeePFActivation>(user.Database, query, new { Id = confirmationId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePFActivationRepository", "GetEmployeePFActivationByConfirmationId", user);
            }
            return employeePFActivation;
        }
        public async Task<ExecutionStatus> PFApprovalAsync(EmployeePFActivationApprovalDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var pfActivationInDb = await GetByIdAsync(model.PFActivationId, user);
                var employeeInfoInDb = await _employeeRepository.GetByIdAsync(pfActivationInDb.EmployeeId, user);
                if (pfActivationInDb != null)
                {
                    if (pfActivationInDb.StateStatus == "Pending")
                    {
                        using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                        {
                            connection.Open();
                            {
                                using (var transaction = connection.BeginTransaction())
                                {
                                    try
                                    {
                                        pfActivationInDb.StateStatus = model.StateStatus;
                                        if (model.StateStatus == "Approved")
                                        {
                                            pfActivationInDb.ApprovedBy = user.ActionUserId;
                                            pfActivationInDb.ApprovedDate = DateTime.Now;
                                            pfActivationInDb.ApprovalRemarks = model.Remarks;
                                        }
                                        else if (model.StateStatus == "Rejected")
                                        {
                                            pfActivationInDb.UpdatedBy = user.ActionUserId;
                                            pfActivationInDb.UpdatedDate = DateTime.Now;
                                        }

                                        var parameters = DapperParam.GetKeyValuePairsDynamic(pfActivationInDb, true);
                                        parameters.Remove("PFActivationId");
                                        var query = Utility.GenerateUpdateQuery(tableName: "HR_EmployeePFActivation", paramkeys: parameters.Select(x => x.Key).ToList());

                                        query += $"WHERE PFActivationId=@PFActivationId";
                                        parameters.Add("PFActivationId", pfActivationInDb.PFActivationId);
                                        int rawAffected = await connection.ExecuteAsync(query, parameters, transaction);

                                        if (rawAffected > 0)
                                        {
                                            if (model.StateStatus == "Approved")
                                            {
                                                query = $@"Update HR_EmployeeInformation SET PFActivationDate=@PFActivationDate,UpdatedBy=@UserId,UpdatedDate=GETDATE() 
                                                Where EmployeeId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                                rawAffected = await connection.ExecuteAsync(query, new { pfActivationInDb.PFActivationDate, UserId = user.ActionUserId, Id = employeeInfoInDb.EmployeeId, user.CompanyId, user.OrganizationId }, transaction);

                                                if (rawAffected > 0)
                                                {
                                                    transaction.Commit();
                                                    executionStatus = new ExecutionStatus();
                                                    executionStatus.Status = true;
                                                    executionStatus.Msg = "Data has been updated successfully";
                                                }
                                                else
                                                {
                                                    transaction.Rollback();
                                                    executionStatus = new ExecutionStatus();
                                                    executionStatus.Status = true;
                                                    executionStatus.Msg = "Data has been failed to save";
                                                }
                                            }
                                            else
                                            {
                                                transaction.Commit();
                                                executionStatus = new ExecutionStatus();
                                                executionStatus.Status = true;
                                                executionStatus.Msg = "Data has been updated successfully";
                                            }

                                            transaction.Commit();
                                            executionStatus = new ExecutionStatus();
                                            executionStatus.Status = true;
                                            executionStatus.Msg = "Data has been updated successfully";
                                        }
                                        else
                                        {
                                            transaction.Rollback();
                                            executionStatus = new ExecutionStatus();
                                            executionStatus.Status = false;
                                            executionStatus.Msg = "Data has been falied to update";
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePFActivationRepository", "PFApprovalAsync", user);
                                    }
                                    finally
                                    {
                                        connection.Close();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Invalid("Data is not pending data.");
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Invalid("Data not found.");
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePFActivationRepository", "PFApprovalAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveAsync(EmployeePFActivationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                if (model.PFActivationId > 0)
                {
                    var pfActivationInDb = await this.GetByIdAsync(model.PFActivationId, user);
                    if (pfActivationInDb != null)
                    {
                        if (pfActivationInDb.StateStatus == "Pending")
                        {
                            using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                            {
                                connection.Open();
                                {
                                    using (var transaction = connection.BeginTransaction())
                                    {
                                        try
                                        {
                                            pfActivationInDb.PFActivationDate = model.PFActivationDate;
                                            pfActivationInDb.PFEffectiveDate = model.PFEffectiveDate;
                                            pfActivationInDb.PFBasedAmount = model.PFBasedAmount;
                                            pfActivationInDb.PFPercentage = model.PFPercentage;
                                            pfActivationInDb.UpdatedBy = user.ActionUserId;
                                            pfActivationInDb.UpdatedDate = DateTime.Now;

                                            var parameters = DapperParam.GetKeyValuePairsDynamic(pfActivationInDb, true);
                                            parameters.Remove("PFActivationId");
                                            var query = Utility.GenerateUpdateQuery(tableName: "HR_EmployeePFActivation", paramkeys: parameters.Select(x => x.Key).ToList());
                                            query += $"WHERE PFActivationId=@PFActivationId";
                                            parameters.Add("PFActivationId", pfActivationInDb.PFActivationId);
                                            var rawAffected = await connection.ExecuteAsync(query, parameters, transaction);

                                            if (rawAffected > 0)
                                            {
                                                transaction.Commit();
                                                executionStatus = new ExecutionStatus();
                                                executionStatus.Status = true;
                                                executionStatus.Msg = "Data has been updated successfully";
                                            }
                                            else
                                            {
                                                transaction.Rollback();
                                                executionStatus = new ExecutionStatus();
                                                executionStatus.Status = false;
                                                executionStatus.Msg = "Data has been falied to update";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePFActivationRepository", "SaveAsync", user);
                                        }
                                        finally
                                        {
                                            connection.Close();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            executionStatus = new ExecutionStatus();
                            executionStatus.Status = false;
                            executionStatus.Msg = "Data is not Pending data";
                        }
                    }
                    else
                    {
                        executionStatus = new ExecutionStatus();
                        executionStatus.Status = false;
                        executionStatus.Msg = "No data found";
                    }
                }
                else
                {
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                    {
                        connection.Open();
                        {
                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    var employeeInfo = await _employeeRepository.GetByIdAsync(model.EmployeeId, user);
                                    EmployeePFActivation pFActivation = new EmployeePFActivation();
                                    pFActivation.EmployeeId = model.EmployeeId;
                                    pFActivation.EmployeeCode = employeeInfo != null ? employeeInfo.EmployeeCode : "";
                                    pFActivation.EmployeeName = employeeInfo != null ? employeeInfo.FullName : "";
                                    pFActivation.PFActivationDate = model.PFActivationDate;
                                    pFActivation.PFEffectiveDate = model.PFEffectiveDate;
                                    pFActivation.PFBasedAmount = model.PFBasedAmount;
                                    pFActivation.PFPercentage = model.PFPercentage;
                                    pFActivation.StateStatus = "Pending";
                                    pFActivation.Remarks = model.Remarks;
                                    pFActivation.CreatedBy = user.ActionUserId;
                                    pFActivation.CreatedDate = DateTime.Now;
                                    pFActivation.CompanyId = user.CompanyId;
                                    pFActivation.OrganizationId = user.OrganizationId;

                                    var parameters = DapperParam.GetKeyValuePairsDynamic(pFActivation, true);
                                    parameters.Remove("PFActivationId");
                                    var query = Utility.GenerateInsertQuery(tableName: "HR_EmployeePFActivation", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                    var rawAffected = await connection.ExecuteAsync(query, parameters, transaction);

                                    if (rawAffected == 0)
                                    {
                                        transaction.Rollback();
                                        executionStatus = new ExecutionStatus();
                                        executionStatus.Status = false;
                                        executionStatus.Msg = "Data has been falied to save";
                                    }
                                    else
                                    {
                                        transaction.Commit();
                                        executionStatus = new ExecutionStatus();
                                        executionStatus.Status = true;
                                        executionStatus.Msg = "Data has been saved successfully";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePFActivationRepository", "SaveAsync", user);
                                }
                                finally
                                {
                                    connection.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePFActivationRepository", "SaveAsync", user);
            }
            return executionStatus;
        }
    }
}
