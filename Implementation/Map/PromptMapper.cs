using Domain.Abstraction;
using Domain.Entity;
using Domain.Pipeline.SendMessage;
using LargeLanguageModelClient.Dto.Prompt;
using LargeLanguageModelClient.Dto.Prompt.Content;

namespace Implementation.Map;

public static class PromptMapper
{
    public static Result<LlmPromptDto> ToPrompt(ConversationEntity conv, ValidatedSendMessageData validatedSendMessageData)
    {
        try
        {
            List<LlmPromptMessageDto> list = [];

            if (validatedSendMessageData.ResponseToMessageId is not null)
            {
                var prevMsg = conv.Messages.FirstOrDefault(m => m.Id == validatedSendMessageData.ResponseToMessageId);
                while (prevMsg is not null)
                {
                    var prevPromptMsg = new LlmPromptMessageDto(
                        IsUserMessage: prevMsg.IsUserMessage,
                        Content: prevMsg.Content.Select(x => new LlmTextContent { Text = x.Content }).ToList<LlmContent>());
                    
                    list.Insert(0, prevPromptMsg);
                    if (prevMsg.PreviousMessage?.Id is not null)
                    {
                        prevMsg = conv.Messages.FirstOrDefault(m => m.Id == prevMsg.PreviousMessage!.Id);
                        continue;
                    }

                    prevMsg = null;
                }
            }
            
            var newMessage = new LlmPromptMessageDto(
                IsUserMessage: true,
                Content: validatedSendMessageData.Content.Select(x => new LlmTextContent { Text = x.Content }).ToList<LlmContent>());
            list.Add(newMessage);

            return new LlmPromptDto(
                ModelIdentifier: validatedSendMessageData.SelectedModel.Id,
                SystemMessage: conv.SystemMessage,
                Messages: list);
        }
        catch (Exception e)
        {
            return e;
        }
    }
}
