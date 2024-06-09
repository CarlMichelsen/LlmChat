using Domain.Abstraction;
using Domain.Pipeline.SendMessage;
using Implementation.Database;
using Interface.Pipeline;
using Interface.Repository;
using Interface.Service;

namespace Implementation.Pipeline.Steps;

public class AppendUserMessagePipelineStep(
    IStreamWriterService streamWriterService,
    IMessageInitiationRepository messageInitiationRepository) : ITransactionPipelineStep<ApplicationContext, SendMessagePipelineData>
{
    public async Task<Result<SendMessagePipelineData>> Execute(
        ApplicationContext context,
        SendMessagePipelineData data,
        CancellationToken cancellationToken)
    {
        if (data.ValidatedSendMessageData is null)
        {
            return await streamWriterService.WriteError("Expected ValidatedSendMessageData to have already been created");
        }

        if (data.Conversation is null)
        {
            return await streamWriterService.WriteError("Expected conversation to have already been created");
        }

        var messageResult = await messageInitiationRepository.InitiateMessage(
            data.ValidatedSendMessageData,
            data.Conversation,
            default);
        if (messageResult.IsError)
        {
            return await streamWriterService.WriteError("Failed to create or append user message", messageResult.Error!);
        }

        var message = messageResult.Unwrap();
        await streamWriterService.WriteIds(default, message.Id);
        data.UserMessage = message;
        return data;
    }
}
