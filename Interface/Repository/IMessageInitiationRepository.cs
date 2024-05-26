using Domain.Abstraction;
using Domain.Conversation;
using Domain.Dto.Chat;
using Domain.Entity;
using LargeLanguageModelClient.Dto.Model;

namespace Interface.Repository;

public interface IMessageInitiationRepository
{
    Task<Result<NewMessageData>> InitiateMessage(
        NewMessageDto newUserMessageDto,
        LlmModelDto model,
        ConversationEntity conversationEntity,
        StreamUsage? llmStreamTotalUsage);
}
