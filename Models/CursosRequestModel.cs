using System.ComponentModel.DataAnnotations;

namespace GradeHoraria.Models
{
    public class CursosRequestModel
    {
        public string Nome { get; set; }
        public string Turno { get; set; }
        public string Sala { get; set; }
        public string Professor { get; set; }
        public int Periodo { get; set; }
    }
}