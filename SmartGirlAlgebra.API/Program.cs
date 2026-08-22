using Microsoft.EntityFrameworkCore;
using SmartGirlAlgebra.API.Data;

var builder = WebApplication.CreateBuilder(args);

// The database is serverless on the free tier and pauses itself after an hour
// idle. Waking it takes up to a minute and throws "not currently available" in
// the meantime, so every connection needs retry built in — not just startup.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql =>
        {
            sql.EnableRetryOnFailure(
                maxRetryCount: 8,
                maxRetryDelay: TimeSpan.FromSeconds(15),
                errorNumbersToAdd: null);
            sql.CommandTimeout(120);
        }));

// The app is served from several hostnames, plus the Static Web Apps preview
// domains. SetIsOriginAllowed is used rather than WithOrigins because WithOrigins
// does not support wildcards — a wildcard entry there silently never matches.
var allowedOrigins = new[]
{
    "https://smartgirlalgebra.fun",
    "https://www.smartgirlalgebra.fun",
    "https://smartgirlalgebra.com",
    "https://www.smartgirlalgebra.com",
    "https://localhost:7001",
    "http://localhost:5001"
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy => policy
        .SetIsOriginAllowed(origin =>
            allowedOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase) ||
            origin.EndsWith(".azurestaticapps.net", StringComparison.OrdinalIgnoreCase))
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations on start, but NEVER let that stop the app booting.
//
// This previously called Migrate() directly. When the serverless database was
// paused, the wake-up threw, the exception went unhandled, and the whole process
// died with HTTP 500.30 — permanently, until someone restarted it. An idle
// database must not be able to take the API down.
await ApplyMigrationsAsync(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowBlazorApp");
app.MapControllers();

// Deliberately does not touch the database, so it still answers while the
// database is waking and can be used to tell "app down" from "database asleep".
app.MapGet("/", () => Results.Ok(new { service = "SmartGirlAlgebra.API", status = "ok" }));
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();

static async Task ApplyMigrationsAsync(WebApplication app)
{
    var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");

    for (var attempt = 1; attempt <= 5; attempt++)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await db.Database.MigrateAsync();
            logger.LogInformation("Database migrated on attempt {Attempt}.", attempt);
            return;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Migration attempt {Attempt} of 5 failed.", attempt);
            if (attempt < 5) await Task.Delay(TimeSpan.FromSeconds(10 * attempt));
        }
    }

    // Start anyway. The schema is almost certainly already applied from an
    // earlier boot, and serving a waking database beats serving nothing.
    logger.LogError("Could not migrate the database at startup; continuing without it.");
}
