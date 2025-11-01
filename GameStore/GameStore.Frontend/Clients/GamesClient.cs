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
            return await httpClient.GetFromJsonAsync<GameSummary[]>("games") ?? Array.Empty<GameSummary>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching games: {ex.Message}");
            return Array.Empty<GameSummary>();
        }
    }

    public async Task<GameDetails?> GetGameAsync(int id)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<GameDetails>($"games/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching game {id}: {ex.Message}");
            return null;
        }
    }

    public async Task AddGameAsync(GameDetails game)
    {
        try
        {
            await httpClient.PostAsJsonAsync("games", game);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding game: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateGameAsync(GameDetails game)
    {
        try
        {
            await httpClient.PutAsJsonAsync($"games/{game.Id}", game);
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