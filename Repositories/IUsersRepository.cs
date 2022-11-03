using Backend_CarStore.Models;

namespace Backend_CarStore.Repositories
{
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