using System;
using System.Collections.Generic;
using Cogni.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Cogni.Database.Context;

public partial class CogniDbContext : DbContext
{
    private readonly string connectionString;
    public CogniDbContext()
    {
    }

    public CogniDbContext(DbContextOptions<CogniDbContext> options, IConfiguration config)
        : base(options)
    {
        connectionString = config["ConnectionStrings:PostgreSQLConnection"];
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<ArticleImage> ArticleImages { get; set; }

    public virtual DbSet<Avatar> Avatars { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<ChatMember> ChatMembers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<MbtiQuestion> MbtiQuestions { get; set; }

    public virtual DbSet<MbtiType> MbtiTypes { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<MessageStatus> MessageStatus { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostImage> PostImages { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<UserTag> UserTags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql(connectionString, x => x.MigrationsAssembly("Cogni"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("article_pkey");

            entity.ToTable("article");

            entity.Property(e => e.Id)
            .HasColumnName("id_article")
            .ValueGeneratedOnAdd();
            entity.Property(e => e.ArticleBody)
                .HasMaxLength(1024)
                .HasColumnName("article_body");
            entity.Property(e => e.ArticleName)
                .HasMaxLength(128)
                .HasColumnName("article_name");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Articles)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("article_id_user_fkey");

            entity.HasMany(a => a.ArticleImages)
            .WithOne()
            .HasForeignKey(ai => ai.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ArticleImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("article_images_pkey");

            entity.ToTable("article_images");

            entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
            entity.Property(e => e.ArticleId).HasColumnName("article_id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleImages)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("article_images_article_id_fkey");
        });

        modelBuilder.Entity<Avatar>(entity =>
        {
        entity.HasKey(e => e.Id).HasName("avatars_pkey");

        entity.ToTable("avatars");
            entity.Property(e => e.Id)
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .HasColumnName("avatar_url");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_added");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Avatars)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("avatars_user_id_fkey");
        });

        modelBuilder.Entity<ChatMember>(e => {
            e.Property(c => c.ChatId).HasColumnName("chat_id");
            e.Property(c => c.UserId).HasColumnName("user_id");
            e.HasKey(gcm => new { gcm.ChatId, gcm.UserId });
            e.ToTable("chat_members");
            e.HasOne(gcm => gcm.Chat)
                .WithMany(g => g.Members)
                .HasForeignKey(gcm => gcm.ChatId);
        });
            
        modelBuilder.Entity<Chat>(e => {
            e.Property(c => c.Id).HasColumnName("chat_id");
            e.Property(c => c.Name).HasColumnName("name");
            e.Property(c => c.OwnerId).HasColumnName("owner_id");
            e.Property(c => c.isDm).HasColumnName("is_dm");
            e.Property(c => c.CreatedAt).HasColumnName("created_at");
            e.ToTable("chats");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customuser_pkey");

            entity.ToTable("customuser");

            entity.HasIndex(e => e.Name, "customuser_name_key");

            entity.Property(e => e.Id)
            .HasColumnName("id_user")
            .ValueGeneratedOnAdd();
            entity.Property(e => e.Description)
                .HasMaxLength(45)
                .HasColumnName("description");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.IdMbtiType).HasColumnName("id_mbti_type");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.LastLogin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_login");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");

            entity.HasOne(d => d.IdMbtiTypeNavigation).WithMany(p => p.Customusers)
                .HasForeignKey(d => d.IdMbtiType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customuser_id_mbti_type_fkey");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Customusers)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customuser_id_role_fkey");
        });


        modelBuilder.Entity<Friend>(entity =>
        {
            //entity.HasKey(e => e.Id).HasName("friends_pkey");
            entity.HasKey(e => new { e.FriendId, e.UserId }).HasName("friends_pkey");

            entity.ToTable("friends");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_added");
            entity.Property(e => e.FriendId).HasColumnName("friend_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.FriendNavigation).WithMany(p => p.FriendFriendNavigations)
                .HasForeignKey(d => d.FriendId)
                .HasConstraintName("friends_friend_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.FriendUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("friends_user_id_fkey");
        });

        modelBuilder.Entity<Like>(entity =>
        {
        entity
            .HasNoKey()
            .ToTable("likes");
            entity.Property(e => e.LikedAt)
                            .HasDefaultValueSql("CURRENT_TIMESTAMP")
                            .HasColumnType("timestamp without time zone")
                            .HasColumnName("liked_at");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Post).WithMany()
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("likes_post_id_fkey");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("likes_user_id_fkey");
        });

        modelBuilder.Entity<MbtiQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mbti_question_pkey");

            entity.ToTable("mbti_question");

            entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id_mbti_question");
            entity.Property(e => e.Question)
                .HasMaxLength(130)
                .HasColumnName("question");
        });

        modelBuilder.Entity<MbtiType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mbti_type_pkey");

            entity.ToTable("mbti_type");

            entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id_mbti_type");
            entity.Property(e => e.NameOfType)
                .HasMaxLength(45)
                .HasColumnName("name_of_type");
            entity.HasData(
                new MbtiType[]
                {
                    new MbtiType{Id = 1, NameOfType ="ENFJ"},
                    new MbtiType{Id = 2, NameOfType ="ENTJ"},
                    new MbtiType{Id = 3, NameOfType ="ENFP"},
                    new MbtiType{Id = 4, NameOfType ="ENTP"},
                    new MbtiType{Id = 5, NameOfType ="INFJ"},
                    new MbtiType{Id = 6, NameOfType ="INTJ"},
                    new MbtiType{Id = 7, NameOfType ="INFP"},
                    new MbtiType{Id = 8, NameOfType ="INTP"},
                    new MbtiType{Id = 9, NameOfType ="ISFP"},
                    new MbtiType{Id = 10, NameOfType ="ISFJ"},
                    new MbtiType{Id = 11, NameOfType ="ESFP"},
                    new MbtiType{Id = 12, NameOfType ="ESFJ"},
                    new MbtiType{Id = 13, NameOfType ="ISTJ"},
                    new MbtiType{Id = 14, NameOfType ="ISTP"},
                    new MbtiType{Id = 15, NameOfType ="ESTP"},
                    new MbtiType{Id = 16, NameOfType ="ESTJ"},
                }
                );
        });
        modelBuilder.Entity<Message>(e => {
            e.Property(m => m.MessageId).HasColumnName("message_id");
            e.Property(m => m.ChatId).HasColumnName("chat_id");
            e.Property(m => m.SenderId).HasColumnName("sender_id");
            e.Property(m => m.Msg).HasColumnName("msg");
            e.Property(m => m.Date).HasColumnName("date");
            e.Property(m => m.IsEdited).HasColumnName("is_edited");
            e.Property(m => m.IsFunctional).HasColumnName("is_functional");
            // e.Property(m => m.Attachments).HasColumnName("attachments");
            // e.Property(m => m.MessageStatuses).HasColumnName("message_statuses");
            e.ToTable("messages");
            e.Property(m => m.MessageId)
                .ValueGeneratedOnAdd()
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);
        });
            
        modelBuilder.Entity<MessageStatus>()
            .ToTable("message_statuses")
            .HasKey(ms => new { ms.UserId, ms.ChatId });

        modelBuilder.Entity<MessageStatus>(e => {
            e.Property(ms => ms.UserId).HasColumnName("user_id");
            e.Property(ms => ms.ChatId).HasColumnName("chat_id");
            e.Property(ms => ms.LastReaden).HasColumnName("last_readen");
            e.ToTable("message_statuses");
            e.HasOne<Chat>()
                .WithMany()
                .HasForeignKey(ms => ms.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        });
            
        
        modelBuilder.Entity<Post>(entity =>
        {
        entity.HasKey(e => e.Id).HasName("post_pkey");

        entity.ToTable("post");
            entity.Property(e => e.Id)
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id_post");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.PostBody)
                .HasMaxLength(1024)
                .HasColumnName("post_body");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("post_id_user_fkey");
        });

        modelBuilder.Entity<PostImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("post_images_pkey");

            entity.ToTable("post_images");

            entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.PostId).HasColumnName("post_id");

            entity.HasOne(d => d.Post).WithMany(p => p.PostImages)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("post_images_post_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id_role");
            entity.Property(e => e.NameRole)
                .HasMaxLength(45)
                .HasColumnName("name_role");
            entity.HasData(
                new Role[]
                {
                    new Role{ Id = 1, NameRole = "User"},
                    new Role{ Id = 2, NameRole = "Admin"},
                    new Role{ Id = 3, NameRole = "Moderator"}
                }
                );
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tag_pkey");

            entity.ToTable("tag");

            entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id_tag");
            entity.Property(e => e.NameTag)
                .HasMaxLength(45)
                .HasColumnName("name_tag");
        });

        modelBuilder.Entity<UserTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_tags_pkey");

            entity.ToTable("user_tags");

            entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id_user_tags");
            entity.Property(e => e.IdTag).HasColumnName("id_tag");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdTagNavigation).WithMany(p => p.UserTags)
                .HasForeignKey(d => d.IdTag)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_tags_id_tag_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UserTags)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_tags_id_user_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}