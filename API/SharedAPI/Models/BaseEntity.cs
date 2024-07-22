namespace API.SharedAPI.Models;

public abstract class BaseEntity
{
    public virtual DateTime CreatedAt { get; set; }
    public virtual DateTime LastModified { get; set; }
}