namespace Domain.Dto;

public class ServiceResponse<T>
{
    public ServiceResponse(T data)
    {
        this.Data = data;
    }

    public ServiceResponse(params string[] errors)
    {
        this.Data = default;
        this.Errors = new List<string>(errors);
    }

    public bool Ok => this.Data is not null && this.Errors.Count == 0;

    public T? Data { get; init; }

    public List<string> Errors { get; init; } = new();

    public DateTime DateTimeUtc => DateTime.UtcNow;
}
