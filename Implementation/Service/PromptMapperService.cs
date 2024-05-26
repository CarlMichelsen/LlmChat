using Domain.Abstraction;
using Domain.Dto.Chat;
using Interface.Service;
using LargeLanguageModelClient.Dto.Prompt;
using LargeLanguageModelClient.Dto.Prompt.Content;

namespace Implementation.Service;

public class PromptMapperService : IPromptMapperService
{
    public async Task<Result<LlmPromptDto>> ToPrompt(NewMessageDto newUserMessageDto)
    {
        await Task.Delay(1);
        
        return new LlmPromptDto(
            ModelIdentifier: newUserMessageDto.ModelIdentifier,
            SystemMessage: "Respond very concisely. Assume that the user is a C# systems development expert and is using modern .net8 C#",
            Messages: new List<LlmPromptMessageDto>
            {
                new LlmPromptMessageDto(
                    IsUserMessage: true,
                    Content: newUserMessageDto.Content.Select(x => new LlmTextContent { Text = x.Content }).ToList<LlmContent>()),
            });
    }
}
