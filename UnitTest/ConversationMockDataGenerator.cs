﻿using Domain.Entity;
using Domain.Entity.Id;
using LargeLanguageModelClient.Dto.Model;

namespace UnitTest;

public static class ConversationMockDataGenerator
{
    public static ProfileEntityId SampleCreatorIdentifier { get; } = new(Guid.NewGuid());

    public static ConversationEntityId SeededConversationId { get; } = new(Guid.NewGuid());

    public static MessageEntityId SeededInitialMessageId { get; } = new(Guid.NewGuid());

    public static MessageEntityId SeededSecondMessageId { get; } = new(Guid.NewGuid());

    public static LlmModelDto MockModel { get; } = new(
        Guid.NewGuid(),
        true,
        "test provider",
        new LlmPriceDto(Guid.NewGuid(), 100, 150),
        "test-mode-01",
        4000,
        128000,
        false,
        false,
        false,
        "Test Model",
        "yeehaw description",
        DateTime.UtcNow);

    public static ProfileEntity GenerateProfileEntity()
    {
        return new ProfileEntity
        {
            Id = SampleCreatorIdentifier,
            DefaultSystemMessage = "I'm a test system message",
            SelectedModel = MockModel.Id,
        };
    }

    public static ConversationEntity GenerateSampleConversation()
    {
        var m1 = new MessageEntity
        {
            Id = SeededInitialMessageId,
            IsUserMessage = true,
            Content =
            [
                new ContentEntity
                {
                    Id = new ContentEntityId(Guid.NewGuid()),
                    ContentType = MessageContentType.Text,
                    Content = "Hello",
                },
            ],
            Prompt = default,
            PreviousMessage = default,
            CompletedUtc = DateTime.UtcNow,
        };

        var m2 = new MessageEntity
        {
            Id = SeededSecondMessageId,
            IsUserMessage = false,
            Content =
            [
                new ContentEntity
                {
                    Id = new ContentEntityId(Guid.NewGuid()),
                    ContentType = MessageContentType.Text,
                    Content = "Hi! how can i help you?",
                },
            ],
            Prompt = default,
            PreviousMessage = m1,
            CompletedUtc = DateTime.UtcNow,
        };

        var profile = GenerateProfileEntity();

        return new ConversationEntity
        {
            Id = SeededConversationId,
            Creator = profile,
            SystemMessage = profile.DefaultSystemMessage,
            Messages = [m1, m2],
            LastAppendedUtc = DateTime.UtcNow,
            CreatedUtc = DateTime.UtcNow,
        };
    }
}
