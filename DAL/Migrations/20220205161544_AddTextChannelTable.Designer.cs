﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(AppContext))]
    [Migration("20220205161544_AddTextChannelTable")]
    partial class AddTextChannelTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Core.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("CanDeleteOthersMessages")
                        .HasColumnType("bit");

                    b.Property<bool>("CanInvite")
                        .HasColumnType("bit");

                    b.Property<bool>("CanManageChannels")
                        .HasColumnType("bit");

                    b.Property<bool>("CanManageRoles")
                        .HasColumnType("bit");

                    b.Property<bool>("CanManageRoom")
                        .HasColumnType("bit");

                    b.Property<bool>("CanModerateParticipants")
                        .HasColumnType("bit");

                    b.Property<bool>("CanPin")
                        .HasColumnType("bit");

                    b.Property<bool>("CanUseAdminChannels")
                        .HasColumnType("bit");

                    b.Property<bool>("CanViewAuditLog")
                        .HasColumnType("bit");

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Core.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("BaseRoleId")
                        .HasColumnType("int");

                    b.Property<string>("PhotoSource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoomDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoomName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BaseRoleId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Core.TextChannel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ChannelDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChannelName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAdminChannel")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("TextChannels");
                });

            modelBuilder.Entity("Core.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastAuth")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Core.Role", b =>
                {
                    b.HasOne("Core.Room", null)
                        .WithMany("RoomRoles")
                        .HasForeignKey("RoomId");
                });

            modelBuilder.Entity("Core.Room", b =>
                {
                    b.HasOne("Core.Role", "BaseRole")
                        .WithMany()
                        .HasForeignKey("BaseRoleId");

                    b.Navigation("BaseRole");
                });

            modelBuilder.Entity("Core.Room", b =>
                {
                    b.Navigation("RoomRoles");
                });
#pragma warning restore 612, 618
        }
    }
}