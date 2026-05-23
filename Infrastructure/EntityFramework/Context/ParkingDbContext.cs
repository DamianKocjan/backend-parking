using AppCore.Models;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Context;

public class ParkingDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    private const string AdminRoleId = "11111111-1111-1111-1111-111111111111";
    private const string RegisteredRoleId = "22222222-2222-2222-2222-222222222222";
    private const string AdminUserId = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
    private const string RegisteredUserId = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb";
    private const string GateEntryId = "8fbd7b91-5c7f-4d85-8a33-22193b2ef718";
    private const string GateExitId = "cce6ee16-89fb-462b-9dff-969b39e847e6";
    private const string VehicleId = "3d54091d-abc8-49ec-9590-93ad3ed5458f";
    private const string TariffId = "d8f1a6a8-3d48-4dd9-8d5e-6f4d0fb5d401";
    private const string SessionId = "f6d4ed47-9a4e-4a67-a8df-497b6d5c1a01";

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<ParkingGate> Gates => Set<ParkingGate>();
    public DbSet<ParkingSession> Sessions => Set<ParkingSession>();
    public DbSet<ParkingTariff> Tariffs => Set<ParkingTariff>();
    public DbSet<CameraCapture> Captures => Set<CameraCapture>();

    public ParkingDbContext()
    {
    }

    public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=parking.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Department).HasMaxLength(100);
        });

        builder.Entity<AppRole>(entity =>
        {
            entity.Property(r => r.Name).HasMaxLength(20);
        });

        builder.Entity<Vehicle>(entity =>
        {
            entity.Property(v => v.LicensePlate).HasMaxLength(20).IsRequired();
            entity.Property(v => v.Brand).HasMaxLength(50).IsRequired();
            entity.Property(v => v.Color).HasMaxLength(30).IsRequired();
            entity.HasIndex(v => v.LicensePlate).IsUnique();
            entity.HasMany(v => v.ParkingSessions)
                .WithOne(s => s.Vehicle)
                .HasForeignKey(s => s.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ParkingGate>(entity =>
        {
            entity.Property(g => g.Name).HasMaxLength(20).IsRequired();
            entity.Property(g => g.Type).HasConversion<string>().HasMaxLength(10);
            entity.Property(g => g.Location).HasMaxLength(50).IsRequired();
            entity.HasIndex(g => g.Name).IsUnique();
            entity.HasMany(g => g.CameraCaptures)
                .WithOne(c => c.ParkingGate)
                .HasForeignKey(c => c.ParkingGateId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ParkingTariff>(entity =>
        {
            entity.Property(t => t.Name).HasMaxLength(50).IsRequired();
            entity.Property(t => t.FreeParkingDuration).HasConversion<long>();
            entity.HasIndex(t => t.Name).IsUnique();
        });

        builder.Entity<ParkingSession>(entity =>
        {
            entity.Property(s => s.GateName).HasMaxLength(20).IsRequired();
            entity.Property(s => s.ParkingFee).HasPrecision(18, 2);
            entity.HasOne(s => s.Vehicle)
                .WithMany(v => v.ParkingSessions)
                .HasForeignKey(s => s.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(s => s.RegisteredAt)
                .WithMany()
                .HasForeignKey("RegisteredAtId")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(s => s.PricedBy)
                .WithMany()
                .HasForeignKey("ParkingTariffId")
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<CameraCapture>(entity =>
        {
            entity.Property(c => c.LicensePlate).HasMaxLength(20).IsRequired();
            entity.Property(c => c.DetectedBrand).HasMaxLength(50).IsRequired();
            entity.Property(c => c.DetectedColor).HasMaxLength(30).IsRequired();
            entity.Property(c => c.ImagePath).HasMaxLength(255);
            entity.Property(c => c.Type).HasConversion<string>().HasMaxLength(10);
            entity.Property(c => c.CapturedAt).HasColumnType("datetime");
            entity.HasOne(c => c.ParkingGate)
                .WithMany(g => g.CameraCaptures)
                .HasForeignKey(c => c.ParkingGateId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        var passwordHasher = new PasswordHasher<AppUser>();

        var adminUser = new AppUser
        {
            Id = AdminUserId,
            UserName = "admin@parking.local",
            NormalizedUserName = "ADMIN@PARKING.LOCAL",
            Email = "admin@parking.local",
            NormalizedEmail = "ADMIN@PARKING.LOCAL",
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "User",
            FullName = "Admin User",
            Department = "Administration",
            Status = SystemUserStatus.Active,
            CreatedAt = new DateTime(2026, 1, 1, 1, 0, 0, DateTimeKind.Utc),
            SecurityStamp = "6c8dbe1e-1c6a-4f56-8ed8-9cb4f5c7b501",
            ConcurrencyStamp = "6c8dbe1e-1c6a-4f56-8ed8-9cb4f5c7b501",
            LockoutEnabled = false
        };
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin123!");

        var registeredUser = new AppUser
        {
            Id = RegisteredUserId,
            UserName = "user@parking.local",
            NormalizedUserName = "USER@PARKING.LOCAL",
            Email = "user@parking.local",
            NormalizedEmail = "USER@PARKING.LOCAL",
            EmailConfirmed = true,
            FirstName = "Jan",
            LastName = "Kowalski",
            FullName = "Jan Kowalski",
            Department = "Parking",
            Status = SystemUserStatus.Active,
            CreatedAt = new DateTime(2026, 1, 1, 1, 15, 0, DateTimeKind.Utc),
            SecurityStamp = "7f0a0d9e-8d6a-4d5c-8f68-51d7e4a11d4a",
            ConcurrencyStamp = "7f0a0d9e-8d6a-4d5c-8f68-51d7e4a11d4a",
            LockoutEnabled = false
        };
        registeredUser.PasswordHash = passwordHasher.HashPassword(registeredUser, "User123!");

        builder.Entity<AppRole>().HasData(
            new AppRole(UserRole.Administrator.ToString())
            {
                Id = AdminRoleId,
                NormalizedName = UserRole.Administrator.ToString().ToUpperInvariant(),
                ConcurrencyStamp = "93e26e69-68c1-4e0b-bc73-3d6970d7e2e1",
                Description = "Application administrator"
            },
            new AppRole(UserRole.Registered.ToString())
            {
                Id = RegisteredRoleId,
                NormalizedName = UserRole.Registered.ToString().ToUpperInvariant(),
                ConcurrencyStamp = "8fbe1e6f-9d0f-4a32-8073-8ab4e6f66b44",
                Description = "Registered parking user"
            }
        );

        builder.Entity<AppUser>().HasData(adminUser, registeredUser);
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                UserId = AdminUserId,
                RoleId = AdminRoleId
            },
            new IdentityUserRole<string>
            {
                UserId = RegisteredUserId,
                RoleId = RegisteredRoleId
            }
        );

        builder.Entity<ParkingGate>().HasData(
            new ParkingGate
            {
                Id = new Guid(GateEntryId),
                Name = "Entry Gate",
                Type = GateType.Entry,
                Location = "Main Gate",
                IsOperational = true
            },
            new ParkingGate
            {
                Id = new Guid(GateExitId),
                Name = "Exit Gate",
                Type = GateType.Exit,
                Location = "Main Gate",
                IsOperational = true
            }
        );

        builder.Entity<Vehicle>().HasData(
            new Vehicle
            {
                Id = new Guid(VehicleId),
                LicensePlate = "TK 8434Y",
                Brand = "Toyota",
                Color = "Black"
            }
        );

        builder.Entity<ParkingTariff>().HasData(
            new ParkingTariff
            {
                Id = new Guid(TariffId),
                Name = "Standard",
                FreeParkingDuration = TimeSpan.FromMinutes(15),
                HourlyRate = 5m,
                DailyMaxRate = 30m,
                IsActive = true
            }
        );

        builder.Entity<ParkingSession>().HasData(
            new
            {
                Id = new Guid(SessionId),
                VehicleId = new Guid(VehicleId),
                RegisteredAtId = new Guid(GateEntryId),
                ParkingTariffId = new Guid(TariffId),
                GateName = "Entry Gate",
                EntryTime = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                ExitTime = (DateTime?)null,
                ParkingFee = (decimal?)null,
                IsActive = true
            }
        );
    }
}
