using System.Threading.Tasks;
using Shared.OtherModels.User;
using Shared.OtherModels.Report;

namespace BLL.Base.Interface
{
    public interface IReportBase
    {
        Task<ReportLayer> ReportLayerAsync(long branchId,AppUser user);
        Task<ReportLayer> ReportLayerAsync(long organizationId, long companyId, long branchId, long divisionId);
    }
}
