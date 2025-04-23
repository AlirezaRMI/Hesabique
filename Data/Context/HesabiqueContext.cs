using Domain.Entities;
using Domain.Entities.Ledger;
using Domain.Entities.Partner;
using Domain.Entities.Tenant;
using Domain.Entities.Trade;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class HesabiqueContext(DbContextOptions<HesabiqueContext> options)
    : DbContext(options)
{
    /*──────────── DbSet ها ────────────*/
    // Core
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRoles> UserRoles => Set<UserRoles>();

    public DbSet<Transaction> Transactions => Set<Transaction>();

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<FeatureFlag> FeatureFlags => Set<FeatureFlag>();

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalLine> JournalLines => Set<JournalLine>();

    public DbSet<Counterparty> Counterparties => Set<Counterparty>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<Cheque> Cheques => Set<Cheque>();

    public DbSet<Partner> Partners => Set<Partner>();
    public DbSet<PartnerSettlement> PartnerSettlements => Set<PartnerSettlement>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserRoles>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

       builder.Entity<UserRoles>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

       builder.Entity<UserRoles>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);


       builder.Entity<Account>()
            .HasOne(a => a.Parent)
            .WithMany(a => a.Children)
            .HasForeignKey(a => a.ParentId)
            .OnDelete(DeleteBehavior.Restrict);


       builder.Entity<JournalLine>()
            .HasOne(l => l.JournalEntry)
            .WithMany(e => e.Lines)
            .HasForeignKey(l => l.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        
        builder.Entity<InvoiceLine>()
            .HasOne(l => l.Invoice)
            .WithMany(i => i.Lines)
            .HasForeignKey(l => l.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Payment>()
            .HasOne(p => p.Cheque)
            .WithOne(c => c.Payment)
            .HasForeignKey<Payment>(p => p.ChequeId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.Entity<Cheque>()
            .HasOne(c => c.BankAccount)
            .WithMany(bk => bk.Cheques)
            .HasForeignKey(c => c.BankAccountId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<PartnerSettlement>()
            .HasOne(s => s.Partner)
            .WithMany(p => p.Settlements)
            .HasForeignKey(s => s.PartnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Transaction>().Property(t => t.Price).HasColumnType("decimal(18,0)");
        builder.Entity<Invoice>().Property(i => i.Total).HasColumnType("decimal(18,0)");
        builder.Entity<Payment>().Property(p => p.Amount).HasColumnType("decimal(18,0)");
        builder.Entity<Cheque>().Property(c => c.Amount).HasColumnType("decimal(18,0)");
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            var fk = entity
                .GetForeignKeys()
                .FirstOrDefault(f => f.PrincipalEntityType.ClrType == typeof(Tenant));
            if (fk is not null)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}