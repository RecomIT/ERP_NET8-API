using System;
using BLL.Base.Interface;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using Shared.OtherModels.Report;
using DAL.DapperObject;
using Dapper;
using System.Data;
using DAL.DapperObject.Interface;
using Shared.Services;

namespace BLL.Base.Implementation
{
    public class ReportBase : IReportBase
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public ReportBase(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        /// <summary>
        /// branch id of the reporting employee
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ReportLayer> ReportLayerAsync(long branchId,AppUser user)
        {
            ReportLayer reportLayer = new ReportLayer();
            try {
                var sp_name = "sp_ReportLayer";
                var parameters = new DynamicParameters();
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("BranchId", branchId);
                parameters.Add("DivisionId", 0);
                reportLayer = await _dapper.SqlQueryFirstAsync<ReportLayer>(Database.ControlPanel, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportLayerAsync", "ReportBase", user);
            }
            return reportLayer;
        }

        public async Task<ReportLayer> ReportLayerAsync(long organizationId, long companyId, long branchId, long divisionId)
        {
            ReportLayer reportLayer = new ReportLayer();
            try {
                var sp_name = "sp_ReportLayer";
                var parameters = new DynamicParameters();
                parameters.Add("OrganizationId", organizationId);
                parameters.Add("CompanyId", companyId);
                parameters.Add("BranchId", branchId);
                parameters.Add("DivisionId", divisionId);

                reportLayer = await _dapper.SqlQueryFirstAsync<ReportLayer>(Database.ControlPanel, sp_name, parameters, CommandType.StoredProcedure);

                if (reportLayer != null)
                {
                    reportLayer.CompanyPic = Utility.IsNullEmptyOrWhiteSpace(reportLayer.CompanyLogoPath) == false ?
                       Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : reportLayer.CompanyPic;
                    reportLayer.ReportLogo = Utility.IsNullEmptyOrWhiteSpace(reportLayer.ReportLogoPath) == false ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : reportLayer.ReportLogo;
                    reportLayer.BranchLogo = Utility.IsNullEmptyOrWhiteSpace(reportLayer.BranchLogoPath) == false ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.BranchLogoPath) : reportLayer.BranchLogo;
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "ReportBusiness", "ReportLayerAsync", "", organizationId, companyId, branchId);
            }
            return reportLayer;
        }
    }
}
