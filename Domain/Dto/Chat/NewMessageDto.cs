using Domain.Dto.Conversation;

namespace Domain.Dto.Chat;

public record NewMessageDto(
    string? ConversationId,
    string? ResponseToMessageId,
    List<MessageContentDto> Content,
    Guid ModelIdentifier);
