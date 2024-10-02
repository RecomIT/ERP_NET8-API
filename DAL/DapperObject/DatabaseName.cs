using Shared.Services;
using Environment = Shared.Services.Environment;

namespace DAL.DapperObject
{
    public static class DatabaseName
    {
        static string AppEnvironment = AppSettings.App_environment;


        // For Migration
        // ............................ Starting
        // ...............................................................


        public static string ControlPanel { get { return "ControlPanel"; } }
        public static string HRMS { get { return "HRMS"; } }
        public static string Payroll { get { return "HRMS"; } }
        public static string Wunderman_PF { get { return "Wunderman_PF"; } }


        // For Migration
        // ............................ Ending
        // ...............................................................



        public static string GetConnectionString(string dbName)
        {
            if (dbName != null)
            {
                if (dbName == "ControlPanel")
                {
                    return MakeConnectionString(dbName);
                }
                else
                {
                    return MakeConnectionString(dbName);
                }
            }
            else
            {
                throw new System.Exception("Database name is empty");
            }
        }



        private static string MakeConnectionString(string dbName)
        {
            string fullConString = string.Empty;

            if (AppEnvironment == Environment.Local)
            {

                fullConString = string.Format(@ConfigurationHelper.config.GetSection("ConnectionStrings").
                    GetSection(ConfigurationHelper.config.GetSection("Active_ConnectionString").Value).Value, dbName);

            }

            else if (AppEnvironment == Environment.Public)
            {
                return AppSettings.ConnectionString(dbName);
            }
            return fullConString;
        }
    }
}
