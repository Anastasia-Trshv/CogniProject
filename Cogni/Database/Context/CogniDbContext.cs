using System;
using System.Collections.Generic;
using Cogni.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cogni.Database.Context;

public partial class CogniDbContext : DbContext
{
    public CogniDbContext()
    {
    }

    public CogniDbContext(DbContextOptions<CogniDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<ArticleImage> ArticleImages { get; set; }

    public virtual DbSet<Avatar> Avatars { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Customuser> Customusers { get; set; }

    public virtual DbSet<DjangoMigration> DjangoMigrations { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<MbtiQuestion> MbtiQuestions { get; set; }

    public virtual DbSet<MbtiType> MbtiTypes { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostImage> PostImages { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<UserTag> UserTags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=89.46.131.128;Port=5432;Database=CogniDB;Username=CogniAdmin;Password=sddbjssb1221j");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.IdArticle).HasName("article_pkey");

            entity.ToTable("article");

            entity.Property(e => e.IdArticle).HasColumnName("id_article");
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
        });

        modelBuilder.Entity<ArticleImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("article_images_pkey");

            entity.ToTable("article_images");

            entity.Property(e => e.Id).HasColumnName("id");
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

            entity.Property(e => e.Id).HasColumnName("id");
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

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chats_pkey");

            entity.ToTable("chats");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Chats)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("chats_user_id_fkey");
        });

        modelBuilder.Entity<Customuser>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("customuser_pkey");

            entity.ToTable("customuser");

            entity.HasIndex(e => e.Name, "customuser_name_key").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Description)
                .HasMaxLength(45)
                .HasColumnName("description");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.IdMbtiType).HasColumnName("id_mbti_type");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Image)
                .HasMaxLength(45)
                .HasColumnName("image");
            entity.Property(e => e.LastLogin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_login");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");
            entity.Property(e => e.TypeMbti)
                .HasMaxLength(4)
                .HasColumnName("type_mbti");

            entity.HasOne(d => d.IdMbtiTypeNavigation).WithMany(p => p.Customusers)
                .HasForeignKey(d => d.IdMbtiType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customuser_id_mbti_type_fkey");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Customusers)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customuser_id_role_fkey");
        });

        modelBuilder.Entity<DjangoMigration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("django_migrations_pkey");

            entity.ToTable("django_migrations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.App)
                .HasMaxLength(255)
                .HasColumnName("app");
            entity.Property(e => e.Applied).HasColumnName("applied");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("friends_pkey");

            entity.ToTable("friends");

            entity.Property(e => e.Id).HasColumnName("id");
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
            entity.HasKey(e => e.IdMbtiQuestion).HasName("mbti_question_pkey");

            entity.ToTable("mbti_question");

            entity.Property(e => e.IdMbtiQuestion).HasColumnName("id_mbti_question");
            entity.Property(e => e.Question)
                .HasMaxLength(45)
                .HasColumnName("question");
        });

        modelBuilder.Entity<MbtiType>(entity =>
        {
            entity.HasKey(e => e.IdMbtiType).HasName("mbti_type_pkey");

            entity.ToTable("mbti_type");

            entity.Property(e => e.IdMbtiType).HasColumnName("id_mbti_type");
            entity.Property(e => e.NameOfType)
                .HasMaxLength(45)
                .HasColumnName("name_of_type");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("messages_pkey");

            entity.ToTable("messages");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AttachmentUrl)
                .HasMaxLength(255)
                .HasColumnName("attachment_url");
            entity.Property(e => e.AvatarId).HasColumnName("avatar_id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.MessageBody).HasColumnName("message_body");

            entity.HasOne(d => d.Avatar).WithMany(p => p.Messages)
                .HasForeignKey(d => d.AvatarId)
                .HasConstraintName("messages_avatar_id_fkey");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("messages_chat_id_fkey");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.IdPost).HasName("post_pkey");

            entity.ToTable("post");

            entity.Property(e => e.IdPost).HasColumnName("id_post");
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

            entity.Property(e => e.Id).HasColumnName("id");
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
            entity.HasKey(e => e.IdRole).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.NameRole)
                .HasMaxLength(45)
                .HasColumnName("name_role");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.IdTag).HasName("tag_pkey");

            entity.ToTable("tag");

            entity.Property(e => e.IdTag).HasColumnName("id_tag");
            entity.Property(e => e.NameTag)
                .HasMaxLength(45)
                .HasColumnName("name_tag");
        });

        modelBuilder.Entity<UserTag>(entity =>
        {
            entity.HasKey(e => e.IdUserTags).HasName("user_tags_pkey");

            entity.ToTable("user_tags");

            entity.Property(e => e.IdUserTags).HasColumnName("id_user_tags");
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
