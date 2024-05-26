using Domain.Dto.Chat.Stream;

namespace Domain.Dto.Chat;

public record NewMessageDto(
    long? ConversationId,
    long? ResponseToMessageId,
    List<ContentDto> Content,
    Guid ModelIdentifier);
