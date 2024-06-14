using Domain.Abstraction;
using Domain.Dto.Conversation;
using Domain.Entity.Id;
using Implementation.Map;
using Interface.Repository;
using Interface.Service;

namespace Implementation.Service;

public class ConversationDtoService(
    IConversationDtoCacheService conversationDtoCacheService,
    ISessionService sessionService,
    IConversationReadRepository conversationReadRepository) : IConversationDtoService
{
    public async Task<Result<ConversationDto>> GetConversationDto(ConversationEntityId conversationId)
    {
        var cached = await conversationDtoCacheService
            .GetConversationDto(conversationId);
        if (cached is not null)
        {
            return cached;
        }

        var dtoResult = await this.GetConversationDtoDirect(conversationId);
        if (dtoResult.IsError)
        {
            return dtoResult.Error!;
        }

        var dto = dtoResult.Unwrap();
        await conversationDtoCacheService.CacheConversationDto(dto);
        return dto;
    }

    private async Task<Result<ConversationDto>> GetConversationDtoDirect(ConversationEntityId conversationId)
    {
        var creatorIdentifier = sessionService.UserProfileId;
        var conversationResult = await conversationReadRepository
            .GetRichConversation(new ProfileEntityId(creatorIdentifier), conversationId);
        if (conversationResult.IsError)
        {
            return conversationResult.Error!;
        }

        var mapResult = ConversationDtoMapper.Map(conversationResult.Unwrap());
        if (mapResult.IsError)
        {
            return mapResult.Error!;
        }

        return mapResult.Unwrap();
    }
}