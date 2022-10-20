using Microsoft.EntityFrameworkCore;
using Backend_CarStore.Models;
using Backend_CarStore.Context;

namespace Backend_CarStore.Repositories
{
    public class CarsRepository : ICarsRepository
    {
        private readonly CarsDbContext _context;
        public CarsRepository(CarsDbContext context)
        {
            _context = context;
        }
        public void AddCars(Cars cars)
        {
            _context.Add(cars);
        }

        public void DeleteCars(Cars cars)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Cars>> SearchCar()
        {
            return await _context.Car.ToListAsync();
        }

        public async Task<Cars> SearchCar(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateCars(Cars cars)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
