using Domain.Abstraction;
using Domain.Pipeline.SendMessage;
using Implementation.Database;
using Interface.Pipeline;
using Interface.Service;

namespace Implementation.Pipeline.Steps;

public class SummaryGenerationStep(
    IStreamWriterService streamWriterService,
    ISummaryService summaryService) : ITransactionPipelineStep<ApplicationContext, SendMessagePipelineData>
{
    public async Task<Result<SendMessagePipelineData>> Execute(
        ApplicationContext context,
        SendMessagePipelineData data,
        CancellationToken cancellationToken)
    {
        if (data.Conversation is null)
        {
            return await streamWriterService.WriteError("Expected conversation to have already been created");
        }

        if (!string.IsNullOrWhiteSpace(data.Conversation.Summary))
        {
            return data;
        }

        var summaryResult = await summaryService.GenerateAndApplySummary(data.Conversation);
        if (summaryResult.IsError)
        {
            return await streamWriterService.WriteError("Failed to generate conversation summary", summaryResult.Error!);
        }

        data.Conversation.Summary = summaryResult.Unwrap();
        await streamWriterService.WriteSummary(data.Conversation.Id, data.Conversation.Summary);
        context.SaveChanges();

        return data;
    }
}
