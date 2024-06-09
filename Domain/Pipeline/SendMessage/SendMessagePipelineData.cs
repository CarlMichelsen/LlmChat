using Domain.Conversation;
using Domain.Dto.Chat;
using Domain.Dto.Conversation;
using Domain.Entity;

namespace Domain.Pipeline.SendMessage;

public class SendMessagePipelineData
{
    public required NewMessageDto NewUserMessageDto { get; init; }
    
    public ValidatedSendMessageData? ValidatedSendMessageData { get; set; }

    public ConversationEntity? Conversation { get; set; }

    public MessageEntity? UserMessage { get; set; }

    public List<MessageContentDto>? ResponseMessageContent { get; set; }

    public StreamUsage? StreamUsage { get; set; }

    public MessageEntity? ResponseMessage { get; set; }

    public Guid NextMessageIdentifier { get; set; }
}
