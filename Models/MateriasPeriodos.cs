using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GradeHoraria.Models
{
    public class MateriasPeriodos
    {
        public int MateriaId { get; set; }
        public Materia Materias { get; set; }

        public int PeriodoId { get; set; }
        public PeriodosCursos Periodo { get; set; }
    }
}