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
        public async Task<IEnumerable<Curso>> GetCurso()
        {
            return await _context.Cursos.ToListAsync();
        }
        public async Task<Curso> GetCurso(int id)
        {
            return await _context.Cursos.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public void AddCurso(Curso cursos)
        {
            _context.Cursos.Add(cursos);
        }
        public void UpdateCurso(Curso cursos)
        {
            _context.Update(cursos);
        }

        public void DeleteCurso(Curso cursos)
        {
            _context.Remove(cursos);
        }
        public async Task<IEnumerable<Materia>> GetMateria()
        {
            return await _context.Materias.ToListAsync();
        }
        public async Task<Materia> GetMateria(int id)
        {
            return await _context.Materias.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public void AddMateria(Materia materias)
        {
            _context.Add(materias);
        }
        public void UpdateMateria(Materia materias)
        {
            _context.Update(materias);
        }

        public void DeleteMateria(Materia materias)
        {
            _context.Remove(materias);
        }
        public async void AddCursoPeriodos(IEnumerable<CursoPeriodo> cursoPeriodos)
        {
            await _context.CursoPeriodo.AddRangeAsync(cursoPeriodos);
            await _context.SaveChangesAsync();
        }
        public ApplicationDbContext GetContext()
        {
            return _context;
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
