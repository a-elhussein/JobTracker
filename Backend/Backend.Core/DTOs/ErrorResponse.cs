namespace Backend.Core.DTOs;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public IReadOnlyDictionary<string, string[]>? Errors { get; init; }
    public string TraceId { get; set; } =  string.Empty;
}