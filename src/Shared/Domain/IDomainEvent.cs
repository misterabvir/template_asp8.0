using MediatR;

namespace Shared.Domain;


/// <summary>
/// Represents a domain event.
/// </summary>
/// <remarks>
/// Domain events are used to communicate changes in the domain model.
/// </remarks>
/// <seealso cref="INotification" />
/// <seealso cref="IDomainEvent" />
public interface IDomainEvent : INotification { }
