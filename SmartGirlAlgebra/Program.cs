using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SmartGirlAlgebra;
using SmartGirlAlgebra.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure API HttpClient
var apiBaseAddress = builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7217/";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });

// Register API services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProgressService>();

// Register algebra services
builder.Services.AddSingleton<ExpressionParser>();
builder.Services.AddSingleton<ExpressionEvaluator>();
builder.Services.AddSingleton<ExpressionSimplifier>();
builder.Services.AddSingleton<EquationParser>();
builder.Services.AddSingleton<LinearEquationSolver>();
builder.Services.AddSingleton<ProblemGenerator>();

var host = builder.Build();

// Initialize auth service
var authService = host.Services.GetRequiredService<AuthService>();
await authService.InitializeAsync();

await host.RunAsync();
