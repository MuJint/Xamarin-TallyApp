using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tally.Framework.Interface
{
    public interface IBaseServices<TEntity> where TEntity : class
    {
        List<TEntity> Query(Expression<Func<TEntity, bool>> expression);

        TEntity QueryById(Expression<Func<TEntity, bool>> expression);

        bool Delete(Guid entity);

        bool Insert(TEntity entity);

        bool Update(TEntity entity);
    }
}