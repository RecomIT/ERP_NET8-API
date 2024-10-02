using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.Pagination;
using Shared.Employee.DTO.Stage;
using Shared.Employee.Filter.Stage;
using Shared.Employee.ViewModel.Stage;
using Shared.Employee.Filter.Info;
using Shared.Employee.Domain.Stage;
using BLL.Employee.Interface.Info;
using BLL.Employee.Interface.Stage;
using DAL.DapperObject.Interface;

namespace BLL.Employee.Implementation.Stage
{
    public class ContractualEmploymentBusiness : IContractualEmploymentBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly IInfoBusiness _infoBusiness;
        public ContractualEmploymentBusiness(IInfoBusiness infoBusiness, ISysLogger sysLogger, IDapperData dapper)
        {
            _infoBusiness = infoBusiness;
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public async Task<ExecutionStatus> SaveEmployeeContractApprovalAsync(ContractualEmploymentApprovalDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var query = "";
                        try
                        {
                            var contractInDb = (await GetContractualEmploymentsInfoAsync(new ContractualEmployment_Filter()
                            {
                                ContractId = model.ContractId.ToString()
                            }, user)).ListOfObject.FirstOrDefault();

                            if (contractInDb.StateStatus == StateStatus.Pending || contractInDb.StateStatus == StateStatus.Recheck)
                            {
                                query = $@"Update HR_ContractualEmployment
                                SET StateStatus=@StateStatus,
                                ApprovedBy=(CASE WHEN @StateStatus='Approved' THEN @StateStatus END),
                                ApprovedDate=(CASE WHEN @StateStatus='Approved' THEN GETDATE() END),
                                CancelledBy=(CASE WHEN @StateStatus='Cancelled' THEN @StateStatus END),
                                CancelledDate=(CASE WHEN @StateStatus='Cancelled' THEN GETDATE() END),
                                RejectedBy=(CASE WHEN @StateStatus='Rejected' THEN @StateStatus END),
                                RejectedDate=(CASE WHEN @StateStatus='Rejected' THEN GETDATE() END),
                                IsApproved=(CASE WHEN @StateStatus='Approved' THEN 1 END),
                                UpdatedBy=@UserId, UpdatedDate=GETDATE()
                                Where ContractId=@ContractId AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                var parameters = DapperParam.AddParamsInKeyValuePairs(model, user);
                                var rawAffected = await connection.ExecuteAsync(query, parameters, transaction, commandTimeout: 0, CommandType.Text);

                                if (rawAffected > 0 && contractInDb.LastContractId > 0)
                                {
                                    parameters.Clear();

                                    query = $@"Update HR_ContractualEmployment
                                    SET ContractEndDate=@LastContractEndDate 
                                    Where ContractId=@LastContractId AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                    parameters = DapperParam.AddParamsInKeyValuePairs(new
                                    {
                                        contractInDb.LastContractEndDate,
                                        contractInDb.LastContractId,
                                        contractInDb.EmployeeId
                                    }, user);


                                    rawAffected = await connection.ExecuteAsync(query, parameters, transaction, commandTimeout: 0, CommandType.Text);

                                    if (rawAffected > 0)
                                    {
                                        executionStatus.Status = true;
                                        executionStatus.Msg = "Data has been Updated successfully.";
                                        transaction.Commit();
                                    }
                                }
                            }
                            else
                            {
                                executionStatus.Status = false;
                                executionStatus.Msg = contractInDb.EmployeeCode + " : " + contractInDb.EmployeeName + " does not have pending contract with this ID";
                            }
                        }
                        catch (Exception ex)
                        {
                            executionStatus = ResponseMessage.Invalid();
                            await _sysLogger.SaveHRMSException(ex, user.Database, "ContractualEmploymentBusiness", "GetContractualEmploymentsInfoAsync", user);
                        }
                        finally { connection.Close(); }
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "ContractualEmploymentBusiness", "GetContractualEmploymentsInfoAsync", user);
            }
            return executionStatus;
        }
        public async Task<DBResponse<ContractualEmploymentViewModel>> GetContractualEmploymentsInfoAsync(ContractualEmployment_Filter filter, AppUser user)
        {
            DBResponse<ContractualEmploymentViewModel> data = new DBResponse<ContractualEmploymentViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var query = $@"BEGIN
                WITH Data_CTE AS(SELECT EMP.EmployeeId,EMP.EmployeeCode,EMP.FullName AS 'EmployeeName',CEMP.StateStatus,CEMP.ContractCode,CEMP.ContractStartDate,
                CEMP.ContractEndDate,CEMP.IsActive,CEMP.IsTerminated,CEMP.Flag,CEMP.LastContractId,CEMP.ContractId,CEMP.LastContractEndDate
                FROM HR_ContractualEmployment CEMP
                INNER JOIN HR_EmployeeInformation EMP ON CEMP.EmployeeId=EMP.EmployeeId
                Where 1=1
                AND (@ContractCode IS NULL OR @ContractCode ='' OR CEMP.ContractCode=@ContractCode)
                AND (@EmployeeId IS NULL OR @EmployeeId =0 OR EMP.EmployeeId=@EmployeeId)
                AND (@StateStatus IS NULL OR @StateStatus='' OR CEMP.StateStatus=@StateStatus)
                AND (@ContractId IS NULL OR @ContractId='' OR CEMP.ContractId=@ContractId)
                AND (@EmployeeCode IS NULL OR @EmployeeCode='' OR EMP.EmployeeCode LIKE '%'+@EmployeeCode+'%')
                AND CEMP.CompanyId=@CompanyId
                AND CEMP.OrganizationId=@OrganizationId),
	            Count_CTE AS (
	            SELECT COUNT(*) AS [TotalRows]
	            FROM Data_CTE)

	            SELECT JSONData=(Select * From (SELECT *
	            FROM Data_CTE
	            ORDER BY 
	            CASE WHEN ISNULL(@SortingCol,'') = '' THEN Data_CTE.EmployeeCode END,
	            CASE WHEN @SortingCol = 'EmployeeCode' AND @SortType ='ASC' THEN EmployeeCode END ASC,
	            CASE WHEN @SortingCol = 'EmployeeCode' AND @SortType ='DESC' THEN EmployeeCode END DESC,
	            CASE WHEN @SortingCol = 'EmployeeName' AND @SortType ='ASC' THEN EmployeeName END ,
	            CASE WHEN @SortingCol = 'EmployeeName' AND @SortType ='DESC' THEN EmployeeName END DESC
	            OFFSET (@PageNumber-1)*@PageSize ROWS
	            FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber
                END";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, query, parameters, CommandType.Text);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<ContractualEmploymentViewModel>>(response.JSONData) ?? new List<ContractualEmploymentViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ContractualEmploymentBusiness", "GetContractualEmploymentsInfoAsync", user);
            }
            return data;
        }
        public async Task<ContractualEmploymentViewModel> GetEmployeeLastContractInfo(long employeeId, AppUser user)
        {
            ContractualEmploymentViewModel item = new ContractualEmploymentViewModel();
            try
            {
                var query = $@" SELECT ContractId,ContractStartDate,ContractEndDate FROM HR_ContractualEmployment 
 Where  ContractStartDate=(SELECT ContractStartDate=MAX(CEMP.ContractStartDate) FROM HR_ContractualEmployment CEMP
Inner Join HR_EmployeeInformation EMP ON CEMP.EmployeeId = EMP.EmployeeId
Where 1=1 AND CEMP.EmployeeId=@EmployeeId AND CEMP.CompanyId =5 AND CEMP.OrganizationId=7
Group By CEMP.EmployeeId)";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                item = await _dapper.SqlQueryFirstAsync<ContractualEmploymentViewModel>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ContractualEmploymentBusiness", "GetEmployeeLastContractInfo", user);
            }
            return item;
        }
        public async Task<ExecutionStatus> SaveRenewContractAysnc(ContractualEmploymentDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var query = "sp_HR_ContractualEmployment_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.ContractId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, query, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "ContractualEmploymentBusiness", "RenewContractAysnc", user);
            }
            return executionStatus;
        }
        public Task<ExecutionStatus> TerminateContractAysnc(AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<ExecutionStatus> UpdateRenewContractAysnc(ContractualEmploymentDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var query = "sp_Payroll_ContractualEmployment_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.ContractId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, query, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "ContractualEmploymentBusiness", "UpdateRenewContractAysnc", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<ExecutionStatus>> UploadEmployeeContractAsync(IEnumerable<ContractualEmploymentDTO> models, AppUser user)
        {
            IEnumerable<ExecutionStatus> executionStatus = new List<ExecutionStatus>();
            try
            {
                foreach (var item in models)
                {
                    ExecutionStatus execution = new ExecutionStatus();
                    var employeeInDb = await _infoBusiness.GetEmployeeOfficeInfoByIdAsync(new EmployeeOfficeInfo_Filter()
                    {
                        EmployeeCode = item.EmployeeCode
                    }, user);
                    if (employeeInDb != null)
                    {
                        if (employeeInDb.JobType == "Contractual")
                        {
                            item.EmployeeId = employeeInDb.EmployeeId;
                            var lastContractInfo = await this.GetEmployeeLastContractInfo(item.EmployeeId, user);
                            item.LastContractId = lastContractInfo.ContractId;
                            if (item.LastContractEndDate == null)
                            {
                                item.LastContractEndDate = item.ContractStartDate.Value.AddDays(-1);
                            }
                            execution = await SaveRenewContractAysnc(item, user);
                        }
                        else
                        {
                            execution.Status = false;
                            execution.Msg = employeeInDb.EmployeeCode + " - is not Contractual employee";
                        }
                    }
                    else
                    {
                        execution.Status = false;
                        execution.Msg = "No employe with the id " + item.EmployeeCode;
                    }
                    executionStatus.AsList().Add(execution);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ContractualEmploymentBusiness", "UploadEmployeeContractAsync", user);
            }
            return executionStatus;
        }
        public async Task<ContractualEmployment> GetContractualEmploymentById(long id, AppUser user)
        {
            ContractualEmployment item = null;
            try
            {
                var query = $@"Select * From HR_ContractualEmployment Where ContractId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                var parameters = new DynamicParameters();
                parameters.Add("ContractId", id);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                item = await _dapper.SqlQueryFirstAsync<ContractualEmployment>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ContractualEmploymentBusiness", "GetContractualEmploymentById", user);
            }
            return item;
        }
    }
}
