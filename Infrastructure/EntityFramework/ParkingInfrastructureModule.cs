using AppCore.Repositories;
using AppCore.Services;
using Infrastructure.EntityFramework.Context;
using Infrastructure.EntityFramework.Repositories;
using Infrastructure.EntityFramework.UnitOfWork;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.EntityFramework;

public static class ParkingInfrastructureModule
{
    private const string ConnectionString = "Data Source=parking.db";

    public static IServiceCollection AddParkingEfModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ParkingDb") ?? ConnectionString;

        services.AddDbContext<ParkingDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<ParkingDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IVehicleRepository, EfVehicleRepository>();
        services.AddScoped<IParkingGateRepository, EfParkingGateRepository>();
        services.AddScoped<IParkingSessionRepository, EfParkingSessionRepository>();
        services.AddScoped<ICameraCaptureRepository, EfCameraCaptureRepository>();
        services.AddScoped<IParkingUnitOfWork, EfParkingUnitOfWork>();
        services.AddScoped<ICameraCaptureService, CameraCaptureService>();
        services.AddScoped<IParkingGateService, ParkingGateService>();

        return services;
    }
}

