using Microsoft.EntityFrameworkCore;
using Booking.Data; // Importera r�tt namespace
using Microsoft.Extensions.Configuration;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Explicit s�kv�g till appsettings.json
var configPath = @"C:\Users\phosf\OneDrive - H�gskolan V�st\Documents\sysArkt_SOS100\SOAgrpAPI\Booking\Booking\appsettings.json";

// Kontrollera om filen finns
if (!File.Exists(configPath))
{
    throw new FileNotFoundException($"Konfigurationsfilen saknas: {configPath}");
}

// L�s in konfigurationen
builder.Configuration.SetBasePath(Path.GetDirectoryName(configPath) ?? Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile(Path.GetFileName(configPath), optional: false, reloadOnChange: true);

// L�gg till tj�nster i DI-containern
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// L�gg till RoomDbContext med explicit anslutningsstr�ng
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Anslutningsstr�ngen 'DefaultConnection' saknas i appsettings.json.");
}

builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseSqlServer(connectionString));

/* 
// L�gg till ApplicationDbContext (S�KERHETSDATABAS) n�r den andra gruppen har lagt upp sin server.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SecurityDatabase")));
*/

var app = builder.Build();

// Konfigurera Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", " API v1");
        c.RoutePrefix = string.Empty; // Swagger visas direkt p� roten
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
