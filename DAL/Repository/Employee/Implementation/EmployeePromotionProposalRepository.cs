using Dapper;
using Shared.Services;
using DAL.Logger.Interface;
using Shared.OtherModels.User;
using Shared.Employee.DTO.Stage;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.Employee.Domain.Stage;
using DAL.Repository.Employee.Interface;

namespace DAL.Repository.Employee.Implementation
{
    public class EmployeePromotionProposalRepository : IEmployeePromotionProposalRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IDesignationRepository designationRepository;

        public EmployeePromotionProposalRepository(IDesignationRepository _designationRepository, IEmployeeRepository _employeeRepository, IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            employeeRepository = _employeeRepository;
            designationRepository = _designationRepository;
        }

        public async Task<ExecutionStatus> ApprovalProposalAsync(long id, long employeeId, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var proposalInDb = await GetByIdAsync(id, user);
                if (proposalInDb.StateStatus == StateStatus.Pending)
                {
                    var employeeInDb = await employeeRepository.GetByIdAsync(employeeId, user);
                    if (employeeInDb.StateStatus == StateStatus.Approved)
                    {
                        string employeePreviousDataJson = "[" + JsonReverseConverter.JsonData(employeeInDb) + "]";
                        proposalInDb.StateStatus = StateStatus.Approved;
                        proposalInDb.ApprovedBy = user.ActionUserId;
                        proposalInDb.ApprovedDate = DateTime.Now;

                        using (var connection = _dapper.SqlConnectionForTransactionalNonQuery(user.Database))
                        {
                            connection.Open();
                            {
                                using (var transaction = connection.BeginTransaction())
                                {
                                    var parameters = Utility.DappperParamsInKeyValuePairs(proposalInDb, user, addBaseProperty: true, addUserId: false, addCompany: false, addOrganization: false);
                                    parameters.Remove("PromotionProposalId");
                                    var updateProposal = Utility.GenerateUpdateQuery(tableName: "HR_EmployeePromotionProposal", paramkeys: parameters.Select(x => x.Key).ToList());
                                    updateProposal += $"WHERE PromotionProposalId = @PromotionProposalId";
                                    parameters.Add("PromotionProposalId", proposalInDb.PromotionProposalId);
                                    int rawAffected = await connection.ExecuteAsync(updateProposal, parameters, transaction);
                                    if (rawAffected > 0)
                                    {
                                        if (proposalInDb.Head == "Designation")
                                        {
                                            rawAffected = 0;
                                            var designationId = Convert.ToInt32(proposalInDb.ProposalValue);
                                            var designationInDb = await designationRepository.GetByIdAsync(id: designationId, user);

                                            if (designationInDb != null && designationInDb.DesignationId > 0)
                                            {
                                                employeeInDb.GradeId = designationInDb.GradeId;
                                                employeeInDb.DesignationId = designationInDb.DesignationId;
                                                employeeInDb.UpdatedBy = user.ActionUserId;
                                                employeeInDb.UpdatedDate = DateTime.Now;

                                                var updateEmployee = $@"UPDATE HR_EmployeeInformation  
                                                SET GradeId=@GradeId, DesignationId=@DesignationId , UpdatedBy=@UpdatedBy , UpdatedDate=@UpdatedDate
                                                WHERE EmployeeId = @EmployeeId";


                                                rawAffected = await connection.ExecuteAsync(updateEmployee, new
                                                {
                                                    employeeInDb.EmployeeId,
                                                    employeeInDb.GradeId,
                                                    employeeInDb.DesignationId,
                                                    employeeInDb.UpdatedBy,
                                                    employeeInDb.UpdatedDate,
                                                }, transaction);

                                                if (rawAffected > 0)
                                                {
                                                    string employeePresentDataJson = "[" + JsonReverseConverter.JsonData(employeeInDb) + "]";
                                                    await _sysLogger.SaveUserActivity(user, "HR_EmployeeInformation", user.Database,
                                                        employeePreviousDataJson,
                                                        employeePresentDataJson,
                                                        employeeInDb.EmployeeId.ToString(),
                                                        "ApprovalProposalAsync",
                                                        "Update",
                                                        employeeInDb.EmployeeId);
                                                    transaction.Commit();
                                                    executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                                                }
                                                else
                                                {
                                                    transaction.Rollback();
                                                    executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Invalid("Employee is not approved yet");
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Invalid("Item is not pending yet");
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePromotionProposalRepository", "ApprovalProposalAsync", user);
                executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
            }
            return executionStatus;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<ExecutionStatus> DeletePendingProposalAsync(PromotionProposalCancellationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var itemInDb = await GetByIdAsync(model.ProposalId, user);
                if (itemInDb != null)
                {
                    if (itemInDb.StateStatus == StateStatus.Pending)
                    {

                        using (var connection = _dapper.SqlConnectionForTransactionalNonQuery(user.Database))
                        {
                            connection.Open();

                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    var deleteQuery = Utility.GenerateDeleteQuery("HR_EmployeePromotionProposal");
                                    deleteQuery += $"WHERE PromotionProposalId = @Id";
                                    int rawAffected = await connection.ExecuteAsync(deleteQuery, new { Id = itemInDb.PromotionProposalId }, transaction);

                                    if (rawAffected > 0)
                                    {
                                        string data_in_json_format = "[" + JsonReverseConverter.JsonData(itemInDb) + "]";
                                        transaction.Commit();
                                        executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                                        await _sysLogger.SaveUserActivity(user, "HR_EmployeePromotionProposal", user.Database, data_in_json_format, null, itemInDb.PromotionProposalId.ToString(), "DeletePendingProposalAsync", "Delete", itemInDb.EmployeeId);
                                    }
                                    else
                                    {
                                        executionStatus = ResponseMessage.Message(false, "Data has been failed to delete");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    executionStatus = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                                    await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePromotionProposalRepository", "DeletePendingProposalAsync", user);
                                }
                            }
                        }
                    }
                    else
                    {
                        executionStatus = executionStatus ?? new ExecutionStatus();
                        executionStatus.Status = false;
                        executionStatus.Msg = "Item not found in pending";
                    }
                }
                else
                {
                    executionStatus = executionStatus ?? new ExecutionStatus();
                    executionStatus.Status = false;
                    executionStatus.Msg = "Item not found";
                }

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePromotionProposalRepository", "DeletePendingProposalAsync", user);
            }
            return executionStatus;
        }
        public Task<IEnumerable<EmployeePromotionProposal>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<EmployeePromotionProposal>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<EmployeePromotionProposal> GetByIdAsync(long id, AppUser user)
        {
            EmployeePromotionProposal employeePromotionProposal = null;
            try
            {
                var query = $@"Select * From HR_EmployeePromotionProposal Where 1=1 AND PromotionProposalId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                employeePromotionProposal = await _dapper.SqlQueryFirstAsync<EmployeePromotionProposal>(user.Database, query, new
                {
                    Id = id,
                    user.CompanyId,
                    user.OrganizationId
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePromotionProposalRepository", "GetByIdAsync", user);
            }
            return employeePromotionProposal;
        }
        public async Task<EmployeePromotionProposal> SingleEmployeePendingProposalAsync(long id, long employeeId, AppUser user)
        {
            EmployeePromotionProposal employeePromotionProposal = null;
            try
            {
                var query = $@"Select * From HR_EmployeePromotionProposal Where 1=1 AND PromotionProposalId!=@Id AND StateStatus='Pending' AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                employeePromotionProposal = await _dapper.SqlQueryFirstAsync<EmployeePromotionProposal>(user.Database, query, new
                {
                    Id = id,
                    EmployeeId = employeeId,
                    user.CompanyId,
                    user.OrganizationId
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePromotionProposalRepository", "PendingProposalAsync", user);
            }
            return employeePromotionProposal;
        }
    }
}
