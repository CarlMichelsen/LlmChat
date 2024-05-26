using Domain.Dto.Chat;

namespace Interface.Service;

public interface IMessageResponseService
{
    Task Respond(NewMessageDto newUserMessageDto, CancellationToken cancellationToken);
}
