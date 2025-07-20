namespace OrderManagement.Domain;

public interface ISoftDeletable
{
    DateTimeOffset? DeletedAt { get; set; }
}