using DAL.Logger.Interface;
using Shared.OtherModels.User;
using DAL.Payroll.Repository.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.Domain.Setup;

namespace DAL.Payroll.Repository.Implementation
{
    public class FiscalYearRepository : IFiscalYearRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;
        public FiscalYearRepository(IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<FiscalYear>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<FiscalYear>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<FiscalYear> GetByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<FiscalYear> GetCurrectFiscalYearAsync(AppUser user)
        {
            FiscalYear fiscalYear = null;
            try {
                var query = $@"SELECT FiscalYearId,FiscalYearFrom,FiscalYearTo,FiscalYearRange FROM Payroll_FiscalYear 
                               Where 1=1
	                           AND CAST(GETDATE() AS DATE) BETWEEN  FiscalYearFrom AND FiscalYearTo AND
	                           CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                fiscalYear = await _dapper.SqlQueryFirstAsync<FiscalYear>(user.Database, query, new { CompanyId = user.CompanyId, OrganizationId = user.OrganizationId });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepositAllowancePaymentHistoryRepository", "GetCurrectFiscalYearAsync", user);
            }
            return fiscalYear;
        }
        public async Task<FiscalYear> GetFiscalYearByDateAsync(string date, AppUser user)
        {
            FiscalYear fiscalYear = null;
            try {
                var query = $@"SELECT FiscalYearId,FiscalYearFrom,FiscalYearTo,FiscalYearRange FROM Payroll_FiscalYear 
                               Where 1=1
	                           AND @Date BETWEEN  FiscalYearFrom AND FiscalYearTo AND
	                           CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                fiscalYear = await _dapper.SqlQueryFirstAsync<FiscalYear>(user.Database, query, new {
                    Date = date,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepositAllowancePaymentHistoryRepository", "GetCurrectFiscalYearAsync", user);
            }
            return fiscalYear;
        }
        public async Task<FiscalYear> GetFiscalYearByYearAsync(int year, AppUser user)
        {
            FiscalYear fiscalYear = null;
            try {
                var current_date = DateTime.Now;
                var july = new DateTime(DateTime.Now.Year, 7, 1);
                DateTime? fiscalYearStart = null;
                DateTime? fiscalYearEnd = null;

                if (year > current_date.Year) {
                    fiscalYearStart = july;
                    fiscalYearEnd = new DateTime(year, 6, 30);
                }
                else {
                    if (current_date.Date >= july) {
                        fiscalYearStart = july;
                        fiscalYearEnd = new DateTime(year + 1, 6, 30);
                    }
                    else {
                        fiscalYearStart = new DateTime(current_date.Date.Year - 1, 7, 01);
                        fiscalYearEnd = new DateTime(current_date.Date.Year, 6, 30);
                    }
                }
                fiscalYear = await this.GetFiscalYearByDateAsync(fiscalYearStart.Value.ToString("yyyy-MM-dd"), user);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "FiscalYearRepository", "GetFiscalYearByYearAsync", user);

            }
            return fiscalYear;
        }
    }
}
