using BLL.Base.Interface;
using BLL.Download.Interface;
using DAL.DapperObject.Interface;
using Shared.OtherModels.User;
using Shared.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BLL.Download.Implementation
{
    public class DownloadBusiness : IDownloadBusiness
    {

        private readonly IDapperData _dapper;
        private ISysLogger _sysLogger;

        public DownloadBusiness(
            IDapperData dapper,
            ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }


        public static string PhysicalDriver = ConfigurationHelper.config.GetSection("PhysicalDriver").Value.ToString();



        public async Task<string> DownloadAsync(dynamic filter, AppUser user)
        {
            try {
                var driverPath = PhysicalDriver + "/";
                var fullFilePath = Path.Combine(driverPath, filter.FilePath, filter.FileName);

                if (System.IO.File.Exists(fullFilePath)) {
                    return fullFilePath;
                }
                else {
                    return null;
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DownloadBusiness", "DownloadAsync", user);
                return null;
            }
        }
    }
}
