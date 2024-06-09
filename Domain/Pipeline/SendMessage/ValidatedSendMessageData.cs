using Domain.Entity;
using Domain.Entity.Id;
using LargeLanguageModelClient.Dto.Model;

namespace Domain.Pipeline.SendMessage;

public class ValidatedSendMessageData
{
    public required ConversationEntityId? RequestConversationId { get; init; }

    public required MessageEntityId? ResponseToMessageId { get; init; }

    public required LlmModelDto SelectedModel { get; init; }

    public required List<ContentEntity> Content { get; init; }
}
