using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Tally.Framework.Interface;

namespace Tally.Framework.Services
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        internal SQLiteConnection _sqlteDataBase = UnitWork.GetDbClient;
        public BaseRepository()
        {
            _sqlteDataBase = UnitWork.GetDbClient;
        }

        public bool Delete(TEntity entity)
        {
            return _sqlteDataBase.Delete(entity) > 0;
        }

        public bool Insert(TEntity entity)
        {
            return _sqlteDataBase.Insert(entity) > 0;
        }

        public List<TEntity> Query(Expression<Func<TEntity, bool>> expression)
        {
            var table = _sqlteDataBase.Table<TEntity>();
            return table.Where(expression).ToList();
        }

        public TEntity QueryById(Expression<Func<TEntity, bool>> expression)
        {
            var table = _sqlteDataBase.Table<TEntity>();
            return table.Where(expression).FirstOrDefault();
        }

        public bool Update(TEntity entity)
        {
            return _sqlteDataBase.Update(entity) > 0;
        }
    }
}
