using System.Net.Http;
using System.Net.Http.Json;
using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GamesClient
{
    private readonly HttpClient httpClient;
    private readonly GenresClient genresClient;

    public GamesClient(HttpClient httpClient, GenresClient genresClient)
    {
        this.httpClient = httpClient;
        this.genresClient = genresClient;
    }

    public async Task<GameSummary[]> GetGamesAsync()
    {
        try
        {
            var result = await httpClient.GetFromJsonAsync<GameSummary[]>("games");
            return result ?? Array.Empty<GameSummary>();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine("Backend not found—ensure API is running at GameStoreApiUrl.");
            return Array.Empty<GameSummary>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching games: {ex.Message}");
            return Array.Empty<GameSummary>();
        }
    }

    public async Task AddGameAsync(GameDetails game)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(game.GenreId);
            var genres = await genresClient.GetGenresAsync();
            var genre = genres.SingleOrDefault(g => g.Id == game.GenreId.Value);
            if (genre is null) throw new InvalidOperationException("Invalid genre ID.");

            await httpClient.PostAsJsonAsync("games", game);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding game: {ex.Message}");
            throw;
        }
    }

    public async Task<GameDetails> GetGameAsync(int id)
    {
        try
        {
            var result = await httpClient.GetFromJsonAsync<GameDetails>($"games/{id}");
            ArgumentNullException.ThrowIfNull(result);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching game {id}: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateGameAsync(GameDetails updatedGame)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(updatedGame.GenreId);
            await httpClient.PutAsJsonAsync($"games/{updatedGame.Id}", updatedGame);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating game: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteGameAsync(int id)
    {
        try
        {
            await httpClient.DeleteAsync($"games/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting game: {ex.Message}");
            throw;
        }
    }
}