using System.ComponentModel.DataAnnotations;

namespace TextAnalyzer.Models;

public class WordModel
{
    [Key]
    public int Id { get; init; }
    [Required]
    [StringLength(10)]
    public required string Word { get; set; }
    [Required]
    [StringLength(1)]
    public required string Pos { get; set; }
}