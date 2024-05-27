using Domain.Abstraction;
using Domain.Conversation;
using Domain.Dto.Chat;
using Domain.Entity;
using Interface.Repository;
using LargeLanguageModelClient.Dto.Model;

namespace Implementation.Repository;

public class ConversationRepository(
    IMessageInitiationRepository messageInitiationRepository,
    IGetOrCreateConversationRepository getOrCreateConversationRepository) : IConversationRepository
{
    public Task<Result<NewMessageData>> InitiateMessage(NewMessageDto newUserMessageDto, LlmModelDto model, ConversationEntity conversationEntity, StreamUsage? llmStreamTotalUsage)
    {
        return messageInitiationRepository.InitiateMessage(newUserMessageDto, model, conversationEntity, llmStreamTotalUsage);
    }

    public Task<Result<ConversationEntity>> GetOrCreateConversation(Guid creatorIdentifier, long? conversationId)
    {
        return getOrCreateConversationRepository.GetOrCreateConversation(creatorIdentifier, conversationId);
    }
}
