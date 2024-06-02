using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Database;

/// <summary>
/// EntityFramework application context.
/// </summary>
public sealed class ApplicationContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationContext"/> class.
    /// </summary>
    /// <param name="options">Options for datacontext.</param>
    public ApplicationContext(
        DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public DbSet<ConversationEntity> Conversations { get; init; }

    public DbSet<MessageEntity> Messages { get; init; }

    public DbSet<PromptEntity> Prompts { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("LlmChat");
    }
}
