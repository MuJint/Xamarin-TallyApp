using Tally.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tally.Framework.Services
{
    public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : class, new()
    {
        private static readonly IBaseRepository<TEntity> _baseRepository = _baseRepository ?? new BaseRepository<TEntity>();

        public bool Delete(Guid entity)
        {
            return _baseRepository.Delete(entity);
        }

        public bool Insert(TEntity entity)
        {
            return _baseRepository.Insert(entity);
        }

        public List<TEntity> Query(Expression<Func<TEntity, bool>> expression)
        {
            return _baseRepository.Query(expression);
        }

        public TEntity QueryById(Expression<Func<TEntity, bool>> expression)
        {
            return _baseRepository.QueryById(expression);
        }

        public bool Update(TEntity entity)
        {
            return _baseRepository.Update(entity);
        }
    }
}