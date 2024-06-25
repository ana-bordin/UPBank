﻿// <auto-generated />
using AgencyAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AgencyAPI.Migrations
{
    [DbContext(typeof(AgencyAPIContext))]
    [Migration("20240624180357_InitialCreated")]
    partial class InitialCreated
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Models.Agency", b =>
                {
                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AddressId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Restriction")
                        .HasColumnType("bit");

                    b.HasKey("Number");

                    b.ToTable("Agency", (string)null);
                });

            modelBuilder.Entity("Models.Employee", b =>
                {
                    b.Property<string>("Document")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AddressId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AgencyNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BirthDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Manager")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Register")
                        .HasColumnType("int");

                    b.Property<double>("Salary")
                        .HasColumnType("float");

                    b.HasKey("Document");

                    b.HasIndex("AgencyNumber");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("Models.RemovedAgency", b =>
                {
                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AddressId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Restriction")
                        .HasColumnType("bit");

                    b.HasKey("Number");

                    b.ToTable("AgencyHistory", (string)null);
                });

            modelBuilder.Entity("Models.RemovedAgencyEmployee", b =>
                {
                    b.Property<string>("Document")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AddressId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BirthDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Manager")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Register")
                        .HasColumnType("int");

                    b.Property<string>("RemovedAgencyNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Salary")
                        .HasColumnType("float");

                    b.HasKey("Document");

                    b.HasIndex("RemovedAgencyNumber");

                    b.ToTable("RemovedAgencyEmployee");
                });

            modelBuilder.Entity("Models.Employee", b =>
                {
                    b.HasOne("Models.Agency", null)
                        .WithMany("Employees")
                        .HasForeignKey("AgencyNumber");
                });

            modelBuilder.Entity("Models.RemovedAgencyEmployee", b =>
                {
                    b.HasOne("Models.RemovedAgency", null)
                        .WithMany("Employees")
                        .HasForeignKey("RemovedAgencyNumber");
                });

            modelBuilder.Entity("Models.Agency", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Models.RemovedAgency", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
