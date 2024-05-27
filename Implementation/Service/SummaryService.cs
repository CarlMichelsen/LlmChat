using Domain.Abstraction;
using Domain.Configuration;
using Domain.Entity;
using Domain.Exception;
using Implementation.Database;
using Interface.Service;
using LargeLanguageModelClient;
using LargeLanguageModelClient.Dto.Prompt;
using LargeLanguageModelClient.Dto.Prompt.Content;
using Microsoft.Extensions.Options;

namespace Implementation.Service;

public class SummaryService(
    IOptions<ConversationOptions> conversationOptions,
    ILargeLanguageModelClient largeLanguageModelClient,
    ApplicationContext applicationContext) : ISummaryService
{
    public async Task<Result<string>> GenerateSummary(ConversationEntity conversationEntity)
    {
        var summaryModelIdentifier = conversationOptions.Value.SummaryModelIdentifier;
        var prompt = this.GeneratePrompt(summaryModelIdentifier, conversationEntity);

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

    public async Task<Result<string>> GenerateAndApplySummary(ConversationEntity conversationEntity)
    {
        var summaryResult = await this.GenerateSummary(conversationEntity);
        if (summaryResult.IsError)
        {
            return summaryResult.Error!;
        }

        var summary = summaryResult.Unwrap();
        conversationEntity.Summary = summary;
        conversationEntity.LastAppendedUtc = DateTime.UtcNow;
        await applicationContext.SaveChangesAsync();

        return summary;
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

    private LlmPromptDto GeneratePrompt(Guid modelIdentifier, ConversationEntity conversationEntity)
    {
        var messages = conversationEntity.Messages.Select(m => new LlmPromptMessageDto(
            IsUserMessage: m.Prompt is null,
            Content: m.Content.Select(Map).ToList())).ToList();
        
        var summaryRequest = "Summarize the conversation so far, briefly. No more than 10 words. Shorter if possible.";
        messages.Add(new LlmPromptMessageDto(IsUserMessage: true, Content: [new LlmTextContent { Text = summaryRequest }]));

        var system = "Keep track of the contents of this conversation. If the final message asks you to summarize the contents of the conversation. Do as you're asked and ignore the summarization request in your summary. The summary should make the conversation easily identifiable";
        return new LlmPromptDto(
            ModelIdentifier: modelIdentifier,
            SystemMessage: system,
            Messages: messages);
    }
}