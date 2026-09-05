namespace CIOT.Common.Domain;

public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}

public interface IConcurrentEntity
{
    long RowVersion { get; set; }
}

public interface ICreatedAuditableEntity
{
    DateTime CreatedAtUtc { get; set; }
    Guid? CreatedByUserId { get; set; }
}

public interface IUpdatedAuditableEntity
{
    DateTime? ModifiedAtUtc { get; set; }
    Guid? ModifiedByUserId { get; set; }
}

public interface ISoftDeletableEntity
{
    bool IsActive { get; set; }
}

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void AddDomainEvent(IDomainEvent domainEvent);
    void ClearDomainEvents();
}

public abstract class EntityBase : ICreatedAuditableEntity, IUpdatedAuditableEntity, IConcurrentEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Guid? CreatedByUserId { get; set; }

    public DateTime? ModifiedAtUtc { get; set; }
    public Guid? ModifiedByUserId { get; set; }

    public string? SourceSystem { get; set; }
    public string? SourceRecordId { get; set; }

    public long RowVersion { get; set; }
}

public abstract class AggregateRoot : EntityBase, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public bool Equals(ValueObject? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }
}
