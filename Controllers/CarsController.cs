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
            _repository.AddCars(cars);

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
            dbCar.Name = cars.Name ?? dbCar.Name;
            dbCar.Phone = cars.Phone ?? cars.Phone;

            _repository.UpdateCars(dbCar);

            return await _repository.SaveChangesAsync()
            ? Ok("Carro atualizado com sucesso")
            : BadRequest("Erro ao atualizar carro");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbCar = await _repository.SearchCar(id);
            if (dbCar == null) return NotFound("Carro não encontrado");

            _repository.DeleteCars(dbCar);

            return await _repository.SaveChangesAsync()
            ? Ok("Carro removido com sucesso")
            : BadRequest("Erro ao remover carro");
        }

    }
}