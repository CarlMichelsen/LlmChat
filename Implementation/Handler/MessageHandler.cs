using Domain.Dto.Chat;
using Domain.Pipeline.SendMessage;
using Implementation.Pipeline;
using Interface.Handler;
using Microsoft.Extensions.Logging;

namespace Implementation.Handler;

public class MessageHandler(
    ILogger<MessageHandler> logger,
    SendMessagePipeline sendMessagePipeline) : IMessageHandler
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
            var convId = initial.NewUserMessageDto.ResponseTo?.ConversationId ?? initial.Conversation?.Id.Value ?? Guid.Empty;
            var rsptomsgId = initial.NewUserMessageDto.ResponseTo?.ResponseToMessageId ?? Guid.Empty;
            var contextData = $"conversationId: {convId} responseToId: {rsptomsgId}";

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
