using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Application.Infrastructure.Data.Base;
using Application.Infrastructure.Data.Exceptions;
using Application.Infrastructure.Data.Interfaces;
using Application.Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
namespace Application.Infrastructure.Data.Repository
{
    public class EntityFrameworkRepositoryBase<T> : IRepository<T> where T : class, IEntity
    {
        public EntityFrameworkRepositoryBase(DbContext dbContext)
        {
            Context = dbContext;
        }

        protected DbContext Context { get; set; }

        public virtual async Task<T> Insert(T model)
        {
            model.CreationDate = DateTime.UtcNow;
            var result = await Context.Set<T>().AddAsync(model);

            await Context.SaveChangesAsync();
            return await Get(result.Entity.Id);
        }

        public virtual async Task<T> Update(T model)
        {
            var originalModel = await Get(model.Id);
            if (originalModel == null) throw new GloballException("IEntity Not Found");

            model.Id = originalModel.Id;
            model.CreationDate = originalModel.CreationDate;
            Context.Entry(originalModel).CurrentValues.SetValues(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public virtual async Task Delete(int id)
        {
            var model = await Get(id);
            if (model == null) throw new GloballException("IEntity Not Found");

            Context.Set<T>().Remove(model);
            await Context.SaveChangesAsync();
        }

        public virtual async Task DeleteRange(IEnumerable<T> models)
        {
            Context.Set<T>().RemoveRange(models);
            await Context.SaveChangesAsync();
        }

        public virtual async Task<T> Get(int id)
        {
            var model = await Get(x => x.Id == id);
            return model;
        }

        public virtual async Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            var model = await Context.Set<T>().FirstOrDefaultAsync(predicate);
            return model;
        }

        public virtual async Task<IEnumerable<T>> List()
        {
            var model = await Context.Set<T>()
                .OrderByDescending(x => x.CreationDate)
                .ToListAsync();

            return model;
        }

        public virtual async Task<IEnumerable<T>> List(Expression<Func<T, bool>> predicate)
        {
            var model = await Context.Set<T>()
                .Where(predicate)
                .OrderByDescending(x => x.CreationDate)
                .ToListAsync();
            return model;
        }

        public virtual async Task<Tuple<int, IEnumerable<T>>> GetPage(int pageNumber, int pageSize, string searchTerm,
            string sortingColumn, SortingType sort, Expression<Func<T, bool>> predicate = null)
        {
            var result = Context.Set<T>()
                .Where(x => string.IsNullOrEmpty(searchTerm));

            if (predicate != null) result = result.Where(predicate);

            if (!string.IsNullOrEmpty(sortingColumn))
                result = sort == SortingType.Ascending
                    ? result.OrderBy(sortingColumn)
                    : result.OrderByDescending(sortingColumn);
            else
                result = result.OrderByDescending(x => x.CreationDate);

            var page = await result.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await result.CountAsync();

            var mod = count % pageSize > 0 ? 1 : 0;
            count = count / pageSize + mod;

            return new Tuple<int, IEnumerable<T>>(count, page);
        }

        public virtual async Task<int> Count()
        {
            var count = await Context.Set<T>().CountAsync();
            return count;
        }

        public virtual async Task<int> Count(Expression<Func<T, bool>> predicate)
        {
            var count = await Context.Set<T>().CountAsync(predicate);
            return count;
        }

        public async Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().AnyAsync(predicate);
        }

        public void ApplySelectedValues(T entity, params Expression<Func<T, object>>[] properties)
        {
            var entry = Context.Entry(entity);
            entry.State = EntityState.Unchanged;
            foreach (var prop in properties)
            {
                var propertyInfo = ((MemberExpression)prop.Body).Member as PropertyInfo;
                if (propertyInfo != null)
                    entry.Property(propertyInfo.Name).IsModified = true;
            }
        }
    }
}
