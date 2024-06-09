using Domain.Pipeline.SendMessage;
using Implementation.Database;
using Implementation.Pipeline.Steps;

namespace Implementation.Pipeline;

public class SendMessagePipeline(
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
}
