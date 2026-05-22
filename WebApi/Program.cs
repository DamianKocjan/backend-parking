using AppCore.Repositories;
using AppCore.Services;
using Infrastructure.Memory;
using Infrastructure.Memory.Services;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();    
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IParkingGateRepository, MemoryParkingGateRepository>();
builder.Services.AddScoped<IParkingSessionRepository, MemoryParkingSessionRepository>();
builder.Services.AddScoped<IVehicleRepository, MemoryVehicleRepository>();
builder.Services.AddScoped<ICameraCaptureRepository, MemoryCameraCaptureRepository>();

builder.Services.AddScoped<IParkingUnitOfWork, MemoryParkingUnitOfWork>();

builder.Services.AddScoped<IParkingGateService, MemoryParkingGateService>();
builder.Services.AddScoped<ICameraCaptureService, MemoryCameraCaptureService>();

builder.Services.AddAppCoreModule(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseExceptionHandler();

app.MapControllers();

app.Run();
