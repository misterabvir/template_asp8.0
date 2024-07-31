namespace Domain.Abstractions;

/// <summary>
/// The base class for all aggregates in the AuthenticationService.Domain.
/// </summary>
/// <typeparam name="TId">The type of the aggregate identifier.</typeparam>
/// <remarks>
/// This class implements the <see cref="IContainAuthenticationService.DomainEvents"/> interface, which allows it to store and retrieve AuthenticationService.Domain events.
/// </remarks>
/// <seealso cref="IContainAuthenticationService.DomainEvents"/>
/// <seealso cref="Entity{TId}"/>
/// <seealso cref="ValueObject"/>
public abstract class Aggregate<TId> : Entity<TId>, IContainDomainEvents where TId : ValueObject
{
    /// <summary>
    /// The collection of AuthenticationService.Domain events that occurred during the lifetime of the aggregate.
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = [];
    
    /// <summary>
    /// The collection of AuthenticationService.Domain events that occurred during the lifetime of the aggregate.
    /// </summary>
    /// <remarks>
    /// This property is initialized as an empty list and can be used to store and retrieve AuthenticationService.Domain events.
    /// </remarks>
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    /// <summary>
    /// Adds a AuthenticationService.Domain event to the collection of AuthenticationService.Domain events.
    /// </summary>
    /// <param name="AuthenticationService.DomainEvent">The AuthenticationService.Domain event to be added.</param>
    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    /// <summary>
    /// Clear all AuthenticationService.Domain events from the collection
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();
}