using System;
using Dapper;
using System.Data;
using BLL.Base.Interface;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.DataService;
using DAL.DapperObject.Interface;
using BLL.PF.Interface;

namespace BLL.PF.Implementation
{
    public class PFServiceBusiness : IPFServiceBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public PFServiceBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetPFEmployeesAsync(long? notEmployee, long designationId, long departmentId, long sectionId, long subsectionId, AppUser user)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var sp_name = "sp_HR_EmployeeInfo";
                var parameters = new DynamicParameters();
                parameters.Add("DesignationId", designationId);
                parameters.Add("DepartmentId", departmentId);
                parameters.Add("SectionId", sectionId);
                parameters.Add("SubSectionId", subsectionId);
                parameters.Add("NotEmployee", notEmployee ?? 0);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("Organization", user.OrganizationId);
                parameters.Add("Flag", "Extension_PF_Summary");
                data = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PFServiceBusiness", "GetPFEmployeesAsync", user);
            }
            return data;
        }
    }
}
