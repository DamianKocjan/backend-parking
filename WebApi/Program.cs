using AppCore.Repositories;
using Infrastructure.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ICarRepository, MemoryCarRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app
    .MapGet(
        "/api/cars/{number}",
        async (ICarRepository repository, string number, HttpContext httpContext) => await repository.FindByPlateNumber(number))
    .WithName("");

app.Run();
