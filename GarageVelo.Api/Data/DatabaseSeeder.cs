using GarageVelo.Api.Entities;

namespace GarageVelo.Api.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        bool changed = false;

        // Seed sites
        if (!db.Sites.Any())
        {
            db.Sites.AddRange(
                new SiteEntity
                {
                    Id = "SITE-001", Name = "Bellecour",
                    Address = "Place Bellecour, 69002 Lyon",
                    Latitude = 45.7578, Longitude = 4.8320
                },
                new SiteEntity
                {
                    Id = "SITE-002", Name = "Part-Dieu",
                    Address = "Gare Part-Dieu, 69003 Lyon",
                    Latitude = 45.7606, Longitude = 4.8602
                },
                new SiteEntity
                {
                    Id = "SITE-003", Name = "Confluence",
                    Address = "Centre Confluence, 69002 Lyon",
                    Latitude = 45.7432, Longitude = 4.8183
                },
                new SiteEntity
                {
                    Id = "SITE-004", Name = "Croix-Rousse",
                    Address = "Place de la Croix-Rousse, 69004 Lyon",
                    Latitude = 45.7745, Longitude = 4.8318
                },
                new SiteEntity
                {
                    Id = "SITE-005", Name = "Guillotière",
                    Address = "Place Gabriel Péri, 69007 Lyon",
                    Latitude = 45.7560, Longitude = 4.8425
                },
                new SiteEntity
                {
                    Id = "SITE-006", Name = "Monplaisir",
                    Address = "Avenue des Frères Lumière, 69008 Lyon",
                    Latitude = 45.7444, Longitude = 4.8690
                }
            );
            changed = true;
        }

        // Seed garages
        if (!db.Garages.Any())
        {
            db.Garages.AddRange(
                new GarageEntity
                {
                    Id = "GV-0001", Name = "Garage Bellecour", SiteId = "SITE-001",
                    Size = "Large", TotalSlots = 20, AvailableSlots = 8,
                    Position = 1, LockCode = "814623"
                },
                new GarageEntity
                {
                    Id = "GV-0002", Name = "Garage Part-Dieu", SiteId = "SITE-002",
                    Size = "Large", TotalSlots = 30, AvailableSlots = 15,
                    Position = 2, LockCode = "927384"
                },
                new GarageEntity
                {
                    Id = "GV-0003", Name = "Garage Confluence", SiteId = "SITE-003",
                    Size = "Medium", TotalSlots = 12, AvailableSlots = 5,
                    Position = 3, LockCode = "536271"
                },
                new GarageEntity
                {
                    Id = "GV-0004", Name = "Garage Vieux Lyon", SiteId = "SITE-001",
                    Size = "Small", TotalSlots = 6, AvailableSlots = 2,
                    Position = 4, LockCode = "148592"
                },
                new GarageEntity
                {
                    Id = "GV-0005", Name = "Garage Tête d'Or", SiteId = "SITE-002",
                    Size = "Medium", TotalSlots = 10, AvailableSlots = 7,
                    Position = 5, LockCode = "673810"
                }
            );
            changed = true;
        }

        // Seed subscription plans
        if (!db.SubscriptionPlans.Any())
        {
            db.SubscriptionPlans.AddRange(
                new SubscriptionPlanEntity
                {
                    PlanType = "Daily", Name = "Jour", Price = 1m, DurationDays = 1
                },
                new SubscriptionPlanEntity
                {
                    PlanType = "Monthly", Name = "Mois", Price = 20m, DurationDays = 30
                },
                new SubscriptionPlanEntity
                {
                    PlanType = "Yearly", Name = "Année", Price = 200m, DurationDays = 365
                }
            );
            changed = true;
        }

        // Seed demo user
        if (!db.Users.Any(u => u.Email == "demo@garagevelo.fr"))
        {
            db.Users.Add(new UserEntity
            {
                Email = "demo@garagevelo.fr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                FirstName = "Jean",
                LastName = "Dupont"
            });
            changed = true;
        }

        if (changed)
            await db.SaveChangesAsync();
    }
}
