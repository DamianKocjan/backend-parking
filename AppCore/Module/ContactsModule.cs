using AppCore.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class AppCoreModule
{
    public static IServiceCollection AddAppCoreModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<CameraCaptureValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateCameraCaptureDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<ParkingGateValidator>();
        services.AddValidatorsFromAssemblyContaining<ParkingTariffValidator>();
        
        services.AddFluentValidationAutoValidation();
        return services;
    }
}