﻿// <auto-generated />
using System;
using CashflowBeta.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CashflowBeta.Migrations
{
    [DbContext(typeof(CashflowContext))]
    [Migration("20241005143243_ExtendTransactionPartnerWithParentChildRelation")]
    partial class ExtendTransactionPartnerWithParentChildRelation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CashflowBeta.Models.Account", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AccountIdentifier")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("BankIdentifier")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("CashflowBeta.Models.Asset", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountID")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("AssetIdentifier")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.HasIndex("AccountID");

                    b.ToTable("Assets");
                });

            modelBuilder.Entity("CashflowBeta.Models.AssetTransaction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountID")
                        .HasColumnType("int");

                    b.Property<int>("AssetID")
                        .HasColumnType("int");

                    b.Property<decimal>("TransactionDuties")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TransactionPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("TransactionTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.HasIndex("AccountID");

                    b.HasIndex("AssetID");

                    b.ToTable("AssetTransactions");
                });

            modelBuilder.Entity("CashflowBeta.Models.Budget", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("CashflowBeta.Models.CurrencyTransaction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountID")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("BudgetID")
                        .HasColumnType("int");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Info")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TransactionPartnerID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("AccountID");

                    b.HasIndex("BudgetID");

                    b.HasIndex("TransactionPartnerID");

                    b.ToTable("CurrencyTransactions");
                });

            modelBuilder.Entity("CashflowBeta.Models.Networth", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AccountID")
                        .HasColumnType("int");

                    b.Property<decimal>("Capital")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.HasIndex("AccountID");

                    b.ToTable("NetworthTrend");
                });

            modelBuilder.Entity("CashflowBeta.Models.TransactionPartner", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AccountIdentifier")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("BankIdentifier")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Bankcode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("BudgetID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ParentPartnerID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("BudgetID");

                    b.HasIndex("ParentPartnerID");

                    b.ToTable("TransactionsPartners");
                });

            modelBuilder.Entity("CashflowBeta.Models.Asset", b =>
                {
                    b.HasOne("CashflowBeta.Models.Account", "Account")
                        .WithMany("Assets")
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("CashflowBeta.Models.AssetTransaction", b =>
                {
                    b.HasOne("CashflowBeta.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CashflowBeta.Models.Asset", "Asset")
                        .WithMany("AssetTransactions")
                        .HasForeignKey("AssetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Asset");
                });

            modelBuilder.Entity("CashflowBeta.Models.CurrencyTransaction", b =>
                {
                    b.HasOne("CashflowBeta.Models.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CashflowBeta.Models.Budget", "Budget")
                        .WithMany("Transactions")
                        .HasForeignKey("BudgetID");

                    b.HasOne("CashflowBeta.Models.TransactionPartner", "TransactionPartner")
                        .WithMany("Transactions")
                        .HasForeignKey("TransactionPartnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Budget");

                    b.Navigation("TransactionPartner");
                });

            modelBuilder.Entity("CashflowBeta.Models.Networth", b =>
                {
                    b.HasOne("CashflowBeta.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountID");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("CashflowBeta.Models.TransactionPartner", b =>
                {
                    b.HasOne("CashflowBeta.Models.Budget", "Budget")
                        .WithMany()
                        .HasForeignKey("BudgetID");

                    b.HasOne("CashflowBeta.Models.TransactionPartner", "ParentPartner")
                        .WithMany("ChildPartners")
                        .HasForeignKey("ParentPartnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Budget");

                    b.Navigation("ParentPartner");
                });

            modelBuilder.Entity("CashflowBeta.Models.Account", b =>
                {
                    b.Navigation("Assets");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("CashflowBeta.Models.Asset", b =>
                {
                    b.Navigation("AssetTransactions");
                });

            modelBuilder.Entity("CashflowBeta.Models.Budget", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("CashflowBeta.Models.TransactionPartner", b =>
                {
                    b.Navigation("ChildPartners");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
