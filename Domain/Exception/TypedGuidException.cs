namespace Domain.Exception;

public class TypedGuidException : System.Exception
{
    public TypedGuidException(string message)
        : base(message)
    {
    }

    public TypedGuidException(string? message, System.Exception? innerException)
        : base(message, innerException)
    {
    }
}
