using Domain.Dto.Conversation;
using Domain.Entity;
using Domain.Entity.Id;

namespace Implementation.Map;

public static class MessageDtoMapper
{
    public static MessageDto Map(MessageEntity messageEntity)
    {
        return new MessageDto
        {
            Id = messageEntity.Id.Value,
            Prompt = messageEntity.Prompt is null ? null : Map(messageEntity.Prompt),
            Content = messageEntity.Content.Select(Map).ToList(),
            CompletedUtc = messageEntity.CompletedUtc,
            PreviousMessageId = messageEntity.PreviousMessage?.Id.Value,
        };
    }

    public static PromptDto Map(PromptEntity promptEntity)
    {
        return new PromptDto
        {
            ModelName = promptEntity.Model,
            ModelId = promptEntity.ModelId,
            ProviderPromptIdentifier = promptEntity.ProviderPromptIdentifier,
            InputTokens = (int)promptEntity.InputTokens,
            OutputTokens = (int)promptEntity.OutputTokens,
            StopReason = promptEntity.StopReason,
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

    public static ContentEntity Map(MessageContentDto messageContentDto, ContentEntityId? contentEntityId = default)
    {
        return new ContentEntity
        {
            Id = contentEntityId ?? new ContentEntityId(Guid.NewGuid()),
            ContentType = Enum.Parse<MessageContentType>(messageContentDto.ContentType),
            Content = messageContentDto.Content,
        };
    }
}
