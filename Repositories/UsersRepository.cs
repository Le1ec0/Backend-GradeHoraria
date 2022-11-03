using Microsoft.EntityFrameworkCore;
using Backend_CarStore.Models;
using Backend_CarStore.Context;

namespace Backend_CarStore.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UsersDbContext _context;
        public UsersRepository(UsersDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Users>> SearchUser()
        {
            return await _context.User.ToListAsync();
        }
        public async Task<Users> SearchUser(int id)
        {
            return await _context.User.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public void AddUser(Users users)
        {
            _context.Add(users);
        }
        public void UpdateUser(Users users)
        {
            _context.Update(users);
        }

        public void DeleteUser(Users users)
        {
            _context.Remove(users);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
