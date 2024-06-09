using Domain.Abstraction;
using Domain.Conversation;
using Domain.Entity;
using Domain.Entity.Id;
using Domain.Exception;
using Domain.Pipeline.SendMessage;
using Implementation.Database;
using Interface.Repository;

namespace Implementation.Repository;

public class MessageInitiationRepository(
    ApplicationContext applicationContext) : IMessageInitiationRepository
{
    public async Task<Result<MessageEntity>> InitiateMessage(
        ValidatedSendMessageData validatedSendMessageData,
        ConversationEntity conversationEntity,
        StreamUsage? llmStreamTotalUsage)
    {
        MessageEntity? responseToMessage = default;
        if (validatedSendMessageData.ResponseToMessageId is not null)
        {
            responseToMessage = conversationEntity.Messages
                .FirstOrDefault(m => m.Id == validatedSendMessageData.ResponseToMessageId);
            if (responseToMessage is null)
            {
                return new SafeUserFeedbackException("Failed to find message to respond to");
            }
        }

        PromptEntity? prompt = default;
        if (llmStreamTotalUsage is not null)
        {
            prompt = new PromptEntity
            {
                Id = new PromptEntityId(Guid.NewGuid()),
                ProviderPromptIdentifier = llmStreamTotalUsage.ProviderPromptIdentifier,
                ModelName = validatedSendMessageData.SelectedModel.ModelIdentifierName,
                ModelId = validatedSendMessageData.SelectedModel.Id,
                Model = validatedSendMessageData.SelectedModel.ModelIdentifierName,
                InputTokens = llmStreamTotalUsage.InputTokens,
                OutputTokens = llmStreamTotalUsage.OutputTokens,
                CurrentMillionInputTokenPrice = validatedSendMessageData.SelectedModel.Price.MillionInputTokenPrice,
                CurrentMillionOutputTokenPrice = validatedSendMessageData.SelectedModel.Price.MillionOutputTokenPrice,
                StopReason = llmStreamTotalUsage.StopReason,
            };
        }

        var newMessage = new MessageEntity
        {
            Id = new MessageEntityId(Guid.NewGuid()),
            Content = validatedSendMessageData.Content,
            Prompt = prompt,
            PreviousMessage = responseToMessage,
            CompletedUtc = DateTime.UtcNow,
        };
        
        conversationEntity.Messages.Add(newMessage);
        conversationEntity.LastAppendedUtc = DateTime.UtcNow;

        await applicationContext.SaveChangesAsync();
        return newMessage;
    }
}
