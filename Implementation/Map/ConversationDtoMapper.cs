using Domain.Abstraction;
using Domain.Dto.Conversation;
using Domain.Entity;
using Domain.Entity.Id;

namespace Implementation.Map;

public static class ConversationDtoMapper
{
    public static Result<ConversationDto> Map(ConversationEntity conversationEntity)
    {
        return new ConversationDto
        {
            Id = conversationEntity.Id.Value,
            Summary = conversationEntity.Summary,
            SystemMessage = conversationEntity.SystemMessage,
            DialogSlices = Map(conversationEntity.DialogSlices),
            LastUpdatedUtc = conversationEntity.LastAppendedUtc,
            CreatedUtc = conversationEntity.CreatedUtc,
        };
    }

    private static List<DialogSliceDto> Map(List<DialogSliceEntity> dialogSliceEntities)
    {
        var list = new List<DialogSliceDto>();
        var visible = true;
        for (int i = 0; i < dialogSliceEntities.Count; i++)
        {
            var dse = dialogSliceEntities.ElementAt(i);
            if (visible)
            {
                var prevDse = dialogSliceEntities.ElementAtOrDefault(i - 1);
                if (prevDse is not null)
                {
                    var currentSelectedMessage = dse.Messages.First(m => m.Id.Value == dse.SelectedMessageGuid);
                    if (currentSelectedMessage.PreviousMessage is not null)
                    {
                        var prevSelectedMessage = prevDse.Messages.First(m => m.Id.Value == prevDse.SelectedMessageGuid);
                        visible = currentSelectedMessage.PreviousMessage.Id == prevSelectedMessage.Id;
                    }
                }
            }

            list.Add(Map(dse, visible));
        }

        return list;
    }

    private static DialogSliceDto Map(DialogSliceEntity dialogSliceEntity, bool visible)
    {
        var msgId = dialogSliceEntity.SelectedMessageGuid == Guid.Empty
            ? default
            : new MessageEntityId(dialogSliceEntity.SelectedMessageGuid);
        
        var selectedIndex = msgId is not null
            ? dialogSliceEntity.Messages.FindIndex(m => m.Id == msgId)
            : -1;

        var messageDtos = dialogSliceEntity.Messages
            .Select(MessageDtoMapper.Map)
            .ToList();

        return new DialogSliceDto
        {
            Id = dialogSliceEntity.Id.Value,
            Messages = messageDtos,
            SelectedIndex = selectedIndex == -1 ? messageDtos.Count - 1 : selectedIndex,
            Visible = visible,
        };
    }
}
