using Domain.Abstraction;
using Domain.Entity.Id;
using Domain.Exception;
using Implementation.Database;
using Interface.Repository;

namespace Implementation.Repository;

public class ConversationDeletionRepository(
    ApplicationContext applicationContext) : IConversationDeletionRepository
{
    public async Task<Result<bool>> DeleteConversation(ConversationEntityId conversationEntityId)
    {
        try
        {
            var conversation = applicationContext.Conversations
                .FirstOrDefault(c => c.Id == conversationEntityId);
            
            if (conversation is null)
            {
                return new SafeUserFeedbackException("Could not find conversation to delete");
            }

            applicationContext.Conversations.Remove(conversation);

            var deleted = await applicationContext.SaveChangesAsync();
            return deleted == 1;
        }
        catch (System.Exception e)
        {
            return e;
        }
    }
}
