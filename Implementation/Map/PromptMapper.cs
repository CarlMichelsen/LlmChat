using Domain.Abstraction;
using Domain.Entity;
using Domain.Entity.Id;
using Domain.Exception;
using LargeLanguageModelClient.Dto.Prompt;
using LargeLanguageModelClient.Dto.Prompt.Content;

namespace Implementation.Map;

public static class PromptMapper
{
    public static Result<LlmPromptDto> ToPrompt(
        ConversationEntity conv,
        MessageEntityId outerConversationBranchMessageId,
        Guid modelIdentifier)
    {
        try
        {
            var allmessages = conv.DialogSlices
                .SelectMany(ds => ds.Messages)
                .ToList();
            
            var branchMessage = allmessages
                .FirstOrDefault(m => m.Id == outerConversationBranchMessageId);
            if (branchMessage is null)
            {
                return new SafeUserFeedbackException("Did not find message to generate prompt from");
            }

            List<LlmPromptMessageDto> list = [];
            do
            {
                var promptMsg = new LlmPromptMessageDto(
                    IsUserMessage: branchMessage.IsUserMessage,
                    Content: branchMessage.Content.Select(x => new LlmTextContent { Text = x.Content }).ToList<LlmContent>());
                list.Insert(0, promptMsg);

                if (branchMessage.PreviousMessage is null)
                {
                    break;
                }

                branchMessage = allmessages.FirstOrDefault(m => m.Id == branchMessage.PreviousMessage.Id);
            }
            while (branchMessage is not null);

            return new LlmPromptDto(
                ModelIdentifier: modelIdentifier,
                SystemMessage: conv.SystemMessage,
                Messages: list);
        }
        catch (Exception e)
        {
            return e;
        }
    }
}
