using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SmartGirlAlgebra;
using SmartGirlAlgebra.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseAddress = builder.Configuration["ApiBaseAddress"] ?? builder.HostEnvironment.BaseAddress;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });

builder.Services.AddScoped<PlayerService>();

// Algebra engine
builder.Services.AddSingleton<ExpressionParser>();
builder.Services.AddSingleton<ExpressionEvaluator>();
builder.Services.AddSingleton<ExpressionSimplifier>();
builder.Services.AddSingleton<EquationParser>();
builder.Services.AddSingleton<LinearEquationSolver>();
builder.Services.AddSingleton<ProblemGenerator>();

var host = builder.Build();

// Picks up an existing sync code if this device already has one. Never blocks
// play — a failure here just means she starts fresh on this device.
var player = host.Services.GetRequiredService<PlayerService>();
await player.InitializeAsync();

await host.RunAsync();
