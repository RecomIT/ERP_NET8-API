using BLL.Base.Interface;
using BLL.Separation.Interface;
using DAL.DapperObject.Interface;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;


namespace BLL.Separation.Implementation
{
    public class EmployeeResignationBusiness : IEmployeeResignationBusiness
    {

        private readonly IDapperData _dapper;
        private ISysLogger _sysLogger;

        public EmployeeResignationBusiness(
            IDapperData dapper,
            ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }




        public async Task<object> GetEmployeesAsync(AppUser user)
        {
            try
            {

                var sp_name = "sp_GetSubOrdinateEmployees";


                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeResignationBusiness", "GetEmployeesAsync", user);

                return null;
            }

        }





        public async Task<object> GetEmployeeDetailsAsync(dynamic filter, AppUser user)
        {
            try
            {

                var sp_name = "sp_HR_GetEmployeeDetails";

                if (filter.EmployeeId == null)
                {
                    filter.EmployeeId = user.EmployeeId.ToString();
                }

                var parameters = DapperParam.AddParams(filter, user);

                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeResignationBusiness", "GetEmployeeResignationAsync", user);

                return null;
            }

        }





        public async Task<object> GetEmployeesDetailsAsync(dynamic filter, AppUser user)
        {
            try
            {

                var sp_name = "sp_HR_GetEmployeeDetails";


                var parameters = DapperParam.AddParams(filter, user);

                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeResignationBusiness", "GetEmployeeResignationAsync", user);

                return null;
            }

        }


        public void DeleteFile(string FilePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(FilePath))
                {
                    if (File.Exists(FilePath))
                    {
                        // Log the file path before deletion
                        Console.WriteLine($"Deleting file: {FilePath}");

                        File.Delete(FilePath);

                        // Log a message after successful deletion
                        Console.WriteLine($"File deleted successfully: {FilePath}");
                    }
                    else
                    {
                        // Log a message if the file doesn't exist
                        Console.WriteLine($"File does not exist: {FilePath}");
                    }
                }
                else
                {
                    // Log a message if FilePath is null or empty
                    Console.WriteLine("File path is null or empty.");
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during file deletion
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
        }


        public async Task<object> SaveEmployeeResignationAsync(dynamic filter, AppUser user)
        {
            try
            {

                string file;
                string filePath = null;
                string fileName = null;
                string extenstion = null;
                string fileSize = null;
                string actualFileName = null;

                if (filter.ResignationRequestId > 0 && filter.File == null)
                {

                }
                else if (filter.ResignationRequestId > 0 && filter.File != null)
                {

                    DeleteFile(string.Format(@"{0}/{1}/{2}", Utility.PhysicalDriver, filter.ExistsFilePath, filter.ExistsFileName));



                    file = await Utility.SaveFileAsync(filter.File, string.Format(@"{0}", user.OrgCode));
                    filePath = file.Substring(0, file.LastIndexOf("/"));
                    fileName = file.Substring(file.LastIndexOf("/") + 1);
                    extenstion = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    fileSize = Math.Round(Convert.ToDecimal(filter.File.Length / 1024), 0, MidpointRounding.AwayFromZero).ToString() + "KB";
                    actualFileName = filter.File.FileName;
                }


                else
                {
                    //file = await Utility.SaveFileAsync(filter.File, string.Format(@"{0}", user.OrgCode));
                    //filePath = file.Substring(0, file.LastIndexOf("/"));
                    //fileName = file.Substring(file.LastIndexOf("/") + 1);
                    //extenstion = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    //fileSize = Math.Round(Convert.ToDecimal((filter.File.Length / 1024)), 0, MidpointRounding.AwayFromZero).ToString() + "KB";
                    //actualFileName = filter.File.FileName;
                }



                if (filter.FileSize == null)
                {
                    filter.FileSize = fileSize;
                }
                if (filter.FilePath == null)
                {
                    filter.FilePath = filePath;
                }
                if (filter.FileType == null)
                {
                    filter.FileType = extenstion;
                }
                if (filter.FileName == null)
                {
                    filter.FileName = fileName;
                }
                if (filter.ActualFileName == null)
                {
                    filter.ActualFileName = actualFileName;
                }



                var sp_name = "sp_HR_SaveEmployeeResignation";


                var parameters = DapperParam.AddParams(filter, user);
                //parameters.Add("ExecutionFlag", Data.Insert);

                var data = await _dapper.SqlQueryFirstAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeResignationBusiness", "GetEmployeeResignationAsync", user);

                return null;
            }

        }







        public async Task<object> GetEmployeeResignationListAsync(dynamic filter, AppUser user)
        {
            try
            {

                var sp_name = "sp_HR_GetEmployeeResignationList";

                if (filter.EmployeeId == null)
                {
                    filter.EmployeeId = user.EmployeeId.ToString();
                }

                var parameters = DapperParam.AddParams(filter, user);
                parameters.Add("RoleName", user.RoleName);


                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeResignationBusiness", "GetEmployeeResignationListAsync", user);

                return null;
            }
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





        public async Task<object> CancelEmployeeResignationAsync(dynamic filter, AppUser user)
        {
            try
            {

                var sp_name = "sp_HR_SaveEmployeeResignation";

                var parameters = DapperParam.AddParams(filter, user);
                parameters.Add("ExecutionFlag", Data.Delete);

                var data = await _dapper.SqlQueryFirstAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeResignationBusiness", "GetEmployeeResignationAsync", user);

                return null;
            }

        }






        // ---------------------------------------- User

        public async Task<object> GetUserResignationListAsync(AppUser user)
        {
            try
            {

                var sp_name = "sp_HR_GetUserResignationList";

                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeResignationBusiness", "GetUserResignationListAsync", user);

                return null;
            }
        }



        // ---------------------------------------- Supervisor
        public async Task<object> GetEmployeeResignationListForSupervisorAsync(dynamic filter, AppUser user)
        {
            try
            {

                var sp_name = "sp_HR_GetEmployeeResignationListForSupervisor";

                var parameters = DapperParam.AddParams(filter, user);

                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeResignationBusiness", "GetEmployeeResignationListForSupervisorAsync", user);

                return null;
            }
        }












        public async Task<object> ApproveEmployeeResignationAsync(dynamic filter, AppUser user)
        {
            try
            {

                var sp_name = "sp_HR_SaveEmployeeResignation";

                var parameters = DapperParam.AddParams(filter, user);
                parameters.Add("ExecutionFlag", Data.Approve);

                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeResignationBusiness", "ApproveEmployeeResignationAsync", user);

                return null;
            }

        }





    }
}
