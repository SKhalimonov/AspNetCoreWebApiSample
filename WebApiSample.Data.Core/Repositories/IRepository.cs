using System;
using System.Collections.Generic;
using System.Linq;
using WebApiSample.Data.Core.Specification;

namespace WebApiSample.Data.Core.Repositories
{
    /// <inheritdoc />
    /// <summary>
    /// Represents entities repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity> : IQueryable<TEntity>
    {
        /// <summary>
        /// Gets the entities finder.
        /// </summary>
        /// <value>The entities finder.</value>
        IFinder<TEntity> Find { get; }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Deletes entities by specification
        /// </summary>
        /// <param name="specification">Query specification</param>
        void Delete(ISpecification<TEntity> specification);

        /// <summary>
        /// Deletes entities by the specified query.
        /// </summary>
        /// <param name="parameters">The parameters which will be used to construct the query. Item1: field name, Item2: operation, Item3: value </param>
        /// <returns>number of rows deleted</returns>
        int Delete(List<Tuple<string, string, dynamic>> parameters);

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        void Save(TEntity entity);

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        void Commit();
    }
}
