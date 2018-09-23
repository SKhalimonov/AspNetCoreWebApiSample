using System;
using WebApiSample.Core.Specifications;
using WebApiSample.Data.Core;
using WebApiSample.Data.Core.Base;

namespace WebApiSample.Core.Extensions
{
    /// <summary>
    /// Represents extensions of <see cref="Finder{TEntity}"/>.
    /// </summary>
    public static class FinderExtensions
    {
        /// <summary>
        /// Find by the id.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="finder">The finder.</param>
        /// <param name="id">The entity id.</param>
        /// <returns>The entity with specified id.</returns>
        public static TEntity ById<TEntity>(this IFinder<TEntity> finder, int id) where TEntity : BaseEntity
        {
            Check.Require<ArgumentNullException>(
                finder != null, "Unable find entity by id because argument finder is null");

            TEntity entity = finder.One(CommonSpecifications.ById<TEntity>(id));

            return entity;
        }
    }
}
