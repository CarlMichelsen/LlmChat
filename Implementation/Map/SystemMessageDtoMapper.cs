using Domain.Abstraction;
using Domain.Dto.SystemMessage;
using Domain.Entity;

namespace Implementation.Map;

public static class SystemMessageDtoMapper
{
    public static Result<SystemMessageDto> Map(SystemMessageEntity systemMessageEntity)
    {
        return new SystemMessageDto
        {
            Id = systemMessageEntity.Id.Value,
            Name = systemMessageEntity.Name,
            Content = systemMessageEntity.Content,
            LastAppendedUtc = new DateTimeOffset(systemMessageEntity.LastAppendedUtc).ToUnixTimeMilliseconds(),
            CreatedUtc = new DateTimeOffset(systemMessageEntity.CreatedUtc).ToUnixTimeMilliseconds(),
        };
    }
}
