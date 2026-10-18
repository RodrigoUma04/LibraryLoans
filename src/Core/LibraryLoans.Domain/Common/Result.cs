namespace LibraryLoans.Domain.Common;

public enum ErrorType { Validation, NotFound, Conflict }

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<string> Errors { get; }
    public ErrorType? ErrorType { get; }

    protected Result(bool isSuccess, IReadOnlyList<string> errors, ErrorType? errorType)
    {
        IsSuccess = isSuccess;
        Errors = errors;
        ErrorType = errorType;
    }

    public static Result Ok() => new(true, [], null);
    public static Result Fail(string error, ErrorType errorType) => new(false, [error], errorType);
    public static Result Fail(IReadOnlyList<string> errors, ErrorType errorType) => new(false, errors, errorType);
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, IReadOnlyList<string> errors, ErrorType? errorType)
        : base(isSuccess, errors, errorType) => Value = value;

    public static Result<T> Ok(T value) => new(true, value, [], null);
    public new static Result<T> Fail(string error, ErrorType errorType) => new(false, default, [error], errorType);
    public new static Result<T> Fail(IReadOnlyList<string> errors, ErrorType errorType) => new(false, default, errors, errorType);
}