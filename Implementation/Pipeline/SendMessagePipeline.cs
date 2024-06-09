using System.Diagnostics;
using Domain.Pipeline.SendMessage;
using Implementation.Database;
using Implementation.Pipeline.Steps;
using Microsoft.Extensions.Logging;

namespace Implementation.Pipeline;

public class SendMessagePipeline(
    ILogger<SendMessagePipeline> logger,
    ApplicationContext context,
    ValidateRequestPipelineStep validateRequestPipelineStep,
    GetOrCreateConversationPipelineStep getOrCreateConversationPipelineStep,
    AppendUserMessagePipelineStep appendUserMessagePipelineStep,
    StreamResponseMessagePipelineStep streamResponseMessagePipelineStep,
    AppendResponseMessagePipelineStep appendResponseMessagePipelineStep,
    SummaryGenerationStep summaryGenerationStep) : BaseTransactionPipeline<ApplicationContext, SendMessagePipelineData>(
        context,
        validateRequestPipelineStep,
        getOrCreateConversationPipelineStep,
        appendUserMessagePipelineStep,
        streamResponseMessagePipelineStep,
        appendResponseMessagePipelineStep,
        summaryGenerationStep)
{
    private readonly Stopwatch stopwatch = new();

    protected override Task PrePipelineExecution(SendMessagePipelineData data)
    {
        this.stopwatch.Restart();
        return Task.CompletedTask;
    }

    protected override Task PostPipelineExecution(SendMessagePipelineData data)
    {
        this.stopwatch.Stop();
        logger.LogInformation("{PipelineName} took {Milliseconds} ms to complete", nameof(SendMessagePipeline), this.stopwatch.ElapsedMilliseconds);
        return Task.CompletedTask;
    }
}
