using Microsoft.EntityFrameworkCore;
using TodoManagement.Core.Entities;
using TodoManagement.Core.Enums;
using TodoManagement.Core.ValueObjects;

namespace WorkManager.Infrastructure.Data;
public class WorkManagerDbContext : DbContext
{
    public WorkManagerDbContext(DbContextOptions<WorkManagerDbContext> options) : base(options) {

    }

    public DbSet<TodoItem> TodoItems { get; set; } = null;
    public DbSet<TodoLog> TodoLogs { get; set; } = null;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureTodoItem(modelBuilder);
        ConfigureTodoLog(modelBuilder);
        ConfigureIndexes(modelBuilder);
        SeedData(modelBuilder);
    }

    private static void ConfigureTodoItem(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>(entity =>
        {
            // Primary Key
            entity.HasKey(e => e.Id);

            // Title Configuration
            entity.Property(e => e.Title)
                  .IsRequired()
                  .HasMaxLength(200)
                  .HasComment("The title/name of the todo item");

            // Description Configuration
            entity.Property(e => e.Description)
                  .HasMaxLength(2000)
                  .HasComment("Optional detailed description");

            // Status Configuration (Store as string)
            entity.Property(e => e.Status)
                  .IsRequired()
                  .HasConversion(
                      v => v.ToString(),
                      v => Enum.Parse<TodoStatus>(v))
                  .HasMaxLength(20)
                  .HasComment("Current status of the todo item");

            // Priority Configuration
            entity.Property(e => e.Priority)
              .HasConversion(
                  priority => priority.Value,           // Convert Priority to int for storage
                  value => new TodoPriority(value))         // Convert int back to Priority when loading
              .HasComment("Priority value from 1 (Very Low) to 5 (Very High)");


            // DateTime Configurations
            entity.Property(e => e.CreatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("datetime('now')")
                  .HasComment("When the todo was created");

            entity.Property(e => e.UpdatedAt)
                  .IsRequired()
                  .HasComment("When the todo was last modified");

            entity.Property(e => e.Deadline)
                  .HasComment("Optional deadline for completion");

            entity.Property(e => e.CompletedAt)
                  .HasComment("When the todo was marked as completed");

            // Navigation Properties
            entity.HasMany(e => e.Logs)
                  .WithOne(l => l.TodoItem)
                  .HasForeignKey(l => l.TodoItemId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureTodoLog(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoLog>(entity =>
        {
            // Primary Key
            entity.HasKey(e => e.Id);

            // Foreign Key
            entity.Property(e => e.TodoItemId)
                  .IsRequired()
                  .HasComment("Reference to the todo item");

            // Action Configuration
            entity.Property(e => e.Action)
                  .IsRequired()
                  .HasMaxLength(50)
                  .HasComment("Type of action performed");

            // Change Values
            entity.Property(e => e.OldValue)
                  .HasMaxLength(1000)
                  .HasComment("Previous value before change");

            entity.Property(e => e.NewValue)
                  .HasMaxLength(1000)
                  .HasComment("New value after change");

            // Timestamp
            entity.Property(e => e.ChangedAt)
                  .IsRequired()
                  .HasDefaultValueSql("datetime('now')")
                  .HasComment("When the change occurred");

            // Description
            entity.Property(e => e.Description)
                  .IsRequired()
                  .HasMaxLength(500)
                  .HasComment("Human-readable description of the change");
        });
    }

    private static void ConfigureIndexes(ModelBuilder modelBuilder)
    {
        // Performance indexes
        modelBuilder.Entity<TodoItem>()
                   .HasIndex(e => e.Status)
                   .HasDatabaseName("IX_TodoItems_Status");

        modelBuilder.Entity<TodoItem>()
                   .HasIndex(e => e.CreatedAt)
                   .HasDatabaseName("IX_TodoItems_CreatedAt");

        modelBuilder.Entity<TodoItem>()
                   .HasIndex(e => e.Deadline)
                   .HasDatabaseName("IX_TodoItems_Deadline");

        // Composite index for common queries
        modelBuilder.Entity<TodoItem>()
                   .HasIndex(e => new { e.Status, e.Priority })
                   .HasDatabaseName("IX_TodoItems_Status_Priority");

        // Audit log indexes
        modelBuilder.Entity<TodoLog>()
                   .HasIndex(e => e.TodoItemId)
                   .HasDatabaseName("IX_TodoLogs_TodoItemId");

        modelBuilder.Entity<TodoLog>()
                   .HasIndex(e => e.ChangedAt)
                   .HasDatabaseName("IX_TodoLogs_ChangedAt");
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var sampleTodoId = new Guid("12345678-1234-5678-9abc-123456789012");
        var seedDate = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc);

        // ✅ Much simpler seeding - EF Core handles Priority conversion automatically
        modelBuilder.Entity<TodoItem>().HasData(new
        {
            Id = sampleTodoId,
            Title = "Welcome to Work Management App! 🎉",
            Description = "This is your first todo item. Try editing it, changing its status, or creating new ones.",
            Status = TodoStatus.Todo,
            Priority = TodoPriority.Medium,
            CreatedAt = seedDate,
            UpdatedAt = seedDate,
            Deadline = (DateTime?)null,
            CompletedAt = (DateTime?)null
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Fallback connection string for design-time
            var connectionString = GetConnectionString();
            optionsBuilder.UseSqlite(connectionString);
        }

        // Development optimizations
#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging()
                     .EnableDetailedErrors()
                     .LogTo(Console.WriteLine);
#endif
    }

    public static string GetConnectionString()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appDataPath, "WorkManagementApp");
        Directory.CreateDirectory(appFolder);

        var databasePath = Path.Combine(appFolder, "TodoDatabase.db");
        return $"Data Source={databasePath};Foreign Keys=true;";
    }
}
