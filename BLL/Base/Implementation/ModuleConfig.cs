using System;
using DAL.DapperObject;
using BLL.Base.Interface;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using Dapper;
using System.Data;
using DAL.DapperObject.Interface;
using Shared.Control_Panel.Domain;

namespace BLL.Base.Implementation
{
    public class ModuleConfig : IModuleConfig
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public ModuleConfig(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<HRModuleConfig> HRModuleConfig(AppUser user)
        {
            HRModuleConfig config = null;
            try {
                var query = $@"SELECT * FROM tblHRModuleConfig Where CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                config = await _dapper.SqlQueryFirstAsync<HRModuleConfig>(Database.ControlPanel, query, parameters, CommandType.Text);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "ModuleConfig", "HRModuleConfig", user);
            }
            return config;
        }
        public async Task<PayrollModuleConfig> PayrollModuleConfig(AppUser user)
        {
            PayrollModuleConfig config = null;
            try {
                var query = $@"SELECT * FROM tblPayrollModuleConfig Where CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                config = await _dapper.SqlQueryFirstAsync<PayrollModuleConfig>(Database.ControlPanel, query, parameters, CommandType.Text);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "ModuleConfig", "PayrollModuleConfig", user);
            }
            return config;
        }
        public async Task<PFModuleConfig> PFModuleConfig(AppUser user)
        {
            PFModuleConfig config = null;
            try {
                var query = $@"SELECT * FROM tblPFModuleConfig Where CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                config = await _dapper.SqlQueryFirstAsync<PFModuleConfig>(Database.ControlPanel, query, parameters, CommandType.Text);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "ModuleConfig", "PFModuleConfig", user);
            }
            return config;
        }
    }
}
