namespace Shared.Services
{
    public static class AppSettings
    {
        internal static string apiUrl = ApiUrl.Hris;
        internal static string clientOrigin = ClientUrl.Hris;

        public static string App_environment = ConfigurationHelper.config.GetSection("App_Environment").Value.ToString();
        public static string Origin
        {
            get
            {
                return ConfigurationHelper.config.GetSection("Clients").GetSection("AngularClient").GetSection("Url").Value;
            }
        }
        public static bool PF_Software_Connection
        {
            get
            {
                var pf_software_connection = ConfigurationHelper.config.GetSection("PF_Software_Connection").Value.ToString();
                if (pf_software_connection.ToLower() == Switch.On.ToLower())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static bool EmailService
        {
            get
            {
                var emailservice = ConfigurationHelper.config.GetSection("EmailService").Value.ToString();
                if (emailservice.ToLower() == Switch.On.ToLower())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static string SymmetricSecurityKey
        {
            get
            {
                // return "rec0mCoNsultL!mtD";
                return "Y75Np55qVRutusfFERqE3jmIwByFA3IJoMLM0mgZX6ycpgauNwrJKQL5IQHy / eka";
            }
        }
        public static string Key
        {
            get
            {
                return "7391824694761634";
            }
        }
        public static string ClientOrigin
        {
            get
            {
                if (App_environment != null)
                {
                    if (App_environment == Environment.Local)
                    {
                        return ClientUrl.Local;
                    }
                    else if (App_environment == Environment.Public)
                    {
                        return clientOrigin;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
        }
        // ----------------------- Starting .....................
        // ----------------------------------------- Valid Issuer
        public static string ApiValidIssuer
        {
            get
            {
                if (App_environment != null)
                {
                    if (App_environment == Environment.Local)
                    {
                        return ApiUrl.Local;
                    }
                    else if (App_environment == Environment.Public)
                    {
                        return apiUrl;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
        }
        // ----------------------------------------- Valid Audience
        public static string ApiValidAudience
        {
            get
            {

                if (App_environment != null)
                {
                    if (App_environment == Environment.Local)
                    {
                        return ApiUrl.Local;
                    }
                    else if (App_environment == Environment.Public)
                    {
                        return apiUrl;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
        }
        // ----------------------- Ending .....................
        public static string ConnectionString(string dbName)
        {
            if (apiUrl == ApiUrl.Hris || apiUrl == ApiUrl.AgaKhan)
            {
                return string.Format(@"server=192.168.10.92;database={0};user id=sa;password={1};multipleactiveresultsets=true;Trust Server Certificate = True", dbName, "sarec0m2o2@");
            }
            else if (apiUrl == ApiUrl.Wounderman)
            {
                return string.Format(@"server=BDWTHR\BDWTHRSQL2019;database={0};user id=recom;password=r3c0m@WT!321;multipleactiveresultsets=true;Trust Server Certificate = True", dbName);
            }
            else if (apiUrl == ApiUrl.ITX)
            {
                return string.Format(@"server=192.168.10.92;database={0};user id=sa;password={1};multipleactiveresultsets=true;Trust Server Certificate = True", dbName, "sarec0m2o2@");
            }
            else if (apiUrl == ApiUrl.PWC_Cloud)
            {
                return string.Format(@"Server=tcp:ipzrgmsusssp001.database.windows.net,1433;Initial Catalog={0};User ID=gtyagi006;Password=ReCom#?2023;MultipleActiveResultSets=true", dbName);
            }
            return "";
        }
        public static string PayslipApiURL = ConfigurationHelper.config.GetSection("PayslipUri").Value.ToString();
        public static string TaxCardURL = ConfigurationHelper.config.GetSection("TaxCardUri").Value.ToString();
        public static string BonusPayslipApiURL = ConfigurationHelper.config.GetSection("BonusPayslipUri").Value.ToString();
        public static string PayslipApiKey = ConfigurationHelper.config.GetSection("Key").Value.ToString();
    }
}
