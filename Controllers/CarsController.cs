using Microsoft.AspNetCore.Mvc;
using Backend_CarStore.Models;
using Backend_CarStore.Repositories;

namespace CarStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly ICarsRepository repository;
        public CarsController(ICarsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            return Ok(Car());
        }

        [HttpPost()]
        public IActionResult Post(Cars cars)
        {
            var car = Car();
            car.Add(cars);
            return Ok(car);
        }

        /*[HttpDelete()]
        public IActionResult Delete(Cars cars)
        {
            var car = Car();
            car.Delete(cars);
            return Ok(Car());
        }

        [HttpPut()]
        public IActionResult Put(Cars cars)
        {
            var car = Car();
            car.Put(cars);
            return Ok(Car());
        }*/
    }
}