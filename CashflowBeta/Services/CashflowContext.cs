﻿using CashflowBeta.Models;
using Microsoft.EntityFrameworkCore;

namespace CashflowBeta.Services;

public class CashflowContext : DbContext
{
    private readonly string db = "cashflow";
    private readonly string pw = "280998";
    private readonly string server = "localhost";
    private readonly string user = "root";

    public DbSet<Account?> Accounts { get; set; }
    public DbSet<CurrencyTransaction> CurrencyTransactions { get; set; }
    public DbSet<TransactionPartner> TransactionsPartners { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<AssetTransaction> AssetTransactions { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Networth> NetworthTrend { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL($"server={server};database={db};user={user};password={pw}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.BankIdentifier).IsRequired();
            entity.Property(e => e.AccountIdentifier).IsRequired();
        });

        modelBuilder.Entity<CurrencyTransaction>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.DateTime).IsRequired();
            entity.Property(e => e.Amount).IsRequired();
            entity.Property(e => e.Currency).IsRequired();
            entity.HasOne(d => d.Account)
                .WithMany(p => p.Transactions);
            entity.HasOne(d => d.TransactionPartner)
                .WithMany(p => p.Transactions);
            entity.HasOne(d => d.Budget)
                .WithMany(p => p.Transactions);
        });

        modelBuilder.Entity<TransactionPartner>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.AccountIdentifier).IsRequired();
            entity.Property(e => e.BankIdentifier).IsRequired();
            ;
        });

        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Amount).IsRequired();
            entity.HasMany(p => p.Transactions);
        });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Amount).IsRequired();
            entity.Property(e => e.AssetIdentifier).IsRequired();
            entity.HasOne(d => d.Account)
                .WithMany(p => p.Assets);
        });

        modelBuilder.Entity<AssetTransaction>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.TransactionPrice).IsRequired();
            entity.Property(e => e.TransactionTime).IsRequired();
            entity.Property(e => e.TransactionDuties).IsRequired();
            entity.HasOne(d => d.Asset)
                .WithMany(p => p.AssetTransactions);
        });

        modelBuilder.Entity<Networth>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Capital).IsRequired();
            entity.Property(e => e.DateTime).IsRequired();
        });
    }
}