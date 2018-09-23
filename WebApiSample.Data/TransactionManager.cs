using WebApiSample.Data.Context;
using WebApiSample.Data.Core;

namespace WebApiSample.Data
{
    /// <inheritdoc />
    /// <summary>
    ///   Represents transaction manager.
    /// </summary>
    public class TransactionManager : ITransactionManager
    {
        private readonly ApplicationDbContext _context;

        public TransactionManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsActive => _context != null;

        public void Begin()
        {
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Rollback()
        {
        }
    }
}
