namespace WebApiSample.Data.Core
{
    /// <summary>
    /// Represents interface for managing transaction.
    /// </summary>
    public interface ITransactionManager
    {
        /// <summary>
        /// Indicates if transaction active
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Begins transaction.
        /// </summary>
        void Begin();

        /// <summary>
        /// Commits transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks transaction.
        /// </summary>
        void Rollback();
    }
}
