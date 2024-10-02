using Microsoft.Extensions.Configuration;


namespace Shared.Services
{
    public sealed class Appsettings
    {
        private IConfiguration Configuration { get; }
        private string apiUrl = "https://hris.myrecombd.com:8090/api"; //  https://agakhan.myrecombd.com:8056/api
        public Appsettings(IConfiguration _Configuration)
        {
            Configuration = _Configuration;
        }
        public string Origin { get {
                return Configuration.GetSection("Clients").GetSection("AngularClient").GetSection("Url").Value;
            } }
        public string SymmetricSecurityKey { get {
                return "rec0mCoNsultL!mtD";
            } }
        public string ApiValidIssuer {get{
                var app_environment = Configuration.GetSection("App_Environment").Value.ToString();
                if(app_environment != null) {
                    if(app_environment == "Local") {
                        return "http://localhost:5000";
                    }
                    else if(app_environment == "Public") {
                        return apiUrl;
                    }
                    else {
                        return null;
                    }
                }
                return null;
            }
        }
        public string ApiValidAudience {
            get {
                var app_environment = Configuration.GetSection("App_Environment").Value.ToString();
                if (app_environment != null) {
                    if (app_environment == "Local") {
                        return "http://localhost:5000";
                    }
                    else if (app_environment == "Public") {
                        return apiUrl;
                    }
                    else {
                        return null;
                    }
                }
                return null;
            }
        }
    }
}
