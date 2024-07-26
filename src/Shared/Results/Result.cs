namespace Shared.Results;

/// <summary>
/// Result of action pattern
/// </summary>
public class Result
{
    /// <summary>
    /// Error of Result
    /// </summary>
    public Error Error { get; set; } = Error.None;
    /// <summary>
    /// Is result success
    /// </summary>
    public bool IsSuccess => Error == Error.None;
    /// <summary>
    /// Is result failure
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Constructor for success result
    /// </summary>
    public Result() { }
    /// <summary>
    /// Constructor for failure result
    /// </summary>
    /// <param name="error"> Error of result </param>
    protected Result(Error error) => Error = error;

    /// <summary>
    /// Create Success result
    /// </summary>
    /// <returns> Result with error 'None' and status 200 </returns>
    public static Result Success() => new();
    /// <summary>
    /// Create Failure result
    /// </summary>
    /// <param name="error"> Error of result </param>
    /// <returns> Result with error and status code </returns>
    public static implicit operator Result(Error error) => new(error);  
}
