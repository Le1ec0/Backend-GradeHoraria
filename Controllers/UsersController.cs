using Microsoft.AspNetCore.Mvc;
using Backend_CarStore.Models;
using Backend_CarStore.Repositories;

namespace CarStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _repository;
        public UsersController(IUsersRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _repository.SearchUser();
            return user.Any()
            ? Ok(user)
            : NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _repository.SearchUser(id);
            return user != null
            ? Ok(user)
            : NotFound("Usuário não encontrado");
        }


        [HttpPost]
        public async Task<IActionResult> Post(Users users)
        {
            _repository.AddUser(users);

            return await _repository.SaveChangesAsync()
            ? Ok("Usuário adicionado com sucesso")
            : BadRequest("Erro ao adicionar usuário");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Users cars)
        {
            var dbUser = await _repository.SearchUser(id);
            if (dbUser == null) return NotFound("Usuário não encontrado");

            dbUser.Name = cars.Name ?? dbUser.Name;
            dbUser.Password = cars.Password ?? dbUser.Password;
            dbUser.Email = cars.Email ?? dbUser.Email;
            dbUser.Phone = cars.Phone ?? dbUser.Phone;

            _repository.UpdateUser(dbUser);

            return await _repository.SaveChangesAsync()
            ? Ok("Usuário atualizado com sucesso")
            : BadRequest("Erro ao atualizar usuário");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbCar = await _repository.SearchUser(id);
            if (dbCar == null) return NotFound("Usuário não encontrado");

            _repository.DeleteUser(dbCar);

            return await _repository.SaveChangesAsync()
            ? Ok("Usuário removido com sucesso")
            : BadRequest("Erro ao remover usuário");
        }

    }
}