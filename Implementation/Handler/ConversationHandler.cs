using Domain.Dto;
using Domain.Dto.Conversation;
using Domain.Exception;
using Interface.Handler;
using Interface.Service;

namespace Implementation.Handler;

public class ConversationHandler(
    IConversationOptionService conversationOptionService) : IConversationHandler
{
    public Task<ServiceResponse<object>> GetConversation()
    {
        throw new NotImplementedException();
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

    private static string MapError(string error, Exception? e = default)
    {
        if (e is SafeUserFeedbackException safe)
        {
            return $"{error} -> {safe.Message}";
        }

        return error;
    }
}
