using Domain.Abstraction;
using Domain.Entity.Id;
using Interface.Repository;
using Interface.Service;

namespace Implementation.Service;

public class ConversationDeletionService(
    ICacheService cacheService,
    ISessionService sessionService,
    IConversationDeletionRepository conversationDeletionRepository) : IConversationDeletionService
{
    public async Task<Result<bool>> DeleteConversation(ConversationEntityId conversationEntityId)
    {
        var conversationDeletionResult = await conversationDeletionRepository
            .DeleteConversation(conversationEntityId);
        
        if (conversationDeletionResult.IsError)
        {
            return conversationDeletionResult.Error!;
        }

        var convKey = CacheKeys.GenerateConversationCacheKey(sessionService.UserProfileId, conversationEntityId.Value);
        var optionKey = CacheKeys.GenerateConversationOptionsCacheKey(sessionService.UserProfileId);
        await Task.WhenAll(cacheService.Remove(convKey), cacheService.Remove(optionKey));

        return conversationDeletionResult.Unwrap();
    }
}
