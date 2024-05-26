using System.IO.Compression;
using Domain.Conversation;
using Domain.Dto.Chat;
using Domain.Dto.Chat.Stream;
using Domain.Entity;
using Implementation.Repository;
using LargeLanguageModelClient.Dto.Model;
using Microsoft.EntityFrameworkCore;
using UnitTest.Database;

namespace UnitTest.Repository;

public class MessageInitiationRepositoryTests : TestDatabase
{
    private const int SeededConversationId = 1;
    private const int SeededInitialMessageId = 1;
    private const int SeededSecondMessageId = 2;

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
        var content = new ContentDto
        {
            Index = 0,
            ContentType = "Text",
            Content = "Tell me a story!",
        };

        var newUserMessageDto = new NewMessageDto(
            ConversationId: SeededConversationId,
            ResponseToMessageId: SeededSecondMessageId,
            Content: [content],
            ModelIdentifier: Guid.Parse("2370891f-9593-4ba6-be41-56e47fa6083f"));

        // Act
        var foundConversation = await this.Context.Conversations
            .Where(c => c.Id == SeededConversationId)
            .Include(c => c.Messages)
            .FirstAsync(CancellationToken.None);
        var result = await this.sut.InitiateMessage(
            newUserMessageDto,
            this.mockModel,
            foundConversation,
            default);
        await this.Context.SaveChangesAsync();

        // Assert
        Assert.IsType<NewMessageData>(result.Unwrap());
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
            Id = SeededInitialMessageId,
            Content = [new ContentEntity { ContentType = MessageContentType.Text, Content = "Hello" }],
            Prompt = default,
            PreviousMessage = default,
            CompletedUtc = DateTime.UtcNow,
        };

        var m2 = new MessageEntity
        {
            Id = SeededSecondMessageId,
            Content = [new ContentEntity { ContentType = MessageContentType.Text, Content = "Hi! how can i help you?" }],
            Prompt = default,
            PreviousMessage = default,
            CompletedUtc = DateTime.UtcNow,
        };

        var conv = new ConversationEntity
        {
            Id = SeededConversationId,
            CreatorIdentifier = this.creatorIdentifier,
            Messages =
            [m1, m2],
            CreatedUtc = DateTime.UtcNow,
        };

        this.Context.Conversations.Add(conv);

        this.Context.SaveChanges();
    }
}
