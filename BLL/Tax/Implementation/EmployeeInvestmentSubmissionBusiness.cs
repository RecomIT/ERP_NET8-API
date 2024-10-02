using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.Payroll.DTO.Tax;
using Shared.OtherModels.User;
using Shared.Payroll.Filter.Tax;
using Shared.Payroll.Domain.Tax;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.Payroll.ViewModel.Tax;
using Shared.OtherModels.Pagination;
using BLL.Tax.Interface;

namespace BLL.Tax.Implementation
{
    public class EmployeeInvestmentSubmissionBusiness : IEmployeeInvestmentSubmissionBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public EmployeeInvestmentSubmissionBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<ExecutionStatus> SaveEmployeeYearlyInvestmentAsync(EmployeeYearlyInvestmentDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_EmployeeYearlyInvestment_Insert_Update_Delete";
                var param = DapperParam.AddParams(model, user);
                param.Add("ExecutionFlag", model.Id > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, param, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SavePayrollException(ex, user.Database, "EmployeeInvestmentSubmissionBusiness", "SaveEmployeeYearlyInvestmentAsync", user);
            }
            return executionStatus;
        }
        public async Task<DBResponse<EmployeeYearlyInvestmentViewModel>> GetEmployeeYearlyInvestmentsAsync(EmployeeYearInvestment_Filter filter, AppUser user)
        {
            DBResponse<EmployeeYearlyInvestmentViewModel> data = new DBResponse<EmployeeYearlyInvestmentViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sqlQuery = $@"WITH Data_CTE AS(
	                        SELECT YI.Id,YI.EmployeeId,YI.FiscalYearId,YI.InvestmentAmount,EMP.EmployeeCode,
	                        EMP.FullName AS EmployeeName, FY.FiscalYearRange,FY.AssesmentYear, YI.CreatedDate, YI.UpdatedDate
                            FROM Payroll_EmployeeYearlyInvestment YI
	                        INNER JOIN HR_EmployeeInformation EMP ON YI.EmployeeId = EMP.EmployeeId
	                        LEFT JOIN Payroll_FiscalYear FY ON YI.FiscalYearId = FY.FiscalYearId
	                        Where 1=1
	                        AND (@Id IS NULL OR @Id =0 OR YI.Id=@Id)
	                        AND (@EmployeeId IS NULL OR @EmployeeId=0 OR YI.EmployeeId =@EmployeeId)
	                        AND (@FiscalYearId IS NULL OR @FiscalYearId=0 OR YI.FiscalYearId =@FiscalYearId)
                            AND YI.CompanyId = @CompanyId  AND YI.OrganizationId = @OrganizationId
	                        ),
	                        Count_CTE AS (
	                        SELECT COUNT(*) AS [TotalRows]
	                        FROM Data_CTE)
	                        SELECT JSONData=(Select * From (SELECT *
	                        FROM Data_CTE
	                        ORDER BY Id
	                        OFFSET (CAST(@PageNumber AS INT)-1)*@PageSize ROWS
	                        FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber";
                var parameters = Utility.DappperParams(filter, user, new string[] { }, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sqlQuery, parameters, CommandType.Text);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<EmployeeYearlyInvestmentViewModel>>(response.JSONData) ?? new List<EmployeeYearlyInvestmentViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "EmployeeInvestmentSubmissionBusiness", "GetEmployeeYearlyInvestmentsAsync", user);
            }
            return data;
        }
        public async Task<decimal?> GetYearlInvestmentAmountInTaxProcessAsync(long employeeId, long fiscalYearId, AppUser user)
        {
            decimal? total = null;
            try
            {
                var query = $@"Select * FROM Payroll_EmployeeYearlyInvestment 
			     Where FiscalYearId=@FiscalYearId AND EmployeeId =@EmployeeId
			     AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                var list = await _dapper.SqlQueryListAsync<EmployeeYearlyInvestment>(user.Database, query, new
                {
                    FiscalYearId = fiscalYearId,
                    EmployeeId = employeeId,
                    user.CompanyId,
                    user.OrganizationId
                }, CommandType.Text);

                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        total = (total ?? 0) + item.InvestmentAmount;
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITBusiness", "GetYearlInvestmentAmountInTaxProcessAsync", user);
            }
            return total;
        }
        public async Task<IEnumerable<EmployeeYearlyInvestmentViewModel>> GetEmployeeYearlyInvestmentByIdAsync(long id, AppUser user)
        {
            IEnumerable<EmployeeYearlyInvestmentViewModel> data = new List<EmployeeYearlyInvestmentViewModel>();
            try
            {
                var sqlQuery = $@"SELECT YI.Id,YI.EmployeeId,YI.FiscalYearId,YI.InvestmentAmount,EMP.EmployeeCode, EMP.FullName AS EmployeeName, FY.FiscalYearRange,FY.AssesmentYear
	                FROM Payroll_EmployeeYearlyInvestment YI
	                INNER JOIN HR_EmployeeInformation EMP ON YI.EmployeeId = EMP.EmployeeId
	                LEFT JOIN Payroll_FiscalYear FY ON YI.FiscalYearId = FY.FiscalYearId
	                Where 1=1 AND YI.Id= " + id + "  AND YI.CompanyId = @CompanyId  AND YI.OrganizationId = @OrganizationId";
                var parameters = DapperParam.AddParams(id, user, new string[] { }, addUserId: false);

                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<EmployeeYearlyInvestmentViewModel>(user.Database, sqlQuery, parameters, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
            }
            return data;
        }
        public async Task<ExecutionStatus> UploadEmployeeYearlyInvestmentExcelAsync(List<EmployeeYearlyInvestmentViewModel> viewModels, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_Payroll_EmployeeYearlyInvestment_Insert_Update_Delete";
                var jsonData = Utility.JsonData(viewModels);
                var paramaters = new DynamicParameters();
                paramaters.Add("JsonData", jsonData);
                paramaters.Add("UserId", user.UserId);
                paramaters.Add("CompanyId", user.CompanyId);
                paramaters.Add("OrganizationId", user.OrganizationId);
                paramaters.Add("UserBranchId", user.BranchId);
                paramaters.Add("ExecutionFlag", "YearlyInvestment_Upload");
                if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                {
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, paramaters, commandType: CommandType.StoredProcedure);
                    return executionStatus;
                }
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
            }
            return executionStatus;
        }
    }
}
