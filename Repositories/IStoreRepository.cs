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
        Task<IEnumerable<RegisterModel>> SearchUser();
        Task<RegisterModel> SearchUser(int id);
        void AddUser(RegisterModel users);
        void UpdateUser(RegisterModel users);
        void DeleteUser(RegisterModel users);

        Task<bool> SaveChangesAsync();

    }
}