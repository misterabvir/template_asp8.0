namespace Domain.Abstractions;

/// <summary>
/// Interface for entities that contain AuthenticationService.Domain events.
/// </summary>
/// <remarks>
/// This interface is used to mark entities that contain AuthenticationService.Domain events.
/// </remarks>
public interface IContainDomainEvents
{
    /// <summary>
    /// Gets the collection of AuthenticationService.Domain events.
    /// </summary>
    /// <remarks>
    /// This property is used to get the collection of AuthenticationService.Domain events.
    /// </remarks>
    /// <returns></returns>
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Clears the collection of AuthenticationService.Domain events.
    /// </summary>
    /// <remarks>
    /// This method is used to clear the collection of AuthenticationService.Domain events after they have been processed.
    /// </remarks>
    /// <returns></returns>
    public void ClearDomainEvents();
}