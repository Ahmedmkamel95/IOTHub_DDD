namespace CIOT.Common.Results;

public record Error(string Code, string Message, int StatusCode = 400)
{
    public static readonly Error None = new(string.Empty, string.Empty, 200);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.", 400);

    public static Error NotFound(string code, string message) => new(code, message, 404);
    public static Error Validation(string code, string message) => new(code, message, 400);
    public static Error Conflict(string code, string message) => new(code, message, 409);
    public static Error Unauthorized(string code, string message) => new(code, message, 401);
    public static Error Forbidden(string code, string message) => new(code, message, 403);
    public static Error Failure(string code, string message) => new(code, message, 500);
}

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Successful result cannot have an error.");
        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Failed result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}
