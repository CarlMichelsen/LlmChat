using Domain.Abstraction;
using Domain.Dto.Chat;
using LargeLanguageModelClient.Dto.Prompt;
using LargeLanguageModelClient.Dto.Prompt.Content;

namespace Implementation.Map;

public static class PromptMapper
{
    public static Result<LlmPromptDto> ToPrompt(NewMessageDto newUserMessageDto)
    {
        try
        {
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
        catch (Exception e)
        {
            return e;
        }
    }
}
