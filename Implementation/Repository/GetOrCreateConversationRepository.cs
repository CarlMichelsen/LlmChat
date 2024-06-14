using Domain.Abstraction;
using Domain.Entity;
using Domain.Entity.Id;
using Domain.Exception;
using Implementation.Database;
using Interface.Repository;

namespace Implementation.Service;

public class GetOrCreateConversationRepository(
    ApplicationContext applicationContext,
    IConversationReadRepository conversationReadRepository,
    IProfileRepository profileRepository) : IGetOrCreateConversationRepository
{
    public async Task<Result<ConversationEntity>> GetOrCreateConversation(Guid creatorIdentifier, ConversationEntityId? conversationId)
    {
        if (conversationId is null)
        {
            var profileResult = await profileRepository
                .GetAndOrCreateProfile(new ProfileEntityId(creatorIdentifier));

            if (profileResult.IsError)
            {
                return new SafeUserFeedbackException("Failed to get and or create profile entity"); 
            }

            var profile = profileResult.Unwrap();
            var conv = new ConversationEntity
            {
                Id = new ConversationEntityId(Guid.NewGuid()),
                Creator = profile,
                SystemMessage = profile.DefaultSystemMessage,
                DialogSlices = [],
                LastAppendedUtc = DateTime.UtcNow,
                CreatedUtc = DateTime.UtcNow,
            };

            var added = applicationContext.Conversations.Add(conv);
            return added.Entity;
        }
        else
        {
            var conv = await conversationReadRepository
                .GetRichConversation(new ProfileEntityId(creatorIdentifier), conversationId!);
            
            if (conv is null)
            {
                return new SafeUserFeedbackException("Conversation not found");
            }

            return conv;
        }
    }
}
