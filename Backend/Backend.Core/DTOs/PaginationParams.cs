namespace Backend.Core.DTOs;

public class PaginationParams
{
    public const int MaxPageSize = 50;
    public int _pageSize = 10;
    public int Page { get; set; } = 1;
    public int PageSize 
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize: value;
        
    }
    public string SortBy { get; set; } = "DateApplied";
    public bool Descending { get; set; } = true;
}