﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TelegramBot.DataAccess;

namespace TelegramBot.Application.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220309140008_AddSubsription")]
    partial class AddSubsription
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.14")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("TelegramBot.Domain.Models.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<byte>("SubscriptionItem")
                        .HasColumnType("smallint");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Subscription");
                });

            modelBuilder.Entity("TelegramBot.Domain.Models.TimeTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<byte>("EvenWeek")
                        .HasColumnType("smallint");

                    b.Property<string>("Group")
                        .HasColumnType("text");

                    b.Property<string>("LessonsOfTheDay")
                        .HasColumnType("text");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("Week")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TimeTables");
                });

            modelBuilder.Entity("TelegramBot.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<long>("IdChat")
                        .HasColumnType("bigint");

                    b.Property<string>("NameGroup")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TelegramBot.Domain.Models.Subscription", b =>
                {
                    b.HasOne("TelegramBot.Domain.Models.User", null)
                        .WithMany("Subscriptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TelegramBot.Domain.Models.TimeTable", b =>
                {
                    b.HasOne("TelegramBot.Domain.Models.User", null)
                        .WithMany("TimeTables")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TelegramBot.Domain.Models.User", b =>
                {
                    b.Navigation("Subscriptions");

                    b.Navigation("TimeTables");
                });
#pragma warning restore 612, 618
        }
    }
}
