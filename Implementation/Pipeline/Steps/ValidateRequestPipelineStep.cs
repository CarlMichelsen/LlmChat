using Domain.Abstraction;
using Domain.Entity.Id;
using Domain.Pipeline.SendMessage;
using Implementation.Database;
using Implementation.Map;
using Interface.Pipeline;
using Interface.Service;

namespace Implementation.Pipeline.Steps;

public class ValidateRequestPipelineStep(
    IStreamWriterService streamWriterService,
    IModelService modelService) : ITransactionPipelineStep<ApplicationContext, SendMessagePipelineData>
{
    public async Task<Result<SendMessagePipelineData>> Execute(
        ApplicationContext context,
        SendMessagePipelineData data,
        CancellationToken cancellationToken)
    {
        if (data.NewUserMessageDto.Content.Count == 0)
        {
            return await streamWriterService.WriteError("Request contains no content");
        }

        var convIdParseSuccess = Guid.TryParse(data.NewUserMessageDto.ConversationId, out Guid convId);
        if (!convIdParseSuccess && !string.IsNullOrWhiteSpace(data.NewUserMessageDto.ConversationId))
        {
            return await streamWriterService.WriteError("Failed to parse conversationId from the request");
        }

        var conversationId = convIdParseSuccess ? new ConversationEntityId(convId) : default;

        var responseToMsgIdParseSuccess = Guid.TryParse(data.NewUserMessageDto.ResponseToMessageId, out Guid msgId);
        if (!responseToMsgIdParseSuccess && !string.IsNullOrWhiteSpace(data.NewUserMessageDto.ResponseToMessageId))
        {
            return await streamWriterService.WriteError("Failed to parse conversationId from the request");
        }

        var responseToMessageId = responseToMsgIdParseSuccess ? new MessageEntityId(msgId) : default;

        var model = await modelService.GetModel(data.NewUserMessageDto.ModelIdentifier);
        if (model is null)
        {
            return await streamWriterService.WriteError("Failed to find model");
        }

        data.ValidatedSendMessageData = new ValidatedSendMessageData
        {
            RequestConversationId = conversationId,
            ResponseToMessageId = responseToMessageId,
            SelectedModel = model,
            Content = data.NewUserMessageDto.Content
                .Select(content => MessageDtoMapper.Map(content))
                .ToList(),
        };

        return data;
    }
}
