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
    public Result<MessageEntity> InitiateMessage(
        ValidatedSendMessageData validatedSendMessageData,
        ConversationEntity conversationEntity,
        MessageEntityId? messageEntityId,
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

        var isUserMessage = prompt is null;

        if (responseToMessage is not null && isUserMessage && responseToMessage.IsUserMessage)
        {
            return new SafeUserFeedbackException("Unable to respond with a user message to another user message.");
        }

        var newMessage = new MessageEntity
        {
            Id = messageEntityId ?? new MessageEntityId(Guid.NewGuid()),
            IsUserMessage = isUserMessage,
            Content = validatedSendMessageData.Content,
            Prompt = prompt,
            PreviousMessage = responseToMessage,
            CompletedUtc = DateTime.UtcNow,
        };
        
        conversationEntity.Messages.Add(newMessage);
        conversationEntity.LastAppendedUtc = DateTime.UtcNow;

        applicationContext.SaveChanges();
        return newMessage;
    }
}
