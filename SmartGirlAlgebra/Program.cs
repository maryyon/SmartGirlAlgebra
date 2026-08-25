using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SmartGirlAlgebra;
using SmartGirlAlgebra.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Two clients on purpose: the API lives on another host, while version content
// is static and served from this app's own origin.
var apiBaseAddress = builder.Configuration["ApiBaseAddress"] ?? builder.HostEnvironment.BaseAddress;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });

builder.Services.AddScoped(sp => new ProfileService(
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) },
    sp.GetRequiredService<Microsoft.JSInterop.IJSRuntime>()));

builder.Services.AddScoped<PlayerService>();

// Algebra engine
builder.Services.AddSingleton<ExpressionParser>();
builder.Services.AddSingleton<ExpressionEvaluator>();
builder.Services.AddSingleton<ExpressionSimplifier>();
builder.Services.AddSingleton<EquationParser>();
builder.Services.AddSingleton<LinearEquationSolver>();
builder.Services.AddSingleton<ProblemGenerator>();

await builder.Build().RunAsync();
