using Domain.Abstraction;
using Domain.Conversation;
using Domain.Entity;
using Domain.Entity.Id;
using Domain.Pipeline.SendMessage;

namespace Interface.Repository;

public interface IMessageInitiationRepository
{
    Task<Result<MessageEntity>> InitiateMessage(
        ValidatedSendMessageData newUserMessageDto,
        ConversationEntity conversationEntity,
        MessageEntityId? messageEntityId,
        StreamUsage? llmStreamTotalUsage);
}
