﻿using Domain.Abstraction;
using Domain.Conversation;
using Domain.Entity;
using Domain.Entity.Id;
using Domain.Pipeline.SendMessage;

namespace Interface.Repository;

public interface IMessageInitiationRepository
{
    Result<(MessageEntity Message, DialogSliceEntity DialogSlice)> InitiateMessage(
        ValidatedSendMessageData newUserMessageDto,
        ConversationEntity conversationEntity,
        MessageEntityId? predeterminedMessageEntityId,
        StreamUsage? llmStreamTotalUsage);
}
