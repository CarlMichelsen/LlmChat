﻿// <auto-generated />
using System;
using Implementation.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240531181146_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("LlmChat")
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entity.ContentEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ContentType")
                        .HasColumnType("integer");

                    b.Property<long?>("MessageEntityId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MessageEntityId");

                    b.ToTable("ContentEntity", "LlmChat");
                });

            modelBuilder.Entity("Domain.Entity.ConversationEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

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
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CompletedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("ConversationEntityId")
                        .HasColumnType("bigint");

                    b.Property<long?>("PreviousMessageId")
                        .HasColumnType("bigint");

                    b.Property<long?>("PromptId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ConversationEntityId");

                    b.HasIndex("PreviousMessageId");

                    b.HasIndex("PromptId");

                    b.ToTable("Messages", "LlmChat");
                });

            modelBuilder.Entity("Domain.Entity.PromptEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

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
