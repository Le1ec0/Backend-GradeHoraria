using Microsoft.AspNetCore.Mvc;
using GradeHoraria.Models;
using GradeHoraria.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GradeHoraria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursoController : ControllerBase
    {
        private readonly IGradeRepository _repository;
        public CursoController(IGradeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var curso = await _repository.SearchCurso();
            return curso.Any()
            ? Ok(curso)
            : NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var curso = await _repository.SearchCurso(id);
            return curso != null
            ? Ok(curso)
            : NotFound("Curso não encontrado.");
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CursosRequestModel cursosRequestModel)
        {
            var curso = new Cursos
            {
                Nome = cursosRequestModel.Nome ?? null,
                UserId = cursosRequestModel.UserId ?? null
            };

            _repository.AddCurso(curso);
            return await _repository.SaveChangesAsync()
            ? Ok("Curso adicionado com sucesso.")
            : BadRequest("Erro ao adicionar curso.");
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CursosRequestModel cursosRequestModel)
        {
            var dbCursos = await _repository.SearchCurso(id);
            if (dbCursos == null) return NotFound("Curso não encontrado.");

            dbCursos.Nome = cursosRequestModel.Nome ?? dbCursos.Nome;
            dbCursos.UserId = cursosRequestModel.UserId ?? dbCursos.UserId;

            _repository.UpdateCurso(dbCursos);

            return await _repository.SaveChangesAsync()
            ? Ok("Curso atualizado com sucesso.")
            : BadRequest("Erro ao atualizar curso.");
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var cursos = await _repository.SearchCurso(id);
            if (cursos == null) return NotFound("Curso não encontrado.");

            _repository.DeleteCurso(cursos);

            return await _repository.SaveChangesAsync()
            ? Ok("Curso removido com sucesso.")
            : BadRequest("Erro ao remover curso.");
        }

    }
    [Route("api/[controller]")]
    public class MateriasController : ControllerBase
    {
        private readonly IGradeRepository _repository;
        public MateriasController(IGradeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var materia = await _repository.SearchMateria();
            return materia.Any()
            ? Ok(materia)
            : NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var materia = await _repository.SearchCurso(id);
            return materia != null
            ? Ok(materia)
            : NotFound("Matéria não encontrada.");
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MateriasRequestModel materiasRequestModel)
        {
            var materias = new Materias
            {
                Nome = materiasRequestModel.Nome ?? null,
                Periodo = materiasRequestModel.Periodo ?? null,
                Turno = materiasRequestModel.Turno ?? null,
                DSemana = materiasRequestModel.DSemana ?? null,
                Sala = materiasRequestModel.Sala ?? null,
                Professor = materiasRequestModel.Professor ?? null,
                CursoId = materiasRequestModel.CursoId ?? null,
                UserId = materiasRequestModel.UserId ?? null
            };
            _repository.AddMateria(materias);
            return await _repository.SaveChangesAsync()
            ? Ok("Matéria adicionada com sucesso.")
            : BadRequest("Erro ao adicionar Matéria.");
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MateriasRequestModel materiasRequestModel)
        {
            var dbMaterias = await _repository.SearchMateria(id);
            if (dbMaterias == null) return NotFound("Matéria não encontrada.");

            dbMaterias.Nome = materiasRequestModel.Nome ?? dbMaterias.Nome;
            dbMaterias.Periodo = materiasRequestModel.Periodo ?? dbMaterias.Periodo;
            dbMaterias.Turno = materiasRequestModel.Turno ?? dbMaterias.Turno;
            dbMaterias.DSemana = materiasRequestModel.DSemana ?? dbMaterias.DSemana;
            dbMaterias.Sala = materiasRequestModel.Sala ?? dbMaterias.Sala;
            dbMaterias.Professor = materiasRequestModel.Professor ?? dbMaterias.Professor;
            dbMaterias.CursoId = materiasRequestModel.CursoId != 0 ? materiasRequestModel.CursoId : dbMaterias.CursoId;

            _repository.UpdateMateria(dbMaterias);

            return await _repository.SaveChangesAsync()
            ? Ok("Matéria atualizada com sucesso.")
            : BadRequest("Erro ao atualizar Matéria.");
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var cursos = await _repository.SearchCurso(id);
            if (cursos == null) return NotFound("Matéria não encontrada.");

            _repository.DeleteCurso(cursos);

            return await _repository.SaveChangesAsync()
            ? Ok("Matéria removida com sucesso.")
            : BadRequest("Erro ao remover Matéria.");
        }

    }

    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpGet("/getall/")]
        public async Task<IActionResult> Get()
        {
            var users = await userManager.Users.ToListAsync();
            return users.Any()
                ? Ok(users)
                : NoContent();

        }

        [HttpGet("/byid/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            return user != null
                ? Ok(user)
                : NotFound("Usuário não encontrado.");
        }

        [HttpGet("/byname/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var user = await userManager.FindByNameAsync(name);
            return user != null
                ? Ok(user)
                : NotFound("Usuário não encontrado.");
        }

        [HttpPost]
        [Route("/login/")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("/register/")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("/register-admin/")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.Coordenador))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Coordenador));

            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
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