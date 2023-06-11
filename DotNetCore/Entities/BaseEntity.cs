namespace DotNetCore.Entities
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; protected set; }
        public bool IsDeleted { get; set; } = false;
        public virtual string CreatorId { get; protected set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public BaseEntity(string creatorId)
        {
            CreatorId = creatorId;
        }
    }
}
