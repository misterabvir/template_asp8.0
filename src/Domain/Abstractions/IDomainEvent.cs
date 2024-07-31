using MediatR;

namespace Domain.Abstractions;


/// <summary>
/// Represents a AuthenticationService.Domain event.
/// </summary>
/// <remarks>
/// AuthenticationService.Domain events are used to communicate changes in the AuthenticationService.Domain model.
/// </remarks>
/// <seealso cref="INotification" />
/// <seealso cref="IAuthenticationService.DomainEvent" />
public interface IDomainEvent : INotification { }
