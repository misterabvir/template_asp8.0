namespace Domain.Results;

/// <summary>
/// Error for <see cref="Result"/> 
/// </summary>
/// <param name="Code">Error Code</param>
/// <param name="Description">Error description</param>
public record Error(Code Code, string Description)
{
    /// <summary>
    /// No Error
    /// </summary>
    public static readonly Error None = new(Code.OK, string.Empty);
    /// <summary> Bad Request Error </summary>
    public static Error BadRequest(string description) => new(Code.BadRequest, description);
    /// <summary> Unauthorized Error </summary>
    public static Error Unauthorized(string description) => new(Code.Unauthorized, description);
    /// <summary> Forbidden Error </summary>
    public static Error Forbidden(string description) => new(Code.Forbidden, description);
    /// <summary> Not Found Error </summary>
    public static Error NotFound(string description) => new(Code.NotFound, description);
    /// <summary> Conflict Error </summary>
    public static Error Conflict(string description) => new(Code.Conflict, description);
    /// <summary> Validation Failed Error </summary>
    public static Error ValidationFailed(string description) => new(Code.ValidationFailed, description);
    /// <summary> Internal Server Error </summary>
    public static Error InternalServerError(string description) => new(Code.InternalServerError, description);


}
