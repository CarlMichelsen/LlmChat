using Domain.Abstraction;
using Domain.Entity.Id;
using Interface.Repository;
using Interface.Service;

namespace Implementation.Service;

public class ConversationSystemMessageService(
    ICacheService cacheService,
    ISessionService sessionService,
    IConversationSystemMessageRepository conversationSystemMessageRepository) : IConversationSystemMessageService
{
    public async Task<Result<string>> SetConversationSystemMessage(
        ConversationEntityId conversationId,
        string systemMessage)
    {
        try
        {
            var systemMessageResult = await conversationSystemMessageRepository
                .SetConversationSystemMessage(conversationId, systemMessage);
            
            if (systemMessageResult.IsError)
            {
                return systemMessageResult.Error!;
            }

            var convKey = CacheKeys.GenerateConversationCacheKey(sessionService.UserProfileId, conversationId.Value);
            var optionKey = CacheKeys.GenerateConversationOptionsCacheKey(sessionService.UserProfileId);
            await Task.WhenAll(cacheService.Remove(convKey), cacheService.Remove(optionKey));

            return systemMessageResult.Unwrap();
        }
        catch (System.Exception e)
        {
            return e;
        }
    }
}
