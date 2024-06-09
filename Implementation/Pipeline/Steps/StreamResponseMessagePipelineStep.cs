using Domain.Abstraction;
using Domain.Conversation;
using Domain.Dto.Chat.Stream;
using Domain.Pipeline.SendMessage;
using Implementation.Database;
using Implementation.Map;
using Implementation.Service;
using Interface.Pipeline;
using Interface.Service;
using LargeLanguageModelClient;
using LargeLanguageModelClient.Dto.Model;

namespace Implementation.Pipeline.Steps;

public class StreamResponseMessagePipelineStep(
    ILargeLanguageModelClient largeLanguageModelClient,
    IStreamWriterService streamWriterService) : ITransactionPipelineStep<ApplicationContext, SendMessagePipelineData>
{
    private TaskCompletionSource<ConcludedMessage>? concludedTask = default;

    public async Task<Result<SendMessagePipelineData>> Execute(
        ApplicationContext context,
        SendMessagePipelineData data,
        CancellationToken cancellationToken)
    {
        if (this.concludedTask is not null)
        {
            return await streamWriterService.WriteError("A message stream is already running");
        }

        this.concludedTask = new();

        if (data.Conversation is null)
        {
            return await streamWriterService.WriteError("Expected conversation to have already been created");
        }

        if (data.UserMessage is null)
        {
            return await streamWriterService.WriteError("Expected user-message to have already been created");
        }

        if (data.ValidatedSendMessageData is null)
        {
            return await streamWriterService.WriteError("Expected ValidatedSendMessageData to have already been created");
        }

        var promptResult = PromptMapper.ToPrompt(data.Conversation, data.ValidatedSendMessageData);
        if (promptResult.IsError)
        {
            return await streamWriterService.WriteError("Failed to map conversation to prompt", promptResult.Error!);
        }

        var newMessageData = new NewMessageData
        {
            Message = data.UserMessage,
            Conversation = data.Conversation,
        };

        var parser = new LargeLanguageModelClientParseService(data.ValidatedSendMessageData.SelectedModel) as ILargeLanguageModelClientParseService;
        var parsedStream = parser.Parse(
            newMessageData,
            largeLanguageModelClient.PromptStream(promptResult.Unwrap(), cancellationToken),
            this.ConcludeMessage);
        
        await foreach (var streamEvent in parsedStream)
        {
            await streamWriterService.WriteContentDelta(streamEvent);
        }

        var conclusion = await this.concludedTask.Task;
        this.concludedTask = default;

        var concluded = new ContentConcludedDto
        {
            ModelName = data.ValidatedSendMessageData.SelectedModel.ModelIdentifierName,
            ModelId = data.ValidatedSendMessageData.SelectedModel.Id,
            ProviderPromptIdentifier = conclusion.ProviderPromptIdentifier,
            InputTokens = (int)conclusion.InputTokens,
            OutputTokens = (int)conclusion.OutputTokens,
            StopReason = conclusion.StreamUsage?.StopReason ?? "unknown",
            MessageId = conclusion.InitialNewMessageData.Message.Id.ToString(),
        };
        
        await streamWriterService.WriteConclusion(concluded);
        data.ResponseMessageContent = conclusion.Content;
        data.StreamUsage = conclusion.StreamUsage;
        return data;
    }

    private Task ConcludeMessage(ConcludedMessage conclusion, LlmModelDto model)
    {
        this.concludedTask?.SetResult(conclusion);
        return Task.CompletedTask;
    }
}
