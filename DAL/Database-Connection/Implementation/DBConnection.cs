using DAL.DapperObject.Interface;
using DAL.Database_Context.Database_Connection.Interface;
using Microsoft.AspNetCore.Http;
using DAL.DapperObject;
using Shared.Helpers;


namespace DAL.Database_Context.Database_Connection.Implementation
{
    public class DBConnection : IDBConnection
    {
        private readonly IClientDatabase _clientDatabase;

        public DBConnection(IClientDatabase clientDatabase)
        {
            _clientDatabase = clientDatabase;
        }


        public string GetControlPanelConnectionString()
        {
            var httpContextAccessor = new HttpContextAccessor();
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext == null || httpContext.Request == null)
            {
                return Database.GetConnectionString(Database.ControlPanel);
            }

            return Database.GetConnectionString(Database.ControlPanel);

            //var connectionString = Database.GetConnectionString(Database.ControlPanel);
            //return connectionString;
        }


        public string GetHRMSConnectionString()
        {
            var httpContextAccessor = new HttpContextAccessor();
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext == null || httpContext.Request == null)
            {
                return Database.GetConnectionString(Database.HRMS);
            }

            var user = UserObjects.UserData(httpContext.Request);
            var databaseName = _clientDatabase.GetDatabaseName(user.Username);
            var connectionString = Database.GetConnectionString(databaseName);

            return connectionString;
        }


    }
}
