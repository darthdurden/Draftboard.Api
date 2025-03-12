﻿// <auto-generated />
using System;
using Draftboard.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Draftboard.Api.Migrations
{
    [DbContext(typeof(DraftboardDbContext))]
    [Migration("20221224153114_ExpandedTeamInfo2")]
    partial class ExpandedTeamInfo2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Draftboard.Api.Data.Models.FantasyTeam", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("DraftCashAdjustment")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("OwnerName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("FantasyTeams");
                });

            modelBuilder.Entity("Draftboard.Api.Data.Models.FantasyTeamPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Contract")
                        .HasColumnType("longtext");

                    b.Property<string>("FantasyTeamId")
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("PickNumber")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<float>("Salary")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("FantasyTeamId");

                    b.HasIndex("PlayerId")
                        .IsUnique();

                    b.ToTable("FantasyTeamPlayers");
                });

            modelBuilder.Entity("Draftboard.Api.Data.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("FantraxId")
                        .HasColumnType("longtext");

                    b.Property<string>("MLBTeam")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("Rank")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Rank");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Draftboard.Api.Data.Models.PlayerPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<string>("PositionId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("PositionId");

                    b.ToTable("PlayerPositions");
                });

            modelBuilder.Entity("Draftboard.Api.Data.Models.PlayerStats", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<float?>("ERA")
                        .HasColumnType("float");

                    b.Property<int?>("Homeruns")
                        .HasColumnType("int");

                    b.Property<float?>("NetStolenBases2")
                        .HasColumnType("float");

                    b.Property<float?>("OnBasePct")
                        .HasColumnType("float");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int?>("RPC")
                        .HasColumnType("int");

                    b.Property<int?>("RunsProduced")
                        .HasColumnType("int");

                    b.Property<int?>("SPC")
                        .HasColumnType("int");

                    b.Property<int>("Season")
                        .HasColumnType("int");

                    b.Property<int?>("Strikeouts")
                        .HasColumnType("int");

                    b.Property<int?>("TotalBases")
                        .HasColumnType("int");

                    b.Property<float?>("WHIP")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("Season");

                    b.ToTable("PlayerStats");
                });

            modelBuilder.Entity("Draftboard.Api.Data.Models.Position", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("Draftboard.Api.Data.Models.FantasyTeamPlayer", b =>
                {
                    b.HasOne("Draftboard.Api.Data.Models.FantasyTeam", "FantasyTeam")
                        .WithMany("FantasyTeamPlayers")
                        .HasForeignKey("FantasyTeamId");

                    b.HasOne("Draftboard.Api.Data.Models.Player", "Player")
                        .WithOne("FantasyTeamPlayer")
                        .HasForeignKey("Draftboard.Api.Data.Models.FantasyTeamPlayer", "PlayerId");

                    b.Navigation("FantasyTeam");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Draftboard.Api.Data.Models.PlayerPosition", b =>
                {
                    b.HasOne("Draftboard.Api.Data.Models.Player", "Player")
                        .WithMany("PlayerPositions")
                        .HasForeignKey("PlayerId");

                    b.HasOne("Draftboard.Api.Data.Models.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId");

                    b.Navigation("Player");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("Draftboard.Api.Data.Models.PlayerStats", b =>
                {
                    b.HasOne("Draftboard.Api.Data.Models.Player", "Player")
                        .WithMany("PlayerStats")
                        .HasForeignKey("PlayerId");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Draftboard.Api.Data.Models.FantasyTeam", b =>
                {
                    b.Navigation("FantasyTeamPlayers");
                });

            modelBuilder.Entity("Draftboard.Api.Data.Models.Player", b =>
                {
                    b.Navigation("FantasyTeamPlayer");

                    b.Navigation("PlayerPositions");

                    b.Navigation("PlayerStats");
                });
#pragma warning restore 612, 618
        }
    }
}
