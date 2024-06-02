using Domain.Abstraction;
using Domain.Dto.Conversation;
using Domain.Entity;
using Interface.Repository;
using Interface.Service;

namespace Implementation.Service;

public class ConversationOptionService(
    IConversationReadRepository conversationReadRepository,
    ISessionService sessionService) : IConversationOptionService
{
    public async Task<Result<List<ConversationOptionDto>>> GetConversationOptions(int amount)
    {
        var creatorIdentifier = sessionService.UserProfileId;
        var conversationsResult = await conversationReadRepository
            .GetShallowConversations(creatorIdentifier, amount);
        if (conversationsResult.IsError)
        {
            return conversationsResult.Error!;
        }

        return conversationsResult
            .Unwrap()
            .Select(Map)
            .ToList();
    }

    private static ConversationOptionDto Map(ConversationEntity conversationEntity)
    {
        return new ConversationOptionDto
        {
            Id = conversationEntity.Id,
            Summary = conversationEntity.Summary,
            LastAppendedUtc = conversationEntity.LastAppendedUtc,
            CreatedUtc = conversationEntity.CreatedUtc,
        };
    }
}
