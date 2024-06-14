using Domain.Entity;
using Domain.Entity.Id;
using Implementation.Map;

namespace UnitTest;

public class PromptMapperTests
{
    private readonly ConversationEntity sampleConversation = ConversationMockDataGenerator.GenerateSampleConversation();

    [Fact]
    public void PromptShouldMapAlternatingUserAndAssistantMessages()
    {
        // Arrange
        var pseudoNow = DateTime.UtcNow.Subtract(TimeSpan.FromMilliseconds(20));

        var content = new ContentEntity
        {
            Id = new ContentEntityId(Guid.NewGuid()),
            ContentType = MessageContentType.Text,
            Content = "Hello, World!",
        };

        var newMessage = new MessageEntity
        {
            Id = new MessageEntityId(Guid.NewGuid()),
            IsUserMessage = true,
            Content = [content],
            Prompt = default,
            PreviousMessage = this.sampleConversation.DialogSlices.Last().Messages.Last(),
            CompletedUtc = pseudoNow,
        };

        var dialogSliceEntity = new DialogSliceEntity
        {
            Id = new DialogSliceEntityId(Guid.NewGuid()),
            Messages = [newMessage],
            SelectedMessageGuid = default,
            CreatedUtc = pseudoNow,
        };

        this.sampleConversation.DialogSlices.Add(dialogSliceEntity);

        // Act
        var promptResult = PromptMapper.ToPrompt(
            this.sampleConversation,
            newMessage.Id,
            ConversationMockDataGenerator.MockModel.Id);

        // Assert
        Assert.True(promptResult.IsSuccess);
        var prompt = promptResult.Unwrap();
        Assert.NotNull(prompt);
        Assert.Equal(this.sampleConversation.DialogSlices.Count, prompt.Messages.Count);
    }
}
