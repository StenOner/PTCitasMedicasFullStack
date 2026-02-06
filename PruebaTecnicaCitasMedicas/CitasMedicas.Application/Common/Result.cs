namespace CitasMedicas.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = [];

    public static Result<T> Success(T data, string message = "Operación exitosa")
    {
        return new Result<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message,
        };
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Errors = [error],
        };
    }

    public static Result<T> Failure(List<string> errors)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Errors = errors,
        };
    }
}