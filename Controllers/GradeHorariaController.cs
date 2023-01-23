using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using GradeHoraria.Models;
using GradeHoraria.Repositories;
using GradeHoraria.Context;
using System.Security.Claims;
using System.Text;

namespace GradeHoraria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursoController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IGradeRepository _repository;
        private readonly ApplicationDbContext _context;
        public CursoController(IGradeRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        [HttpGet("/GetAllCursos/")]
        public async Task<IActionResult> Get()
        {
            var curso = await _context.Cursos
            .Include(u => u.Materias)
            .Include(u => u.ApplicationUser)
            .ToListAsync();

            return curso.Any()
            ? Ok(curso)
            : NoContent();
        }

        [HttpGet("/GetCursoById/{id}/")]
        public async Task<IActionResult> GetById(int id)
        {
            var curso = await _context.Cursos
            .Include(u => u.Materias)
            .Include(u => u.ApplicationUser)
            .ToListAsync();

            return curso != null
            ? Ok(curso)
            : NotFound("Curso não encontrado.");
        }

        [Authorize(Roles = "Admin, Coordenador, Professor")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("/PostCurso/")]
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

        [Authorize(Roles = "Admin, Coordenador, Professor")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("/PutCursoById/{id}/")]
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

        [Authorize(Roles = "Admin, Coordenador, Professor")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("/DeleteCursoById/{id}/")]
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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IGradeRepository _repository;
        private readonly ApplicationDbContext _context;
        public MateriasController(IGradeRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        [HttpGet("/GetAllMaterias/")]
        public async Task<IActionResult> Get()
        {
            var materia = await _context.Materias
            .Include(u => u.Cursos)
            .Include(u => u.ApplicationUser)
            .ToListAsync();

            return materia.Any()
            ? Ok(materia)
            : NoContent();
        }

        [HttpGet("/GetMateriasById/{id}/")]
        public async Task<IActionResult> GetById(int id)
        {
            var materia = await _context.Materias
            .Include(u => u.Cursos)
            .Include(u => u.ApplicationUser)
            .ToListAsync();

            return materia != null
            ? Ok(materia)
            : NotFound("Matéria não encontrada.");
        }

        [Authorize(Roles = "Admin, Coordenador, Professor")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("/PostMaterias/")]
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

        [Authorize(Roles = "Admin, Coordenador, Professor")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("/PutMateriasById/{id}/")]
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

        [Authorize(Roles = "Admin, Coordenador, Professor")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("/DeleteMateriasById/{id}/")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var materias = await _repository.SearchMateria(id);
            if (materias == null) return NotFound("Matéria não encontrada.");

            _repository.DeleteMateria(materias);

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
        private readonly IGradeRepository _repository;
        private readonly ApplicationDbContext _context;
        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("/GetAllUsers/")]
        public async Task<IActionResult> Get()
        {
            var users = await userManager.Users
            .Include(u => u.Cursos)
            .Include(u => u.Materias)
            .ToListAsync();

            return users.Any()
                ? Ok(users)
                : NoContent();
        }

        [HttpGet("/GetLoggedUser/")]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.UserName);
        }

        [HttpGet("/GetUserById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var users = await _context.Users
            .Include(u => u.Cursos)
            .Include(u => u.Materias)
            .FirstOrDefaultAsync(u => u.Id == id);

            return users != null
                ? Ok(users)
                : NotFound("Usuário não encontrado.");
        }

        [HttpGet("/GetUserByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var users = await _context.Users
            .Include(u => u.Cursos)
            .Include(u => u.Materias)
            .FirstOrDefaultAsync(u => u.UserName == name);

            return users != null
                ? Ok(users)
                : NotFound("Usuário não encontrado.");
        }

        [HttpPost]
        [Route("/UserLogin/")]
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
        [Route("/RegisterUser/")]
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

            if (!await roleManager.RoleExistsAsync(UserRoles.Usuario))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Usuario));

            if (await roleManager.RoleExistsAsync(UserRoles.Usuario))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Usuario);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("/RegisterAdminUser/")]
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