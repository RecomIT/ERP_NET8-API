using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using BLL.Salary.Payment.Interface;
using Shared.Payroll.Domain.Payment;
using Shared.OtherModels.Response;
using Shared.Payroll.ViewModel.Payment;
using Shared.Payroll.Filter.Payment;
using Shared.OtherModels.Pagination;
using System.Data;
using Shared.Services;

namespace BLL.Salary.Payment.Implementation
{
    public class MonthlyAllowanceConfigBusiness : IMonthlyAllowanceConfigBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;

        public MonthlyAllowanceConfigBusiness(ISysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }

        public async Task<IEnumerable<MonthlyAllowanceConfig>> GetMonthlyAllowanceConfigsAsync(long employeeId, string activationdate, AppUser user)
        {
            IEnumerable<MonthlyAllowanceConfig> list = new List<MonthlyAllowanceConfig>();
            try
            {
                var query = $@"SELECT Config.*,[AllowanceName]=ALW.[Name] FROM Payroll_MonthlyAllowanceConfig Config
                INNER JOIN Payroll_AllowanceName ALW ON Config.AllowanceNameId = ALW.AllowanceNameId
                WHERE 1=1
                AND (Config.EmployeeId=@EmployeeId)
                AND (Config.IsActive=1)
                AND (CAST(@ActivationFrom AS DATE) >= Config.ActivationFrom)
                AND (Config.StateStatus='Approved')
                AND (Config.CompanyId=@CompanyId)
                AND (Config.OrganizationId=@OrganizationId)";

                list = await _dapper.SqlQueryListAsync<MonthlyAllowanceConfig>(user.Database, query, new
                {
                    EmployeeId = employeeId,
                    ActivationFrom = activationdate,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyAllowanceConfigBusiness", "GetEmployeeMonthlyAllowanceConfigsAsync", user);
            }
            return list;
        }

        public async Task<DBResponse<MonthlyAllowanceConfigViewModel>> GetMonthlyAllowanceConfigsAsync(MonthlyAllowanceConfig_Filter filter, AppUser user)
        {
            DBResponse<MonthlyAllowanceConfigViewModel> data = new DBResponse<MonthlyAllowanceConfigViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                filter.AllowanceNameId = Utility.TryParseLong(filter.AllowanceNameId).ToString();
                filter.EmployeeId = Utility.TryParseLong(filter.EmployeeId).ToString();
                var sp_name = $@"WITH Data_CTE AS(
                SELECT Config.Id, Config.EmployeeId,EMP.EmployeeCode,[EmployeeName]=EMP.FullName,ALW.AllowanceNameId,[AllowanceName]=ALW.[Name],
                Config.BaseOfPayment,Config.[Percentage],Config.Amount,Config.TotalAmount,Config.StateStatus,Config.IsActive,Config.IsProrated,
                Config.ActivationFrom,Config.ActivationTo,Config.IsVisibleInSalarySheet,Config.Remarks
                FROM Payroll_MonthlyAllowanceConfig Config
                INNER JOIN Payroll_AllowanceName ALW ON Config.AllowanceNameId = ALW.AllowanceNameId
                INNER JOIN HR_EmployeeInformation EMP ON Config.EmployeeId = EMP.EmployeeId
                LEFT JOIN HR_EmployeeDetail DTL ON EMP.EmployeeId = DTL.EmployeeId
                Where 1=1
                AND (@EmployeeId IS NULL OR @EmployeeId = 0 OR Config.EmployeeId=@EmployeeId)
                AND (@AllowanceNameId IS NULL OR @AllowanceNameId = 0 OR Config.AllowanceNameId=@AllowanceNameId)
                AND (@StateStatus IS NULL OR @StateStatus ='' OR Config.StateStatus=@StateStatus)
                AND Config.CompanyId=@CompanyId AND Config.OrganizationId=@OrganizationId
                ),
                Count_CTE AS (
                SELECT COUNT(*) AS [TotalRows]
                FROM Data_CTE)
                SELECT JSONData=(Select * From (SELECT *
                FROM Data_CTE
                ORDER BY AllowanceNameId
                OFFSET (@PageNumber-1)*@PageSize ROWS
                FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),
                TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber";
                var parameters = Utility.DappperParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.Text);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<MonthlyAllowanceConfigViewModel>>(response.JSONData) ?? new List<MonthlyAllowanceConfigViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyAllowanceConfigBusiness", "GetMonthlyAllowanceConfigsAsync", user);
            }
            return data;
        }
    }
}
