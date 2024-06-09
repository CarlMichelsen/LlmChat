using Domain.Abstraction;
using Domain.Conversation;
using Domain.Entity;
using Domain.Pipeline.SendMessage;

namespace Interface.Repository;

public interface IMessageInitiationRepository
{
    Task<Result<MessageEntity>> InitiateMessage(
        ValidatedSendMessageData newUserMessageDto,
        ConversationEntity conversationEntity,
        StreamUsage? llmStreamTotalUsage);
}
