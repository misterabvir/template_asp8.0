namespace Domain.Results;

/// <summary>
/// Http Status Codes for <see cref="Error"/>
/// </summary>
public enum Code{
    
    /// <summary>
    /// OK
    /// </summary>
    OK = 200,

    /// <summary>
    /// Bad Request
    /// </summary>
    BadRequest = 400,

    /// <summary>
    /// Unauthorized
    /// </summary>
    Unauthorized = 401,

    /// <summary>
    /// Forbidden
    /// </summary>
    Forbidden = 403,

    /// <summary>
    /// Not Found
    /// </summary>
    NotFound = 404,

    /// <summary>
    /// Conflict
    /// </summary>
    Conflict = 409,

    /// <summary>
    /// Validation Failed
    /// </summary>
    ValidationFailed = 422,

    /// <summary>
    /// Internal Server Error
    /// </summary>
    InternalServerError = 500
}