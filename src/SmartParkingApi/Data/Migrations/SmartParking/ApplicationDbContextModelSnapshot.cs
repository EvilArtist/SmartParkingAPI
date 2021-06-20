﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartParkingCoreModels.Data;

namespace SmartParkingApi.Data.Migrations.SmartParking
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartParkingCoreModels.Parking.CameraConfiguration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CameraId")
                        .HasColumnType("int");

                    b.Property<string>("CameraName")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Oem")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<Guid?>("ParkingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Password")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<Guid>("ProtocolId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ServerName")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("StreamId")
                        .HasColumnType("int");

                    b.Property<string>("URLTemplate")
                        .HasMaxLength(127)
                        .HasColumnType("nvarchar(127)");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserName")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("ParkingId");

                    b.HasIndex("ProtocolId");

                    b.ToTable("CameraConfigurations");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.CameraProtocolType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProtocolName")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Url")
                        .HasMaxLength(127)
                        .HasColumnType("nvarchar(127)");

                    b.HasKey("Id");

                    b.ToTable("CameraProtocolType");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.ParkingConfig", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("ParkingConfig");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.SerialPortConfiguration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Baudrate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DeviceName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Oem")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParkingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ParkingId");

                    b.ToTable("SerialPortConfigurations");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.SlotType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("SlotName")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("SlotTypes");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.SlotTypeConfiguration", b =>
                {
                    b.Property<Guid>("ParkingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SlotTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SlotCount")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ParkingId", "SlotTypeId");

                    b.HasIndex("SlotTypeId");

                    b.ToTable("SlotTypeConfigurations");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.CameraConfiguration", b =>
                {
                    b.HasOne("SmartParkingCoreModels.Parking.ParkingConfig", "Parking")
                        .WithMany()
                        .HasForeignKey("ParkingId");

                    b.HasOne("SmartParkingCoreModels.Parking.CameraProtocolType", "Protocol")
                        .WithMany()
                        .HasForeignKey("ProtocolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parking");

                    b.Navigation("Protocol");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.SerialPortConfiguration", b =>
                {
                    b.HasOne("SmartParkingCoreModels.Parking.ParkingConfig", "Parking")
                        .WithMany()
                        .HasForeignKey("ParkingId");

                    b.Navigation("Parking");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.SlotTypeConfiguration", b =>
                {
                    b.HasOne("SmartParkingCoreModels.Parking.ParkingConfig", "Parking")
                        .WithMany("SlotTypeConfigurations")
                        .HasForeignKey("ParkingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartParkingCoreModels.Parking.SlotType", "SlotType")
                        .WithMany("SlotTypeConfigurations")
                        .HasForeignKey("SlotTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parking");

                    b.Navigation("SlotType");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.ParkingConfig", b =>
                {
                    b.Navigation("SlotTypeConfigurations");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.SlotType", b =>
                {
                    b.Navigation("SlotTypeConfigurations");
                });
#pragma warning restore 612, 618
        }
    }
}
