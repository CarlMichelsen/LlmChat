using Domain.Entity;
using Domain.Entity.Id;
using Domain.Pipeline.SendMessage;
using Implementation.Repository;
using LargeLanguageModelClient.Dto.Model;
using Microsoft.EntityFrameworkCore;
using UnitTest.Database;

namespace UnitTest.Repository;

public class MessageInitiationRepositoryTests : TestDatabase
{
    private static ConversationEntityId seededConversationId = new ConversationEntityId(Guid.NewGuid());
    private static MessageEntityId seededInitialMessageId = new MessageEntityId(Guid.NewGuid());
    private static MessageEntityId seededSecondMessageId = new MessageEntityId(Guid.NewGuid());

    private readonly LlmModelDto mockModel = new(
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

    private readonly Guid creatorIdentifier = Guid.NewGuid();
    private readonly MessageInitiationRepository sut;

    public MessageInitiationRepositoryTests()
    {
        this.sut = new MessageInitiationRepository(this.Context);
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
            RequestConversationId = seededConversationId,
            ResponseToMessageId = seededSecondMessageId,
            Content = [content],
            SelectedModel = this.mockModel,
        };

        // Act
        var foundConversation = await this.Context.Conversations
            .Where(c => c.Id == seededConversationId)
            .Include(c => c.Messages)
            .FirstAsync(CancellationToken.None);
        var result = await this.sut.InitiateMessage(
            validatedSendMessageData,
            foundConversation,
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
        var m1 = new MessageEntity
        {
            Id = seededInitialMessageId,
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
            Id = seededSecondMessageId,
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
            PreviousMessage = default,
            CompletedUtc = DateTime.UtcNow,
        };

        var conv = new ConversationEntity
        {
            Id = seededConversationId,
            CreatorIdentifier = this.creatorIdentifier,
            Messages =
            [m1, m2],
            LastAppendedUtc = DateTime.UtcNow,
            CreatedUtc = DateTime.UtcNow,
        };

        this.Context.Conversations.Add(conv);
        this.Context.SaveChanges();
    }
}
