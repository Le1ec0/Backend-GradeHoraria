using GradeHoraria.Models;

namespace GradeHoraria.Repositories
{
    public interface IGradeRepository
    {
        Task<IEnumerable<Curso>> GetCurso();
        Task<Curso> GetCurso(int id);
        void AddCurso(Curso cursos);
        void UpdateCurso(Curso cursos);
        void DeleteCurso(Curso cursos);
        Task<Periodo> GetPeriodo(int id);
        void UpdatePeriodo(Periodo periodos);
        Task<IEnumerable<Materia>> GetMateria();
        Task<Materia> GetMateria(int id);
        void AddMateria(Materia materias);
        void UpdateMateria(Materia materias);
        void DeleteMateria(Materia materias);
        void AddCursoPeriodos(IEnumerable<CursoPeriodo> cursoPeriodos);
        Task<bool> SaveChangesAsync();
    }
}