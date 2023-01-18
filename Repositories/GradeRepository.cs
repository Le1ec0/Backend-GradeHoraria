using Microsoft.EntityFrameworkCore;
using GradeHoraria.Models;
using GradeHoraria.Context;

namespace GradeHoraria.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private readonly ApplicationDbContext _context;
        public GradeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Cursos>> SearchCurso()
        {
            return await _context.Cursos.ToListAsync();
        }
        public async Task<Cursos> SearchCurso(int id)
        {
            return await _context.Cursos.Where(x => x.CursoId == id).FirstOrDefaultAsync();
        }
        public void AddCurso(Cursos cursos)
        {
            _context.Add(cursos);
        }
        public void UpdateCurso(Cursos cursos)
        {
            _context.Update(cursos);
        }

        public void DeleteCurso(Cursos cursos)
        {
            _context.Remove(cursos);
        }
        public async Task<IEnumerable<Materias>> SearchMateria()
        {
            return await _context.Materias.ToListAsync();
        }
        public async Task<Materias> SearchMateria(int id)
        {
            return await _context.Materias.Where(x => x.MateriaId == id).FirstOrDefaultAsync();
        }
        public void AddMateria(Materias materias)
        {
            _context.Add(materias);
        }
        public void UpdateMateria(Materias materias)
        {
            _context.Update(materias);
        }

        public void DeleteMateria(Materias materias)
        {
            _context.Remove(materias);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
