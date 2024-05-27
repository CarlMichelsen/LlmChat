using Domain.Abstraction;
using Domain.Conversation;
using Domain.Dto.Chat;
using Domain.Entity;
using Domain.Exception;
using Implementation.Database;
using Interface.Repository;
using LargeLanguageModelClient.Dto.Model;

namespace Implementation.Repository;

public class MessageInitiationRepository(
    ApplicationContext applicationContext) : IMessageInitiationRepository
{
    public async Task<Result<NewMessageData>> InitiateMessage(
        NewMessageDto newUserMessageDto,
        LlmModelDto model,
        ConversationEntity conversationEntity,
        StreamUsage? llmStreamTotalUsage)
    {
        try
        {
            if (newUserMessageDto.ConversationId is null)
            {
                return new SafeUserFeedbackException(
                    "Failed to access conversation");
            }

            return await this.AppendExsistingConversation(
                newUserMessageDto,
                model,
                conversationEntity,
                llmStreamTotalUsage);
        }
        catch (Exception e)
        {
            return e;
        }
    }

    private async Task<Result<NewMessageData>> AppendExsistingConversation(
        NewMessageDto newUserMessageDto,
        LlmModelDto model,
        ConversationEntity conversationEntity,
        StreamUsage? llmStreamTotalUsage)
    {
        MessageEntity? responseToMessage = default;
        if (newUserMessageDto.ResponseToMessageId is not null)
        {
            responseToMessage = conversationEntity.Messages
                .FirstOrDefault(m => m.Id == newUserMessageDto.ResponseToMessageId);
            if (responseToMessage is null)
            {
                return new SafeUserFeedbackException("Did not find message to respond to");
            }
        }

        PromptEntity? prompt = default;
        if (llmStreamTotalUsage is not null)
        {
            prompt = new PromptEntity
            {
                ProviderPromptIdentifier = llmStreamTotalUsage.ProviderPromptIdentifier,
                Model = model.ModelIdentifierName,
                InputTokens = llmStreamTotalUsage.InputTokens,
                OutputTokens = llmStreamTotalUsage.OutputTokens,
                CurrentMillionInputTokenPrice = model.Price.MillionInputTokenPrice,
                CurrentMillionOutputTokenPrice = model.Price.MillionOutputTokenPrice,
            };
        }

        var newMessage = new MessageEntity
        {
            Content = newUserMessageDto.Content
                .Select(x => new ContentEntity
                {
                    ContentType = Enum.Parse<MessageContentType>(x.ContentType),
                    Content = x.Content,
                })
                .ToList(),
            Prompt = prompt,
            PreviousMessage = responseToMessage,
            CompletedUtc = DateTime.UtcNow,
        };
        
        conversationEntity.Messages.Add(newMessage);
        conversationEntity.LastAppendedUtc = DateTime.UtcNow;

        await applicationContext.SaveChangesAsync();
        return new NewMessageData
        {
            Conversation = conversationEntity,
            Message = newMessage,
        };
    }
}
