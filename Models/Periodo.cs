using System.ComponentModel.DataAnnotations.Schema;

namespace GradeHoraria.Models
{
    public class Periodo
    {
        public string Semestre { get; set; }
        public string Sala { get; set; }
        public int CursoId { get; set; }
        public Curso Cursos { get; set; }
        public List<Materia> Materias { get; set; }
        public virtual ICollection<CursoPeriodo> CursoPeriodos { get; set; }
        public virtual ICollection<PeriodoMateria> PeriodoMaterias { get; set; }
    }
}