using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using BLL.Salary.Payment.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.Filter.Payment;

namespace BLL.Salary.Payment.Implementation
{
    public class ServiceAnniversaryAllowanceBusiness : IServiceAnniversaryAllowanceBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public ServiceAnniversaryAllowanceBusiness(ISysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }

        public async Task<IEnumerable<ServiceAnniversaryAllowance>> GetEmployeeServiceAnniversaryAllowancesAsync(EligibilityInServiceAnniversary_Filter filter, int month, int year, DateTime dateOfJoining, AppUser user)
        {
            List<ServiceAnniversaryAllowance> list = new List<ServiceAnniversaryAllowance>();
            try
            {
                var query = $@"SELECT Config.* FROM [dbo].[Payroll_ServiceAnniversaryAllowance] Config
INNER JOIN Payroll_AllowanceName ALW ON Config.AllowanceNameId = ALW.AllowanceNameId
WHERE 1=1
AND (Config.Gender IS NULL OR Config.Gender ='' OR Config.Gender=@Gender)
AND (Config.JobType IS NULL OR Config.JobType ='' OR Config.JobType=@JobType)
AND (Config.Religion IS NULL OR Config.Religion ='' OR Config.Religion=@Religion)
AND (Config.MaritalStatus IS NULL OR Config.MaritalStatus ='' OR Config.MaritalStatus=@MaritalStatus)
AND (Config.PhysicalCondition IS NULL OR Config.PhysicalCondition='' OR Config.PhysicalCondition=@PhysicalCondition)
AND (@PaymentDate >= Config.ActivationFrom AND (Config.ActivationTo IS NULL OR @PaymentDate BETWEEN Config.ActivationFrom AND Config.ActivationTo))
AND (Config.StateStatus='Approved')
AND (Config.CompanyId=@CompanyId)
AND (Config.OrganizationId=@OrganizationId)";

                var parameters = DapperParam.AddParams(filter, user, addUserId: false);
                var items = await _dapper.SqlQueryListAsync<ServiceAnniversaryAllowance>(user.Database, query, parameters, CommandType.Text);

                if (user.OrganizationId == 11 && user.CompanyId == 19)
                {
                    foreach (var item in items)
                    {
                        DateTime fromDate = new DateTime();
                        DateTime toDate = new DateTime();
                        DateTime employeeEligibleDate = new DateTime(year, dateOfJoining.Month, dateOfJoining.Day);
                        if (item.CutOffDay > 0)
                        {
                            toDate = new DateTime(month == 12 ? year + 1 : year, month == 12 ? 1 : month + 1, item.CutOffDay.Value);
                            fromDate = new DateTime(year, month, item.CutOffDay.Value + 1);
                            if (employeeEligibleDate.IsDateBetweenTwoDates(fromDate, toDate))
                            {
                                list.Add(item);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in items)
                    {
                        DateTime fromDate = new DateTime();
                        DateTime toDate = new DateTime();
                        DateTime employeeEligibleDate = new DateTime(year, dateOfJoining.Month, dateOfJoining.Day);
                        if (item.CutOffDay > 0)
                        {
                            toDate = new DateTime(year, month, item.CutOffDay.Value);
                            fromDate = toDate.AddMonths(-1).AddDays(1);
                            if (employeeEligibleDate.IsDateBetweenTwoDates(fromDate, toDate))
                            {
                                list.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ServiceAnniversaryAllowanceBusiness", "GetEmployeeServiceAnniversaryAllowanceAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<ServiceAnniversaryAllowanceViewModel>> GetServiceAnniversaryAllowancesAsync(ServiceAnniversaryAllowance_Filter filter, AppUser user)
        {
            IEnumerable<ServiceAnniversaryAllowanceViewModel> list = new List<ServiceAnniversaryAllowanceViewModel>();
            try
            {
                var query = $@"SELECT Config.*,[AllowanceName]=ALW.[Name] FROM [dbo].[Payroll_ServiceAnniversaryAllowance] Config
                INNER JOIN Payroll_AllowanceName ALW ON Config.AllowanceNameId = ALW.AllowanceNameId 
                Where Config.CompanyId=@CompanyId AND Config.OrganizationId=@OrganizationId";
                list = await _dapper.SqlQueryListAsync<ServiceAnniversaryAllowanceViewModel>(user.Database, query, new { user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ServiceAnniversaryAllowanceBusiness", "GetServiceAnniversaryAllowanceAsync", user);
            }
            return list;
        }
    }
}
