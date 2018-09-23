namespace WebApiSample.Data.Core.Base
{
    /// <summary>
    /// Represents interface for base entity which has model of storing.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets the entity id.
        /// </summary>
        /// <value>The entity id value.</value>
        int Id { get; }
    }
}
