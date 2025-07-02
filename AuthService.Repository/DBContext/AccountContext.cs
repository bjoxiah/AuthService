using AuthService.Domain;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repository.DBContext;

public class AccountContext: DbContext
{
    public AccountContext(DbContextOptions<AccountContext> options) : base(options) { }
    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId);
            entity.HasIndex(e => e.Username)
                .IsUnique();
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100);
        });
    }
}