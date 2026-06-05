using AppCore.Authorization;
using AppCore.Models;
using AppCore.Repositories;
using AppCore.Services;
using Infrastructure.EntityFramework.Context;
using Infrastructure.EntityFramework.Repositories;
using Infrastructure.EntityFramework.UnitOfWork;
using Infrastructure.Identity;
using Infrastructure.Security;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IDataExportService, DataExportService>();
        return services;
    }
    
     public static IServiceCollection AddJwt(this IServiceCollection services, JwtSettings jwtOptions)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = jwtOptions.GetSymmetricKey(),
                        ClockSkew = TimeSpan.Zero // brak tolerancji czasu
                    };
                }
            );
        services.AddAuthorization(options =>
        {
            // Polityki oparte o role
            // metoda RequireRole akceptuje dowolną liczbę parametrów typu string
            options.AddPolicy(AppPolicies.AdminOnly.ToString(), policy =>
                policy.RequireRole(UserRole.Administrator.ToString()));


            // dodaj polityki dla pozostałych stałych
            // np. która wymaga użytkownika z jedną z ról: Administrator, Student
    
            // Polityka złożona — wymaga roli i aktywnego konta
            // Zakładamy, że w AppPolicies jest stała ActiveUser
            options.AddPolicy(AppPolicies.ActiveUser.ToString(), policy =>
                policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("status", SystemUserStatus.Active.ToString()));

            options.AddPolicy(AppPolicies.AnonymousUser.ToString(), policy =>
                policy.RequireAssertion(context =>
                    !context.User.Identity?.IsAuthenticated ?? true));

            // Domyślna polityka — każdy zalogowany użytkownik
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            // Polityka fallback — stosowana gdy brak atrybutu [Authorize]
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
        
        services.AddScoped<IDataSeeder, IdentityDbSeeder>();
        services.AddScoped<IAuthService, AuthService>();
        
        return services;
    }
}

