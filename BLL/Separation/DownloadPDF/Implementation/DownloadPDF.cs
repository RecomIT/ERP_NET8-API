using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.Services;
using System.Threading.Tasks;
using System;
using System.IO;
using DAL.DapperObject.Interface;
using BLL.Separation.DownloadPDF.Interface;

namespace BLL.Separation.DownloadPDF.Implementation
{
    public class DownloadPDF : IDownloadPDF
    {
        private readonly IDapperData _dapper;
        private ISysLogger _sysLogger;

        public DownloadPDF(
            IDapperData dapper,
            ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }


        public static string PhysicalDriver = ConfigurationHelper.config.GetSection("PhysicalDriver").Value.ToString();

        public async Task<string> DownloadResignationLetterAsync(dynamic filter, AppUser user)
        {
            try
            {
                var driverPath = PhysicalDriver + "/";
                var fullFilePath = Path.Combine(driverPath, filter.FilePath, filter.FileName);

                if (File.Exists(fullFilePath))
                {
                    return fullFilePath;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeResignationBusiness", "DownloadResignationLetterFilePathAsync", user);
                return null;
            }
        }
    }
}
