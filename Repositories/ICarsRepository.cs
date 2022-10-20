using Backend_CarStore.Models;

namespace Backend_CarStore.Repositories
{
    public interface ICarsRepository
    {
        Task<IEnumerable<Cars>> SearchCar();
        Task<Cars> SearchCar(int id);
        void AddCars(Cars cars);
        void UpdateCars(Cars cars);
        void DeleteCars(Cars cars);

        Task<bool> SaveChangesAsync();

    }
}