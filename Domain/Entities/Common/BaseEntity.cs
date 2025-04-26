using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain.Entities.Common;

public class BaseEntity<T>
{
    public T? Id { get; set; }

    public TimeOnly CreatedTime { get; set; }=TimeOnly.FromDateTime(DateTime.UtcNow);
    public TimeOnly UpdatedTime { get; set; }=TimeOnly.FromDateTime(DateTime.UtcNow);
    public DateOnly CreateDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateOnly? UpdateDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public bool IsDelete { get; set; }
}

public class BaseEntity : BaseEntity<string>
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid().ToString("N");
    }
};