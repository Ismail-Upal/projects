using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Entities;

public class Game
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public int GenreId { get; set; }
    [Range(1, 100)]
    public decimal Price { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public Genre Genre { get; set; } = null!;
}