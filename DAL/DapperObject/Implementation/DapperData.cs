using Dapper;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System;
using DAL.DapperObject.Interface;

namespace DAL.DapperObject.Implementation
{
    public class DapperData : IDapperData
    {
        private readonly DapperContext _dapperContext;
        public DapperData(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
        public async Task<IEnumerable<T>> SqlQueryListAsync<T>(string database, string sql, object parameters, CommandType? commandType)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = await connection.QueryAsync<T>(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
                return data;
            }
        }
        public async Task<IEnumerable<T>> SqlQueryListAsync<T>(string database, string sql, object parameters, CommandType? commandType, long clientId)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = await connection.QueryAsync<T>(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
                return data;
            }
        }
        public async Task<IEnumerable<T>> SqlQueryListAsync<T>(string database, string sql)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = await connection.QueryAsync<T>(sql, commandTimeout: 0);
                return data;
            }
        }
        public async Task<IEnumerable<T>> SqlQueryListAsync<T>(string database, string sql, long clientId)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = await connection.QueryAsync<T>(sql, commandTimeout: 0);
                return data;
            }
        }
        public async Task<IEnumerable<T>> SqlQueryListAsync<T>(string database, string sql, object parameters)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = await connection.QueryAsync<T>(sql, parameters, commandTimeout: 0);
                return data;
            }
        }
        public async Task<IEnumerable<T>> SqlQueryListAsync<T>(string database, string sql, object parameters, long clientId)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = await connection.QueryAsync<T>(sql, parameters, commandTimeout: 0);
                return data;
            }
        }
        public async Task<T> SqlQueryFirstAsync<T>(string database, string sql, object parameters, CommandType? commandType)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = await connection.QueryFirstOrDefaultAsync<T>(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
                return data;
            }
        }
        public async Task<T> SqlQueryFirstAsync<T>(string database, string sql, object parameters, CommandType? commandType, long clientId)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = await connection.QueryFirstOrDefaultAsync<T>(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
                return data;
            }
        }
        public async Task<T> SqlQueryFirstAsync<T>(string database, string sql, object parameters)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = await connection.QueryFirstOrDefaultAsync<T>(sql, parameters, commandTimeout: 0);
                return data;
            }
        }
        public async Task<T> SqlQueryFirstAsync<T>(string database, string sql, object parameters, long clientId)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = await connection.QueryFirstOrDefaultAsync<T>(sql, parameters, commandTimeout: 0);
                return data;
            }
        }
        public async Task<T> SqlQueryFirstAsync<T>(string database, string sql)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = await connection.QueryFirstOrDefaultAsync<T>(sql, commandTimeout: 0);
                return data;
            }
        }
        public async Task<T> SqlQueryFirstAsync<T>(string database, string sql, long clientId)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = await connection.QueryFirstOrDefaultAsync<T>(sql, commandTimeout: 0);
                return data;
            }
        }
        //public async Task SqlExecuteNonQuery(string database, string sql, object parameters, CommandType? commandType)
        //{
        //    using (var connection = _dapperContext.CreateConnection(database)) {
        //        await connection.ExecuteAsync(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
        //    }
        //}
        //public async Task SqlExecuteNonQuery(string database, string sql, object parameters, CommandType? commandType, long clientId)
        //{
        //    using (var connection = _dapperContext.CreateConnection(database)) {
        //        await connection.ExecuteAsync(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
        //    }
        //}
        public IEnumerable<T> SqlQueryList<T>(string database, string sql, object parameters, CommandType? commandType)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = connection.Query<T>(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
                return data;
            }
        }
        public IEnumerable<T> SqlQueryList<T>(string database, string sql, object parameters, CommandType? commandType, long clientId)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = connection.Query<T>(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
                return data;
            }
        }
        public T SqlQueryFirst<T>(string database, string sql, object parameters, CommandType? commandType)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = connection.QueryFirstOrDefault<T>(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
                return data;
            }
        }
        public T SqlQueryFirst<T>(string database, string sql, object parameters, CommandType? commandType, long clientId)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var data = connection.QueryFirstOrDefault<T>(sql, parameters, commandTimeout: 0);
                return data;
            }
        }

        public async Task<IEnumerable<T>> SqlQueryListAsync<T>(long clientId, string sql)
        {
            using (var connection = _dapperContext.CreateConnection(clientId))
            {
                var data = await connection.QueryAsync<T>(sql, commandTimeout: 0);
                return data;
            }
        }

        public async Task<IEnumerable<T>> SqlQueryListAsync<T>(long clientId, string sql, object parameters)
        {
            using (var connection = _dapperContext.CreateConnection(clientId))
            {
                var data = await connection.QueryAsync<T>(sql, parameters, commandTimeout: 0);
                return data;
            }
        }

        public async Task<IEnumerable<T>> SqlQueryListAsync<T>(long clientId, string sql, object parameters, CommandType? commandType)
        {
            using (var connection = _dapperContext.CreateConnection(clientId))
            {
                var data = await connection.QueryAsync<T>(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
                return data;
            }
        }

        public async Task SqlExecuteNonQuery(long clientId, string sql, object parameters, CommandType? commandType)
        {
            using (var connection = _dapperContext.CreateConnection(clientId))
            {
                await connection.ExecuteAsync(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
            }
        }

        public async Task<T> SqlQueryFirstAsync<T>(long client, string sql)
        {
            using (var connection = _dapperContext.CreateConnection(client))
            {
                var data = await connection.QueryFirstOrDefaultAsync<T>(sql, commandTimeout: 0);
                return data;
            }
        }

        public async Task<T> SqlQueryFirstAsync<T>(long clientId, string sql, object parameters)
        {
            using (var connection = _dapperContext.CreateConnection(clientId))
            {
                var data = await connection.QueryFirstOrDefaultAsync<T>(sql, parameters, commandTimeout: 0);
                return data;
            }
        }

        public async Task<T> SqlQueryFirstAsync<T>(long clientId, string sql, object parameters, CommandType? commandType)
        {
            using (var connection = _dapperContext.CreateConnection(clientId))
            {
                var data = await connection.QueryFirstOrDefaultAsync<T>(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
                return data;
            }
        }

        public IEnumerable<T> SqlQueryList<T>(long clientId, string sql, object parameters, CommandType? commandType)
        {
            using (var connection = _dapperContext.CreateConnection(clientId))
            {
                var data = connection.Query<T>(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
                return data;
            }
        }

        public T SqlQueryFirst<T>(long clientId, string sql, object parameters, CommandType? commandType)
        {
            using (var connection = _dapperContext.CreateConnection(clientId))
            {
                var data = connection.QueryFirstOrDefault<T>(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
                return data;
            }
        }

        public async Task<T> ExecuteScalerFunction<T>(string database, string sql, object parameters, CommandType? commandType)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var command = new CommandDefinition(sql, parameters: parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
                var data = await connection.ExecuteScalarAsync<T>(command);
                return data;
            }
        }

        public async Task<IEnumerable<T>> ExecuteTableValuedFunction<T>(string database, string sql, object parameters, CommandType? commandType)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                var command = new CommandDefinition(sql, parameters: parameters, commandType: CommandType.StoredProcedure, commandTimeout: 0);
                var data = await connection.QueryAsync<T>(command);
                return data;
            }
        }

        public async Task<IEnumerable<T>> RemoteSqlQueryListAsync<T>(string database, string sql, object parameters, CommandType? commandType)
        {
            using (var connection = _dapperContext.EstablishRemoteConnection(database))
            {
                var command = new CommandDefinition(sql, parameters: parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
                var data = await connection.QueryAsync<T>(command);
                return data;
            }
        }

        public async Task<T> RemoteSqlQueryAsync<T>(string database, string sql, object parameters, CommandType? commandType)
        {
            using (var connection = _dapperContext.EstablishRemoteConnection(database))
            {
                var command = new CommandDefinition(sql, parameters: parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
                var data = await connection.QueryFirstOrDefaultAsync<T>(command);
                return data;
            }
        }

        public async Task<DataTable> SqlDataTable(string database, string sql, Dictionary<string, string> parameters, CommandType? commandType)
        {
            using (SqlConnection connection = (SqlConnection)_dapperContext.CreateConnection(database))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.CommandType = commandType ?? CommandType.Text;
                command.CommandTimeout = 0;
                if (parameters.Count > 0)
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }
                SqlDataAdapter sqlDa = new SqlDataAdapter(command);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                await Task.Delay(1);
                return dtbl;
            }
        }

        public async Task<int> SqlExecuteNonQuery(string database, string sql, object parameters, CommandType? commandType)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                return await connection.ExecuteAsync(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
            }
        }

        public async Task<int> SqlExecuteNonQuery(string database, string sql, object parameters, CommandType? commandType, long clientId)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                return await connection.ExecuteAsync(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
            }
        }

        public IDbConnection SqlConnectionForTransactionalNonQuery(string database)
        {
            return _dapperContext.CreateConnection(database);
        }

        public async Task<IDbConnection> SqlConnectionForTransactionalAsync(string database)
        {
            return await Task.Run(() => _dapperContext.CreateConnection(database));
        }

        public async Task<int> SqlExecuteNonQueryStatus(string database, string sql, object parameters, CommandType? commandType)
        {
            using (var connection = _dapperContext.CreateConnection(database))
            {
                return await connection.ExecuteAsync(sql, parameters, commandType: commandType ?? CommandType.Text, commandTimeout: 0);
            }

        }
    }
}
