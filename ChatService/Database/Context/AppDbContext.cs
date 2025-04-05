using ChatService.Database.Entities;
using ChatService.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
namespace ChatService.Database.Context;
public class AppDbContext : DbContext
{
    private readonly string connectionString;
    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config)
        : base(options)
    {
        connectionString = config["ConnectionStrings:PostgreSQLConnection"];
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(connectionString);
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatMember> ChatMembers { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<MessageStatus> MessageStatus { get; set; }
    public DbSet<User> Users { get; set; } // DEV-ONLY

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ChatMember>()
            .HasKey(gcm => new { gcm.ChatId, gcm.UserId });

        modelBuilder.Entity<ChatMember>()
            .HasOne(gcm => gcm.Chat)
            .WithMany(g => g.Members)
            .HasForeignKey(gcm => gcm.ChatId);
        
        modelBuilder.Entity<Message>()
            .Property(m => m.MessageId)
            .ValueGeneratedOnAdd()
            .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);
        
        modelBuilder.Entity<MessageStatus>()
            .HasKey(ms => new { ms.UserId, ms.ChatId });

        modelBuilder.Entity<MessageStatus>()
            .HasOne<Chat>()
            .WithMany()
            .HasForeignKey(ms => ms.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        // DEV-ONLY
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}
