using System.ComponentModel.DataAnnotations;

public class MateriasRequestModel
{
    [Required]
    public int? CursoId { get; set; }
    [Required]
    public string? Nome { get; set; }
    [Required]
    public string? Turno { get; set; }
    [Required]
    public string? DSemana { get; set; }
    [Required]
    public string? Sala { get; set; }
    [Required]
    public string? Professor { get; set; }
    [Required]
    public string? UserId { get; set; }
}