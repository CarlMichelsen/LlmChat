using Domain.Abstraction;
using Domain.Entity;
using Domain.Entity.Id;
using Domain.Exception;
using Implementation.Database;
using Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Repository;

public class ConversationReadRepository(
    ApplicationContext applicationContext) : IConversationReadRepository
{
    public async Task<Result<ConversationEntity>> GetRichConversation(Guid creatorIdentifier, ConversationEntityId conversationId)
    {
        try
        {
            var conversation = await applicationContext.Conversations
                .Where(c => c.CreatorIdentifier == creatorIdentifier && c.Id == conversationId)
                .Include(c => c.Messages)
                    .ThenInclude(m => m.Content)
                .Include(c => c.Messages)
                    .ThenInclude(m => m.Prompt)
                .Include(c => c.Messages)
                    .ThenInclude(m => m.PreviousMessage)
                .FirstOrDefaultAsync();
            
            if (conversation is null)
            {
                return new SafeUserFeedbackException("Did not find conversation");
            }
            
            return conversation;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<List<ConversationEntity>>> GetShallowConversations(Guid creatorIdentifier, int amount)
    {
        try
        {
            var conversations = await applicationContext.Conversations
                .Where(c => c.CreatorIdentifier == creatorIdentifier)
                .OrderByDescending(c => c.LastAppendedUtc)
                .Take(amount)
                .ToListAsync();

            return conversations;
        }
        catch (Exception e)
        {
            return e;
        }
    }
}
