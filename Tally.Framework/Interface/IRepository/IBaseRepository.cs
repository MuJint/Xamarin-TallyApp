using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tally.Framework.Interface
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        List<TEntity> Query(Expression<Func<TEntity, bool>> expression);
        TEntity QueryById(Expression<Func<TEntity, bool>> expression);
        bool Delete(long id);
        bool Insert(TEntity entity);
        bool Update(TEntity entity);
    }
}
