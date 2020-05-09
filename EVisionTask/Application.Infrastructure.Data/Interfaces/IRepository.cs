using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Infrastructure.Data.Base;

namespace Application.Infrastructure.Data.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        Task<T> Insert(T model);
        Task<T> Update(T model);
        Task Delete(int id);
        Task DeleteRange(IEnumerable<T> models);
        Task<T> Get(int id);
        Task<T> Get(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> List();
        Task<IEnumerable<T>> List(Expression<Func<T, bool>> predicate);

        Task<Tuple<int, IEnumerable<T>>> GetPage(int pageNumber, int pageSize, string searchTerm, string sortingColumn,
            SortingType sort, Expression<Func<T, bool>> predicate = null);

        Task<int> Count();
        Task<int> Count(Expression<Func<T, bool>> predicate);
        Task<bool> Any(Expression<Func<T, bool>> predicate);
        void ApplySelectedValues(T entity, params Expression<Func<T, object>>[] properties);
    }
}
