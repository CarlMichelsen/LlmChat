using Domain.Conversation;
using Domain.Dto.Chat.Stream;
using Domain.Dto.Conversation;
using Interface.Service;
using LargeLanguageModelClient;
using LargeLanguageModelClient.Dto.Model;
using LargeLanguageModelClient.Dto.Response.Stream;
using LargeLanguageModelClient.Dto.Response.Stream.Event;

namespace Implementation.Service;

public class LargeLanguageModelClientParseService(
    LlmModelDto model) : ILargeLanguageModelClientParseService
{
    private readonly Dictionary<int, StreamContentDto> responseContent = [];
    private bool ready = true;
    private LlmStreamTotalUsage? lastKnownTotalUsage = default;
    private string? lastKnownPromptIdentifier = default;
    private string? lastKnownModelName = default;

    public async IAsyncEnumerable<ContentDeltaDto> Parse(
        NewMessageData newMessageData,
        IAsyncEnumerable<LlmStreamEvent> streamEvents,
        Func<ConcludedMessage, LlmModelDto, Task> handleConcludeMessage)
    {
        if (!this.ready)
        {
            yield return new ContentDeltaDto(
                ConversationId: newMessageData.Conversation.Id.ToString(),
                UserMessageId: default,
                Content: default,
                Concluded: default,
                Summary: default,
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
                        newMessageData,
                        messageStart);
                    continue;
                
                case LlmStreamContentDelta contentDelta:
                    yield return this.ContentDelta(
                        newMessageData,
                        contentDelta);
                    continue;
                
                case LlmStreamTotalUsage totalUsage:
                    this.TotalUsage(
                        newMessageData,
                        totalUsage);
                    continue;
                
                case LlmStreamError error:
                    yield return this.Error(
                        newMessageData,
                        error);
                    continue;
            }
        }

        await this.Conclude(
            newMessageData,
            handleConcludeMessage);
        
        this.ready = true;
    }

    private void AppendContent(StreamContentDto streamContent)
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
        NewMessageData newMessageData,
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
            ConversationId = newMessageData.Conversation.Id,
            ProviderPromptIdentifier = this.lastKnownPromptIdentifier!,
            ModelIdentifierName = this.lastKnownModelName ?? model.ModelIdentifierName ?? string.Empty,
            InputTokens = this.lastKnownTotalUsage!.InputTokens,
            OutputTokens = this.lastKnownTotalUsage!.OutputTokens,
            CurrentMillionInputTokenPrice = model.Price.MillionInputTokenPrice,
            CurrentMillionOutputTokenPrice = model.Price.MillionOutputTokenPrice,
            Content = this.responseContent
                .Select(kv => kv.Value)
                .OrderBy(x => x.Index)
                .Select(kv => new MessageContentDto { ContentType = kv.ContentType, Content = kv.Content })
                .ToList(),
            InitialNewMessageData = newMessageData,
            StreamUsage = streamUsage,
        };

        await handleConcludeMessage(conclude, model);
    }

    private ContentDeltaDto MessageStart(
        NewMessageData newMessageData,
        LlmStreamMessageStart messageStart)
    {
        var contentString = messageStart.Message.Message.Content.FirstOrDefault()?.GetContent();
        var content = new StreamContentDto
        {
            Index = 0,
            ContentType = Enum.GetName(LlmContentType.Text)!,
            Content = contentString ?? string.Empty,
        };
        this.AppendContent(content);

        var contentDeltaDto = new ContentDeltaDto(
            ConversationId: newMessageData.Conversation.Id.ToString(),
            UserMessageId: default,
            Content: content,
            Concluded: default,
            Summary: default,
            Error: default);

        this.lastKnownPromptIdentifier = messageStart.Message.ProviderPromptIdentifier;
        this.lastKnownModelName = messageStart.Message.ModelIdentifierName;

        return contentDeltaDto;
    }

    private ContentDeltaDto ContentDelta(
        NewMessageData newMessageData,
        LlmStreamContentDelta contentDelta)
    {
        var content = new StreamContentDto
        {
            Index = contentDelta.Index,
            ContentType = Enum.GetName(contentDelta.Delta.Type)!,
            Content = contentDelta.Delta.GetContent(),
        };
        this.AppendContent(content);

        var contentDeltaDto = new ContentDeltaDto(
            ConversationId: newMessageData.Conversation.Id.ToString(),
            UserMessageId: default,
            Content: content,
            Concluded: default,
            Summary: default,
            Error: default);
        
        return contentDeltaDto;
    }

    private void TotalUsage(
        NewMessageData newMessageData,
        LlmStreamTotalUsage totalUsage)
    {
        this.lastKnownTotalUsage = totalUsage;
    }

    private ContentDeltaDto Error(
        NewMessageData newMessageData,
        LlmStreamError error)
    {
        var contentDeltaDto = new ContentDeltaDto(
            ConversationId: newMessageData.Conversation.Id.ToString(),
            UserMessageId: default,
            Content: default,
            Concluded: default,
            Summary: default,
            Error: error.Message);
        
        return contentDeltaDto;
    }
}