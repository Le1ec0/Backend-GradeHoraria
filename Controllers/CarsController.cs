using Microsoft.AspNetCore.Mvc;
using Backend_CarStore.Models;

namespace CarStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private static List<Cars> Car()
        {
            return new List<Cars>{
                new Cars {Id = 0, Plate = "GCS-1255", Brand = "Mitsubishi", Model = "Lancer", Color = "Azul", Year = 1995}
            };
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