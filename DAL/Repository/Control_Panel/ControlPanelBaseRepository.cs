using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq.Expressions;
using DAL.UnitOfWork.Control_Panel.Interface;
using DAL.Repository.Base.Interface;

namespace DAL.Repository.Control_Panel
{
    public class ControlPanelBaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IControlPanelUnitOfWork _controlPanelUnitOfWork;
        internal DbSet<T> dbSet = null;
        public ControlPanelBaseRepository(IControlPanelUnitOfWork controlPanelUnitOfWork)
        {
            if (controlPanelUnitOfWork == null) throw new ArgumentNullException("ControlPanel DbContext is not assigned");
            _controlPanelUnitOfWork = controlPanelUnitOfWork;
            dbSet = _controlPanelUnitOfWork.Db.Set<T>();
        }
        public int Count(Expression<Func<T, bool>> whereCondition)
        {
            return dbSet.Where(whereCondition).Count();
        }
        public void DeleteAll(Expression<Func<T, bool>> whereCondition)
        {
            IEnumerable<T> entities = GetAll(whereCondition);
            dbSet.RemoveRange(entities);
        }
        public async Task DeleteAllAsync(Expression<Func<T, bool>> whereCondition)
        {
            IEnumerable<T> entities = await GetAllAsync(whereCondition);
            dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }
        public void DeleteById(object Id)
        {
            T entity = GetById(Id);
            dbSet.Remove(entity);
        }
        public async Task DeleteByIdAsync(object Id)
        {
            T entity = await GetByIdAsync(Id);
            dbSet.Remove(entity);
            await Task.CompletedTask;
        }
        public void DeleteSingle(Expression<Func<T, bool>> whereCondition)
        {
            T entity = GetSingle(whereCondition);
            dbSet.Remove(entity);
        }
        public async Task DeleteSingleAsync(Expression<Func<T, bool>> whereCondition)
        {
            T entity = await GetSingleAsync(whereCondition);
            dbSet.Remove(entity);
        }
        public IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return dbSet.FromSqlRaw(query, parameters).AsNoTracking().ToList();
        }
        public bool Exists(Expression<Func<T, bool>> whereCondition)
        {
            return dbSet.AsNoTracking().Any(whereCondition);
        }
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await dbSet.AsNoTracking().AnyAsync(whereCondition);
        }
        public IEnumerable<T> GetAll()
        {
            return dbSet.AsNoTracking().ToList();
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> whereCondition)
        {
            return dbSet.Where(whereCondition).AsNoTracking().ToList();
        }
        public IEnumerable<T> GetAll(string childtableName)
        {
            return dbSet.Include(childtableName).AsNoTracking().ToList();
        }
        public IEnumerable<T> GetAll(string childtableName, Expression<Func<T, bool>> whereCondition)
        {
            return dbSet.Include(childtableName).Where(whereCondition).AsNoTracking().ToList();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await dbSet.Where(whereCondition).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(string childtableName)
        {
            return await dbSet.Include(childtableName).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(string childtableName, Expression<Func<T, bool>> whereCondition)
        {
            return await dbSet.Include(childtableName).Where(whereCondition).AsNoTracking().ToListAsync();
        }
        public T GetById(object Id)
        {
            return dbSet.Find(Id);
        }
        public async Task<T> GetByIdAsync(object Id)
        {
            return await dbSet.FindAsync(Id);
        }
        public IEnumerable<T> GetPagedRecords(Expression<Func<T, bool>> whereCondition, Expression<Func<T, string>> orderBy, int pageNo, int pageSize)
        {
            return dbSet.Where(whereCondition).OrderBy(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize).AsNoTracking().AsEnumerable();
        }
        public T GetSingle(string childtableName)
        {
            return dbSet.Include(childtableName).AsNoTracking().FirstOrDefault();
        }
        public T GetSingle(Expression<Func<T, bool>> whereCondition)
        {
            return dbSet.AsNoTracking().FirstOrDefault(whereCondition);
        }
        public T GetSingle(string childtableName, Expression<Func<T, bool>> whereCondition)
        {
            return dbSet.Include(childtableName).AsNoTracking().FirstOrDefault(whereCondition);
        }
        public async Task<T> GetSingleAsync(string childtableName)
        {
            return await dbSet.Include(childtableName).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await dbSet.AsNoTracking().FirstOrDefaultAsync(whereCondition);
        }
        public async Task<T> GetSingleAsync(string childtableName, Expression<Func<T, bool>> whereCondition)
        {
            return await dbSet.Include(childtableName).AsNoTracking().FirstOrDefaultAsync(whereCondition);
        }
        public void Insert(T entity)
        {
            dbSet.Add(entity);
        }
        public void InsertAll(IList<T> entities)
        {
            dbSet.AddRange(entities);
        }
        public async Task InsertAllAsync(IList<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }
        public async Task InsertAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }
        public bool Save()
        {
            return _controlPanelUnitOfWork.Db.SaveChanges() > 0;
        }
        public async Task<bool> SaveAsync()
        {
            return await _controlPanelUnitOfWork.Db.SaveChangesAsync() > 0;
        }
        public T SingleOrDefault(Expression<Func<T, bool>> whereCondition)
        {
            return dbSet.AsNoTracking().SingleOrDefault(whereCondition);
        }
        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await dbSet.AsNoTracking().SingleOrDefaultAsync(whereCondition);
        }
        public IEnumerable<dynamic> SqlQuery(string Sql, Dictionary<string, object> Parameters)
        {
            using (var cmd = _controlPanelUnitOfWork.Db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                foreach (KeyValuePair<string, object> param in Parameters)
                {
                    DbParameter dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value;
                    cmd.Parameters.Add(dbParameter);
                }

                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var dataRow = GetDataRow(dataReader);
                        yield return dataRow;
                    }
                }
            }
        }
        public IEnumerable<dynamic> SqlQuery(string Sql)
        {
            using (var cmd = _controlPanelUnitOfWork.Db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var dataRow = GetDataRow(dataReader);
                        yield return dataRow;
                    }
                }
            }
        }
        private static dynamic GetDataRow(DbDataReader dataReader)
        {
            var dataRow = new ExpandoObject() as IDictionary<string, object>;
            for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                dataRow.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
            return dataRow;
        }
        public void Update(T entity)
        {
            _controlPanelUnitOfWork.Db.Entry(entity).State = EntityState.Modified;
        }
        public void UpdateAll(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                _controlPanelUnitOfWork.Db.Entry(entity).State = EntityState.Modified;
            }
        }
        public async Task UpdateAllAsync(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                _controlPanelUnitOfWork.Db.Entry(entity).State = EntityState.Modified;
            }
            await Task.CompletedTask;
        }
        public async Task UpdateAsync(T entity)
        {
            _controlPanelUnitOfWork.Db.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }
        public async Task<int> CountAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await dbSet.Where(whereCondition).AsNoTracking().CountAsync();
        }
        public async Task<IEnumerable<T>> GetPagedRecordsAsync(Expression<Func<T, bool>> whereCondition, Expression<Func<T, string>> orderBy, int pageNo, int pageSize)
        {
            return await dbSet.Where(whereCondition).OrderBy(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<T>> ExecWithStoreProcedureAsync(string query, params object[] parameters)
        {
            return await dbSet.FromSqlRaw(query, parameters).AsNoTracking().ToListAsync();
        }

    }
}
