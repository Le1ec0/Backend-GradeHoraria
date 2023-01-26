using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GradeHoraria.Models
{
    public class PeriodosCursos
    {
        public int CursoId { get; set; }
        public Curso Cursos { get; set; }

        public int PeriodoId { get; set; }
        public Periodo Periodos { get; set; }

        public ICollection<MateriasPeriodos> Materias { get; set; }
    }
}