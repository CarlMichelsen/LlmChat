using Domain.Entity;
using Domain.Pipeline.SendMessage;
using Implementation.Map;

namespace UnitTest;

public class PromptMapperTests
{
    private readonly ConversationEntity sampleConversation = ConversationMockDataGenerator.GenerateSampleConversation();

    [Fact]
    public void PromptShouldMapAlternatingUserAndAssistantMessages()
    {
        // Arrange
        var validatedSendMessageData = new ValidatedSendMessageData
        {
            RequestConversationId = this.sampleConversation.Id,
            ResponseToMessageId = this.sampleConversation.Messages.Where(m => !m.IsUserMessage).Last().Id,
            SelectedModel = ConversationMockDataGenerator.MockModel,
            Content =
            [
                new ContentEntity
                {
                    Id = new Domain.Entity.Id.ContentEntityId(Guid.NewGuid()),
                    ContentType = MessageContentType.Text,
                    Content = "Hello, World!",
                },
            ],
        };

        // Act
        var promptResult = PromptMapper.ToPrompt(this.sampleConversation, validatedSendMessageData);

        // Assert
        Assert.True(promptResult.IsSuccess);
        var prompt = promptResult.Unwrap();
        Assert.NotNull(prompt);
        Assert.Equal(this.sampleConversation.Messages.Count + 1, prompt.Messages.Count);
    }
}
