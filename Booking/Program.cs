using Microsoft.EntityFrameworkCore;
using Booking.Data;
using System.IO;
using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var scalarBasePath = builder.Configuration["Scalar:BasePath"] ?? "/BookingAPI";

// Läs konfiguration (appsettings.json) relativt projektmappen
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Databaskonfiguration från appsettings.json med fallback till miljövariabel
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       Environment.GetEnvironmentVariable("CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Anslutningssträngen 'DefaultConnection' saknas i appsettings.json och som miljövariabel.");
}
Console.WriteLine($"Anslutningssträng: {connectionString}");

builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseSqlServer(connectionString));

// Lägg till tjänster i Dependency Injection
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Lägg till HttpClient för kommunikation med RoomAPI
builder.Services.AddHttpClient("RoomAPI", client =>
{
    client.BaseAddress = new Uri("https://informatik1.ei.hv.se/RoomAPI/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");

    // Logga för att verifiera att HttpClient är korrekt konfigurerad
    Console.WriteLine($"HttpClient BaseAddress: {client.BaseAddress}");
});

var app = builder.Build();

// Sätt API-prefix
app.UsePathBase("/BookingAPI");

// Middleware pipeline
app.UseHttpsRedirection();
app.UseMiddleware<ApiLoggingMiddleware>();
app.UseAuthorization();
app.MapControllers();

// Aktivera OpenAPI Scalar
app.MapOpenApi();

// Aktivera Scalar UI även i produktion
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.MapScalarApiReference(options =>
    {
        options.Title = "Booking API Dokumentation";
        options.Theme = ScalarTheme.Mars;
        options.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
    });
}

// Kör applikationen
app.Run();
