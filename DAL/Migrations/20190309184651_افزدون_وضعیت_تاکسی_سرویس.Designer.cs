﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(TaxiContext))]
    [Migration("20190309184651_افزدون_وضعیت_تاکسی_سرویس")]
    partial class افزدون_وضعیت_تاکسی_سرویس
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DAL.Academy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AcademyCategoryId");

                    b.Property<string>("Address");

                    b.Property<int>("Age");

                    b.Property<bool>("AllowActivity")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("AvatarPath")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("Default.png");

                    b.Property<DateTime?>("BeginDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<DateTime?>("BirthDay");

                    b.Property<string>("BossName");

                    b.Property<string>("Email");

                    b.Property<bool>("Gender");

                    b.Property<string>("Hs_Change");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsEmailVerified");

                    b.Property<bool>("IsMobielVerifed");

                    b.Property<string>("LastName");

                    b.Property<string>("Longtude");

                    b.Property<string>("Name");

                    b.Property<string>("OfficeNumber");

                    b.Property<string>("OtherPhoneNumber");

                    b.Property<string>("Password");

                    b.Property<string>("PhoneNubmber");

                    b.Property<int>("districtId");

                    b.Property<string>("latitude");

                    b.HasKey("Id");

                    b.HasIndex("AcademyCategoryId");

                    b.HasIndex("districtId");

                    b.ToTable("academies");
                });

            modelBuilder.Entity("DAL.AcademyCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Hs_Change");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("academyCategories");
                });

            modelBuilder.Entity("DAL.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Hs_Change");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<int>("ProvinceId");

                    b.HasKey("Id");

                    b.HasIndex("ProvinceId");

                    b.ToTable("cities");
                });

            modelBuilder.Entity("DAL.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AcademyId");

                    b.Property<DateTime?>("BeginDate");

                    b.Property<DateTime?>("EndDateTime");

                    b.Property<string>("Hs_Change");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<string>("TeacherName");

                    b.HasKey("Id");

                    b.HasIndex("AcademyId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("DAL.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CityId");

                    b.Property<string>("Hs_Change");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("districts");
                });

            modelBuilder.Entity("DAL.Driver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age");

                    b.Property<bool>("AllowActivity")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("AvatarPath")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("Default.png");

                    b.Property<DateTime?>("BeginDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<DateTime?>("BirthDay");

                    b.Property<string>("CarColor");

                    b.Property<string>("CarName");

                    b.Property<string>("CarType");

                    b.Property<int>("CityId");

                    b.Property<int>("DriverCode");

                    b.Property<string>("DrivingLicense");

                    b.Property<string>("Email");

                    b.Property<bool>("Gender");

                    b.Property<bool>("HasPlan");

                    b.Property<string>("Hs_Change");

                    b.Property<string>("IranianIdCode");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsEmailVerified");

                    b.Property<bool>("IsMaried");

                    b.Property<bool>("IsMobielVerifed");

                    b.Property<string>("LastName");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("PelakNumber");

                    b.Property<string>("PhoneNubmber");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("drivers");
                });

            modelBuilder.Entity("DAL.GeneralSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LogoPath");

                    b.Property<string>("SiteName");

                    b.HasKey("Id");

                    b.ToTable("GeneralSetting");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            LogoPath = "",
                            SiteName = ""
                        });
                });

            modelBuilder.Entity("DAL.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ammount");

                    b.Property<string>("Hs_Change");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("ParrentId");

                    b.Property<int?>("RequsetServiceId");

                    b.Property<bool>("Success");

                    b.Property<string>("TrackingCode");

                    b.HasKey("Id");

                    b.HasIndex("ParrentId");

                    b.HasIndex("RequsetServiceId");

                    b.ToTable("payments");
                });

            modelBuilder.Entity("DAL.Pricing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CityId");

                    b.Property<int>("Comission");

                    b.Property<int>("ConstPrice");

                    b.Property<decimal>("FormKilometer");

                    b.Property<string>("Hs_Change");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<int>("PricePerKilometer");

                    b.Property<decimal>("ToKilometer");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("pricings");
                });

            modelBuilder.Entity("DAL.Province", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Hs_Change");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("provinces");
                });

            modelBuilder.Entity("DAL.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Hs_Change");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDeleted = false,
                            Name = "برنامه نویس"
                        },
                        new
                        {
                            Id = 2,
                            IsDeleted = false,
                            Name = "مدیر"
                        },
                        new
                        {
                            Id = 3,
                            IsDeleted = false,
                            Name = "کاربر  معمولی"
                        });
                });

            modelBuilder.Entity("DAL.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CoursId");

                    b.Property<int>("EndHour");

                    b.Property<int>("EndMiniute");

                    b.Property<int>("StartHour");

                    b.Property<int>("StartMiniute");

                    b.Property<int>("weekday");

                    b.HasKey("Id");

                    b.HasIndex("CoursId");

                    b.ToTable("sections");
                });

            modelBuilder.Entity("DAL.ServiceRequset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<int>("Age");

                    b.Property<int>("CourseId");

                    b.Property<decimal>("Distance");

                    b.Property<string>("FullName");

                    b.Property<string>("Hs_Change");

                    b.Property<string>("IamgePath");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Longtude");

                    b.Property<string>("Note");

                    b.Property<string>("RequsetCode");

                    b.Property<int>("RequsetState");

                    b.Property<int>("StudentParrentId");

                    b.Property<int?>("cabAsFirstId");

                    b.Property<int?>("cabAsFourthId");

                    b.Property<int?>("cabAsSecondId");

                    b.Property<int?>("cabAsThirdId");

                    b.Property<bool>("gender");

                    b.Property<string>("latitue");

                    b.Property<int>("price");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentParrentId");

                    b.HasIndex("cabAsFirstId");

                    b.HasIndex("cabAsFourthId");

                    b.HasIndex("cabAsSecondId");

                    b.HasIndex("cabAsThirdId");

                    b.ToTable("serviceRequsets");
                });

            modelBuilder.Entity("DAL.StudentParent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age");

                    b.Property<bool>("AllowActivity")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("AvatarPath")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("Default.png");

                    b.Property<DateTime?>("BeginDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<DateTime?>("BirthDay");

                    b.Property<string>("ClassName");

                    b.Property<string>("Email");

                    b.Property<bool>("Gender");

                    b.Property<string>("Hs_Change");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsEmailVerified");

                    b.Property<bool>("IsMobielVerifed");

                    b.Property<string>("LastName");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("PhoneNubmber");

                    b.HasKey("Id");

                    b.ToTable("studentParents");
                });

            modelBuilder.Entity("DAL.TaxiCab", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DriverId");

                    b.Property<string>("Hs_Change");

                    b.Property<bool>("IsCompelete");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<int>("TaxiCabState");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.ToTable("taxiCabs");
                });

            modelBuilder.Entity("DAL.Academy", b =>
                {
                    b.HasOne("DAL.AcademyCategory", "category")
                        .WithMany()
                        .HasForeignKey("AcademyCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.District", "district")
                        .WithMany("Academies")
                        .HasForeignKey("districtId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.City", b =>
                {
                    b.HasOne("DAL.Province", "Province")
                        .WithMany("Cities")
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.Course", b =>
                {
                    b.HasOne("DAL.Academy", "Academy")
                        .WithMany("Courses")
                        .HasForeignKey("AcademyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.District", b =>
                {
                    b.HasOne("DAL.City", "City")
                        .WithMany("Districts")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.Driver", b =>
                {
                    b.HasOne("DAL.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.Payment", b =>
                {
                    b.HasOne("DAL.StudentParent", "Parent")
                        .WithMany("Payments")
                        .HasForeignKey("ParrentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.ServiceRequset", "ServiceRequset")
                        .WithMany("payments")
                        .HasForeignKey("RequsetServiceId");
                });

            modelBuilder.Entity("DAL.Pricing", b =>
                {
                    b.HasOne("DAL.City", "City")
                        .WithMany("Pricings")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.Section", b =>
                {
                    b.HasOne("DAL.Course", "Course")
                        .WithMany("Sections")
                        .HasForeignKey("CoursId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.ServiceRequset", b =>
                {
                    b.HasOne("DAL.Course", "course")
                        .WithMany("ServiceRequsets")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.StudentParent", "studentParent")
                        .WithMany("ServiceRequsets")
                        .HasForeignKey("StudentParrentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.TaxiCab", "cabAsFirst")
                        .WithMany("FirstPassnger")
                        .HasForeignKey("cabAsFirstId");

                    b.HasOne("DAL.TaxiCab", "cabAsFourth")
                        .WithMany("FourthPassnger")
                        .HasForeignKey("cabAsFourthId");

                    b.HasOne("DAL.TaxiCab", "cabAsSecond")
                        .WithMany("SecondPassnger")
                        .HasForeignKey("cabAsSecondId");

                    b.HasOne("DAL.TaxiCab", "cabAsThird")
                        .WithMany("ThirdPassnger")
                        .HasForeignKey("cabAsThirdId");
                });

            modelBuilder.Entity("DAL.TaxiCab", b =>
                {
                    b.HasOne("DAL.Driver", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
