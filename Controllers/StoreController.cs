using Microsoft.AspNetCore.Mvc;
using Backend_CarStore.Models;
using Backend_CarStore.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(Cars cars)
        {
            _repository.AddCar(cars);

            return await _repository.SaveChangesAsync()
            ? Ok("Carro adicionado com sucesso")
            : BadRequest("Erro ao adicionar carro");
        }

        [Authorize]
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

        [Authorize]
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

            dbUser.Username = users.Username ?? dbUser.Username;
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
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Users model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] Users model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}