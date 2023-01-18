using GradeHoraria.Models;

namespace GradeHoraria.Repositories
{
    public interface IGradeRepository
    {
        Task<IEnumerable<Cursos>> SearchCurso();
        Task<Cursos> SearchCurso(int id);
        void AddCurso(Cursos cursos);
        void UpdateCurso(Cursos cursos);
        void DeleteCurso(Cursos cursos);
        Task<IEnumerable<Materias>> SearchMateria();
        Task<Materias> SearchMateria(int id);
        void AddCurso(Materias materias);
        void UpdateCurso(Materias materias);
        void DeleteMateria(Materias materias);
        Task<bool> SaveChangesAsync();
    }
}