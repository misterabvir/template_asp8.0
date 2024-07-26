namespace Shared.Domain;

/// <summary>
/// The base class for all aggregates in the domain.
/// </summary>
/// <typeparam name="TId">The type of the aggregate identifier.</typeparam>
/// <remarks>
/// This class implements the <see cref="IContainDomainEvents"/> interface, which allows it to store and retrieve domain events.
/// </remarks>
/// <seealso cref="IContainDomainEvents"/>
/// <seealso cref="Entity{TId}"/>
/// <seealso cref="ValueObject"/>
public abstract class Aggregate<TId> : Entity<TId>, IContainDomainEvents where TId : ValueObject
{
    /// <summary>
    /// The collection of domain events that occurred during the lifetime of the aggregate.
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = [];
    
    /// <summary>
    /// The collection of domain events that occurred during the lifetime of the aggregate.
    /// </summary>
    /// <remarks>
    /// This property is initialized as an empty list and can be used to store and retrieve domain events.
    /// </remarks>
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    /// <summary>
    /// Adds a domain event to the collection of domain events.
    /// </summary>
    /// <param name="domainEvent">The domain event to be added.</param>
    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    /// <summary>
    /// Clear all domain events from the collection
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();
}