using Domain.Abstraction;
using Domain.Dto.Conversation;
using Domain.Entity;
using Domain.Exception;

namespace Implementation.Map;

public static class ConversationDtoMapper
{
    public static Result<ConversationDto> Map(ConversationEntity conversationEntity)
    {
        var dialogSliceResult = Map(conversationEntity.Messages);
        if (dialogSliceResult.IsError)
        {
            return dialogSliceResult.Error!;
        }

        return new ConversationDto
        {
            Id = conversationEntity.Id.ToString(),
            Summary = conversationEntity.Summary,
            SystemMessage = conversationEntity.SystemMessage,
            DialogSlices = dialogSliceResult.Unwrap(),
            LastUpdatedUtc = conversationEntity.LastAppendedUtc,
            CreatedUtc = conversationEntity.CreatedUtc,
        };
    }

    private static Result<List<DialogSliceDto>> Map(List<MessageEntity> messageEntities)
    {
        var rootMessages = messageEntities.Where(m => m.PreviousMessage is null).ToList();
        if (rootMessages.Count == 0)
        {
            var ex = new SafeUserFeedbackException("Found no root messages in conversation");
            return ex;
        }

        List<DialogSliceDto> dialogSliceList = [];
        MapSlicesRecursive(messageEntities, dialogSliceList, rootMessages, 0, true);
        
        return dialogSliceList;
    }

    private static void MapSlicesRecursive(
        List<MessageEntity> allMessages,
        List<DialogSliceDto> dialogSliceList,
        List<MessageEntity> nextMessages,
        int index,
        bool visible)
    {
        var isVisible = visible;
        if (nextMessages.Count == 0)
        {
            return;
        }

        DialogSliceDto? slice = dialogSliceList.ElementAtOrDefault(index);
        if (slice is null)
        {
            slice = new DialogSliceDto
            {
                Messages = [],
                SelectedIndex = 0,
                Visible = isVisible,
            };
            dialogSliceList.Add(slice);
        }

        var sliceMessages = nextMessages
            .OrderBy(x => x.CompletedUtc)
            .Select(MessageDtoMapper.Map)
            .ToList();

        slice.Messages.AddRange(sliceMessages);
        slice.Messages = slice.Messages.DistinctBy(x => x.Id).ToList();

        var prevSlice = dialogSliceList.ElementAtOrDefault(index - 1);
        if (prevSlice is not null && isVisible)
        {
            var exisists = prevSlice.Messages.ElementAt(prevSlice.SelectedIndex).Id == slice.Messages.ElementAt(slice.SelectedIndex).PreviousMessageId;
            slice.Visible = exisists;
            isVisible = exisists;
        }

        foreach (var sliceMessage in nextMessages)
        {
            var nextSliceMessages = allMessages
                .Where(x => x.PreviousMessage is not null)
                .Where(x => x.PreviousMessage!.Id == sliceMessage.Id)
                .ToList();
            
            MapSlicesRecursive(
                allMessages,
                dialogSliceList,
                nextSliceMessages,
                index + 1,
                isVisible);
        }
    }
}
