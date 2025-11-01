using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Entities;

public class Genre
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
}