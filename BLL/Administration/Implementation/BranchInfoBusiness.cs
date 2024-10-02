using DAL.DapperObject;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using BLL.Administration.Interface;
using Shared.Services;
using DAL.DapperObject.Interface;
using Shared.Control_Panel.Domain;

namespace BLL.Administration.Implementation
{
    public class BranchInfoBusiness : IBranchInfoBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public BranchInfoBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<Branch> GetBranchByIdAsync(long id, AppUser user)
        {
            Branch branch = null;
            try {
                var query = "SELECT * FROM tblBranches Where BranchId=@BranchId";
                branch = await _dapper.SqlQueryFirstAsync<Branch>(Database.ControlPanel, query, new {
                    BranchId = id
                });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "BranchInfoBusiness", "GetBranchByIdAsync", user);
            }
            return branch;
        }

        public async Task<IEnumerable<Branch>> GetBranchsAsync(string branchName, AppUser user)
        {
            IEnumerable<Branch> list = new List<Branch>();
            try {
                var query = $@"SELECT * From tblBranches Where 1=1
                AND (@BranchName IS NULL OR @BranchName='' OR BranchName=@BranchName)
                AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                var parameters = new { BranchName = branchName, CompanyId = user.CompanyId, OrganizationId = user.OrganizationId };
                list = await _dapper.SqlQueryListAsync<Branch>(Database.ControlPanel, query, parameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "BranchInfoBusiness", "GetBranchsAsync", user);
            }
            return list;
        }

        public async Task<byte[]> GetLogo(long id, AppUser user)
        {
            byte[] logo = null;
            try {
                var branch = await this.GetBranchByIdAsync(id, user);
                if (branch != null && Utility.IsNullEmptyOrWhiteSpace(branch.ReportLogoPath) == false) {
                    logo = Utility.GetFileBytes(Utility.PhysicalDriver + "/" + branch.ReportLogoPath);
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "BranchInfoBusiness", "GetLogo", user);
            }
            return logo;
        }
    }
}
