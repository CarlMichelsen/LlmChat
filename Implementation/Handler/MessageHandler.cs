using Domain.Dto.Chat;
using Interface.Handler;
using Interface.Service;

namespace Implementation.Handler;

public class MessageHandler(
    IMessageResponseService messageResponseService) : IMessageHandler
{
    public async Task SendMessage(
        NewMessageDto newUserMessageDto,
        CancellationToken cancellationToken)
    {
        await messageResponseService.Respond(newUserMessageDto, cancellationToken);
    }
}
