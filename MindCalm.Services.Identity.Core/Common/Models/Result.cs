namespace MindCalm.Services.Identity.Core.Common.Models;

public class Result<T> where T : class
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public IEnumerable<string>? Errors { get; set; }

    private Result(bool isSuccess, T? data, string? message, IEnumerable<string>? errors)
    {
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
        Errors = errors;
    }

    public static Result<T> Success(T data, string? message = null)
    {
        return new Result<T>(true, data, message, null);
    }

    public static Result<T> Failed(T? data = null, string? message = null, IEnumerable<string>? errors = null)
    {
        return new Result<T>(false, data, message, errors);
    }
}