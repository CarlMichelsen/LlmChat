using Domain.Conversation;
using Domain.Dto.Chat.Stream;
using Interface.Service;
using LargeLanguageModelClient;
using LargeLanguageModelClient.Dto.Model;
using LargeLanguageModelClient.Dto.Response.Stream;
using LargeLanguageModelClient.Dto.Response.Stream.Event;

namespace Implementation.Service;

public class LargeLanguageModelClientParseService(
    LlmModelDto model) : ILargeLanguageModelClientParseService
{
    private readonly Dictionary<int, ContentDto> responseContent = [];
    private bool ready = true;
    private LlmStreamTotalUsage? lastKnownTotalUsage = default;
    private string? lastKnownPromptIdentifier = default;
    private string? lastKnownModelName = default;

    public async IAsyncEnumerable<ContentDeltaDto> Parse(
        NewMessageData initiatedMessage,
        LlmModelDto model,
        IAsyncEnumerable<LlmStreamEvent> streamEvents,
        Func<ConcludedMessage, LlmModelDto, Task> handleConcludeMessage)
    {
        if (!this.ready)
        {
            yield return new ContentDeltaDto(
                ConversationId: initiatedMessage.Conversation.Id.ToString(),
                Content: default,
                Concluded: default,
                Error: "Parsing engine is already running");
            
            yield break;
        }

        this.ready = false;
        this.lastKnownTotalUsage = default;
        this.lastKnownPromptIdentifier = default;
        this.lastKnownModelName = default;
        this.responseContent.Clear();

        await foreach (var streamEvent in streamEvents)
        {
            switch (streamEvent)
            {
                case LlmStreamMessageStart messageStart:
                    yield return this.MessageStart(
                        initiatedMessage,
                        messageStart);
                    continue;
                
                case LlmStreamContentDelta contentDelta:
                    yield return this.ContentDelta(
                        initiatedMessage,
                        contentDelta);
                    continue;
                
                case LlmStreamTotalUsage totalUsage:
                    yield return this.TotalUsage(
                        initiatedMessage,
                        totalUsage);
                    continue;
                
                case LlmStreamError error:
                    yield return this.Error(
                        initiatedMessage,
                        error);
                    continue;
            }
        }

        await this.Conclude(
            initiatedMessage,
            handleConcludeMessage);
        
        this.ready = true;
    }

    private void AppendContent(ContentDto streamContent)
    {
        if (this.responseContent.TryGetValue(streamContent.Index, out var content))
        {
            content.Content += streamContent.Content;
        }
        else
        {
            this.responseContent.Add(streamContent.Index, streamContent);
        }
    }

    private async Task Conclude(
        NewMessageData initiatedMessage,
        Func<ConcludedMessage, LlmModelDto, Task> handleConcludeMessage)
    {
        StreamUsage? streamUsage = default;
        if (this.lastKnownTotalUsage is not null)
        {
            streamUsage = new StreamUsage
            {
                ProviderPromptIdentifier = this.lastKnownTotalUsage.ProviderPromptIdentifier,
                InputTokens = this.lastKnownTotalUsage.InputTokens,
                OutputTokens = this.lastKnownTotalUsage.OutputTokens,
                StopReason = this.lastKnownTotalUsage.StopReason,
            };
        }

        var conclude = new ConcludedMessage
        {
            ConversationId = initiatedMessage.Conversation.Id,
            ProviderPromptIdentifier = this.lastKnownPromptIdentifier!,
            ModelIdentifierName = this.lastKnownModelName ?? model.ModelIdentifierName ?? string.Empty,
            InputTokens = this.lastKnownTotalUsage!.InputTokens,
            OutputTokens = this.lastKnownTotalUsage!.OutputTokens,
            CurrentMillionInputTokenPrice = model.Price.MillionInputTokenPrice,
            CurrentMillionOutputTokenPrice = model.Price.MillionOutputTokenPrice,
            Content = this.responseContent.Select(kv => kv.Value).OrderBy(x => x.Index).ToList(),
            NewMessageData = initiatedMessage,
            StreamUsage = streamUsage,
        };
        
        await handleConcludeMessage(conclude, model);
    }

    private ContentDeltaDto MessageStart(
        NewMessageData initiatedMessage,
        LlmStreamMessageStart messageStart)
    {
        var contentString = messageStart.Message.Message.Content.FirstOrDefault()?.GetContent();
        var content = new ContentDto
        {
            Index = 0,
            ContentType = Enum.GetName(LlmContentType.Text)!,
            Content = contentString ?? string.Empty,
        };
        this.AppendContent(content);

        var contentDeltaDto = new ContentDeltaDto(
            ConversationId: initiatedMessage.Conversation.Id.ToString(),
            Content: content,
            Concluded: default,
            Error: default);

        this.lastKnownPromptIdentifier = messageStart.Message.ProviderPromptIdentifier;
        this.lastKnownModelName = messageStart.Message.ModelIdentifierName;

        return contentDeltaDto;
    }

    private ContentDeltaDto ContentDelta(
        NewMessageData initiatedMessage,
        LlmStreamContentDelta contentDelta)
    {
        var content = new ContentDto
        {
            Index = contentDelta.Index,
            ContentType = Enum.GetName(contentDelta.Delta.Type)!,
            Content = contentDelta.Delta.GetContent(),
        };
        this.AppendContent(content);

        var contentDeltaDto = new ContentDeltaDto(
            ConversationId: initiatedMessage.Conversation.Id.ToString(),
            Content: content,
            Concluded: default,
            Error: default);
        
        return contentDeltaDto;
    }

    private ContentDeltaDto TotalUsage(
        NewMessageData initiatedMessage,
        LlmStreamTotalUsage totalUsage)
    {
        var concluded = new ContentConcludedDto(
            InputTokens: totalUsage.InputTokens,
            OutputTokens: totalUsage.OutputTokens);

        var contentDeltaDto = new ContentDeltaDto(
            ConversationId: initiatedMessage.Conversation.Id.ToString(),
            Content: default,
            Concluded: concluded,
            Error: default);
        
        this.lastKnownTotalUsage = totalUsage;
        
        return contentDeltaDto;
    }

    private ContentDeltaDto Error(
        NewMessageData initiatedMessage,
        LlmStreamError error)
    {
        var contentDeltaDto = new ContentDeltaDto(
            ConversationId: initiatedMessage.Conversation.Id.ToString(),
            Content: default,
            Concluded: default,
            Error: error.Message);
        
        return contentDeltaDto;
    }
}