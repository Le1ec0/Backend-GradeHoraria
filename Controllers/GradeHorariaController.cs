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

        [Authorize(Roles = "AdminMaster, Admin")]
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

        [Authorize(Roles = "AdminMaster, Admin, Coordenador, Professor")]
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

        [Authorize(Roles = "AdminMaster")]
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

        [Authorize(Roles = "AdminMaster, Admin")]
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

        [Authorize(Roles = "AdminMaster, Admin, Coordenador, Professor")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("/PutMateriasById/{id}/")]
        public async Task<IActionResult> Put(int id, [FromBody] MateriasRequestModel materiasRequestModel)
        {
            var dbMaterias = await _repository.SearchMateria(id);
            if (dbMaterias == null) return NotFound("Matéria não encontrada.");

            dbMaterias.Nome = materiasRequestModel.Nome ?? dbMaterias.Nome;
            dbMaterias.Periodo = materiasRequestModel.Periodo ?? dbMaterias.Periodo;
            dbMaterias.Turno = materiasRequestModel.Turno ?? dbMaterias.Turno;
            dbMaterias.DSemana = materiasRequestModel.DSemana != 0 ? materiasRequestModel.DSemana : dbMaterias.DSemana;
            dbMaterias.Sala = materiasRequestModel.Sala ?? dbMaterias.Sala;
            dbMaterias.Professor = materiasRequestModel.Professor ?? dbMaterias.Professor;
            dbMaterias.CursoId = materiasRequestModel.CursoId != 0 ? materiasRequestModel.CursoId : dbMaterias.CursoId;

            _repository.UpdateMateria(dbMaterias);

            return await _repository.SaveChangesAsync()
            ? Ok("Matéria atualizada com sucesso.")
            : BadRequest("Erro ao atualizar Matéria.");
        }

        [Authorize(Roles = "AdminMaster")]
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("/Authorize/GetAllUsers")]
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

        [Authorize(Roles = "AdminMaster, Admin, Coordenador, Professor, Usuario")]
        [HttpGet("/Authorize/GetLoggedUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user.UserName);
        }

        [HttpGet("/Authorize/GetUserById")]
        public async Task<IActionResult> GetById(string id = null)
        {
            if (id == null)
            {
                return NotFound("Usuário não informado.");
            }
            //id = Request.Query["id"];
            var users = await _context.Users
            .Include(u => u.Cursos)
            .Include(u => u.Materias)
            .FirstOrDefaultAsync(u => u.UserName == id);

            return users != null
            ? Ok(users)
            : NotFound("Usuário não encontrado.");
        }

        [HttpGet("/Authorize/GetUserByName")]
        public async Task<IActionResult> GetByName(string name = null)
        {
            if (name == null)
            {
                return NotFound("Usuário não informado.");
            }
            //name = Request.Query["name"];
            var users = await _context.Users
            .Include(u => u.Cursos)
            .Include(u => u.Materias)
            .FirstOrDefaultAsync(u => u.UserName == name);

            return users != null
            ? Ok(users)
            : NotFound("Usuário não encontrado.");
        }

        [HttpPost]
        [Route("/Authorize/UserLogin")]
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
        [Route("/Authorize/RegisterAdminMaster/")]
        public async Task<IActionResult> RegisterAdminMaster([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "AdminMaster already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "AdminMaster creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(UserRoles.AdminMaster))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.AdminMaster));

            if (await roleManager.RoleExistsAsync(UserRoles.AdminMaster))
            {
                await userManager.AddToRoleAsync(user, UserRoles.AdminMaster);
            }

            return Ok(new Response { Status = "Success", Message = "AdminMaster created successfully!" });
        }

        [HttpPost]
        [Route("/Authorize/RegisterAdmin/")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Admin already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Admin creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new Response { Status = "Success", Message = "Admin created successfully!" });
        }

        [HttpPost]
        [Route("/Authorize/RegisterCoordenador")]
        public async Task<IActionResult> RegisterCoordenador([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Coordenador already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Coordenador creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(UserRoles.Coordenador))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Coordenador));

            if (await roleManager.RoleExistsAsync(UserRoles.Coordenador))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Coordenador);
            }
            return Ok(new Response { Status = "Success", Message = "Coordenador created successfully!" });
        }

        [HttpPost]
        [Route("/Authorize/RegisterProfessor")]
        public async Task<IActionResult> RegisterProfessor([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Professor already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Professor creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(UserRoles.Professor))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Professor));

            if (await roleManager.RoleExistsAsync(UserRoles.Professor))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Professor);
            }
            return Ok(new Response { Status = "Success", Message = "Professor created successfully!" });
        }

        [HttpPost]
        [Route("/Authorize/RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel model)
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