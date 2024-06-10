using Domain.Abstraction;
using Domain.Pipeline.SendMessage;
using Implementation.Database;
using Interface.Pipeline;
using Interface.Repository;
using Interface.Service;

namespace Implementation.Pipeline.Steps;

public class AppendUserMessagePipelineStep(
    IConversationDtoCacheService conversationDtoCacheService,
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

        var messageResult = messageInitiationRepository.InitiateMessage(
            data.ValidatedSendMessageData,
            data.Conversation,
            default,
            default);
        if (messageResult.IsError)
        {
            return await streamWriterService.WriteError("Failed to create or append user message", messageResult.Error!);
        }

        var message = messageResult.Unwrap();
        await conversationDtoCacheService.CacheConversation(data.Conversation);
        await streamWriterService.WriteIds(data.Conversation.Id, message.Id);
        data.UserMessage = message;
        return data;
    }
}
