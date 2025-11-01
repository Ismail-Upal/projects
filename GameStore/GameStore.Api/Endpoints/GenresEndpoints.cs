using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using AutoMapper;
using Microsoft.AspNetCore.OpenApi;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoints
{
    public static void MapGenresEndpoints(this WebApplication app)
    {
        var genres = app.MapGroup("/genres");

        genres.MapGet("/", async (GameStoreDbContext db) => await db.Genres.ToListAsync())
              .Produces<List<GenreDto>>();

        genres.MapGet("/{id}", async (int id, GameStoreDbContext db) =>
        {
            var genre = await db.Genres.FindAsync(id);
            return genre is null ? Results.NotFound() : Results.Ok(genre);
        })
              .Produces<GenreDto>(200)
              .Produces(404);

        genres.MapPost("/", async (GenreDto genreDto, GameStoreDbContext db, IMapper mapper) =>
        {
            var genre = mapper.Map<Genre>(genreDto);
            db.Genres.Add(genre);
            await db.SaveChangesAsync();
            return Results.Created($"/genres/{genre.Id}", genre);
        });

        genres.MapPut("/{id}", async (int id, GenreDto genreDto, GameStoreDbContext db, IMapper mapper) =>
        {
            var genre = await db.Genres.FindAsync(id);
            if (genre is null) return Results.NotFound();
            mapper.Map(genreDto, genre);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        genres.MapDelete("/{id}", async (int id, GameStoreDbContext db) =>
        {
            var genre = await db.Genres.FindAsync(id);
            if (genre is null) return Results.NotFound();
            db.Genres.Remove(genre);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}