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

        [HttpPost]
        public async Task<IActionResult> Post(Cars cars)
        {
            _repository.AddCars(cars);
            return await _repository.SaveChangesAsync()
            ? Ok("Carro adicionado com sucesso")
            : BadRequest("Erro ao adicionar carro");
        }

        /*[HttpDelete]
        public IActionResult Delete(Cars cars)
        {
            var car = Car();
            car.Delete(cars);
            return Ok(Car());
        }

        [HttpPut]
        public IActionResult Put(Cars cars)
        {
            var car = Car();
            car.Put(cars);
            return Ok(Car());
        }*/
    }
}