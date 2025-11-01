using System;
using GameStore.Frontend.Models;
using System.Net.Http;

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
            var result = await httpClient.GetFromJsonAsync<Genre[]>("genres");
            return result ?? Array.Empty<Genre>();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine("Genres endpoint not found—ensure backend is running.");
            return Array.Empty<Genre>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching genres: {ex.Message}");
            return Array.Empty<Genre>();
        }
    }
}