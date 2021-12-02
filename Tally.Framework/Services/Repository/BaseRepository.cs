using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tally.Framework.Interface;

namespace Tally.Framework.Services
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        //internal SQLiteConnection _sqlteDataBase;
        public BaseRepository()
        {
            //_sqlteDataBase = UnitWork.GetDbClient;
        }

        public bool Delete(Guid id)
        {
            Type type = typeof(TEntity);
            var col = LiteDbExtend.DbFactory.GetCollection<TEntity>(type.Name);
            var res = col.Delete(id);
            return res;
        }

        public bool Insert(TEntity entity)
        {
            Type type = typeof(TEntity);
            var col = LiteDbExtend.DbFactory.GetCollection<TEntity>(type.Name);
            return col.Insert(entity) > 0;
        }

        public List<TEntity> Query(Expression<Func<TEntity, bool>> expression)
        {
            Type type = typeof(TEntity);
            var col = LiteDbExtend.DbFactory.GetCollection<TEntity>(type.Name);
            return col.Find(expression).ToList();
        }

        public TEntity QueryById(Expression<Func<TEntity, bool>> expression)
        {
            Type type = typeof(TEntity);
            var col = LiteDbExtend.DbFactory.GetCollection<TEntity>(type.Name);
            return col.FindOne(expression);
        }

        public bool Update(TEntity entity)
        {
            Type type = typeof(TEntity);
            var col = LiteDbExtend.DbFactory.GetCollection<TEntity>(type.Name);
            return col.Update(entity);
        }
    }
}