using Domain.Abstraction;
using Domain.Pipeline.SendMessage;
using Implementation.Database;
using Interface.Pipeline;
using Interface.Repository;
using Interface.Service;

namespace Implementation.Pipeline.Steps;

public class GetOrCreateConversationPipelineStep(
    IStreamWriterService streamWriterService,
    IGetOrCreateConversationRepository getOrCreateConversationRepository,
    ISessionService sessionService) : ITransactionPipelineStep<ApplicationContext, SendMessagePipelineData>
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

        var conversationResult = await getOrCreateConversationRepository.GetOrCreateConversation(
            sessionService.UserProfileId,
            data.ValidatedSendMessageData.ResponseTo?.ConversationId);
        if (conversationResult.IsError)
        {
            return await streamWriterService.WriteError("Failed to get or create conversation", conversationResult.Error!);
        }

        data.Conversation = conversationResult.Unwrap();
        return data;
    }
}