using Infrastructure.EntityFramework;
using Infrastructure.Security;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();    
builder.Services.AddProblemDetails();
 
builder.Services.AddParkingEfModule(builder.Configuration);
builder.Services.AddAppCoreModule(builder.Configuration);

builder.Services.AddSingleton<JwtSettings>();
builder.Services.AddJwt(new JwtSettings(builder.Configuration));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    using var scope = app.Services.CreateScope(); // zasięg dostepu do kontenera DI
    using (scope)
    {
        // "wyciągniecie" z kontenera instacji klasy implementującej IDataSeeder
        var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
        await seeder.SeedAsync();    // wywołanie metody Seedera
    }

}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();

app.MapControllers();

app.Run();
