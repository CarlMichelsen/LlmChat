using Domain.Abstraction;
using Domain.Entity;
using Domain.Entity.Id;
using Domain.Exception;
using Implementation.Database;
using Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Service;

public class GetOrCreateConversationRepository(
    ApplicationContext applicationContext) : IGetOrCreateConversationRepository
{
    public async Task<Result<ConversationEntity>> GetOrCreateConversation(Guid creatorIdentifier, ConversationEntityId? conversationId)
    {
        if (conversationId is null)
        {
            var conv = new ConversationEntity
            {
                Id = new ConversationEntityId(Guid.NewGuid()),
                CreatorIdentifier = creatorIdentifier,
                Messages = [],
                LastAppendedUtc = DateTime.UtcNow,
                CreatedUtc = DateTime.UtcNow,
            };

            var added = await applicationContext.Conversations.AddAsync(conv);
            await applicationContext.SaveChangesAsync();
            return added.Entity;
        }
        else
        {
            var conv = await this.GetConversationEntity(creatorIdentifier, conversationId!);
            if (conv is null)
            {
                return new SafeUserFeedbackException("Conversation not found");
            }

            return conv;
        }
    }

    private Task<ConversationEntity?> GetConversationEntity(
        Guid creatorIdentifier,
        ConversationEntityId conversationId)
    {
        return applicationContext.Conversations
            .Where(c => c.CreatorIdentifier == creatorIdentifier && c.Id == conversationId)
            .Include(c => c.Messages)
                .ThenInclude(m => m.Content)
            .Include(c => c.Messages)
                .ThenInclude(m => m.Prompt)
            .Include(c => c.Messages)
                .ThenInclude(m => m.PreviousMessage)
            .FirstOrDefaultAsync();
    }
}
