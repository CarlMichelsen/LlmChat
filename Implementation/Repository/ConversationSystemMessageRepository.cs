using Domain.Abstraction;
using Domain.Entity.Id;
using Domain.Exception;
using Implementation.Database;
using Interface.Repository;

namespace Implementation.Repository;

public class ConversationSystemMessageRepository(
    ApplicationContext applicationContext) : IConversationSystemMessageRepository
{
    public async Task<Result<string>> SetConversationSystemMessage(
        ConversationEntityId conversationId,
        string systemMessage)
    {
        if (string.IsNullOrWhiteSpace(systemMessage))
        {
            return new SafeUserFeedbackException("Invalid system message");
        }

        var conversation = await applicationContext.Conversations
            .FindAsync(conversationId);

        if (conversation is null)
        {
            return new SafeUserFeedbackException("Failed to find conversation in order to set system message");
        }

        conversation.SystemMessage = systemMessage;
        await applicationContext.SaveChangesAsync();
        
        return conversation.SystemMessage;
    }
}
