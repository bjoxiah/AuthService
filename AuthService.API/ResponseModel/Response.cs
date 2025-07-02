namespace AuthService.API.ResponseModel;

public class Response<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public string? Message { get; set; }

    public static Response<T> Ok(T data, string? message = null) => new()
    {
        Success = true,
        Data = data,
        Message = message
    };

    public static Response<T> Fail(List<string> errors, string? message = null) => new()
    {
        Success = false,
        Errors = errors,
        Message = message
    };

    public static Response<T> Fail(string error, string? message = null) => new()
    {
        Success = false,
        Errors = new List<string> { error },
        Message = message
    };
}
