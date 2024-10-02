using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.Services;
using DAL.DapperObject.Interface;
using Shared.Payroll.Process.Tax;
using Shared.Payroll.Domain.Setup;
using BLL.Tax.Interface;

namespace BLL.Tax.Implementation
{
    public class EmployeeFreeCarBusiness : IEmployeeFreeCarBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapperData;

        public EmployeeFreeCarBusiness(ISysLogger sysLogger, IDapperData dapperData)
        {
            _sysLogger = sysLogger;
            _dapperData = dapperData;
        }

        public async Task<decimal> CCTillAmountInTaxProcessAsync(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, int year, int month, AppUser user)
        {
            decimal amount = 0;
            try
            {
                var query = $@"SELECT ISNULL((Select SUM(CurrentMonthIncome) From Payroll_EmployeeTaxProcessDetail Where TaxItem='FREE CAR' AND FiscalYearId=@FiscalYearId AND SalaryMonth<>@Month AND EmployeeId=@EmployeeId),0)";
                amount = await _dapperData.SqlQueryFirstAsync<decimal>(user.Database, query, new { fiscalYear.FiscalYearId, Month = month, employee.EmployeeId });

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeFreeCarBusiness", "CCTillAmountInTaxProcessAsync", user);
            }
            return amount;
        }

        public async Task<int> GetEmployeeFreeCarByEmployeeIdInTaxProcessAsync(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, int year, int month, AppUser user)
        {
            int cc = 0;
            try
            {
                var query = $@"SELECT ISNULL((SELECT CCOfCar FROM Payroll_EmployeeFreeCar 
			    Where FiscalYearId=@FiscalYearId AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId 
			    AND OrganizationId=@OrganizationId AND @Date BETWEEN StartDate AND EndDate),0)";

                cc = await _dapperData.SqlQueryFirstAsync<int>(user.Database, query.Trim(), new { fiscalYear.FiscalYearId, employee.EmployeeId, DateTimeExtension.LastDateOfAMonth(year, month).Date, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeFreeCarBusiness", "GetEmployeeFreeCarByEmployeeIdInTaxProcessAsync", user);
            }
            return cc;
        }
    }
}
