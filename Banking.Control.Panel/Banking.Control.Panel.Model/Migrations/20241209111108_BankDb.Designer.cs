﻿// <auto-generated />
using Banking.Control.Panel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Banking.Control.Panel.Model.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241209111108_BankDb")]
    partial class BankDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddressId"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AddressId");

                    b.HasIndex("ClientId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Banking.Control.Panel.Model.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Balance")
                        .HasColumnType("float");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("AccountId");

                    b.HasIndex("ClientId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Banking.Control.Panel.Model.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClientId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(59)
                        .HasColumnType("nvarchar(59)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(59)
                        .HasColumnType("nvarchar(59)");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonalId")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("ProfilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClientId");

                    b.ToTable("Clients");

                    b.HasData(
                        new
                        {
                            ClientId = 1,
                            Email = "admin@systems.com",
                            FirstName = "admin",
                            IsActive = false,
                            LastName = "admin",
                            MobileNumber = "123456",
                            Password = "AQAAAAIAAYagAAAAEA4+WPlrzfd/cGMPqf5CJnbKak7ifHc0BtcZ5cKjveiZAAAAAAAAAAAAAAAAAAAAAA==",
                            PersonalId = "123456789",
                            Role = "Admin",
                            Sex = "Male"
                        });
                });

            modelBuilder.Entity("Banking.Control.Panel.Model.SearchHistory", b =>
                {
                    b.Property<int>("SearchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SearchId"));

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("SearchName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SearchId");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("SearchHistorys");
                });

            modelBuilder.Entity("Address", b =>
                {
                    b.HasOne("Banking.Control.Panel.Model.Client", "Clients")
                        .WithMany("Address")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clients");
                });

            modelBuilder.Entity("Banking.Control.Panel.Model.Account", b =>
                {
                    b.HasOne("Banking.Control.Panel.Model.Client", "Client")
                        .WithMany("Accounts")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Banking.Control.Panel.Model.SearchHistory", b =>
                {
                    b.HasOne("Banking.Control.Panel.Model.Client", "Client")
                        .WithOne("search")
                        .HasForeignKey("Banking.Control.Panel.Model.SearchHistory", "ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Banking.Control.Panel.Model.Client", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Address");

                    b.Navigation("search");
                });
#pragma warning restore 612, 618
        }
    }
}
