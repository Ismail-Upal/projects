using GameStore.Frontend.Clients;
using GameStore.Frontend.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents();

var gameStoreApiUrl = builder.Configuration["GameStoreApiUrl"] ??
    throw new Exception("GameStoreApiUrl is not set");

builder.Services.AddHttpClient<GamesClient>(client =>
    client.BaseAddress = new Uri(gameStoreApiUrl));

builder.Services.AddHttpClient<GenresClient>(client =>
    client.BaseAddress = new Uri(gameStoreApiUrl));

builder.Services.AddScoped<GamesClient>(); // For dep injection

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();