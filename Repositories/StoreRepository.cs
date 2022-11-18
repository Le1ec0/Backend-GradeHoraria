using Microsoft.EntityFrameworkCore;
using CarStore.Models;
using CarStore.Context;

namespace CarStore.Repositories
{
    public class CarsRepository : ICarsRepository
    {
        private readonly ApplicationDbContext _context;
        public CarsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Cars>> SearchCar()
        {
            return await _context.Car.ToListAsync();
        }
        public async Task<Cars> SearchCar(string id)
        {
            return await _context.Car.Where(x => x.CarId == id).FirstOrDefaultAsync();
        }
        public void AddCar(Cars cars)
        {
            _context.Add(cars);
        }

        public void AddCar(CarsPost carsPost)
        {
            _context.Add(carsPost);
        }
        public void UpdateCar(Cars cars)
        {
            _context.Update(cars);
        }

        public void DeleteCar(Cars cars)
        {
            _context.Remove(cars);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
    /*public class UsersRepository : IUsersRepository
    {
        private readonly StoreDbContext _context;
        public UsersRepository(StoreDbContext context)
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
    }*/
}
