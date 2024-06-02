using Implementation.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.Database;

public class TestDatabase
{
    private readonly SqliteConnection connection;
    private readonly ApplicationContext context;

    protected TestDatabase()
    {
        this.connection = new SqliteConnection("DataSource=:memory:");
        this.connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseSqlite(this.connection) // Generate a unique in-memory database for each test
            .Options;
        
        this.context = new ApplicationContext(options);
        
        this.context.Database.EnsureCreated();

        this.context.SaveChanges();
    }

    protected ApplicationContext Context => this.context;

    public void Dispose()
    {
        this.context.Database.EnsureDeleted();
        this.context.Dispose();

        this.connection.Close();
        this.connection.Dispose();
    }

    protected async Task ExecuteDatabaseSeed()
    {
        this.SeedDatabase();
        await this.context.SaveChangesAsync();
    }

    protected virtual void SeedDatabase()
    {
        // empty by default
    }
}
