namespace Shared.Results;

/// <summary>
/// Generic result class
/// </summary>
/// <typeparam name="TValue">Type of result action</typeparam>
public class Result<TValue> : Result
{
    /// <summary>
    /// Contains result value
    /// </summary>
    public TValue Value { get; set; } = default!;
    /// <summary>
    /// Creates successful result with value
    /// </summary>
    /// <param name="value">Result value</param>
    protected Result(TValue value) : base() => Value = value;

    /// <summary>
    /// Creates failure result with error
    /// </summary>
    /// <param name="error">Result error</param>  
    protected Result(Error error) : base(error) { }

    /// <summary>
    /// Convert value to successful result
    /// </summary>
    /// <param name="value">Value</param>
    public static implicit operator Result<TValue>(TValue value) => new(value);
    
    /// <summary>
    /// Convert error to failure result
    /// </summary>
    /// <param name="error">Error</param>
    public static implicit operator Result<TValue>(Error error) => new(error);
}

