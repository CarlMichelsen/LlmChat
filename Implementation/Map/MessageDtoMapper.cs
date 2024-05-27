using Domain.Dto.Conversation;
using Domain.Entity;

namespace Implementation.Map;

public static class MessageDtoMapper
{
    public static MessageDto Map(MessageEntity messageEntity)
    {
        return new MessageDto
        {
            Id = messageEntity.Id.ToString(),
            Content = messageEntity.Content.Select(Map).ToList(),
            CompletedUtc = messageEntity.CompletedUtc,
        };
    }

    public static MessageContentDto Map(ContentEntity contentEntity)
    {
        return new MessageContentDto
        {
            ContentType = Enum.GetName(contentEntity.ContentType)!,
            Content = contentEntity.Content,
        };
    }
}
