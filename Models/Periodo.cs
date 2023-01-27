using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GradeHoraria.Models
{
    public class Periodo
    {
        [Key]
        public int Periodo_Id { get; set; }
        public ICollection<CursoPeriodo> CursoPeriodos { get; set; }
    }
}