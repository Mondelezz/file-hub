namespace Domain.Common;

public class EntityBase : IEntityId, IEntityDate, IEntityState
{
    public long Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public bool IsDeleted { get; set; }
}
