namespace Shared.Results;

public class Result
{
    public Error Error { get; set; } = Error.None;
    public bool IsSuccess => Error == Error.None;
    public bool IsFailure => !IsSuccess;
    protected Result() { }
    protected Result(Error error) => Error = error;
    public static Result Success() => new();
    public static implicit operator Result(Error error) => new(error);
    
}

public class Result<TValue> : Result
{
    public TValue Value { get; set; } = default!;
    protected Result(TValue value) : base() => Value = value;
    protected Result(Error error) : base(error) { }
    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Error error) => new(error);
}

public record Error(Code Code, string Description)
{
    public static readonly Error None = new(Code.OK, string.Empty);
    public static Error BadRequest(string description) => new(Code.BadRequest, description);
    public static Error Unauthorized(string description) => new(Code.Unauthorized, description);
    public static Error Forbidden(string description) => new(Code.Forbidden, description);
    public static Error NotFound(string description) => new(Code.NotFound, description);
    public static Error Conflict(string description) => new(Code.Conflict, description);
    public static Error ValidationFailed(string description) => new(Code.ValidationFailed, description);
    public static Error InternalServerError(string description) => new(Code.InternalServerError, description);
}

public enum Code{
    OK = 200,
    Created = 201,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    ValidationFailed = 422,
    InternalServerError = 500
}