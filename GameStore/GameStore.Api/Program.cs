using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<GameStoreDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(GenreMappingProfile).Assembly);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(builder.Configuration["CorsOrigins"]?.Split(',') ?? new[] { "https://localhost:5001" })
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// JSON options for DateOnly
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonConverterFactory(typeof(DateOnly), new DateOnlyJsonConverter()));
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();

app.MapGamesEndpoints();
app.MapGenresEndpoints();

// Seed DB if empty
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GameStoreDbContext>();
    context.Database.EnsureCreated();
    if (!context.Games.Any())
    {
        // Seed data
        context.Genres.AddRange(
            new Genre { Id = 1, Name = "Fighting" },
            new Genre { Id = 2, Name = "Roleplaying" },
            new Genre { Id = 3, Name = "Sports" }
        );
        context.SaveChanges();

        context.Games.AddRange(
            new Game { Id = 1, Name = "Street Fighter II", GenreId = 1, Price = 19.99m, ReleaseDate = new DateOnly(1992, 6, 10) },
            new Game { Id = 2, Name = "Final Fantasy XIV", GenreId = 2, Price = 53.44m, ReleaseDate = new DateOnly(2013, 8, 27) },
            new Game { Id = 3, Name = "FIFA 25", GenreId = 3, Price = 64.99m, ReleaseDate = new DateOnly(2024, 9, 26) }
        );
        context.SaveChanges();
    }
}

app.Run();