using AppCore.Repositories;
using AppCore.Services;
using Infrastructure.Memory;
using Infrastructure.Memory.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IParkingGateRepository, MemoryParkingGateRepository>();
builder.Services.AddScoped<IParkingSessionRepository, MemoryParkingSessionRepository>();
builder.Services.AddScoped<IVehicleRepository, MemoryVehicleRepository>();

builder.Services.AddScoped<IParkingUnitOfWork, MemoryParkingUnitOfWork>();

builder.Services.AddScoped<IParkingGateService, MemoryParkingGateService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
