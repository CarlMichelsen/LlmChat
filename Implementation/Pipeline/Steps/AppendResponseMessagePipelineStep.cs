using Domain.Abstraction;
using Domain.Entity.Id;
using Domain.Pipeline.SendMessage;
using Implementation.Database;
using Implementation.Map;
using Interface.Pipeline;
using Interface.Repository;
using Interface.Service;

namespace Implementation.Pipeline.Steps;

public class AppendResponseMessagePipelineStep(
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

        if (data.UserMessage is null)
        {
            return await streamWriterService.WriteError("Expected user-message to have already been created");
        }

        if (data.ResponseMessageContent is null)
        {
            return await streamWriterService.WriteError("Expected response-message to have already been created");
        }

        var validatedResponseMessageData = new ValidatedSendMessageData
        {
            RequestConversationId = data.Conversation.Id,
            ResponseToMessageId = data.UserMessage.Id,
            SelectedModel = data.ValidatedSendMessageData.SelectedModel,
            Content = data.ResponseMessageContent.Select(c => MessageDtoMapper.Map(c)).ToList(),
        };
        var responseInitiationResult = await messageInitiationRepository.InitiateMessage(
            validatedResponseMessageData,
            data.Conversation,
            data.NextMessageIdentifier == Guid.Empty ? default : new MessageEntityId(data.NextMessageIdentifier),
            data.StreamUsage);
        if (responseInitiationResult.IsError)
        {
            return await streamWriterService.WriteError("Failed to initiate response message in exsisting conversation", responseInitiationResult.Error!);
        }

        data.ResponseMessage = responseInitiationResult.Unwrap();
        await conversationDtoCacheService.InvalidateConversationCache(data.Conversation.Id);
        return data;
    }
}
