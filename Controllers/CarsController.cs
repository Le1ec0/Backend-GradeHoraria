using Microsoft.AspNetCore.Mvc;
using Backend_CarStore.Models;

namespace CarStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarsController : ControllerBase
    {
        private static List<Cars> Car()
        {
            return new List<Cars>{
                new Cars {Id = 0, Plate = "JFK-1310", Brand = "Teste", Model = "Teste", Color = "Azul", Year = 1995}
            };
        }

        [HttpGet()]
        public IActionResult Get()
        {
            return Ok(Car());
        }

        [HttpPost()]
        public IActionResult Post()
        {
            return Ok(Car());
        }

        [HttpDelete()]
        public IActionResult Delete()
        {
            return Ok(Car());
        }

        [HttpPut()]
        public IActionResult Put()
        {
            return Ok(Car());
        }
    }
}