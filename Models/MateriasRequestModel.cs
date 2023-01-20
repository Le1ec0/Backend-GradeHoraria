using System.ComponentModel.DataAnnotations;

public class MateriasRequestModel
{
    public int? CursoId { get; set; }
    public string? Nome { get; set; }
    public string? Periodo { get; set; }
    public string? Turno { get; set; }
    public string? DSemana { get; set; }
    public string? Sala { get; set; }
    public string? Professor { get; set; }
    public string? UserId { get; set; }
}