using GradeHoraria.Models;

namespace GradeHoraria.Repositories
{
    public interface ICarsRepository
    {
        Task<IEnumerable<Cursos>> SearchCurso();
        Task<Cursos> SearchCurso(int id);
        void AddCurso(Cursos cars);
        void UpdateCurso(Cursos cars);
        void DeleteCurso(Cursos cars);
        Task<bool> SaveChangesAsync();
    }
}