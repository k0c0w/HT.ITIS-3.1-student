namespace Dotnet.Homeworks.Shared.Dto;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }

    public Result(bool isSuccessful, string? error = default)
    {
        IsSuccess = isSuccessful;
        if (error is not null) 
            Error = error;
    }

    public static implicit operator Result(bool isSuccessful)
    {
        return new Result(isSuccessful);
    }

    public static implicit operator Result(string errors)
    {
        return new Result(false, errors);
    }
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? val, bool isSuccessful, string? error = default)
        : base(isSuccessful, error)
    {
        _value = val;
    }

    public TValue? Value => IsSuccess
        ? _value
        : throw new Exception(Error);

    public static implicit operator Result<TValue>(TValue value)
    {
        return new Result<TValue>(value, true);
    }

    public static implicit operator Result<TValue>(bool isSuccessful)
    {
        return new Result<TValue>(default, isSuccessful);
    }

    public static implicit operator Result<TValue>(string errors)
    {
        return new Result<TValue>(default, false, errors);
    }
}