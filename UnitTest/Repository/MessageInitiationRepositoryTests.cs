using Domain.Entity;
using Domain.Entity.Id;
using Domain.Pipeline.SendMessage;
using Implementation.Repository;
using Microsoft.EntityFrameworkCore;
using UnitTest.Database;

namespace UnitTest.Repository;

public class MessageInitiationRepositoryTests : TestDatabase
{
    private readonly MessageInitiationRepository sut;

    public MessageInitiationRepositoryTests()
    {
        this.sut = new MessageInitiationRepository();
    }

    [Fact]
    public async Task RespondingToExsistingConversationResultsInResponseBeingProperlyAddedToSlice()
    {
        // Arrange
        await this.ExecuteDatabaseSeed();
        var content = new ContentEntity
        {
            Id = new ContentEntityId(Guid.NewGuid()),
            ContentType = MessageContentType.Text,
            Content = "Tell me a story!",
        };

        var validatedSendMessageData = new ValidatedSendMessageData
        {
            RequestConversationId = ConversationMockDataGenerator.SeededConversationId,
            ResponseToMessageId = ConversationMockDataGenerator.SeededSecondMessageId,
            Content = [content],
            SelectedModel = ConversationMockDataGenerator.MockModel,
        };

        // Act
        var foundConversation = await this.Context.Conversations
            .Where(c => c.Id == ConversationMockDataGenerator.SeededConversationId)
            .Include(c => c.Messages)
            .FirstAsync(CancellationToken.None);
        var result = this.sut.InitiateMessage(
            validatedSendMessageData,
            foundConversation,
            default,
            default);
        await this.Context.SaveChangesAsync();

        // Assert
        Assert.IsType<MessageEntity>(result.Unwrap());
        Assert.True(result.IsSuccess);
        Assert.IsNotType<Exception>(result.Error);
        Assert.Null(result.Error);
        var conv = await this.Context.Conversations
            .Include(c => c.Messages)
                .ThenInclude(m => m.Content)
            .FirstOrDefaultAsync();
        
        Assert.NotNull(conv);
        Assert.True(conv.Messages.Exists(m => m.Content.First().Content == content.Content));
    }

    protected override void SeedDatabase()
    {
        var conv = ConversationMockDataGenerator.GenerateSampleConversation();
        this.Context.Conversations.Add(conv);
        this.Context.SaveChanges();
    }
}
