namespace WebApiSample.Data.Core.Base
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }

        public bool IsNew => Id <= 0;
    }
}
