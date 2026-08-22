using Microsoft.EntityFrameworkCore;
using SmartGirlAlgebra.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// The app is served from several hostnames, plus the Static Web Apps preview domains.
// SetIsOriginAllowed is used rather than WithOrigins because WithOrigins does not
// support wildcards — a wildcard entry there silently never matches.
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

// Apply migrations on start. The app owns a single table, so this keeps deployment
// to "push the code" with no separate migration step.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowBlazorApp");
app.MapControllers();

// Gives the root and /health something real to answer with, so a deploy can be
// verified without guessing at a route.
app.MapGet("/", () => Results.Ok(new { service = "SmartGirlAlgebra.API", status = "ok" }));
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();
