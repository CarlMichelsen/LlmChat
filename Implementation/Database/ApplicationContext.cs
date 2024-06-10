using Domain.Entity;
using Domain.Entity.Id;
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

    public DbSet<ProfileEntity> Profiles { get; init; }

    public DbSet<MessageEntity> Messages { get; init; }

    public DbSet<ContentEntity> Content { get; init; }

    public DbSet<PromptEntity> Prompts { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("LlmChat");

        modelBuilder.Entity<ConversationEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasConversion(id => id.Value, guid => new ConversationEntityId(guid));
        });

        modelBuilder.Entity<ProfileEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasConversion(id => id.Value, guid => new ProfileEntityId(guid));
        });

        modelBuilder.Entity<MessageEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasConversion(id => id.Value, guid => new MessageEntityId(guid));
        });

        modelBuilder.Entity<ContentEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasConversion(id => id.Value, guid => new ContentEntityId(guid));
        });

        modelBuilder.Entity<PromptEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasConversion(id => id.Value, guid => new PromptEntityId(guid));
        });
    }
}
