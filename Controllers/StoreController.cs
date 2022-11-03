using Microsoft.AspNetCore.Mvc;
using Backend_CarStore.Models;
using Backend_CarStore.Repositories;

namespace CarStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly ICarsRepository _repository;
        public CarsController(ICarsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var car = await _repository.SearchCar();
            return car.Any()
            ? Ok(car)
            : NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var car = await _repository.SearchCar(id);
            return car != null
            ? Ok(car)
            : NotFound("Carro não encontrado");
        }


        [HttpPost]
        public async Task<IActionResult> Post(Cars cars)
        {
            _repository.AddCar(cars);

            return await _repository.SaveChangesAsync()
            ? Ok("Carro adicionado com sucesso")
            : BadRequest("Erro ao adicionar carro");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Cars cars)
        {
            var dbCar = await _repository.SearchCar(id);
            if (dbCar == null) return NotFound("Carro não encontrado");

            dbCar.Plate = cars.Plate ?? dbCar.Plate;
            dbCar.Brand = cars.Brand ?? dbCar.Brand;
            dbCar.Model = cars.Model ?? dbCar.Model;
            dbCar.Color = cars.Color ?? dbCar.Color;
            dbCar.Year = cars.Year ?? cars.Year;

            _repository.UpdateCar(dbCar);

            return await _repository.SaveChangesAsync()
            ? Ok("Carro atualizado com sucesso")
            : BadRequest("Erro ao atualizar carro");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbCar = await _repository.SearchCar(id);
            if (dbCar == null) return NotFound("Carro não encontrado");

            _repository.DeleteCar(dbCar);

            return await _repository.SaveChangesAsync()
            ? Ok("Carro removido com sucesso")
            : BadRequest("Erro ao remover carro");
        }

    }

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
        public async Task<IActionResult> Put(int id, Users users)
        {
            var dbUser = await _repository.SearchUser(id);
            if (dbUser == null) return NotFound("Usuário não encontrado");

            dbUser.Name = users.Name ?? dbUser.Name;
            dbUser.Password = users.Password ?? dbUser.Password;
            dbUser.Email = users.Email ?? dbUser.Email;
            dbUser.Phone = users.Phone ?? dbUser.Phone;

            _repository.UpdateUser(dbUser);

            return await _repository.SaveChangesAsync()
            ? Ok("Usuário atualizado com sucesso")
            : BadRequest("Erro ao atualizar usuário");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbUser = await _repository.SearchUser(id);
            if (dbUser == null) return NotFound("Usuário não encontrado");

            _repository.DeleteUser(dbUser);

            return await _repository.SaveChangesAsync()
            ? Ok("Usuário removido com sucesso")
            : BadRequest("Erro ao remover usuário");
        }
    }
}