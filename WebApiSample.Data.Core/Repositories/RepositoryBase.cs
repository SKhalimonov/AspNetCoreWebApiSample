using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApiSample.Data.Core.Base;
using WebApiSample.Data.Core.Specification;

namespace WebApiSample.Data.Core.Repositories
{
    /// <inheritdoc />
    /// <summary>
    /// Represents base functionality of entities repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        /// <inheritdoc />
        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable" /> is executed.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type" /> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.</returns>
        public Type ElementType => RepositoryQuery.ElementType;

        /// <inheritdoc />
        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable" />.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Linq.Expressions.Expression" /> that is associated with this instance of <see cref="T:System.Linq.IQueryable" />.</returns>
        public Expression Expression => RepositoryQuery.Expression;

        /// <inheritdoc />
        /// <summary>
        /// Gets the entities finder.
        /// </summary>
        /// <value>The entities finder.</value>
        public virtual IFinder<TEntity> Find => new Finder<TEntity>(this);

        /// <inheritdoc />
        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Linq.IQueryProvider" /> that is associated with this data source.</returns>
        public IQueryProvider Provider => RepositoryQuery.Provider;

        /// <summary>
        /// Gets the repository query.
        /// </summary>
        /// <value>The repository query.</value>
        protected abstract IQueryable<TEntity> RepositoryQuery { get; }

        /// <inheritdoc />
        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public abstract void Delete(TEntity entity);

        /// <summary>
        /// Deletes entities by the specified query.
        /// </summary>
        /// <param name="specification">Query specification</param>
        public abstract void Delete(ISpecification<TEntity> specification);

        /// <inheritdoc />
        /// <summary>
        /// Deletes entities by the specified query.
        /// </summary>
        /// <param name="parameters">The parameters which will be used to construct the query. Item1: field name, Item2: operation, Item3: value </param>
        public abstract int Delete(List<Tuple<string, string, dynamic>> parameters);

        /// <inheritdoc />
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return RepositoryQuery.GetEnumerator();
        }

        /// <inheritdoc />
        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public abstract void Save(TEntity entity);

        public abstract void Commit();

        /// <inheritdoc />
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return RepositoryQuery.GetEnumerator();
        }
    }
}
