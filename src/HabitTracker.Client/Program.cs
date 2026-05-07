using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HabitTracker.Client;
using HabitTracker.Domain.Interfaces;
using HabitTracker.Infrastructure.Persistence;
using HabitTracker.Application.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// N-Layered Services
builder.Services.AddScoped<IHabitRepository, IndexedDbHabitRepository>();
builder.Services.AddScoped<IHabitService, HabitService>();

await builder.Build().RunAsync();
