using GradeHoraria.Models;

namespace GradeHoraria.Repositories
{
    public interface IGradeRepository
    {
        Task<IEnumerable<Curso>> SearchCurso();
        Task<Curso> SearchCurso(int id);
        void AddCurso(Curso cursos);
        void UpdateCurso(Curso cursos);
        void DeleteCurso(Curso cursos);
        Task<IEnumerable<Materia>> SearchMateria();
        Task<Materia> SearchMateria(int id);
        void AddMateria(Materia materias);
        void UpdateMateria(Materia materias);
        void DeleteMateria(Materia materias);
        Task<bool> SaveChangesAsync();
    }
}