using System.ComponentModel.DataAnnotations;

public class CursosRequestModel
{
    [Required]
    public string? Nome { get; set; }
    [Required]
    public int? Periodo { get; set; }
    [Required]
    public string? UserId { get; set; }
}