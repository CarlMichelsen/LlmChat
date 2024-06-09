﻿// <auto-generated />
using System;
using Implementation.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("LlmChat")
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entity.ContentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ContentType")
                        .HasColumnType("integer");

                    b.Property<Guid?>("MessageEntityId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("MessageEntityId");

                    b.ToTable("Content", "LlmChat");
                });

            modelBuilder.Entity("Domain.Entity.ConversationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatorIdentifier")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastAppendedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Conversations", "LlmChat");
                });

            modelBuilder.Entity("Domain.Entity.MessageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CompletedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ConversationEntityId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsUserMessage")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("PreviousMessageId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PromptId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ConversationEntityId");

                    b.HasIndex("PreviousMessageId");

                    b.HasIndex("PromptId");

                    b.ToTable("Messages", "LlmChat");
                });

            modelBuilder.Entity("Domain.Entity.PromptEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<long>("CurrentMillionInputTokenPrice")
                        .HasColumnType("bigint");

                    b.Property<long>("CurrentMillionOutputTokenPrice")
                        .HasColumnType("bigint");

                    b.Property<long>("InputTokens")
                        .HasColumnType("bigint");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ModelId")
                        .HasColumnType("uuid");

                    b.Property<string>("ModelName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("OutputTokens")
                        .HasColumnType("bigint");

                    b.Property<string>("ProviderPromptIdentifier")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StopReason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Prompts", "LlmChat");
                });

            modelBuilder.Entity("Domain.Entity.ContentEntity", b =>
                {
                    b.HasOne("Domain.Entity.MessageEntity", null)
                        .WithMany("Content")
                        .HasForeignKey("MessageEntityId");
                });

            modelBuilder.Entity("Domain.Entity.MessageEntity", b =>
                {
                    b.HasOne("Domain.Entity.ConversationEntity", null)
                        .WithMany("Messages")
                        .HasForeignKey("ConversationEntityId");

                    b.HasOne("Domain.Entity.MessageEntity", "PreviousMessage")
                        .WithMany()
                        .HasForeignKey("PreviousMessageId");

                    b.HasOne("Domain.Entity.PromptEntity", "Prompt")
                        .WithMany()
                        .HasForeignKey("PromptId");

                    b.Navigation("PreviousMessage");

                    b.Navigation("Prompt");
                });

            modelBuilder.Entity("Domain.Entity.ConversationEntity", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Domain.Entity.MessageEntity", b =>
                {
                    b.Navigation("Content");
                });
#pragma warning restore 612, 618
        }
    }
}
