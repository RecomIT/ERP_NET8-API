using AutoMapper;
using Dapper;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using DAL.DapperObject.Interface;
using Shared.Control_Panel.ViewModels;

namespace DAL.DapperObject.Implementation
{
    public sealed class ClientDatabase : IClientDatabase
    {
        private string sqlQuery;
        private readonly IDapperData _dapper;
        public List<ClientDB> _dblist;
        public ClientDatabase(IDapperData dapper)
        {
            _dapper = dapper;
            _dblist = new List<ClientDB>();
            ClietDbs();
        }
        public void ClietDbs()
        {
            sqlQuery = string.Format(@"sp_Storage_list");
            _dblist = _dapper.SqlQueryList<ClientDB>(Database.ControlPanel, sqlQuery, null, CommandType.StoredProcedure).ToList();
        }


        private ClientDB GetClientDB(long organizationId)
        {
            ClientDB clientDB = new ClientDB();
            sqlQuery = string.Format(@"sp_Storage_Name");
            var parameter = new DynamicParameters();
            parameter.Add("organizationId", organizationId);
            clientDB = _dapper.SqlQueryFirst<ClientDB>(Database.ControlPanel, sqlQuery, parameter, commandType: CommandType.StoredProcedure);
            return clientDB;
        }
        public string GetDatabaseName(long organizationId)
        {
            string database = string.Empty;
            var obj = _dblist.FirstOrDefault(c => c.ClientId == organizationId);
            if (obj == null)
            {
                var newClientDatabase = GetClientDB(organizationId);
                if (newClientDatabase == null)
                {
                    database = string.Empty;
                }
                else
                {
                    _dblist.Add(newClientDatabase);
                    database = newClientDatabase.Database;
                }
            }
            else
            {
                database = _dblist.FirstOrDefault(c => c.ClientId == organizationId).Database;
            }
            return database;
        }

        public ClientDB GetClientObj(long organizationId)
        {
            var obj = _dblist.FirstOrDefault(c => c.ClientId == organizationId);
            if (obj == null)
            {
                var newClientDatabase = GetClientDB(organizationId);
                if (newClientDatabase != null)
                {
                    _dblist.Add(obj);
                }
            }
            return obj;
        }

        public string GetDatabaseName(string username)
        {
            sqlQuery = string.Format(@"sp_Storage_Name");
            var parameter = new DynamicParameters();
            parameter.Add("Username", username);
            var database = _dapper.SqlQueryFirst<ClientDB>(Database.ControlPanel, sqlQuery, parameter, commandType: CommandType.StoredProcedure);
            return database.Database;
        }

        public ClientDB GetClientObj(string username)
        {
            sqlQuery = string.Format(@"sp_Storage_Name");
            var parameter = new DynamicParameters();
            parameter.Add("Username", username);
            var clientObj = _dapper.SqlQueryFirst<ClientDB>(Database.ControlPanel, sqlQuery, parameter, commandType: CommandType.StoredProcedure);
            return clientObj;
        }
    }
}
