using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelp
{
    public interface IOrderByExpression<TEntity> where TEntity : class
    {
        IOrderedQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query);
        IOrderedQueryable<TEntity> ApplyThenBy(IOrderedQueryable<TEntity> query);
    }
}
