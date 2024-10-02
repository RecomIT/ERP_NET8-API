using BLL.Base.Interface;
using BLL.Employee.Interface.Account;
using DAL.Context.Employee;
using DAL.DapperObject;
using DAL.DapperObject.Interface;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Shared.Employee.Domain.Account;
using Shared.Employee.DTO.Account;
using Shared.Employee.Filter.Account;
using Shared.Employee.ViewModel.Account;
using Shared.OtherModels.Pagination;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;

namespace BLL.Employee.Implementation.Account
{
    public class AccountInfoBusiness : IAccountInfoBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly EmployeeModuleDbContext _employeeModuleDbContext;

        public AccountInfoBusiness(IDapperData dapper, ISysLogger sysLogger, EmployeeModuleDbContext employeeModuleDbContext)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _employeeModuleDbContext = employeeModuleDbContext;
        }
        public async Task<ExecutionStatus> EmployeeAccountInfoValidatorAsync(EmployeeAccountInfoDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeAccountInfo";
                var parameters = DapperParam.AddParams(model, user, new string[] { "Year", "Month", "IsActive", "IsApproved", "EffectiveFrom", "DeactivationFrom" });
                parameters.Add("Flag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "AccountInfoBusiness", "EmployeeAccountInfoValidatorAsync", user);
            }
            return executionStatus;
        }

        public async Task<DBResponse<EmployeeAccountInfoViewModel>> GetEmployeeAccountInfosAsync(EmployeeAccount_Filter filter, AppUser user)
        {
            DBResponse<EmployeeAccountInfoViewModel> data = new DBResponse<EmployeeAccountInfoViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sp_name = $@"WITH Data_CTE AS(Select Acc.AccountInfoId,Acc.EmployeeId,Emp.FullName 'EmployeeName',Acc.PaymentMode,Acc.AccountNo,
			Acc.BankId,(Select BankName From HR_Banks Where BankId=ISNULL(Acc.BankId,0)) 'BankName',
			Acc.BankBranchId,(Select BankBranchName From HR_BankBranches Where BankBranchId=ISNULL(Acc.BankBranchId,0)) 'BankBranchName',
			Acc.AgentName,Emp.GradeId,(Select GradeName From HR_Grades Where GradeId = Acc.GradeId) as 'GradeName',
			Emp.DesignationId,(Select DesignationName From HR_Designations Where DesignationId = Acc.DesignationId) as 'DesignationName',
			Emp.DepartmentId,(Select DepartmentName From HR_Departments Where DepartmentId = Acc.DepartmentId) as 'DepartmentName',
			Acc.EffectiveFrom,Acc.DeactivationFrom,Acc.StateStatus,Acc.CreatedBy,Acc.CreatedDate,Acc.UpdatedBy,
			Acc.UpdatedDate,Acc.ApprovedBy,Acc.ApprovedDate,Acc.CheckedBy,Acc.CheckedDate,Acc.IsActive,Acc.Remarks,Acc.[Year],Acc.[Month],Acc.ActivationReason
			From HR_EmployeeAccountInfo Acc
			INNER JOIN HR_EmployeeInformation Emp on Acc.EmployeeId = Emp.EmployeeId
			LEFT JOIN HR_Banks b on Acc.BankId = b.BankId
			LEFT JOIN HR_BankBranches bb on Acc.BankBranchId = bb.BankBranchId
			Where 1=1
			AND (@AccountInfoId IS NULL OR @AccountInfoId=0 OR Acc.AccountInfoId=@AccountInfoId)
			AND (@EmployeeId IS NULL OR @EmployeeId =0 OR Acc.EmployeeId=@EmployeeId)
			AND (@PaymentMode IS NULL OR @PaymentMode ='' OR Acc.PaymentMode=@PaymentMode)
			AND (@BankId IS NULL OR @BankId =0 OR Acc.BankId=@BankId)
			AND (@BankBranchId IS NULL OR @BankBranchId =0 OR Acc.BankBranchId=@BankBranchId)
			AND (@AgentName IS NULL OR @AgentName ='' OR Acc.AgentName=@AgentName)
			AND (@AccountNo IS NULL OR @AccountNo='' OR Acc.AccountNo Like '%'+@AccountNo+'%')
			AND (@IsActive IS NULL OR @IsActive ='' OR Acc.IsActive=@IsActive)
			AND (@StateStatus IS NULL OR @StateStatus ='' OR Acc.StateStatus=@StateStatus)
			AND (Acc.CompanyId=@CompanyId)
			AND (Acc.OrganizationId=@OrganizationId)),
			Count_CTE AS (
			SELECT COUNT(*) AS [TotalRows]
			FROM Data_CTE)

			SELECT JSONData=(Select * From (SELECT *
			FROM Data_CTE
			ORDER BY Data_CTE.EmployeeId
			OFFSET (@PageNumber-1)*@PageSize ROWS
			FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.Text);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<EmployeeAccountInfoViewModel>>(response.JSONData) ?? new List<EmployeeAccountInfoViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CountryBusiness", "GetCountriessAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveEmployeeAccountInfoAsync(EmployeeAccountInfoDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeAccountInfo";
                var parameters = DapperParam.AddParams(model, user, new string[] { "Year", "Month", "IsActive", "IsApproved", "EffectiveFrom", "DeactivationFrom" });
                parameters.Add("Flag", model.AccountInfoId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "AccountInfoBusiness", "EmployeeAccountInfoValidatorAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveApprovalOfEmployeeAccountAsync(AccountInfoStatusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeAccountInfo";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("Flag", Data.Checking);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "AccountInfoBusiness", "SaveApprovalOfEmployeeAccount", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UploadAccountInfoAsync(List<EmployeeAccountInfoViewModel> models, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_HR_EmployeeAccountInfo";
                var jsonData = Utility.JsonData(models);
                var paramaters = new DynamicParameters();
                paramaters.Add("JsonData", jsonData);
                paramaters.Add("UserId", user.UserId);
                paramaters.Add("CompanyId", user.CompanyId);
                paramaters.Add("OrganizationId", user.OrganizationId);
                paramaters.Add("BranchId", user.BranchId);
                paramaters.Add("Flag", "Upload_AccountInfo");
                if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                {
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, paramaters, commandType: CommandType.StoredProcedure);
                    return executionStatus;
                }
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "EmployeeBusiness", "UploadAccountInfoAsync", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return executionStatus;
        }
        public async Task<EmployeeAccountInfoViewModel> GetAccountActivationInfoBeforeDate(long employeeId, string before_date, AppUser user)
        {
            EmployeeAccountInfoViewModel employeeAccountInfoViewModel = new EmployeeAccountInfoViewModel();
            try
            {
                var sqlQuery = string.Format(@"SELECT TOP 1 * FROM HR_EmployeeAccountInfo 
                Where EmployeeId=@EmployeeId AND CAST(EffectiveFrom AS DATE) < CAST(@BeforeDate AS DATE)
                Order By AccountInfoId desc");
                var paramaters = new DynamicParameters();
                paramaters.Add("EmployeeId", employeeId);
                paramaters.Add("BeforeDate", before_date);
                employeeAccountInfoViewModel = await _dapper.SqlQueryFirstAsync<EmployeeAccountInfoViewModel>(user.Database, sqlQuery, paramaters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {


            }
            return employeeAccountInfoViewModel;
        }
        public async Task<EmployeeAccountInfo> GetActiveAccountInfoByEmployeeId(long employeeId, AppUser user)
        {
            EmployeeAccountInfo employeeAccountInfo = null;
            try
            {
                employeeAccountInfo = await _employeeModuleDbContext.HR_EmployeeAccountInfo.Where(i => i.EmployeeId == employeeId && i.IsActive == true).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return employeeAccountInfo;
        }

        public async Task<EmployeeAccountInfo> GetPendingAccountInfoByEmployeeId(long employeeId, AppUser user)
        {
            EmployeeAccountInfo employeeAccountInfo = null;
            try
            {
                employeeAccountInfo = await _employeeModuleDbContext.HR_EmployeeAccountInfo.Where(i => i.EmployeeId == employeeId && i.IsActive == false && i.StateStatus == StateStatus.Pending).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return employeeAccountInfo;
        }

        public async Task<bool> IsAccountNumberAlreadyActive(string accountNumber, AppUser user)
        {
            bool accountNumberAlreadyActive = false;
            try
            {
                accountNumberAlreadyActive= await _employeeModuleDbContext.HR_EmployeeAccountInfo.Where(i =>
                i.AccountNo == accountNumber
                && i.IsActive == true
                && i.CompanyId == user.CompanyId
                && i.OrganizationId == user.OrganizationId).FirstOrDefaultAsync() != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return accountNumberAlreadyActive;
        }
    }
}

