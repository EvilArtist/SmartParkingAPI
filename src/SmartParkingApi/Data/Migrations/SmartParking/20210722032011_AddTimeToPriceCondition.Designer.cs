// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartParkingCoreModels.Data;

namespace SmartParkingApi.Data.Migrations.SmartParking
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210722032011_AddTimeToPriceCondition")]
    partial class AddTimeToPriceCondition
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<Guid?>("ParkingLaneId")
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

                    b.HasIndex("ParkingLaneId");

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

            modelBuilder.Entity("SmartParkingCoreModels.Parking.Card", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CardStatusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("IdentityCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SubscriptionTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("VehicleTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CardStatusId");

                    b.HasIndex("SubscriptionTypeId");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.CardStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Description")
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("CardStatuses");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.ParkingConfig", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Parkings");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.ParkingLane", b =>
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

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParkingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ParkingId");

                    b.ToTable("ParkingLanes");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.PriceBook.PriceList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<Guid>("PriceListConditionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SubscriptionTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("VehicleTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PriceListConditionId");

                    b.HasIndex("SubscriptionTypeId");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("PriceLists");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.PriceBook.PriceListCondition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Condition")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.Property<string>("condition_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PriceListConditions");

                    b.HasDiscriminator<string>("condition_type").HasValue("PriceListCondition");
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

                    b.Property<Guid?>("ParkingLaneId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ParkingLaneId");

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
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("SlotName")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

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

            modelBuilder.Entity("SmartParkingCoreModels.Parking.SubscriptionType", b =>
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
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("SubscriptionTypes");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.VehicleType", b =>
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
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<Guid>("SlotTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SlotTypeId");

                    b.ToTable("VehicleTypes");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.PriceBook.PriceListDefaultCondition", b =>
                {
                    b.HasBaseType("SmartParkingCoreModels.Parking.PriceBook.PriceListCondition");

                    b.HasDiscriminator().HasValue("Default");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.PriceBook.PriceListDurationCondition", b =>
                {
                    b.HasBaseType("SmartParkingCoreModels.Parking.PriceBook.PriceListCondition");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasDiscriminator().HasValue("Duration");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.PriceBook.PriceListHollidayCondition", b =>
                {
                    b.HasBaseType("SmartParkingCoreModels.Parking.PriceBook.PriceListCondition");

                    b.HasDiscriminator().HasValue("Holliday");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.PriceBook.PriceListWeekdayCondition", b =>
                {
                    b.HasBaseType("SmartParkingCoreModels.Parking.PriceBook.PriceListCondition");

                    b.HasDiscriminator().HasValue("DayOfWeek");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.CameraConfiguration", b =>
                {
                    b.HasOne("SmartParkingCoreModels.Parking.ParkingLane", "ParkingLane")
                        .WithMany("Cameras")
                        .HasForeignKey("ParkingLaneId");

                    b.HasOne("SmartParkingCoreModels.Parking.CameraProtocolType", "Protocol")
                        .WithMany()
                        .HasForeignKey("ProtocolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParkingLane");

                    b.Navigation("Protocol");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.Card", b =>
                {
                    b.HasOne("SmartParkingCoreModels.Parking.CardStatus", "Status")
                        .WithMany()
                        .HasForeignKey("CardStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartParkingCoreModels.Parking.SubscriptionType", "SubscriptionType")
                        .WithMany("Cards")
                        .HasForeignKey("SubscriptionTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartParkingCoreModels.Parking.VehicleType", "VehicleType")
                        .WithMany("Cards")
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");

                    b.Navigation("SubscriptionType");

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.ParkingLane", b =>
                {
                    b.HasOne("SmartParkingCoreModels.Parking.ParkingConfig", "Parking")
                        .WithMany("ParkingLanes")
                        .HasForeignKey("ParkingId");

                    b.Navigation("Parking");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.PriceBook.PriceList", b =>
                {
                    b.HasOne("SmartParkingCoreModels.Parking.PriceBook.PriceListCondition", "Condition")
                        .WithMany()
                        .HasForeignKey("PriceListConditionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartParkingCoreModels.Parking.SubscriptionType", "SubscriptionType")
                        .WithMany()
                        .HasForeignKey("SubscriptionTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartParkingCoreModels.Parking.VehicleType", "VehicleType")
                        .WithMany()
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("SmartParkingCoreModels.Parking.PriceBook.PriceCalcutation", "Calcutation", b1 =>
                        {
                            b1.Property<Guid>("PriceListId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("HourBlock")
                                .HasColumnType("float");

                            b1.Property<string>("Name")
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.Property<double>("Price")
                                .HasColumnType("float");

                            b1.Property<int>("Type")
                                .HasColumnType("int");

                            b1.HasKey("PriceListId");

                            b1.ToTable("PriceLists");

                            b1.WithOwner()
                                .HasForeignKey("PriceListId");
                        });

                    b.Navigation("Calcutation");

                    b.Navigation("Condition");

                    b.Navigation("SubscriptionType");

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.SerialPortConfiguration", b =>
                {
                    b.HasOne("SmartParkingCoreModels.Parking.ParkingLane", "ParkingLane")
                        .WithMany("MutiFunctionGates")
                        .HasForeignKey("ParkingLaneId");

                    b.Navigation("ParkingLane");
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

            modelBuilder.Entity("SmartParkingCoreModels.Parking.VehicleType", b =>
                {
                    b.HasOne("SmartParkingCoreModels.Parking.SlotType", "SlotType")
                        .WithMany()
                        .HasForeignKey("SlotTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SlotType");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.ParkingConfig", b =>
                {
                    b.Navigation("ParkingLanes");

                    b.Navigation("SlotTypeConfigurations");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.ParkingLane", b =>
                {
                    b.Navigation("Cameras");

                    b.Navigation("MutiFunctionGates");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.SlotType", b =>
                {
                    b.Navigation("SlotTypeConfigurations");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.SubscriptionType", b =>
                {
                    b.Navigation("Cards");
                });

            modelBuilder.Entity("SmartParkingCoreModels.Parking.VehicleType", b =>
                {
                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}
