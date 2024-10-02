using Dapper;
using Shared.Services;
using DAL.Logger.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.Domain.Stage;
using Shared.Employee.ViewModel.Stage;
using DAL.Repository.Employee.Interface;
using DAL.DapperObject.Interface;
using DAL.UnitOfWork.Control_Panel.Interface;
using DAL.Repository.Control_Panel;

namespace DAL.Repository.Employee.Implementation
{
    public class EmploymentConfirmationRepository : IEmploymentConfirmationRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IControlPanelUnitOfWork _controlPanelDbContext;
        private readonly IEmployeePFActivationRepository _employeePFActivationRepository;
        private readonly PayrollModuleConfigReposiitory _payrollModuleConfigReposiitory;
        public EmploymentConfirmationRepository(IDALSysLogger sysLogger, IEmployeePFActivationRepository employeePFActivationRepository, IEmployeeRepository employeeRepository, IDapperData dapper,
            IControlPanelUnitOfWork controlPanelDbContext)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _controlPanelDbContext = controlPanelDbContext;
            _employeeRepository = employeeRepository;
            _employeePFActivationRepository = employeePFActivationRepository;
            _payrollModuleConfigReposiitory = new PayrollModuleConfigReposiitory(_controlPanelDbContext);
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<EmployeeConfirmationProposal>> GetAllAsync(AppUser user)
        {
            IEnumerable<EmployeeConfirmationProposal> list = new List<EmployeeConfirmationProposal>();
            try
            {
                var query = $@"SELECT * FROM HR_EmployeeConfirmationProposal Where CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                list = await _dapper.SqlQueryListAsync<EmployeeConfirmationProposal>(user.Database, query, new { user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationRepository", "GetAllAsync", user);
            }
            return list;
        }
        public Task<IEnumerable<EmployeeConfirmationProposal>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<EmployeeConfirmationProposal> GetByIdAsync(long id, AppUser user)
        {
            EmployeeConfirmationProposal employeeConfirmationProposal = null;
            try
            {
                var query = $@"SELECT * FROM HR_EmployeeConfirmationProposal Where ConfirmationProposalId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                employeeConfirmationProposal = await _dapper.SqlQueryFirstAsync<EmployeeConfirmationProposal>(user.Database, query, new { Id = id, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationRepository", "GetByIdAsync", user);
            }
            return employeeConfirmationProposal;
        }
        public async Task<DBResponse> GetEmploymentConfirmationsAsync(object model, AppUser user)
        {
            DBResponse response = new DBResponse();
            try
            {
                var query = $@"WITH Data_CTE AS(
	            SELECT CON.ConfirmationProposalId,CON.EmployeeId,EMP.EmployeeCode,[EmployeeName]=EMP.FullName,
                EMP.GradeName,EMP.DesignationName,EMP.DepartmentName,EMP.DateOfJoining,CON.ConfirmationDate,
                CON.EffectiveDate,CON.AppraiserComment,CON.StateStatus,CON.CreatedDate,CON.CreatedBy,CON.WithPFActivation,
                PF.PFEffectiveDate,PF.PFActivationDate
                FROM HR_EmployeeConfirmationProposal CON
                INNER JOIN [dbo].[vw_HR_EmployeeList] EMP ON CON.EmployeeId = EMP.EmployeeId
                LEFT JOIN HR_EmployeePFActivation PF ON CON.EmployeeId= PF.EmployeeId AND CON.ConfirmationProposalId=PF.ConfirmationProposalId
                WHERE 1=1
	            AND (@ConfirmationProposalId IS NULL OR @ConfirmationProposalId=0 OR CON.ConfirmationProposalId=@ConfirmationProposalId)
	            AND (@EmployeeId IS NULL OR @EmployeeId=0  OR EMP.EmployeeId=@EmployeeId)
	            AND (@EmployeeCode IS NULL OR @EmployeeCode='' OR EMP.EmployeeCode LIKE '%'+@EmployeeCode+'%')
	            AND (@StateStatus IS NULL OR @StateStatus='' OR CON.StateStatus = @StateStatus)
	            AND (
		            (@FromDate IS NULL AND @ToDate IS NULL)
		            OR
		            ((ISNULL(@FromDate,'') <> '' AND ISNULL(@ToDate,'') <> '') AND ConfirmationDate Between Convert(date,@FromDate) AND Convert(date,@ToDate)) 
		            OR
		            (@FromDate <> '' AND ConfirmationDate = Convert(date,@FromDate))
		            OR	
		            (@ToDate <> '' AND ConfirmationDate =Convert(date,@ToDate))
		            OR
		            (@FromDate ='' AND @ToDate = '')
	            )
	            AND CON.CompanyId=@CompanyId
	            AND CON.OrganizationId=@OrganizationId),
	            Count_CTE AS (
	            SELECT COUNT(*) AS [TotalRows]
	            FROM Data_CTE)
	            SELECT JSONData=(Select * From (SELECT *
	            FROM Data_CTE
	            ORDER BY EmployeeId
	            OFFSET (@PageNumber-1)*@PageSize ROWS
	            FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber";

                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, query, model);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationRepository", "GetEmploymentConfirmationsAsync", user);
            }
            return response;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetUnconfirmedEmployeeInfosInApplyAsync(AppUser user)
        {
            IEnumerable<Select2Dropdown> list = new List<Select2Dropdown>();
            try
            {
                var query = $@"SELECT [Id]=CAST(EMP.EmployeeId AS NVARCHAR(50)), 
                [Values]=CAST(EMP.EmployeeId AS NVARCHAR(50)),
                [Text]=(EMP.FullName+' ['+EMP.EmployeeCode+'] '+EMP.JobType)
                FROM HR_EmployeeInformation EMP
                Where 1=1
                AND EMP.DateOfConfirmation IS NULL 
                AND EMP.EmployeeId 
                NOT IN (SELECT EmployeeId FROM HR_EmployeeConfirmationProposal 
                Where StateStatus IN('Pending','Approved') AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId)
                AND EMP.CompanyId=@CompanyId
                AND EMP.OrganizationId=@OrganizationId";

                list = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, query, new { user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationRepository", "GetUnconfirmedEmployeeInfosInApplyAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetUnconfirmedEmployeeInfosInUpdateAsync(AppUser user)
        {
            IEnumerable<Select2Dropdown> list = new List<Select2Dropdown>();
            try
            {
                var query = $@"SELECT [Id]=CAST(EMP.EmployeeId AS NVARCHAR(50)), 
                [Values]=CAST(EMP.EmployeeId AS NVARCHAR(50)),
                [Text]=(EMP.FullName+' ['+EMP.EmployeeCode+'] '+EMP.JobType)
                FROM HR_EmployeeInformation EMP
                Where 1=1
                AND EMP.DateOfConfirmation IS NULL 
                AND EMP.EmployeeId 
                NOT IN (SELECT EmployeeId FROM HR_EmployeeConfirmationProposal 
                Where StateStatus IN('Approved') AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId)
                AND EMP.CompanyId=@CompanyId
                AND EMP.OrganizationId=@OrganizationId";

                list = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, query, new { user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationRepository", "GetUnconfirmedEmployeeInfosInUpdateAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveAsync(EmploymentConfirmationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                if (model.ConfirmationProposalId > 0)
                {
                    var confirmationInDb = await this.GetByIdAsync(model.ConfirmationProposalId, user);
                    if (confirmationInDb.StateStatus == "Pending")
                    {
                        using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                        {
                            connection.Open();
                            {
                                using (var transaction = connection.BeginTransaction())
                                {
                                    try
                                    {
                                        confirmationInDb.ConfirmationDate = model.ConfirmationDate;
                                        confirmationInDb.TotalRatingScore = model.TotalRatingScore;
                                        confirmationInDb.AppraiserComment = model.AppraiserComment;
                                        confirmationInDb.EffectiveDate = model.EffectiveDate;
                                        confirmationInDb.UpdatedBy = user.ActionUserId;
                                        confirmationInDb.UpdatedDate = DateTime.Now;

                                        var parameters = DapperParam.GetKeyValuePairsDynamic(confirmationInDb, true);
                                        parameters.Remove("ConfirmationProposalId");

                                        var query = Utility.GenerateUpdateQuery(tableName: "HR_EmployeeConfirmationProposal", paramkeys: parameters.Select(x => x.Key).ToList());
                                        query += $"WHERE ConfirmationProposalId = @ConfirmationProposalId";
                                        parameters.Add("ConfirmationProposalId", confirmationInDb.ConfirmationProposalId);
                                        var rawAffected = await connection.ExecuteAsync(query, parameters, transaction);

                                        if (rawAffected > 0)
                                        {
                                            var pfActionvationInDb = await _employeePFActivationRepository.GetEmployeePFActivationByConfirmationId(confirmationInDb.ConfirmationProposalId, user);
                                            if (pfActionvationInDb != null)
                                            {
                                                if (pfActionvationInDb.StateStatus == "Pending")
                                                {
                                                    if (pfActionvationInDb.PFActivationDate.Value.Date != model.PFActivationDate.Value.Date ||
                                                        pfActionvationInDb.PFEffectiveDate.Value.Date != model.PFEffectiveDate.Value.Date)
                                                    {
                                                        pfActionvationInDb.PFActivationDate = model.PFActivationDate;
                                                        pfActionvationInDb.PFEffectiveDate = model.PFEffectiveDate;
                                                        pfActionvationInDb.UpdatedBy = user.ActionUserId;
                                                        pfActionvationInDb.UpdatedDate = DateTime.Now;
                                                        parameters.Clear();

                                                        parameters = DapperParam.GetKeyValuePairsDynamic(pfActionvationInDb, true);
                                                        parameters.Remove("PFActivationId");
                                                        query = Utility.GenerateUpdateQuery(tableName: "HR_EmployeePFActivation", paramkeys: parameters.Select(x => x.Key).ToList());
                                                        query += $"WHERE PFActivationId=@PFActivationId";
                                                        parameters.Add("PFActivationId", pfActionvationInDb.PFActivationId);
                                                        rawAffected = await connection.ExecuteAsync(query, parameters, transaction);

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

                                                        // 
                                                    }
                                                    else
                                                    {
                                                        transaction.Commit();
                                                        executionStatus = new ExecutionStatus();
                                                        executionStatus.Status = true;
                                                        executionStatus.Msg = "Data has been saved successfully";
                                                    }

                                                }
                                                else
                                                {
                                                    transaction.Commit();
                                                    executionStatus = new ExecutionStatus();
                                                    executionStatus.Status = true;
                                                    executionStatus.Msg = "Data has been saved successfully";
                                                }
                                            }
                                            else
                                            {
                                                transaction.Commit();
                                                executionStatus = new ExecutionStatus();
                                                executionStatus.Status = true;
                                                executionStatus.Msg = "Data has been saved successfully";
                                            }
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
                                        transaction.Rollback();
                                        executionStatus = new ExecutionStatus();
                                        executionStatus.Status = false;
                                        executionStatus.Msg = "Data has been falied to update";
                                        await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationRepository", "SaveAsync", user);
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
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                    {
                        connection.Open();
                        {
                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    EmployeeConfirmationProposal employmentConfirmation = new EmployeeConfirmationProposal();
                                    employmentConfirmation.ConfirmationDate = model.ConfirmationDate;
                                    employmentConfirmation.EmployeeId = model.EmployeeId;
                                    employmentConfirmation.EffectiveDate = model.EffectiveDate;
                                    employmentConfirmation.StateStatus = "Pending";
                                    employmentConfirmation.AppraiserId = user.ActionUserId;
                                    employmentConfirmation.AppraiserComment = model.AppraiserComment;
                                    employmentConfirmation.CreatedBy = user.ActionUserId;
                                    employmentConfirmation.CreatedDate = DateTime.Now;
                                    employmentConfirmation.WithPFActivation = model.WithPFActivation;
                                    employmentConfirmation.CompanyId = user.CompanyId;
                                    employmentConfirmation.OrganizationId = user.OrganizationId;

                                    var parameters = DapperParam.GetKeyValuePairsDynamic(employmentConfirmation, true);
                                    parameters.Remove("ConfirmationProposalId");

                                    string query = Utility.GenerateInsertQuery(tableName: "HR_EmployeeConfirmationProposal", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                    var insertedRaw = await connection.QueryFirstOrDefaultAsync<EmployeeConfirmationProposal>(query, parameters, transaction);

                                    if (insertedRaw.ConfirmationProposalId > 0)
                                    {

                                        var payrollModuleConfig =
                                            await _payrollModuleConfigReposiitory.GetSingleAsync(item => item.CompanyId == user.CompanyId && item.OrganizationId == user.OrganizationId);

                                        if (payrollModuleConfig != null)
                                        {
                                            if (payrollModuleConfig.IsProvidentFundactivated == true)
                                            {
                                                if (model.PFEffectiveDate.HasValue && model.PFActivationDate.HasValue)
                                                {
                                                    parameters.Clear();

                                                    EmployeePFActivation employeePFActivation = new EmployeePFActivation();
                                                    employeePFActivation.EmployeeId = model.EmployeeId;
                                                    employeePFActivation.PFActivationDate = model.PFActivationDate.Value;
                                                    employeePFActivation.PFEffectiveDate = model.PFEffectiveDate.Value;
                                                    employeePFActivation.PFBasedAmount = payrollModuleConfig.BaseOfProvidentFund;
                                                    employeePFActivation.PFPercentage = Utility.TryParseDecimal(payrollModuleConfig.PercentageOfProvidentFund);
                                                    employeePFActivation.StateStatus = "Pending";
                                                    employeePFActivation.CreatedBy = user.ActionUserId;
                                                    employeePFActivation.CreatedDate = DateTime.Now;
                                                    employeePFActivation.CompanyId = user.CompanyId;
                                                    employeePFActivation.OrganizationId = user.OrganizationId;
                                                    employeePFActivation.ConfirmationProposalId = insertedRaw.ConfirmationProposalId;

                                                    parameters = DapperParam.GetKeyValuePairsDynamic(employeePFActivation, true);
                                                    parameters.Remove("PFActivationId");
                                                    query = Utility.GenerateInsertQuery(tableName: "HR_EmployeePFActivation", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
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
                                                else
                                                {
                                                    transaction.Commit();
                                                    executionStatus = new ExecutionStatus();
                                                    executionStatus.Status = true;
                                                    executionStatus.Msg = "Data has been saved successfully";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            transaction.Commit();
                                            executionStatus = new ExecutionStatus();
                                            executionStatus.Msg = "Data has been saved successfully";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationRepository", "SaveAsync", user);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationRepository", "SaveAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ConfirmationApprovalAsync(EmploymentConfirmationStatusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var confirmationInDb = await GetByIdAsync(model.ConfirmationProposalId, user);
                var employeeInfoInDb = await _employeeRepository.GetByIdAsync(confirmationInDb.EmployeeId, user);
                if (confirmationInDb != null)
                {
                    if (confirmationInDb.StateStatus == "Pending")
                    {

                        using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                        {
                            connection.Open();
                            {
                                using (var transaction = connection.BeginTransaction())
                                {
                                    try
                                    {
                                        confirmationInDb.StateStatus = model.StateStatus;
                                        if (model.StateStatus == "Approved")
                                        {
                                            confirmationInDb.ApprovedBy = user.ActionUserId;
                                            confirmationInDb.ApprovedDate = DateTime.Now;
                                            confirmationInDb.ApprovalRemarks = model.Remarks;
                                        }
                                        else if (model.StateStatus == "Rejected")
                                        {
                                            confirmationInDb.UpdatedBy = user.ActionUserId;
                                            confirmationInDb.UpdatedDate = DateTime.Now;
                                        }

                                        var parameters = DapperParam.GetKeyValuePairsDynamic(confirmationInDb, true);
                                        parameters.Remove("ConfirmationProposalId");
                                        var query = Utility.GenerateUpdateQuery(tableName: "HR_EmployeeConfirmationProposal", paramkeys: parameters.Select(x => x.Key).ToList());

                                        query += $"WHERE ConfirmationProposalId=@ConfirmationProposalId";
                                        parameters.Add("ConfirmationProposalId", confirmationInDb.ConfirmationProposalId);
                                        int rawAffected = await connection.ExecuteAsync(query, parameters, transaction);

                                        if (rawAffected > 0)
                                        {
                                            parameters.Clear();
                                            var pfActivationInDb = await _employeePFActivationRepository.GetEmployeePFActivationByConfirmationId(confirmationInDb.ConfirmationProposalId, user);
                                            if (pfActivationInDb != null && pfActivationInDb.PFActivationId > 0)
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

                                                parameters = DapperParam.GetKeyValuePairsDynamic(pfActivationInDb, true);
                                                parameters.Remove("PFActivationId");
                                                query = Utility.GenerateUpdateQuery(tableName: "HR_EmployeePFActivation", paramkeys: parameters.Select(x => x.Key).ToList());
                                                query += $"WHERE PFActivationId=@PFActivationId";
                                                parameters.Add("PFActivationId", pfActivationInDb.PFActivationId);
                                                rawAffected = await connection.ExecuteAsync(query, parameters, transaction);

                                                if (rawAffected > 0)
                                                {

                                                    if (model.StateStatus == "Approved")
                                                    {
                                                        query = $@"Update HR_EmployeeInformation SET DateOfConfirmation=@DateOfConfirmation,PFActivationDate=@PFActivationDate,UpdatedBy=@UserId,UpdatedDate=GETDATE() 
                                                Where EmployeeId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                                        rawAffected = await connection.ExecuteAsync(query, new { DateOfConfirmation = confirmationInDb.ConfirmationDate, pfActivationInDb.PFActivationDate, UserId = user.ActionUserId, Id = employeeInfoInDb.EmployeeId, user.CompanyId, user.OrganizationId }, transaction);


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


                                                }
                                                else
                                                {
                                                    transaction.Rollback();
                                                    executionStatus = new ExecutionStatus();
                                                    executionStatus.Status = false;
                                                    executionStatus.Msg = "PF Data has been falied to update";
                                                }
                                            }
                                            else
                                            {

                                                if (model.StateStatus == "Approved")
                                                {
                                                    query = $@"Update HR_EmployeeInformation SET DateOfConfirmation=@DateOfConfirmation,UpdatedBy=@UserId,UpdatedDate=GETDATE() 
                                                Where EmployeeId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                                    rawAffected = await connection.ExecuteAsync(query, new { DateOfConfirmation = confirmationInDb.ConfirmationDate, UserId = user.ActionUserId, Id = employeeInfoInDb.EmployeeId, user.CompanyId, user.OrganizationId }, transaction);

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
                                                    transaction.Rollback();
                                                    executionStatus = new ExecutionStatus();
                                                    executionStatus.Status = true;
                                                    executionStatus.Msg = "Data has been failed to save";
                                                }
                                            }
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
                                        transaction.Rollback();
                                        executionStatus = new ExecutionStatus();
                                        executionStatus.Status = false;
                                        executionStatus.Msg = ResponseMessage.SomthingWentWrong;
                                        await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationRepository", "ConfirmationApprovalAsync", user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationRepository", "ConfirmationApprovalAsync", user);
            }
            return executionStatus;
        }
    }
}
