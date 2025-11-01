using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public class GameDetailsDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public int? GenreId { get; set; }
    [Required, Range(1, 100)]
    public decimal Price { get; set; } = 10m;
    [Required]
    public DateOnly ReleaseDate { get; set; }
}