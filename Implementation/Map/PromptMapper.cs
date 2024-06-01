using Domain.Abstraction;
using Domain.Dto.Chat;
using Domain.Entity;
using LargeLanguageModelClient.Dto.Prompt;
using LargeLanguageModelClient.Dto.Prompt.Content;

namespace Implementation.Map;

public static class PromptMapper
{
    public static Result<LlmPromptDto> ToPrompt(ConversationEntity conv, NewMessageDto newUserMessageDto)
    {
        try
        {
            List<LlmPromptMessageDto> list = [];

            if (!string.IsNullOrWhiteSpace(newUserMessageDto.ResponseToMessageId) && long.TryParse(newUserMessageDto.ResponseToMessageId, out var responseToId))
            {
                var prevMsg = conv.Messages.FirstOrDefault(m => m.Id == responseToId);
                while (prevMsg is not null)
                {
                    var prevPromptMsg = new LlmPromptMessageDto(
                        IsUserMessage: prevMsg.Prompt is null,
                        Content: prevMsg.Content.Select(x => new LlmTextContent { Text = x.Content }).ToList<LlmContent>());
                    
                    list.Insert(0, prevPromptMsg);
                    if (prevMsg.PreviousMessage?.Id is not null)
                    {
                        prevMsg = conv.Messages.FirstOrDefault(m => m.Id == prevMsg.PreviousMessage?.Id);
                        continue;
                    }

                    prevMsg = null;
                }
            }
            
            var newMessage = new LlmPromptMessageDto(
                IsUserMessage: true,
                Content: newUserMessageDto.Content.Select(x => new LlmTextContent { Text = x.Content }).ToList<LlmContent>());
            list.Add(newMessage);

            return new LlmPromptDto(
                ModelIdentifier: newUserMessageDto.ModelIdentifier,
                SystemMessage: "Respond very concisely. Assume that the user is a C# systems development expert and is using modern .net8 C#",
                Messages: list);
        }
        catch (Exception e)
        {
            return e;
        }
    }
}
