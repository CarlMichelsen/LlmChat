using Domain.Conversation;
using Domain.Dto.Chat.Stream;
using LargeLanguageModelClient.Dto.Model;
using LargeLanguageModelClient.Dto.Response.Stream;

namespace Interface.Service;

public interface ILargeLanguageModelClientParseService
{
    IAsyncEnumerable<ContentDeltaDto> Parse(
        NewMessageData initiatedMessage,
        IAsyncEnumerable<LlmStreamEvent> streamEvents,
        Func<ConcludedMessage, LlmModelDto, Task> handleConcludeMessage);
}
