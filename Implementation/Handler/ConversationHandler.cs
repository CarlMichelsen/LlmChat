using Domain.Dto;
using Domain.Dto.Conversation;
using Domain.Entity.Id;
using Domain.Exception;
using Interface.Handler;
using Interface.Repository;
using Interface.Service;

namespace Implementation.Handler;

public class ConversationHandler(
    IConversationDtoService conversationDtoService,
    IConversationOptionService conversationOptionService,
    IConversationDeletionRepository conversationDeletionRepository,
    IConversationSystemMessageRepository conversationSystemMessageRepository) : IConversationHandler
{
    public async Task<ServiceResponse<ConversationDto>> GetConversation(ConversationEntityId conversationId)
    {
        var conversationDtoResult = await conversationDtoService
            .GetConversationDto(conversationId);
        if (conversationDtoResult.IsError)
        {
            var err = MapError("Failed to get conversationDto", conversationDtoResult.Error!);
            return new ServiceResponse<ConversationDto>(err);
        }
        
        return new ServiceResponse<ConversationDto>(conversationDtoResult.Unwrap());
    }

    public async Task<ServiceResponse<bool>> DeleteConversation(ConversationEntityId conversationId)
    {
        var deleteResult = await conversationDeletionRepository
            .DeleteConversation(conversationId);
        
        if (deleteResult.IsError)
        {
            var err = MapError("Failed to delete conversation", deleteResult.Error!);
            return new ServiceResponse<bool>(err);
        }

        return new ServiceResponse<bool>(deleteResult.Unwrap());
    }

    public async Task<ServiceResponse<List<ConversationOptionDto>>> GetConversationList()
    {
        var conversationOptionsResult = await conversationOptionService.GetConversationOptions(50);
        if (conversationOptionsResult.IsError)
        {
            var err = MapError("Failed to get conversationOptions", conversationOptionsResult.Error!);
            return new ServiceResponse<List<ConversationOptionDto>>(err);
        }

        return new ServiceResponse<List<ConversationOptionDto>>(conversationOptionsResult.Unwrap());
    }

    public async Task<ServiceResponse<string>> SetConversationSystemMessage(ConversationEntityId conversationId, string newSystemMessage)
    {
        var setSystemMessageResult = await conversationSystemMessageRepository
            .SetConversationSystemMessage(conversationId, newSystemMessage);
        
        if (setSystemMessageResult.IsError)
        {
            return ServiceResponse<string>.CreateErrorResponse("Failed to set system message");
        }

        return new ServiceResponse<string>(setSystemMessageResult.Unwrap());
    }

    private static string MapError(string initial, Exception e)
    {
        if (e is SafeUserFeedbackException safe)
        {
            return $"{initial} -> {safe.Message}";
        }

        return initial;
    }
}
