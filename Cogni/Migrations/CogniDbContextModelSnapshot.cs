﻿// <auto-generated />
using System;
using Cogni.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cogni.Migrations
{
    [DbContext(typeof(CogniDbContext))]
    partial class CogniDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Cogni.Database.Entities.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_article");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ArticleBody")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnName("article_body");

                    b.Property<string>("ArticleName")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("article_name");

                    b.Property<int>("IdUser")
                        .HasColumnType("integer")
                        .HasColumnName("id_user");

                    b.HasKey("Id")
                        .HasName("article_pkey");

                    b.HasIndex("IdUser");

                    b.ToTable("article", (string)null);
                });

            modelBuilder.Entity("Cogni.Database.Entities.ArticleImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ArticleId")
                        .HasColumnType("integer")
                        .HasColumnName("article_id");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("image_url");

                    b.HasKey("Id")
                        .HasName("article_images_pkey");

                    b.HasIndex("ArticleId");

                    b.ToTable("article_images", (string)null);
                });

            modelBuilder.Entity("Cogni.Database.Entities.Avatar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AvatarUrl")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("avatar_url");

                    b.Property<DateTime?>("DateAdded")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_added")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<bool?>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("is_active");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("avatars_pkey");

                    b.HasIndex("UserId");

                    b.ToTable("avatars", (string)null);
                });

            modelBuilder.Entity("Cogni.Database.Entities.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("chats_pkey");

                    b.HasIndex("UserId");

                    b.ToTable("chats", (string)null);
                });

            modelBuilder.Entity("Cogni.Database.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_role");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("NameRole")
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("name_role");

                    b.HasKey("Id")
                        .HasName("role_pkey");

                    b.ToTable("role", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            NameRole = "User"
                        },
                        new
                        {
                            Id = 2,
                            NameRole = "Admin"
                        },
                        new
                        {
                            Id = 3,
                            NameRole = "Moderator"
                        });
                });

            modelBuilder.Entity("Cogni.Database.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_tag");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("NameTag")
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("name_tag");

                    b.HasKey("Id")
                        .HasName("tag_pkey");

                    b.ToTable("tag", (string)null);
                });

            modelBuilder.Entity("Cogni.Database.Entities.UserTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_user_tags");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("IdTag")
                        .HasColumnType("integer")
                        .HasColumnName("id_tag");

                    b.Property<int>("IdUser")
                        .HasColumnType("integer")
                        .HasColumnName("id_user");

                    b.HasKey("Id")
                        .HasName("user_tags_pkey");

                    b.HasIndex("IdTag");

                    b.HasIndex("IdUser");

                    b.ToTable("user_tags", (string)null);
                });

            modelBuilder.Entity("Cogni.Friend", b =>
                {
                    b.Property<int>("FriendId")
                        .HasColumnType("integer")
                        .HasColumnName("friend_id");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<DateTime?>("DateAdded")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_added")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("FriendId", "UserId")
                        .HasName("friends_pkey");

                    b.HasIndex("UserId");

                    b.ToTable("friends", (string)null);
                });

            modelBuilder.Entity("Cogni.Like", b =>
                {
                    b.Property<DateTime?>("LikedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("liked_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int?>("PostId")
                        .HasColumnType("integer")
                        .HasColumnName("post_id");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("likes", (string)null);
                });

            modelBuilder.Entity("Cogni.MbtiQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_mbti_question");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Question")
                        .HasMaxLength(130)
                        .HasColumnType("character varying(130)")
                        .HasColumnName("question");

                    b.HasKey("Id")
                        .HasName("mbti_question_pkey");

                    b.ToTable("mbti_question", (string)null);
                });

            modelBuilder.Entity("Cogni.MbtiType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_mbti_type");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("NameOfType")
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("name_of_type");

                    b.HasKey("Id")
                        .HasName("mbti_type_pkey");

                    b.ToTable("mbti_type", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            NameOfType = "ENFJ"
                        },
                        new
                        {
                            Id = 2,
                            NameOfType = "ENTJ"
                        },
                        new
                        {
                            Id = 3,
                            NameOfType = "ENFP"
                        },
                        new
                        {
                            Id = 4,
                            NameOfType = "ENTP"
                        },
                        new
                        {
                            Id = 5,
                            NameOfType = "INFJ"
                        },
                        new
                        {
                            Id = 6,
                            NameOfType = "INTJ"
                        },
                        new
                        {
                            Id = 7,
                            NameOfType = "INFP"
                        },
                        new
                        {
                            Id = 8,
                            NameOfType = "INTP"
                        },
                        new
                        {
                            Id = 9,
                            NameOfType = "ISFP"
                        },
                        new
                        {
                            Id = 10,
                            NameOfType = "ISFJ"
                        },
                        new
                        {
                            Id = 11,
                            NameOfType = "ESFP"
                        },
                        new
                        {
                            Id = 12,
                            NameOfType = "ESFJ"
                        },
                        new
                        {
                            Id = 13,
                            NameOfType = "ISTJ"
                        },
                        new
                        {
                            Id = 14,
                            NameOfType = "ISTP"
                        },
                        new
                        {
                            Id = 15,
                            NameOfType = "ESTP"
                        },
                        new
                        {
                            Id = 16,
                            NameOfType = "ESTJ"
                        });
                });

            modelBuilder.Entity("Cogni.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AttachmentUrl")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("attachment_url");

                    b.Property<int>("AvatarId")
                        .HasColumnType("integer")
                        .HasColumnName("avatar_id");

                    b.Property<int>("ChatId")
                        .HasColumnType("integer")
                        .HasColumnName("chat_id");

                    b.Property<string>("MessageBody")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("message_body");

                    b.HasKey("Id")
                        .HasName("messages_pkey");

                    b.HasIndex("AvatarId");

                    b.HasIndex("ChatId");

                    b.ToTable("messages", (string)null);
                });

            modelBuilder.Entity("Cogni.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_post");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("IdUser")
                        .HasColumnType("integer")
                        .HasColumnName("id_user");

                    b.Property<string>("PostBody")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnName("post_body");

                    b.HasKey("Id")
                        .HasName("post_pkey");

                    b.HasIndex("IdUser");

                    b.ToTable("post", (string)null);
                });

            modelBuilder.Entity("Cogni.PostImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("image_url");

                    b.Property<int>("PostId")
                        .HasColumnType("integer")
                        .HasColumnName("post_id");

                    b.HasKey("Id")
                        .HasName("post_images_pkey");

                    b.HasIndex("PostId");

                    b.ToTable("post_images", (string)null);
                });

            modelBuilder.Entity("Cogni.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_user");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AToken")
                        .HasColumnType("text");

                    b.Property<string>("BannerImage")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("description");

                    b.Property<string>("Email")
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("email");

                    b.Property<int>("IdMbtiType")
                        .HasColumnType("integer")
                        .HasColumnName("id_mbti_type");

                    b.Property<int>("IdRole")
                        .HasColumnType("integer")
                        .HasColumnName("id_role");

                    b.Property<DateTime?>("LastLogin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("last_login")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("name");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("RToken")
                        .HasColumnType("text");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("customuser_pkey");

                    b.HasIndex("IdMbtiType");

                    b.HasIndex("IdRole");

                    b.HasIndex(new[] { "Name" }, "customuser_name_key");

                    b.ToTable("customuser", (string)null);
                });

            modelBuilder.Entity("Cogni.Database.Entities.Article", b =>
                {
                    b.HasOne("Cogni.User", "IdUserNavigation")
                        .WithMany("Articles")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("article_id_user_fkey");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Cogni.Database.Entities.ArticleImage", b =>
                {
                    b.HasOne("Cogni.Database.Entities.Article", "Article")
                        .WithMany("ArticleImages")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("article_images_article_id_fkey");

                    b.Navigation("Article");
                });

            modelBuilder.Entity("Cogni.Database.Entities.Avatar", b =>
                {
                    b.HasOne("Cogni.User", "User")
                        .WithMany("Avatars")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("avatars_user_id_fkey");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Cogni.Database.Entities.Chat", b =>
                {
                    b.HasOne("Cogni.User", "User")
                        .WithMany("Chats")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("chats_user_id_fkey");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Cogni.Database.Entities.UserTag", b =>
                {
                    b.HasOne("Cogni.Database.Entities.Tag", "IdTagNavigation")
                        .WithMany("UserTags")
                        .HasForeignKey("IdTag")
                        .IsRequired()
                        .HasConstraintName("user_tags_id_tag_fkey");

                    b.HasOne("Cogni.User", "IdUserNavigation")
                        .WithMany("UserTags")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("user_tags_id_user_fkey");

                    b.Navigation("IdTagNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Cogni.Friend", b =>
                {
                    b.HasOne("Cogni.User", "FriendNavigation")
                        .WithMany("FriendFriendNavigations")
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("friends_friend_id_fkey");

                    b.HasOne("Cogni.User", "User")
                        .WithMany("FriendUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("friends_user_id_fkey");

                    b.Navigation("FriendNavigation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Cogni.Like", b =>
                {
                    b.HasOne("Cogni.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("likes_post_id_fkey");

                    b.HasOne("Cogni.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("likes_user_id_fkey");

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Cogni.Message", b =>
                {
                    b.HasOne("Cogni.Database.Entities.Avatar", "Avatar")
                        .WithMany("Messages")
                        .HasForeignKey("AvatarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("messages_avatar_id_fkey");

                    b.HasOne("Cogni.Database.Entities.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("messages_chat_id_fkey");

                    b.Navigation("Avatar");

                    b.Navigation("Chat");
                });

            modelBuilder.Entity("Cogni.Post", b =>
                {
                    b.HasOne("Cogni.User", "IdUserNavigation")
                        .WithMany("Posts")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("post_id_user_fkey");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Cogni.PostImage", b =>
                {
                    b.HasOne("Cogni.Post", "Post")
                        .WithMany("PostImages")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("post_images_post_id_fkey");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Cogni.User", b =>
                {
                    b.HasOne("Cogni.MbtiType", "IdMbtiTypeNavigation")
                        .WithMany("Customusers")
                        .HasForeignKey("IdMbtiType")
                        .IsRequired()
                        .HasConstraintName("customuser_id_mbti_type_fkey");

                    b.HasOne("Cogni.Database.Entities.Role", "IdRoleNavigation")
                        .WithMany("Customusers")
                        .HasForeignKey("IdRole")
                        .IsRequired()
                        .HasConstraintName("customuser_id_role_fkey");

                    b.Navigation("IdMbtiTypeNavigation");

                    b.Navigation("IdRoleNavigation");
                });

            modelBuilder.Entity("Cogni.Database.Entities.Article", b =>
                {
                    b.Navigation("ArticleImages");
                });

            modelBuilder.Entity("Cogni.Database.Entities.Avatar", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Cogni.Database.Entities.Chat", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Cogni.Database.Entities.Role", b =>
                {
                    b.Navigation("Customusers");
                });

            modelBuilder.Entity("Cogni.Database.Entities.Tag", b =>
                {
                    b.Navigation("UserTags");
                });

            modelBuilder.Entity("Cogni.MbtiType", b =>
                {
                    b.Navigation("Customusers");
                });

            modelBuilder.Entity("Cogni.Post", b =>
                {
                    b.Navigation("PostImages");
                });

            modelBuilder.Entity("Cogni.User", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("Avatars");

                    b.Navigation("Chats");

                    b.Navigation("FriendFriendNavigations");

                    b.Navigation("FriendUsers");

                    b.Navigation("Posts");

                    b.Navigation("UserTags");
                });
#pragma warning restore 612, 618
        }
    }
}
