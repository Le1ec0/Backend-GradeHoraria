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
        Task<bool> SaveChangesAsync();
    }
}