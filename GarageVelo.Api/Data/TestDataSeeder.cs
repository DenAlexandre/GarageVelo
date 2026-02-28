using GarageVelo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GarageVelo.Api.Data;

/// <summary>
/// Seeds realistic test data for development/testing.
/// Requires DatabaseSeeder to run first (base garages, plans, demo user).
/// </summary>
public static class TestDataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        // Skip if test data already present
        if (await db.Users.CountAsync() > 1)
            return;

        var now = DateTime.UtcNow;

        // ── Users (8 additional, all with password "test1234") ──
        var hash = BCrypt.Net.BCrypt.HashPassword("test1234");

        var marie = new UserEntity
        {
            Email = "marie.leclerc@email.com",
            PasswordHash = hash,
            FirstName = "Marie",
            LastName = "Leclerc",
            CreatedAt = now.AddMonths(-6)
        };
        var pierre = new UserEntity
        {
            Email = "pierre.moreau@email.com",
            PasswordHash = hash,
            FirstName = "Pierre",
            LastName = "Moreau",
            CreatedAt = now.AddMonths(-4)
        };
        var sophie = new UserEntity
        {
            Email = "sophie.bernard@email.com",
            PasswordHash = hash,
            FirstName = "Sophie",
            LastName = "Bernard",
            CreatedAt = now.AddMonths(-3)
        };
        var lucas = new UserEntity
        {
            Email = "lucas.petit@email.com",
            PasswordHash = hash,
            FirstName = "Lucas",
            LastName = "Petit",
            CreatedAt = now.AddMonths(-2)
        };
        var emma = new UserEntity
        {
            Email = "emma.robert@email.com",
            PasswordHash = hash,
            FirstName = "Emma",
            LastName = "Robert",
            CreatedAt = now.AddMonths(-1)
        };
        var thomas = new UserEntity
        {
            Email = "thomas.richard@email.com",
            PasswordHash = hash,
            FirstName = "Thomas",
            LastName = "Richard",
            CreatedAt = now.AddDays(-20)
        };
        var camille = new UserEntity
        {
            Email = "camille.durand@email.com",
            PasswordHash = hash,
            FirstName = "Camille",
            LastName = "Durand",
            CreatedAt = now.AddDays(-10)
        };
        var antoine = new UserEntity
        {
            Email = "antoine.leroy@email.com",
            PasswordHash = hash,
            FirstName = "Antoine",
            LastName = "Leroy",
            CreatedAt = now.AddDays(-3)
        };

        db.Users.AddRange(marie, pierre, sophie, lucas, emma, thomas, camille, antoine);
        await db.SaveChangesAsync();

        // Get demo user
        var demo = await db.Users.FirstAsync(u => u.Email == "demo@garagevelo.fr");

        // ── Additional garages (5 more, around Lyon) ──
        db.Garages.AddRange(
            new GarageEntity
            {
                Id = "GV-0006", Name = "Garage Perrache", SiteId = "SITE-003",
                Size = "Large", TotalSlots = 25, AvailableSlots = 12,
                Position = 6, LockCode = "391847"
            },
            new GarageEntity
            {
                Id = "GV-0007", Name = "Garage Croix-Rousse", SiteId = "SITE-004",
                Size = "Small", TotalSlots = 8, AvailableSlots = 0,
                Position = 7, LockCode = "725163"
            },
            new GarageEntity
            {
                Id = "GV-0008", Name = "Garage Guillotière", SiteId = "SITE-005",
                Size = "Medium", TotalSlots = 14, AvailableSlots = 9,
                Position = 8, LockCode = "468290"
            },
            new GarageEntity
            {
                Id = "GV-0009", Name = "Garage Jean Macé", SiteId = "SITE-005",
                Size = "Medium", TotalSlots = 10, AvailableSlots = 4,
                Position = 9, LockCode = "153792"
            },
            new GarageEntity
            {
                Id = "GV-0010", Name = "Garage Monplaisir", SiteId = "SITE-006",
                Size = "Large", TotalSlots = 18, AvailableSlots = 11,
                Position = 10, LockCode = "842615"
            }
        );
        await db.SaveChangesAsync();

        // ── Subscriptions ──

        // Active subscriptions
        db.Subscriptions.AddRange(
            // Marie — abonnement annuel actif depuis 2 mois
            new SubscriptionEntity
            {
                UserId = marie.Id, GarageId = "GV-0001", PlanType = "Yearly",
                StartDate = now.AddMonths(-2), EndDate = now.AddMonths(10)
            },
            // Pierre — abonnement mensuel actif depuis 10 jours
            new SubscriptionEntity
            {
                UserId = pierre.Id, GarageId = "GV-0002", PlanType = "Monthly",
                StartDate = now.AddDays(-10), EndDate = now.AddDays(20)
            },
            // Sophie — abonnement journalier actif aujourd'hui
            new SubscriptionEntity
            {
                UserId = sophie.Id, GarageId = "GV-0003", PlanType = "Daily",
                StartDate = now.AddHours(-6), EndDate = now.AddHours(18)
            },
            // Thomas — abonnement mensuel actif depuis 5 jours
            new SubscriptionEntity
            {
                UserId = thomas.Id, GarageId = "GV-0008", PlanType = "Monthly",
                StartDate = now.AddDays(-5), EndDate = now.AddDays(25)
            }
        );

        // Expired subscriptions
        db.Subscriptions.AddRange(
            // Demo — abonnement mensuel expiré il y a 15 jours
            new SubscriptionEntity
            {
                UserId = demo.Id, GarageId = "GV-0001", PlanType = "Monthly",
                StartDate = now.AddDays(-45), EndDate = now.AddDays(-15)
            },
            // Lucas — abonnement journalier expiré hier
            new SubscriptionEntity
            {
                UserId = lucas.Id, GarageId = "GV-0004", PlanType = "Daily",
                StartDate = now.AddDays(-2), EndDate = now.AddDays(-1)
            },
            // Emma — abonnement annuel expiré il y a 1 mois
            new SubscriptionEntity
            {
                UserId = emma.Id, GarageId = "GV-0005", PlanType = "Yearly",
                StartDate = now.AddMonths(-13), EndDate = now.AddMonths(-1)
            },
            // Marie — ancien abonnement mensuel (avant son annuel actif)
            new SubscriptionEntity
            {
                UserId = marie.Id, GarageId = "GV-0003", PlanType = "Monthly",
                StartDate = now.AddMonths(-4), EndDate = now.AddMonths(-3)
            },
            // Pierre — ancien abonnement journalier expiré
            new SubscriptionEntity
            {
                UserId = pierre.Id, GarageId = "GV-0006", PlanType = "Daily",
                StartDate = now.AddDays(-30), EndDate = now.AddDays(-29)
            }
        );
        await db.SaveChangesAsync();

        // ── Payments ──
        db.Payments.AddRange(
            // Marie — paiement annuel réussi
            new PaymentEntity
            {
                UserId = marie.Id, Amount = 200m, Status = "Completed",
                PaymentMethod = "Card", CreatedAt = now.AddMonths(-2)
            },
            // Marie — ancien paiement mensuel réussi
            new PaymentEntity
            {
                UserId = marie.Id, Amount = 20m, Status = "Completed",
                PaymentMethod = "Card", CreatedAt = now.AddMonths(-4)
            },
            // Pierre — paiement mensuel réussi
            new PaymentEntity
            {
                UserId = pierre.Id, Amount = 20m, Status = "Completed",
                PaymentMethod = "Card", CreatedAt = now.AddDays(-10)
            },
            // Pierre — ancien paiement journalier
            new PaymentEntity
            {
                UserId = pierre.Id, Amount = 1m, Status = "Completed",
                PaymentMethod = "Card", CreatedAt = now.AddDays(-30)
            },
            // Sophie — paiement journalier réussi aujourd'hui
            new PaymentEntity
            {
                UserId = sophie.Id, Amount = 1m, Status = "Completed",
                PaymentMethod = "ApplePay", CreatedAt = now.AddHours(-6)
            },
            // Lucas — paiement journalier réussi
            new PaymentEntity
            {
                UserId = lucas.Id, Amount = 1m, Status = "Completed",
                PaymentMethod = "Card", CreatedAt = now.AddDays(-2)
            },
            // Lucas — paiement échoué (tentative avant le réussi)
            new PaymentEntity
            {
                UserId = lucas.Id, Amount = 1m, Status = "Failed",
                PaymentMethod = "Card", CreatedAt = now.AddDays(-2).AddMinutes(-5)
            },
            // Emma — paiement annuel passé
            new PaymentEntity
            {
                UserId = emma.Id, Amount = 200m, Status = "Completed",
                PaymentMethod = "GooglePay", CreatedAt = now.AddMonths(-13)
            },
            // Thomas — paiement mensuel réussi
            new PaymentEntity
            {
                UserId = thomas.Id, Amount = 20m, Status = "Completed",
                PaymentMethod = "Card", CreatedAt = now.AddDays(-5)
            },
            // Thomas — paiement échoué avant
            new PaymentEntity
            {
                UserId = thomas.Id, Amount = 20m, Status = "Failed",
                PaymentMethod = "Card", CreatedAt = now.AddDays(-5).AddMinutes(-10)
            },
            // Demo — ancien paiement mensuel
            new PaymentEntity
            {
                UserId = demo.Id, Amount = 20m, Status = "Completed",
                PaymentMethod = "Card", CreatedAt = now.AddDays(-45)
            },
            // Camille — paiement en attente (pas encore finalisé)
            new PaymentEntity
            {
                UserId = camille.Id, Amount = 20m, Status = "Pending",
                PaymentMethod = "Card", CreatedAt = now.AddMinutes(-30)
            },
            // Antoine — paiement échoué (nouvel utilisateur, carte refusée)
            new PaymentEntity
            {
                UserId = antoine.Id, Amount = 1m, Status = "Failed",
                PaymentMethod = "Card", CreatedAt = now.AddDays(-1)
            }
        );
        await db.SaveChangesAsync();

        // ── Login Sessions ──
        db.LoginSessions.AddRange(
            // Marie — session active
            new LoginSessionEntity
            {
                UserId = marie.Id, Token = "test-token-marie-active",
                IpAddress = "192.168.1.10", UserAgent = "GarageVelo/1.0 Android/14",
                CreatedAt = now.AddHours(-2), ExpiresAt = now.AddDays(7), IsRevoked = false
            },
            // Marie — ancienne session révoquée
            new LoginSessionEntity
            {
                UserId = marie.Id, Token = "test-token-marie-old",
                IpAddress = "192.168.1.10", UserAgent = "GarageVelo/1.0 Android/14",
                CreatedAt = now.AddDays(-30), ExpiresAt = now.AddDays(-23), IsRevoked = true
            },
            // Pierre — session active
            new LoginSessionEntity
            {
                UserId = pierre.Id, Token = "test-token-pierre-active",
                IpAddress = "10.0.0.42", UserAgent = "GarageVelo/1.0 iOS/17.4",
                CreatedAt = now.AddHours(-5), ExpiresAt = now.AddDays(7), IsRevoked = false
            },
            // Sophie — session active (mobile)
            new LoginSessionEntity
            {
                UserId = sophie.Id, Token = "test-token-sophie-active",
                IpAddress = "172.16.0.5", UserAgent = "GarageVelo/1.0 Android/15",
                CreatedAt = now.AddHours(-6), ExpiresAt = now.AddDays(7), IsRevoked = false
            },
            // Lucas — session expirée
            new LoginSessionEntity
            {
                UserId = lucas.Id, Token = "test-token-lucas-expired",
                IpAddress = "192.168.0.100", UserAgent = "GarageVelo/1.0 iOS/17.2",
                CreatedAt = now.AddDays(-10), ExpiresAt = now.AddDays(-3), IsRevoked = false
            },
            // Emma — session révoquée (logout)
            new LoginSessionEntity
            {
                UserId = emma.Id, Token = "test-token-emma-revoked",
                IpAddress = "192.168.1.55", UserAgent = "GarageVelo/1.0 Android/13",
                CreatedAt = now.AddDays(-5), ExpiresAt = now.AddDays(2), IsRevoked = true
            },
            // Thomas — 2 sessions actives (2 appareils)
            new LoginSessionEntity
            {
                UserId = thomas.Id, Token = "test-token-thomas-phone",
                IpAddress = "10.0.1.15", UserAgent = "GarageVelo/1.0 Android/14",
                CreatedAt = now.AddHours(-3), ExpiresAt = now.AddDays(7), IsRevoked = false
            },
            new LoginSessionEntity
            {
                UserId = thomas.Id, Token = "test-token-thomas-tablet",
                IpAddress = "10.0.1.16", UserAgent = "GarageVelo/1.0 iPadOS/17.4",
                CreatedAt = now.AddHours(-1), ExpiresAt = now.AddDays(7), IsRevoked = false
            },
            // Demo — session ancienne révoquée
            new LoginSessionEntity
            {
                UserId = demo.Id, Token = "test-token-demo-old",
                IpAddress = "127.0.0.1", UserAgent = "curl/7.85.0",
                CreatedAt = now.AddDays(-40), ExpiresAt = now.AddDays(-33), IsRevoked = true
            },
            // Antoine — session active (nouvel utilisateur)
            new LoginSessionEntity
            {
                UserId = antoine.Id, Token = "test-token-antoine-active",
                IpAddress = "192.168.2.30", UserAgent = "GarageVelo/1.0 Android/14",
                CreatedAt = now.AddDays(-1), ExpiresAt = now.AddDays(6), IsRevoked = false
            }
        );
        await db.SaveChangesAsync();
    }
}
