using Domain.Abstraction;
using Domain.Conversation;
using Domain.Entity;
using Domain.Entity.Id;
using Domain.Exception;
using Domain.Pipeline.SendMessage;
using Interface.Repository;

namespace Implementation.Repository;

public class MessageInitiationRepository : IMessageInitiationRepository
{
    public Result<(MessageEntity Message, DialogSliceEntity DialogSlice)> InitiateMessage(
        ValidatedSendMessageData validatedSendMessageData,
        ConversationEntity conversationEntity,
        MessageEntityId? predeterminedMessageEntityId,
        StreamUsage? llmStreamTotalUsage)
    {
        (DialogSliceEntity DialogSlice, MessageEntity Message)? responseToDialogSliceAndMessage = default;
        if (validatedSendMessageData.ResponseTo is not null)
        {
            var dialogSlice = conversationEntity.DialogSlices
                .FirstOrDefault(dse => dse.Messages.Exists(m => m.Id == validatedSendMessageData.ResponseTo.MessageId));
            if (dialogSlice is null)
            {
                return new SafeUserFeedbackException("Failed to find dialog slice to respond to");
            }

            var message = dialogSlice.Messages
                .FirstOrDefault(m => m.Id == validatedSendMessageData.ResponseTo.MessageId);
            if (message is null)
            {
                return new SafeUserFeedbackException("Failed to find message to respond to");
            }

            responseToDialogSliceAndMessage = (DialogSlice: dialogSlice, Message: message);
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
        var newMessage = new MessageEntity
        {
            Id = predeterminedMessageEntityId ?? new MessageEntityId(Guid.NewGuid()),
            IsUserMessage = isUserMessage,
            Content = validatedSendMessageData.Content,
            Prompt = prompt,
            PreviousMessage = responseToDialogSliceAndMessage?.Message,
            CompletedUtc = DateTime.UtcNow,
        };

        if (responseToDialogSliceAndMessage is not null
            && newMessage.IsUserMessage
            && responseToDialogSliceAndMessage.Value.Message.IsUserMessage)
        {
            return new SafeUserFeedbackException("Unable to respond to a user message with another user message.");
        }

        var dialogSliceEntityIndex = responseToDialogSliceAndMessage?.DialogSlice is not null
            ? conversationEntity.DialogSlices.FindIndex(ds => ds.Id == responseToDialogSliceAndMessage.Value.DialogSlice.Id) + 1
            : 0;
        
        if (dialogSliceEntityIndex == 0 && !isUserMessage)
        {
            return new SafeUserFeedbackException("Initial message has to be from user.");
        }

        var currentDialogSlice = conversationEntity.DialogSlices.ElementAtOrDefault(dialogSliceEntityIndex) ?? new DialogSliceEntity
            {
                Id = new DialogSliceEntityId(Guid.NewGuid()),
                Messages = [],
                SelectedMessageGuid = default,
                CreatedUtc = DateTime.UtcNow,
            };
        currentDialogSlice.Messages.Add(newMessage);
        currentDialogSlice.SelectedMessageGuid = newMessage.Id.Value;
        conversationEntity.DialogSlices.Add(currentDialogSlice);

        conversationEntity.LastAppendedUtc = DateTime.UtcNow;
        return (newMessage, currentDialogSlice);
    }
}
