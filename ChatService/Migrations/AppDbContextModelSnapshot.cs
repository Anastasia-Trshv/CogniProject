﻿// <auto-generated />
using System;
using System.Collections.Generic;
using ChatService.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChatService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ChatService.Database.Entities.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("MessageId"));

                    b.PrimitiveCollection<List<string>>("Attachments")
                        .HasColumnType("text[]");

                    b.Property<Guid>("ChatId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsEdited")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsFunctional")
                        .HasColumnType("boolean");

                    b.Property<string>("Msg")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid");

                    b.HasKey("MessageId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("ChatService.Database.Entities.MessageStatus", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ChatId")
                        .HasColumnType("uuid");

                    b.Property<int>("LastReaden")
                        .HasColumnType("integer");

                    b.Property<int?>("MessageId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "ChatId");

                    b.HasIndex("ChatId");

                    b.HasIndex("MessageId");

                    b.ToTable("MessageStatus");
                });

            modelBuilder.Entity("ChatService.Entities.Chat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<bool>("isDm")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("ChatService.Entities.ChatMember", b =>
                {
                    b.Property<Guid>("ChatId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("ChatId", "UserId");

                    b.ToTable("ChatMembers");
                });

            modelBuilder.Entity("ChatService.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChatService.Database.Entities.MessageStatus", b =>
                {
                    b.HasOne("ChatService.Entities.Chat", null)
                        .WithMany()
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatService.Database.Entities.Message", null)
                        .WithMany("MessageStatuses")
                        .HasForeignKey("MessageId");
                });

            modelBuilder.Entity("ChatService.Entities.ChatMember", b =>
                {
                    b.HasOne("ChatService.Entities.Chat", "Chat")
                        .WithMany("Members")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");
                });

            modelBuilder.Entity("ChatService.Database.Entities.Message", b =>
                {
                    b.Navigation("MessageStatuses");
                });

            modelBuilder.Entity("ChatService.Entities.Chat", b =>
                {
                    b.Navigation("Members");
                });
#pragma warning restore 612, 618
        }
    }
}
