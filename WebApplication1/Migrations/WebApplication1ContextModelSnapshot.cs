﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WebApplication1.Models;

namespace WebApplication1.Migrations
{
    [DbContext(typeof(WebApplication1Context))]
    partial class WebApplication1ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApplication1.Models.AuxParam", b =>
                {
                    b.Property<int>("AuxParmID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Company");

                    b.Property<string>("Verno");

                    b.HasKey("AuxParmID");

                    b.ToTable("AuxParam");
                });

            modelBuilder.Entity("WebApplication1.Models.companyModell", b =>
                {
                    b.Property<int>("companyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("companyName");

                    b.Property<string>("companyOrgNr");

                    b.Property<string>("compayPgNr");

                    b.HasKey("companyId");

                    b.ToTable("companyModell");
                });

            modelBuilder.Entity("WebApplication1.Models.customerModel", b =>
                {
                    b.Property<int>("customerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int?>("companyId");

                    b.Property<string>("eMail");

                    b.HasKey("customerId");

                    b.HasIndex("companyId");

                    b.ToTable("customerModel");
                });

            modelBuilder.Entity("WebApplication1.Models.eInvoice", b =>
                {
                    b.Property<int>("eInvoiceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<double>("Amount");

                    b.Property<string>("Authorizor");

                    b.Property<int?>("AuxParmID");

                    b.Property<string>("Bg");

                    b.Property<string>("City");

                    b.Property<string>("CurrencyCode");

                    b.Property<string>("DueD");

                    b.Property<bool>("DueDateSpecified");

                    b.Property<double>("Freight");

                    b.Property<bool>("FreightSpecified");

                    b.Property<string>("InvoiceD");

                    b.Property<bool>("InvoiceDateSpecified");

                    b.Property<double>("InvoiceFee");

                    b.Property<string>("InvoiceNo");

                    b.Property<string>("InvoiceRecipient");

                    b.Property<string>("InvoiceType");

                    b.Property<string>("Message");

                    b.Property<string>("Ocr");

                    b.Property<string>("OrgNo");

                    b.Property<string>("OurCustomerNo");

                    b.Property<string>("OurReference");

                    b.Property<int>("PayType");

                    b.Property<string>("Pg");

                    b.Property<string>("PostCode");

                    b.Property<string>("Project");

                    b.Property<string>("PurchOrderNo");

                    b.Property<bool>("Redable");

                    b.Property<string>("Supplier");

                    b.Property<string>("SupplierNo");

                    b.Property<double>("Vat");

                    b.Property<double>("VatAmount");

                    b.Property<string>("VatCode");

                    b.Property<string>("YourReference");

                    b.Property<string>("pdfPaths");

                    b.HasKey("eInvoiceId");

                    b.HasIndex("AuxParmID");

                    b.ToTable("eInvoice");
                });

            modelBuilder.Entity("WebApplication1.Models.Images", b =>
                {
                    b.Property<int>("ImagesId")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("ImageData");

                    b.Property<string>("ImageName");

                    b.Property<int>("eInvoiceId");

                    b.Property<string>("imageMimeType");

                    b.HasKey("ImagesId");

                    b.HasIndex("eInvoiceId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("WebApplication1.Models.InvoiceAccounts", b =>
                {
                    b.Property<int>("InvoiceAccountsId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccAuthorizor");

                    b.Property<string>("Account");

                    b.Property<double>("CreditAmount");

                    b.Property<double>("DebetAmount");

                    b.Property<string>("Profit_centre");

                    b.Property<string>("Project");

                    b.Property<DateTime>("Reg_date");

                    b.Property<int?>("eInvoiceId");

                    b.HasKey("InvoiceAccountsId");

                    b.HasIndex("eInvoiceId");

                    b.ToTable("InvoiceAccounts");
                });

            modelBuilder.Entity("WebApplication1.Models.InvoiceRows", b =>
                {
                    b.Property<int>("InvoiceRowId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ItemRow");

                    b.Property<int?>("eInvoiceId");

                    b.HasKey("InvoiceRowId");

                    b.HasIndex("eInvoiceId");

                    b.ToTable("InvoiceRows");
                });

            modelBuilder.Entity("WebApplication1.Models.suplierModel", b =>
                {
                    b.Property<int>("suplierId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("companyModellcompanyId");

                    b.Property<string>("currencyCode");

                    b.Property<string>("currencyFormat");

                    b.Property<string>("invoiceDateFormat");

                    b.Property<string>("purchOrderNoFormat");

                    b.Property<string>("suplierBgNr");

                    b.Property<string>("suplierBgNrFormat");

                    b.Property<string>("suplierName");

                    b.Property<string>("suplierOrcNrFormat");

                    b.Property<string>("suplierOrgNr");

                    b.Property<string>("suplierOrgNrFormat");

                    b.Property<string>("suplierPgNr");

                    b.Property<string>("suplierPgNrFormat");

                    b.HasKey("suplierId");

                    b.HasIndex("companyModellcompanyId");

                    b.ToTable("suplierModel");
                });

            modelBuilder.Entity("WebApplication1.Models.customerModel", b =>
                {
                    b.HasOne("WebApplication1.Models.companyModell", "company")
                        .WithMany()
                        .HasForeignKey("companyId");
                });

            modelBuilder.Entity("WebApplication1.Models.eInvoice", b =>
                {
                    b.HasOne("WebApplication1.Models.AuxParam", "AuxP")
                        .WithMany()
                        .HasForeignKey("AuxParmID");
                });

            modelBuilder.Entity("WebApplication1.Models.Images", b =>
                {
                    b.HasOne("WebApplication1.Models.eInvoice")
                        .WithMany("images1")
                        .HasForeignKey("eInvoiceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApplication1.Models.InvoiceAccounts", b =>
                {
                    b.HasOne("WebApplication1.Models.eInvoice")
                        .WithMany("AccRows")
                        .HasForeignKey("eInvoiceId");
                });

            modelBuilder.Entity("WebApplication1.Models.InvoiceRows", b =>
                {
                    b.HasOne("WebApplication1.Models.eInvoice")
                        .WithMany("ItemRow")
                        .HasForeignKey("eInvoiceId");
                });

            modelBuilder.Entity("WebApplication1.Models.suplierModel", b =>
                {
                    b.HasOne("WebApplication1.Models.companyModell")
                        .WithMany("suplierModels")
                        .HasForeignKey("companyModellcompanyId");
                });
#pragma warning restore 612, 618
        }
    }
}
