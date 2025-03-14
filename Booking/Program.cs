using Microsoft.EntityFrameworkCore;
using Booking.Data; // Säkerställ att detta är korrekt namespace
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Läs konfiguration (appsettings.json) relativt projektmappen
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Lägg till tjänster i Dependency Injection
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Databaskonfiguration från appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Anslutningssträngen 'DefaultConnection' saknas i appsettings.json.");
}

builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseSqlServer(connectionString));

// Lägg till HttpClient för kommunikation med RoomAPI
builder.Services.AddHttpClient("RoomAPI", client =>
{
    // ÄNDRA vid behov (URL där RoomAPI körs)
    client.BaseAddress = new Uri("https://localhost:7186/");
});

// Bygg applikationen
var app = builder.Build();

// Swagger-konfiguration (bara vid utveckling)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking API v1");
        c.RoutePrefix = string.Empty; // Swagger visas på startsidan
    });
}

// Middleware pipeline
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
