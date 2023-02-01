using GradeHoraria.Models;
using Microsoft.AspNetCore.Identity;

namespace GradeHoraria.Repositories
{
    public interface IGradeRepository
    {
        Task<IEnumerable<Curso>> GetCurso();
        Task<Curso> GetCurso(int id);
        void AddCurso(Curso cursos);
        Task AddUser(IdentityUser identityUsers);
        void UpdateCurso(Curso cursos);
        void DeleteCurso(Curso cursos);
        Task<IEnumerable<Materia>> GetMateria();
        Task<Materia> GetMateria(int id);
        void AddMateria(Materia materias);
        void UpdateMateria(Materia materias);
        void DeleteMateria(Materia materias);
        Task<bool> SaveChangesAsync();
    }
}