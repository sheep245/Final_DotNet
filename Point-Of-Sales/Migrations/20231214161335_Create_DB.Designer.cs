﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Point_Of_Sales.Config;

#nullable disable

namespace Point_Of_Sales.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231214161335_Create_DB")]
    partial class Create_DB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Point_Of_Sales.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("Pwd")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Avatar")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RetailStoreId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("RetailStoreId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.Inventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("InventoryID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("RetailStoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("RetailStoreId");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Creation_Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("Import_Price")
                        .HasColumnType("float");

                    b.Property<bool>("Is_Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Product_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Retail_Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("Product_Name", "Barcode")
                        .IsUnique();

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.PurchaseDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("PurchaseHistoryId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("pdetail_ID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("PurchaseHistoryId");

                    b.ToTable("PurchaseDetails");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.PurchaseHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date_Of_Purchase")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<double>("Paid_Back")
                        .HasColumnType("float");

                    b.Property<double>("Received_Money")
                        .HasColumnType("float");

                    b.Property<double>("Total_Amount")
                        .HasColumnType("float");

                    b.Property<string>("pID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("PurchaseHistories");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.RetailStore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RetailStoreID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RetailStores");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.Account", b =>
                {
                    b.HasOne("Point_Of_Sales.Entities.Employee", "Employee")
                        .WithOne("Account")
                        .HasForeignKey("Point_Of_Sales.Entities.Account", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.Employee", b =>
                {
                    b.HasOne("Point_Of_Sales.Entities.RetailStore", "RetailStore")
                        .WithMany("Employees")
                        .HasForeignKey("RetailStoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RetailStore");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.Inventory", b =>
                {
                    b.HasOne("Point_Of_Sales.Entities.Product", "Product")
                        .WithMany("Inventories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Point_Of_Sales.Entities.RetailStore", "RetailStore")
                        .WithMany("Inventories")
                        .HasForeignKey("RetailStoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("RetailStore");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.PurchaseDetail", b =>
                {
                    b.HasOne("Point_Of_Sales.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Point_Of_Sales.Entities.PurchaseHistory", "PurchaseHistory")
                        .WithMany("PurchaseDetails")
                        .HasForeignKey("PurchaseHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("PurchaseHistory");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.PurchaseHistory", b =>
                {
                    b.HasOne("Point_Of_Sales.Entities.Customer", "Customer")
                        .WithMany("PurchaseHistories")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Point_Of_Sales.Entities.Employee", "Employee")
                        .WithMany("PurchaseHistories")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.Customer", b =>
                {
                    b.Navigation("PurchaseHistories");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.Employee", b =>
                {
                    b.Navigation("Account")
                        .IsRequired();

                    b.Navigation("PurchaseHistories");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.Product", b =>
                {
                    b.Navigation("Inventories");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.PurchaseHistory", b =>
                {
                    b.Navigation("PurchaseDetails");
                });

            modelBuilder.Entity("Point_Of_Sales.Entities.RetailStore", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Inventories");
                });
#pragma warning restore 612, 618
        }
    }
}
