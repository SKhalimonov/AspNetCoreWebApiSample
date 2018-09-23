using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiSample.Data.Context;
using WebApiSample.Data.Core.Base;
using WebApiSample.Data.Core.Repositories;
using WebApiSample.Data.Core.Specification;

namespace WebApiSample.Data.Repositories
{
    public class Repository<TEntity> : RepositoryBase<TEntity>
        where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        protected ILogger Logger { get; }

        protected override IQueryable<TEntity> RepositoryQuery => _context.Set<TEntity>().AsQueryable();

        public override void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Logger.LogError(ex.Message);
                RejectChanges();

                throw;
            }
        }

        public void RejectChanges()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        {
                            entry.CurrentValues.SetValues(entry.OriginalValues);
                            entry.State = EntityState.Unchanged;
                            break;
                        }

                    case EntityState.Deleted:
                        {
                            entry.State = EntityState.Unchanged;
                            break;
                        }

                    case EntityState.Added:
                        {
                            entry.State = EntityState.Detached;
                            break;
                        }
                }
            }
        }

        public override void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (_context.Set<TEntity>().Local.All(e => e.Id != entity.Id))
            {
                _context.Set<TEntity>().Attach(entity);
            }

            _context.Set<TEntity>().Remove(entity);
        }

        public override void Delete(ISpecification<TEntity> specification)
        {
            var entities = _context.Set<TEntity>().Where(specification.SatisfiedBy).ToList();

            if (entities.Any())
            {
                entities.ForEach(this.Delete);
            }
        }

        public override int Delete(List<Tuple<string, string, dynamic>> parameters)
        {
            throw new NotImplementedException();
        }

        public override void Save(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Id > 0)
            {
                if (_context.Set<TEntity>().Local.All(e => e.Id != entity.Id))
                {
                    _context.Set<TEntity>().Attach(entity);
                }

                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                _context.Set<TEntity>().Add(entity);
            }
        }
    }
}
