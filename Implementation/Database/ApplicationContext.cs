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

    public DbSet<DialogSliceEntity> DialogSlices { get; init; }

    public DbSet<MessageEntity> Messages { get; init; }

    public DbSet<ContentEntity> Content { get; init; }

    public DbSet<PromptEntity> Prompts { get; init; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var softDeleteEntries = this.ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(e => e.State == EntityState.Deleted);
        
        foreach (var entityEntry in softDeleteEntries)
        {
            entityEntry.State = EntityState.Modified;
            entityEntry.Property(nameof(ISoftDeletable.DeletedAtUtc)).CurrentValue = DateTime.UtcNow;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("LlmChat");

        modelBuilder.Entity<ConversationEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasConversion(id => id.Value, guid => new ConversationEntityId(guid));

            entity.HasQueryFilter(e => e.DeletedAtUtc == null);
            entity.HasIndex(p => p.DeletedAtUtc).HasFilter("\"Conversations\".\"DeletedAtUtc\" IS NULL");
        });

        modelBuilder.Entity<ProfileEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasConversion(id => id.Value, guid => new ProfileEntityId(guid));
        });

        modelBuilder.Entity<DialogSliceEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasConversion(id => id.Value, guid => new DialogSliceEntityId(guid));
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
