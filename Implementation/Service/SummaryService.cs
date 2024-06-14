using Domain.Abstraction;
using Domain.Configuration;
using Domain.Entity;
using Domain.Entity.Id;
using Domain.Exception;
using Interface.Service;
using LargeLanguageModelClient;
using LargeLanguageModelClient.Dto.Prompt;
using LargeLanguageModelClient.Dto.Prompt.Content;
using Microsoft.Extensions.Options;

namespace Implementation.Service;

public class SummaryService(
    IOptions<ConversationOptions> conversationOptions,
    ILargeLanguageModelClient largeLanguageModelClient) : ISummaryService
{
    public async Task<Result<string>> GenerateSummary(ConversationEntity conversationEntity, MessageEntityId fromMessageId)
    {
        var summaryModelIdentifier = conversationOptions.Value.SummaryModelIdentifier;
        var prompt = this.GeneratePrompt(summaryModelIdentifier, conversationEntity, fromMessageId);

        var response = await largeLanguageModelClient.Prompt(prompt, CancellationToken.None);
        if (!response.Ok)
        {
            return new SafeUserFeedbackException(string.Join(", ", response.Errors));
        }

        var text = response.Data?.Message.Content.FirstOrDefault(c => c.Type == LlmContentType.Text)?.GetContent();
        if (string.IsNullOrWhiteSpace(text))
        {
            return new SafeUserFeedbackException("No text content in summary response");
        }

        return text;
    }

    private static LlmContent Map(ContentEntity contentEntity)
    {
        return contentEntity.ContentType switch
        {
            MessageContentType.Text => new LlmTextContent { Text = contentEntity.Content },
            MessageContentType.Image => new LlmImageContent { MediaType = "image/jpeg", Data = contentEntity.Content },
            _ => throw new NotImplementedException("Failed to map ContentEntity to LlmContent when generating summary prompt"),
        };
    }

    private LlmPromptDto GeneratePrompt(Guid modelIdentifier, ConversationEntity conversationEntity, MessageEntityId fromMessageId)
    {
        var messages = conversationEntity.DialogSlices
            .SelectMany(ds => ds.Messages)
            .ToList();

        var currentMessage = messages.FirstOrDefault(m => m.Id == fromMessageId);
        if (currentMessage is null)
        {
            throw new InvalidOperationException("Unable to find branch-message to generate summary");
        }

        if (currentMessage.IsUserMessage)
        {
            throw new InvalidOperationException("Unable to generate summary from user branch-message");
        }

        var promptMessageEntities = new List<MessageEntity>();
        do
        {
            promptMessageEntities.Insert(0, currentMessage);
            if (currentMessage.PreviousMessage is null)
            {
                break;
            }

            currentMessage = messages.FirstOrDefault(m => m.Id == currentMessage.PreviousMessage.Id);
        }
        while (currentMessage is not null);
        var promptMessages = promptMessageEntities
            .Select(m => new LlmPromptMessageDto(
                IsUserMessage: m.IsUserMessage,
                Content: m.Content.Select(Map).ToList()))
            .ToList();
        
        var summaryRequest = "Summarize the conversation so far, briefly. No more than 10 words. Shorter if possible.";
        promptMessages.Add(new LlmPromptMessageDto(IsUserMessage: true, Content: [new LlmTextContent { Text = summaryRequest }]));

        var system = "Keep track of the contents of this conversation. If the final message asks you to summarize the contents of the conversation. Do as you're asked and ignore the summarization request in your summary. The summary should make the conversation easily identifiable";
        return new LlmPromptDto(
            ModelIdentifier: modelIdentifier,
            SystemMessage: system,
            Messages: promptMessages);
    }
}