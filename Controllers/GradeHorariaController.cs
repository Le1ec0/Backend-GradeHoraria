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
    /*[Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IGradeRepository _repository;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticateController(RoleManager<IdentityRole> roleManager,
        IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
        {
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("/Authorize/GetAllUsers")]
        public async Task<IActionResult> Get()
        {
            var users = await _userManager.Users
            .Include(u => u.Cursos)
            .Include(u => u.Materias)
            .ToListAsync();

            return users.Any()
                ? Ok(users)
                : NoContent();
        }

        [Authorize]
        [HttpGet("/Authorize/GetLoggedUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized("Usuário não logado.");
            }
            return Ok(user.DisplayName);
        }

        [HttpGet("/Authorize/GetUserById")]
        public async Task<IActionResult> GetById(string id = null)
        {
            id = Request.Query["id"];
            if (id == null)
            {
                return NotFound("Usuário não informado.");
            }
            //id = Request.Query["id"];
            var users = await _context.Users
            .Include(u => u.Cursos)
            .Include(u => u.Materias)
            .FirstOrDefaultAsync(u => u.Id == id);

            return users != null
            ? Ok(users)
            : NotFound("Usuário não encontrado.");
        }

        [HttpGet("/Authorize/GetUserByName")]
        public async Task<IActionResult> GetByName(string name = null)
        {
            name = Request.Query["name"];
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
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "AdminMaster already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "AdminMaster creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.AdminMaster))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.AdminMaster));

            if (await _roleManager.RoleExistsAsync(UserRoles.AdminMaster))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.AdminMaster);
            }

            return Ok(new Response { Status = "Success", Message = "AdminMaster created successfully!" });
        }

        [HttpPost]
        [Route("/Authorize/RegisterAdmin/")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Admin already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Admin creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new Response { Status = "Success", Message = "Admin created successfully!" });
        }

        [HttpPost]
        [Route("/Authorize/RegisterCoordenador")]
        public async Task<IActionResult> RegisterCoordenador([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Coordenador already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Coordenador creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Coordenador))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Coordenador));

            if (await _roleManager.RoleExistsAsync(UserRoles.Coordenador))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Coordenador);
            }
            return Ok(new Response { Status = "Success", Message = "Coordenador created successfully!" });
        }

        [HttpPost]
        [Route("/Authorize/RegisterProfessor")]
        public async Task<IActionResult> RegisterProfessor([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Professor already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Professor creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Professor))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Professor));

            if (await _roleManager.RoleExistsAsync(UserRoles.Professor))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Professor);
            }
            return Ok(new Response { Status = "Success", Message = "Professor created successfully!" });
        }

        [HttpPost]
        [Route("/Authorize/RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Usuario))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Usuario));

            if (await _roleManager.RoleExistsAsync(UserRoles.Usuario))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Usuario);
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
    }*/

    [ApiController]
    [Route("api/[controller]")]
    public class CursoController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IGradeRepository _repository;
        private readonly ApplicationDbContext _context;
        public CursoController(IGradeRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        [HttpGet("/Cursos/GetAllCursos")]
        public async Task<IActionResult> Get()
        {
            var curso = await _context.Cursos
            .ToListAsync();

            return curso.Any()
            ? Ok(curso)
            : NoContent();
        }

        [HttpGet("/Cursos/GetCursoById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var curso = await _context.Cursos
            .ToListAsync();

            return curso != null
            ? Ok(curso)
            : NotFound("Curso não encontrado.");
        }

        //[Authorize(Roles = "AdminMaster, Admin")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("/Cursos/PostCurso")]
        public async Task<IActionResult> Post([FromBody] CursosRequestModel cursosRequestModel)
        {
            var curso = new Curso
            {
                Nome = cursosRequestModel.Nome ?? null,
                Turno = cursosRequestModel.Turno ?? null,
                Sala = cursosRequestModel.Sala ?? null,
                Professor = cursosRequestModel.Professor ?? null,
            };

            var cursoPeriodos = new List<CursoPeriodo>();
            foreach (var periodoId in cursosRequestModel.Periodo_Id)
            {
                cursoPeriodos.Add(new CursoPeriodo
                {
                    Curso = curso,
                    Periodo_Id = periodoId
                });
            }

            curso.CursoPeriodos = cursoPeriodos;
            _repository.AddCurso(curso);
            return await _repository.SaveChangesAsync()
            ? Ok("Curso adicionado com sucesso.")
            : BadRequest("Erro ao adicionar curso.");
        }

        //[Authorize(Roles = "AdminMaster, Admin, Coordenador, Professor")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("/Cursos/PutCursoById/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CursosRequestModel cursosRequestModel)
        {
            var dbCursos = await _repository.SearchCurso(id);
            if (dbCursos == null) return NotFound("Curso não encontrado.");

            dbCursos.Nome = cursosRequestModel.Nome ?? dbCursos.Nome;
            //dbCursos.Id = cursosRequestModel.Id ?? dbCursos.Id;

            _repository.UpdateCurso(dbCursos);

            return await _repository.SaveChangesAsync()
            ? Ok("Curso atualizado com sucesso.")
            : BadRequest("Erro ao atualizar curso.");
        }

        //[Authorize(Roles = "AdminMaster")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("/Cursos/DeleteCursoById/{id}")]
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
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IGradeRepository _repository;
        private readonly ApplicationDbContext _context;
        public MateriasController(IGradeRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        [HttpGet("/Materias/GetAllMaterias")]
        public async Task<IActionResult> Get()
        {
            var materia = await _context.Materias
            .ToListAsync();

            return materia.Any()
            ? Ok(materia)
            : NoContent();
        }

        [HttpGet("/Materias/GetMateriasById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var materia = await _context.Materias
            .ToListAsync();

            return materia != null
            ? Ok(materia)
            : NotFound("Matéria não encontrada.");
        }

        //[Authorize(Roles = "AdminMaster, Admin")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("/Materias/PostMaterias")]
        public async Task<IActionResult> Post([FromBody] MateriasRequestModel materiasRequestModel)
        {
            var materias = new Materia
            {
                //Id = materiasRequestModel.Id ?? null,
                Nome = materiasRequestModel.Nome ?? null,
                DSemana = materiasRequestModel.DSemana ?? null,
                Professor = materiasRequestModel.Professor ?? null,
                Periodo_Id = materiasRequestModel.Periodo_Id ?? null
            };
            _repository.AddMateria(materias);
            return await _repository.SaveChangesAsync()
            ? Ok("Matéria adicionada com sucesso.")
            : BadRequest("Erro ao adicionar Matéria.");
        }

        //[Authorize(Roles = "AdminMaster, Admin, Coordenador, Professor")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("/Materias/PutMateriasById/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MateriasRequestModel materiasRequestModel)
        {
            var dbMaterias = await _repository.SearchMateria(id);
            if (dbMaterias == null) return NotFound("Matéria não encontrada.");

            dbMaterias.Nome = materiasRequestModel.Nome ?? dbMaterias.Nome;
            dbMaterias.DSemana = materiasRequestModel.DSemana ?? dbMaterias.DSemana;
            dbMaterias.Professor = materiasRequestModel.Professor ?? dbMaterias.Professor;
            dbMaterias.Periodo_Id = materiasRequestModel.Periodo_Id ?? dbMaterias.Periodo_Id;

            _repository.UpdateMateria(dbMaterias);

            return await _repository.SaveChangesAsync()
            ? Ok("Matéria atualizada com sucesso.")
            : BadRequest("Erro ao atualizar Matéria.");
        }

        //[Authorize(Roles = "AdminMaster")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("/Materias/DeleteMateriasById/{id}/")]
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
}