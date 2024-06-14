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
        ResponseToData? responseToData = default;
        if (data.NewUserMessageDto.ResponseTo is not null)
        {
            responseToData = new ResponseToData
            {
                ConversationId = new ConversationEntityId(data.NewUserMessageDto.ResponseTo.ConversationId),
                MessageId = new MessageEntityId(data.NewUserMessageDto.ResponseTo.ResponseToMessageId),
            };
        }

        var model = await modelService.GetModel(data.NewUserMessageDto.ModelIdentifier);
        if (model is null)
        {
            return await streamWriterService.WriteError("Failed to find model");
        }

        data.ValidatedSendMessageData = new ValidatedSendMessageData
        {
            ResponseTo = responseToData,
            SelectedModel = model,
            Content = data.NewUserMessageDto.Content
                .Select(content => MessageDtoMapper.Map(content))
                .ToList(),
        };

        return data;
    }
}
