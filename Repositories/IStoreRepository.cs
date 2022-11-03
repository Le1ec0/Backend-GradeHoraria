using Backend_CarStore.Models;

namespace Backend_CarStore.Repositories
{
    public interface ICarsRepository
    {
        Task<IEnumerable<Cars>> SearchCar();
        Task<Cars> SearchCar(int id);
        void AddCar(Cars cars);
        void UpdateCar(Cars cars);
        void DeleteCar(Cars cars);

        Task<bool> SaveChangesAsync();

    }
    public interface IUsersRepository
    {
        Task<IEnumerable<Users>> SearchUser();
        Task<Users> SearchUser(int id);
        void AddUser(Users users);
        void UpdateUser(Users users);
        void DeleteUser(Users users);

        Task<bool> SaveChangesAsync();

    }
}