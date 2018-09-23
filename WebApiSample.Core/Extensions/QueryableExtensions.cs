using System.Linq;
using WebApiSample.Data.Core;
using WebApiSample.Data.Core.Base;
using WebApiSample.Data.Core.Repositories;

namespace WebApiSample.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IFinder<TEntity> GetFinder<TEntity>(this IQueryable<TEntity> source) where TEntity : IEntity
        {
            return new Finder<TEntity>(source);
        }

        public static IQueryable<TEntity> Paging<TEntity>(this IQueryable<TEntity> source, int offset, int limit) where TEntity : IEntity
        {
            return source.Skip(offset * limit).Take(limit);
        }

        public static IQueryable<TEntity> Paging<TEntity>(this IQueryable<TEntity> source, int? offset, int? limit) where TEntity : IEntity
        {
            return offset.HasValue && limit.HasValue ? source.Paging(offset.Value, limit.Value) : source;
        }
    }
}
