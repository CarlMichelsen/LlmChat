using Domain.Exception;

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

    private ServiceResponse()
    {
    }

    public bool Ok => this.Data is not null && this.Errors.Count == 0;

    public T? Data { get; init; }

    public List<string> Errors { get; init; } = new();

    public DateTime DateTimeUtc => DateTime.UtcNow;

    public static ServiceResponse<T> CreateErrorResponse(string error, System.Exception? exception = default)
    {
        List<string> errs = [error];
        if (exception is SafeUserFeedbackException safe)
        {
            errs.Add(safe.Message);
            if (safe.Details.Count != 0)
            {
                errs.AddRange(safe.Details);
            }
        }

        return new ServiceResponse<T>
        {
            Data = default,
            Errors = errs,
        };
    }
}
