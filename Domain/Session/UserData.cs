namespace Domain.Session;

public record UserData(
    string Id,
    string Name,
    string Email,
    string AvatarUrl);