using Shared.OtherModels.User;
using System;
using System.Threading.Tasks;

namespace DAL.Logger.Interface
{
    public interface IDALSysLogger
    {
        Task SaveAccessActivity(string username, string ip, string deviceModel, string devicename, string deviceType, bool? isMobile, bool? isTablet, bool? isDesktop, string deviceOS, string deviceOsVersion, string browser, string browserVersion, string browserMajorVersion);
        Task SaveUserActivity(string targetedTable, string db, string previousDataInJsonFormat, string presentDataInJsonFormat, string primaryKey, string actionMethod, string action, string userId, long organizationId, long companyId, long branchId);
        Task SaveUserActivity(AppUser user, string targetedTable, string db, string previousDataInJsonFormat, string presentDataInJsonFormat, string primaryKey, string actionMethod, string actionName, long employeeId=0);
        Task SaveSystemException(Exception ex, string db, string businessClassName, string methodName, string username, long organizationId, long companyId, long? branchId);
        Task SaveControlPanelException(Exception ex, string db, string businessClassName, string methodName, string username, long organizationId, long companyId, long? branchId);
        Task SaveHRMSException(Exception ex, string db, string businessClassName, string methodName, AppUser user);
        Task SavePayrollException(Exception ex, string db, string businessClassName, string methodName, AppUser user);
        Task SaveControlPanelException(Exception ex, string db, string businessClassName, string methodName, AppUser user);
    }
}
