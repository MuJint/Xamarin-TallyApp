using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tally.Framework.Interface;

namespace Tally.Framework.Services
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        internal LiteDatabase _dbFactory;
        public BaseRepository()
        {
            _dbFactory = UnitWork.GetDbClient;
        }

        public bool Delete(long id)
        {
            return _dbFactory.GetCollection<TEntity>().Delete(id);
        }

        public bool Insert(TEntity entity)
        {
            return _dbFactory.GetCollection<TEntity>().Insert(entity);
        }

        public List<TEntity> Query(Expression<Func<TEntity, bool>> expression)
        {
            var table = _dbFactory.GetCollection<TEntity>(typeof(TEntity).Name);
            var all = table.FindAll();
            return table.Find(expression).ToList();
        }

        public TEntity QueryById(Expression<Func<TEntity, bool>> expression)
        {
            var table = _dbFactory.GetCollection<TEntity>();
            return table.FindOne(expression);
        }

        public bool Update(TEntity entity)
        {
            return _dbFactory.GetCollection<TEntity>().Update(entity);
        }
    }
}
