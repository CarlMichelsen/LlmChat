using Domain.Abstraction;
using Domain.Dto.Chat;
using LargeLanguageModelClient.Dto.Prompt;

namespace Interface.Service;

public interface IPromptMapperService
{
    Task<Result<LlmPromptDto>> ToPrompt(NewMessageDto newUserMessageDto);
}
