using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DAL.DapperObject.Interface
{
    public interface IDapperData
    {
        Task<IEnumerable<T>> SqlQueryListAsync<T>(string database, string sql);
        Task<IEnumerable<T>> SqlQueryListAsync<T>(long clientId, string sql);
        Task<IEnumerable<T>> SqlQueryListAsync<T>(string database, string sql, long clientId);
        Task<IEnumerable<T>> SqlQueryListAsync<T>(string database, string sql, object parameters);
        Task<IEnumerable<T>> SqlQueryListAsync<T>(long clientId, string sql, object parameters);
        Task<IEnumerable<T>> SqlQueryListAsync<T>(string database, string sql, object parameters, long clientId);
        Task<IEnumerable<T>> SqlQueryListAsync<T>(string database, string sql, object parameters, CommandType? commandType);
        Task<IEnumerable<T>> SqlQueryListAsync<T>(long clientId, string sql, object parameters, CommandType? commandType);
        Task<IEnumerable<T>> SqlQueryListAsync<T>(string database, string sql, object parameters, CommandType? commandType, long clientId);
        //Task SqlExecuteNonQuery(string database, string sql, object parameters, CommandType? commandType);
        //Task SqlExecuteNonQuery(long clientId, string sql, object parameters, CommandType? commandType);
        //Task SqlExecuteNonQuery(string database, string sql, object parameters, CommandType? commandType, long clientId);
        Task<T> SqlQueryFirstAsync<T>(string database, string sql);
        Task<T> SqlQueryFirstAsync<T>(long client, string sql);
        Task<T> SqlQueryFirstAsync<T>(string database, string sql, long clientId);
        Task<T> SqlQueryFirstAsync<T>(string database, string sql, object parameters);
        Task<T> SqlQueryFirstAsync<T>(long clientId, string sql, object parameters);
        Task<T> SqlQueryFirstAsync<T>(string database, string sql, object parameters, long clientId);
        Task<T> SqlQueryFirstAsync<T>(string database, string sql, object parameters, CommandType? commandType);
        Task<T> SqlQueryFirstAsync<T>(long clientId, string sql, object parameters, CommandType? commandType);
        Task<T> SqlQueryFirstAsync<T>(string database, string sql, object parameters, CommandType? commandType, long clientId);
        IEnumerable<T> SqlQueryList<T>(string database, string sql, object parameters, CommandType? commandType);
        IEnumerable<T> SqlQueryList<T>(long clientId, string sql, object parameters, CommandType? commandType);
        IEnumerable<T> SqlQueryList<T>(string database, string sql, object parameters, CommandType? commandType, long clientId);
        T SqlQueryFirst<T>(string database, string sql, object parameters, CommandType? commandType);
        T SqlQueryFirst<T>(long clientId, string sql, object parameters, CommandType? commandType);
        T SqlQueryFirst<T>(string database, string sql, object parameters, CommandType? commandType, long clientId);
        Task<T> ExecuteScalerFunction<T>(string database, string sql, object parameters, CommandType? commandType);
        Task<IEnumerable<T>> ExecuteTableValuedFunction<T>(string database, string sql, object parameters, CommandType? commandType);
        Task<IEnumerable<T>> RemoteSqlQueryListAsync<T>(string database, string sql, object parameters, CommandType? commandType);
        Task<T> RemoteSqlQueryAsync<T>(string database, string sql, object parameters, CommandType? commandType);
        Task<DataTable> SqlDataTable(string database, string sql, Dictionary<string, string> parameters, CommandType? commandType);
        Task<int> SqlExecuteNonQuery(string database, string sql, object parameters, CommandType? commandType);
        Task<int> SqlExecuteNonQuery(string database, string sql, object parameters, CommandType? commandType, long clientId);
        IDbConnection SqlConnectionForTransactionalNonQuery(string database);
        Task<IDbConnection> SqlConnectionForTransactionalAsync(string database);
        Task<int> SqlExecuteNonQueryStatus(string database, string sql, object parameters, CommandType? commandType);
    }
}
