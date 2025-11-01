using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using AutoMapper;
using Microsoft.AspNetCore.OpenApi;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    public static void MapGamesEndpoints(this WebApplication app)
    {
        var games = app.MapGroup("/games");

        games.MapGet("/", async (GameStoreDbContext db) => 
            await db.Games.Include(g => g.Genre).Select(g => new GameSummaryDto
            {
                Id = g.Id,
                Name = g.Name,
                Genre = g.Genre.Name,
                Price = g.Price,
                ReleaseDate = g.ReleaseDate
            }).ToListAsync())
              .Produces<List<GameSummaryDto>>();

        games.MapGet("/{id}", async (int id, GameStoreDbContext db) =>
        {
            var game = await db.Games.Include(g => g.Genre).FirstOrDefaultAsync(g => g.Id == id);
            if (game is null) return Results.NotFound();
            return Results.Ok(new GameDetailsDto
            {
                Id = game.Id,
                Name = game.Name,
                GenreId = game.GenreId,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate
            });
        })
              .Produces<GameDetailsDto>(200)
              .Produces(404);

        games.MapPost("/", async (GameDetailsDto dto, GameStoreDbContext db) =>
        {
            var game = new Game
            {
                Name = dto.Name,
                GenreId = dto.GenreId!.Value,
                Price = dto.Price,
                ReleaseDate = dto.ReleaseDate
            };
            db.Games.Add(game);
            await db.SaveChangesAsync();
            return Results.Created($"/games/{game.Id}", game);
        });

        games.MapPut("/{id}", async (int id, GameDetailsDto dto, GameStoreDbContext db) =>
        {
            var game = await db.Games.FindAsync(id);
            if (game is null) return Results.NotFound();
            game.Name = dto.Name;
            game.GenreId = dto.GenreId!.Value;
            game.Price = dto.Price;
            game.ReleaseDate = dto.ReleaseDate;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        games.MapDelete("/{id}", async (int id, GameStoreDbContext db) =>
        {
            var game = await db.Games.FindAsync(id);
            if (game is null) return Results.NotFound();
            db.Games.Remove(game);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}