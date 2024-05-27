using Domain.Abstraction;
using Domain.Dto.Conversation;
using Domain.Entity;
using Domain.Exception;
using Implementation.Map;
using Interface.Repository;
using Interface.Service;
using Microsoft.Extensions.Logging;

namespace Implementation.Service;

public class ConversationDtoService(
    ILogger<ConversationDtoService> logger,
    ISessionService sessionService,
    IConversationReadRepository conversationReadRepository) : IConversationDtoService
{
    public Task<Result<ConversationDto>> GetConversationDto(long conversationId)
    {
        // TODO: implement caching with invalidation
        return this.GetConversationDtoDirect(conversationId);
    }

    private Result<ConversationDto> Map(ConversationEntity conversationEntity)
    {
        var dialogSliceResult = this.Map(conversationEntity.Messages);
        if (dialogSliceResult.IsError)
        {
            return dialogSliceResult.Error!;
        }

        return new ConversationDto
        {
            Id = conversationEntity.Id.ToString(),
            Summary = conversationEntity.Summary,
            DialogSlices = dialogSliceResult.Unwrap(),
            LastUpdatedUtc = conversationEntity.LastAppendedUtc,
            CreatedUtc = conversationEntity.CreatedUtc,
        };
    }

    private Result<List<DialogSliceDto>> Map(List<MessageEntity> messageEntities)
    {
        var rootMessages = messageEntities.Where(m => m.PreviousMessage is null).ToList();
        if (rootMessages.Count == 0)
        {
            var ex = new SafeUserFeedbackException("Found no root messages in conversation");
            logger.LogCritical(ex, "Did not find any root messages");
            return ex;
        }

        List<DialogSliceDto> dialogSliceList = [];
        this.MapSlicesRecursive(messageEntities, dialogSliceList, rootMessages, 0);
        
        return dialogSliceList;
    }

    private void MapSlicesRecursive(
        List<MessageEntity> allMessages,
        List<DialogSliceDto> dialogSliceList,
        List<MessageEntity> nextMessages,
        int index)
    {
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
            };
            dialogSliceList.Add(slice);
        }

        var sliceMessages = nextMessages
            .OrderBy(x => x.CompletedUtc)
            .Select(MessageDtoMapper.Map)
            .ToList();

        slice.Messages.AddRange(sliceMessages);
        slice.Messages = slice.Messages.DistinctBy(x => x.Id).ToList();

        foreach (var sliceMessage in nextMessages)
        {
            var nextSliceMessages = allMessages
                .Where(x => x.PreviousMessage is not null)
                .Where(x => x.PreviousMessage!.Id == sliceMessage.Id)
                .ToList();
            
            this.MapSlicesRecursive(
                allMessages,
                dialogSliceList,
                nextSliceMessages,
                index + 1);
        }
    }

    private async Task<Result<ConversationDto>> GetConversationDtoDirect(long conversationId)
    {
        var creatorIdentifier = sessionService.UserProfileId;
        var conversationResult = await conversationReadRepository
            .GetRichConversation(creatorIdentifier, conversationId);
        if (conversationResult.IsError)
        {
            return conversationResult.Error!;
        }

        var mapResult = this.Map(conversationResult.Unwrap());
        if (mapResult.IsError)
        {
            return mapResult.Error!;
        }

        return mapResult.Unwrap();
    }
}