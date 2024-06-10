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
    IConversationSystemMessageRepository conversationSystemMessageRepository) : IConversationHandler
{
    public async Task<ServiceResponse<ConversationDto>> GetConversation(ConversationEntityId conversationId)
    {
        var conversationDtoResult = await conversationDtoService
            .GetConversationDto(conversationId);
        if (conversationDtoResult.IsError)
        {
            var err = MapError("Failed to get conversationDto", conversationDtoResult.Error!);
            var errRes = new ServiceResponse<ConversationDto>(err);
            return errRes;
        }
        
        var conv = conversationDtoResult.Unwrap();
        var res = new ServiceResponse<ConversationDto>(conv);
        return res;
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
