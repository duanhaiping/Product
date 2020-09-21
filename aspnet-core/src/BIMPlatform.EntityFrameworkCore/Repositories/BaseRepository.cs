using BIMPlatform.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Linq.Expressions;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BIMPlatform.Repositories
{
    public class BaseRepository<TEntity, TKey> : EfCoreRepository<BIMPlatformDbContext, TEntity, TKey>, IBaseRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        public BaseRepository(IDbContextProvider<BIMPlatformDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public void Add(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
            DbContext.SaveChangesAsync();
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            DbContext.Set<TEntity>().AddRange(entities);
            DbContext.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
            this.DbContext.SaveChangesAsync();
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            DbContext.Set<TEntity>().RemoveRange(entities);
            this.DbContext.SaveChangesAsync();
        }

        public void DeleteByKey(params object[] keyValues)
        {
            TEntity entity = this.FindByKeyValues(keyValues);
            if(entity!= null)
            {
                DbContext.Set<TEntity>().Remove(entity);
                this.DbContext.SaveChangesAsync();
            }
        }

        public TEntity FindByKeyValues(params object[] keyValues)
        {
            return DbContext.Set<TEntity>().Find(keyValues);
        }

        public IList<TEntity> FindDistinctList(Expression<Func<TEntity, bool>> expression = null, params string[] includePath)
        {
            IQueryable<TEntity> defaultQuery = Query(expression, includePath);
            return defaultQuery.Distinct().ToList();
        }

        public IList<TEntity> FindList(Expression<Func<TEntity, bool>> expression = null, params string[] includePath)
        {
            IQueryable<TEntity> defaultQuery = Query(expression, includePath);
            return defaultQuery.ToList();
        }

        public IList<TEntity> FindListByOrder<TKey1>(Expression<Func<TEntity, bool>> expression = null, Expression<Func<TEntity, TKey1>> orderBy = default, bool ascending = true, params string[] includePath)
        {
            IQueryable<TEntity> defaultQuery = Query(expression, includePath);
            if (orderBy != null)
            {
                if (ascending)
                    defaultQuery = defaultQuery.OrderBy(orderBy);
                else
                    defaultQuery = defaultQuery.OrderByDescending(orderBy);
            }

            return defaultQuery.ToList();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression = null, params string[] includePath)
        {
            IQueryable<TEntity> defaultQuery = Query(expression, includePath);
            return defaultQuery.FirstOrDefault();
        }

        public IList<TEntity> LoadPageList<TKey1>(out long count, int pageIndex, int pageSize, Expression<Func<TEntity, bool>> expression = null, Expression<Func<TEntity, TKey1>> orderBy = default, bool ascending = true, params string[] includePath)
        {
            IQueryable<TEntity> defaultQuery = Query(expression, includePath);
            if (orderBy != null)
            {
                if (ascending)
                    defaultQuery = defaultQuery.OrderBy(orderBy);
                else
                    defaultQuery = defaultQuery.OrderByDescending(orderBy);
            }
            count = defaultQuery.Count();
            defaultQuery = defaultQuery.Skip(pageIndex).Take(pageSize);

            return defaultQuery.ToList();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> expression = null, params string[] includePath)
        {
            IQueryable<TEntity> defaultQuery = DbContext.Set<TEntity>();
            if (expression != null)
                defaultQuery = defaultQuery.Where(expression);

            if (includePath != null)
            {
                foreach (string path in includePath)
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        defaultQuery = defaultQuery.Include(path);
                    }
                }
            }

            return defaultQuery;
        }      
    }
}
