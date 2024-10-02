
using Shared.OtherModels.User;
using BLL.Salary.Salary.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.DTO.Salary;

namespace BLL.Salary.Salary.Implementation
{
    public class SalaryComponentHistoriesBusiness : ISalaryComponentHistoriesBusiness
    {
        private readonly IDapperData _dapper;
        public SalaryComponentHistoriesBusiness(IDapperData dapper)
        {
            _dapper = dapper;
        }
        public async Task<IEnumerable<SalaryComponentHistoryIdDTO>> GetComponentHistoryIds(long salaryProcessId, AppUser user)
        {
            IEnumerable<SalaryComponentHistoryIdDTO> list = new List<SalaryComponentHistoryIdDTO>();
            try
            {
                var query = $@"SELECT EmployeeId,Flag,ComponentId FROM Payroll_SalaryComponentHistory Where SalaryProcessId=@SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                list = await _dapper.SqlQueryListAsync<SalaryComponentHistoryIdDTO>(user.Database, query, new
                {
                    SalaryProcessId = salaryProcessId,
                    user.CompanyId,
                    user.OrganizationId
                });
            }
            catch (Exception ex)
            {
            }
            return list;
        }
    }
}
