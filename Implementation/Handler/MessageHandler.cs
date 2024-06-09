using Domain.Dto.Chat;
using Domain.Pipeline.SendMessage;
using Implementation.Database;
using Interface.Handler;
using Interface.Pipeline;
using Microsoft.Extensions.Logging;

namespace Implementation.Handler;

public class MessageHandler(
    ILogger<MessageHandler> logger,
    ITransactionPipeline<ApplicationContext, SendMessagePipelineData> sendMessagePipeline) : IMessageHandler
{
    public async Task SendMessage(
        NewMessageDto newUserMessageDto,
        CancellationToken cancellationToken)
    {
        var initial = new SendMessagePipelineData
        {
            NewUserMessageDto = newUserMessageDto,
        };

        var result = await sendMessagePipeline.Execute(initial, cancellationToken);
        if (result.IsError)
        {
            var contextData = $"conversationId: {initial.NewUserMessageDto.ConversationId} responseToId: {initial.NewUserMessageDto.ResponseToMessageId}";

            if (result.Error is OperationCanceledException)
            {
                logger.LogCritical(result.Error, $"Operation was cancelled {contextData}");
                return;
            }

            logger.LogCritical(
                result.Error!,
                $"SendMessagePipeline returned an error result {contextData}");
        }
    }
}
