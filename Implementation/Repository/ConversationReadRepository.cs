﻿using Domain.Abstraction;
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
    public async Task<Result<ConversationEntity>> GetRichConversation(ProfileEntityId creatorIdentifier, ConversationEntityId conversationId)
    {
        try
        {
            var conversation = await applicationContext.Conversations
                .Include(c => c.Creator)
                .Where(c => c.Creator.Id == creatorIdentifier && c.Id == conversationId)
                .Include(c => c.DialogSlices)
                    .ThenInclude(d => d.Messages)
                        .ThenInclude(m => m.Content)
                .Include(c => c.DialogSlices)
                    .ThenInclude(d => d.Messages)
                        .ThenInclude(m => m.Prompt)
                .Include(c => c.DialogSlices)
                    .ThenInclude(d => d.Messages)
                        .ThenInclude(m => m.PreviousMessage)
                .FirstOrDefaultAsync();
            
            if (conversation is null)
            {
                return new SafeUserFeedbackException("Did not find conversation");
            }

            conversation.DialogSlices = conversation.DialogSlices
                .OrderBy(ds => ds.CreatedUtc)
                .Select(ds =>
                {
                    ds.Messages.Sort((m1, m2) => m1.CompletedUtc.CompareTo(m2.CompletedUtc));
                    return ds;
                })
                .ToList();
            return conversation;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<List<ConversationEntity>>> GetShallowConversations(ProfileEntityId creatorIdentifier, int amount)
    {
        try
        {
            var conversations = await applicationContext.Conversations
                .Include(c => c.Creator)
                .Where(c => c.Creator.Id == creatorIdentifier)
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
