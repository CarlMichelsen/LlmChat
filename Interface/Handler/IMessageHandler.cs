using Domain.Dto.Chat;

namespace Interface.Handler;

public interface IMessageHandler
{
    Task SendMessage(NewMessageDto newUserMessageDto, CancellationToken cancellationToken);
}
