using System.Net.Http.Json;
using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GenresClient
{
    private readonly HttpClient httpClient;

    public GenresClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Genre[]> GetGenresAsync()
    {
        try
        {
            return await httpClient.GetFromJsonAsync<Genre[]>("genres") ?? Array.Empty<Genre>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching genres: {ex.Message}");
            return Array.Empty<Genre>();
        }
    }
}