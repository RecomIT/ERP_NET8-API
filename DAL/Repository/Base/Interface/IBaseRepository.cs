using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repository.Base.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        // Sync
        T SingleOrDefault(Expression<Func<T, bool>> whereCondition);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Expression<Func<T, bool>> whereCondition);
        IEnumerable<T> GetAll(string childtableName);
        IEnumerable<T> GetAll(string childtableName, Expression<Func<T, bool>> whereCondition);
        T GetById(object Id);
        T GetSingle(string childtableName);
        T GetSingle(Expression<Func<T, bool>> whereCondition);
        T GetSingle(string childtableName, Expression<Func<T, bool>> whereCondition);
        bool Exists(Expression<Func<T, bool>> whereCondition);
        bool Save();

        // Async 
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> whereCondition);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> whereCondition);
        Task<IEnumerable<T>> GetAllAsync(string childtableName);
        Task<IEnumerable<T>> GetAllAsync(string childtableName, Expression<Func<T, bool>> whereCondition);
        Task<T> GetByIdAsync(object Id);
        Task<T> GetSingleAsync(string childtableName);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> whereCondition);
        Task<T> GetSingleAsync(string childtableName, Expression<Func<T, bool>> whereCondition);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> whereCondition);
        Task<bool> SaveAsync();

        // Sync
        void Insert(T entity);
        void InsertAll(IList<T> entities);
        void Update(T entity);
        void UpdateAll(IList<T> entities);
        void DeleteById(object Id);
        void DeleteSingle(Expression<Func<T, bool>> whereCondition);
        void DeleteAll(Expression<Func<T, bool>> whereCondition);

        // Async
        Task InsertAsync(T entity);
        Task InsertAllAsync(IList<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateAllAsync(IList<T> entities);
        Task DeleteByIdAsync(object Id);
        Task DeleteSingleAsync(Expression<Func<T, bool>> whereCondition);
        Task DeleteAllAsync(Expression<Func<T, bool>> whereCondition);

        /// Extra...
        /// Sync
        int Count(Expression<Func<T, bool>> whereCondition);
        IEnumerable<T> GetPagedRecords(Expression<Func<T, bool>> whereCondition, Expression<Func<T, string>> orderBy, int pageNo, int pageSize);
        IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters);
        IEnumerable<dynamic> SqlQuery(string Sql, Dictionary<string, object> Parameters);
        IEnumerable<dynamic> SqlQuery(string Sql);
        // Async
        Task<int> CountAsync(Expression<Func<T, bool>> whereCondition);
        Task<IEnumerable<T>> GetPagedRecordsAsync(Expression<Func<T, bool>> whereCondition, Expression<Func<T, string>> orderBy, int pageNo, int pageSize);
        Task<IEnumerable<T>> ExecWithStoreProcedureAsync(string query, params object[] parameters);
    }
}
