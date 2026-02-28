using GarageVelo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GarageVelo.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<SiteEntity> Sites => Set<SiteEntity>();
    public DbSet<GarageEntity> Garages => Set<GarageEntity>();
    public DbSet<SubscriptionPlanEntity> SubscriptionPlans => Set<SubscriptionPlanEntity>();
    public DbSet<SubscriptionEntity> Subscriptions => Set<SubscriptionEntity>();
    public DbSet<PaymentEntity> Payments => Set<PaymentEntity>();
    public DbSet<LoginSessionEntity> LoginSessions => Set<LoginSessionEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Users
        modelBuilder.Entity<UserEntity>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
        });

        // Sites — string PK
        modelBuilder.Entity<SiteEntity>(e =>
        {
            e.Property(s => s.Id).ValueGeneratedNever();
        });

        // Garages — string PK, FK to Site
        modelBuilder.Entity<GarageEntity>(e =>
        {
            e.Property(g => g.Id).ValueGeneratedNever();

            e.HasOne(g => g.Site)
                .WithMany(s => s.Garages)
                .HasForeignKey(g => g.SiteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // SubscriptionPlans
        modelBuilder.Entity<SubscriptionPlanEntity>(e =>
        {
            e.HasIndex(p => p.PlanType).IsUnique();
        });

        // Subscriptions
        modelBuilder.Entity<SubscriptionEntity>(e =>
        {
            e.HasOne(s => s.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(s => s.Garage)
                .WithMany(g => g.Subscriptions)
                .HasForeignKey(s => s.GarageId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(s => s.UserId);
        });

        // Payments
        modelBuilder.Entity<PaymentEntity>(e =>
        {
            e.HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(p => p.UserId);
        });

        // LoginSessions
        modelBuilder.Entity<LoginSessionEntity>(e =>
        {
            e.HasOne(ls => ls.User)
                .WithMany(u => u.LoginSessions)
                .HasForeignKey(ls => ls.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(ls => ls.UserId);
            e.HasIndex(ls => ls.Token);
        });
    }
}
