namespace Shared.Domain;

/// <summary>
/// Interface for entities that contain domain events.
/// </summary>
/// <remarks>
/// This interface is used to mark entities that contain domain events.
/// </remarks>
public interface IContainDomainEvents
{
    /// <summary>
    /// Gets the collection of domain events.
    /// </summary>
    /// <remarks>
    /// This property is used to get the collection of domain events.
    /// </remarks>
    /// <returns></returns>
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Clears the collection of domain events.
    /// </summary>
    /// <remarks>
    /// This method is used to clear the collection of domain events after they have been processed.
    /// </remarks>
    /// <returns></returns>
    public void ClearDomainEvents();
}