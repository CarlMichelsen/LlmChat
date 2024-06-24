using Domain.Abstraction;
using Domain.Dto.Conversation;
using Domain.Entity;
using Domain.Entity.Id;
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
            .GetShallowConversations(new ProfileEntityId(creatorIdentifier), amount);
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
            Id = conversationEntity.Id.Value,
            Summary = conversationEntity.Summary,
            LastAppendedEpoch = new DateTimeOffset(conversationEntity.LastAppendedUtc).ToUnixTimeSeconds(),
            CreatedEpoch = new DateTimeOffset(conversationEntity.CreatedUtc).ToUnixTimeSeconds(),
        };
    }
}
