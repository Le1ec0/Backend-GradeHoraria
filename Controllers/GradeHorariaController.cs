using GradeHoraria.Context;
using GradeHoraria.Models;
using GradeHoraria.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Identity.Client;

namespace GradeHoraria.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGradeRepository _repository;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticateController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration,
        IGradeRepository repository, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor,
        IServiceProvider serviceProvider)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _repository = repository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        private async Task<string> GetAppTokenAsync()
        {
            var clientId = _configuration.GetValue<string>("AzureAd:ClientId");
            var clientSecret = _configuration.GetValue<string>("AzureAd:ClientSecret");
            var tenantId = _configuration.GetValue<string>("AzureAd:TenantId");
            var graphPath = _configuration.GetValue<string>("AzureAd:GraphPath");

            var context = new AuthenticationContext($"{_configuration.GetValue<string>("AzureAd:Instance")}{_configuration.GetValue<string>("AzureAd:TenantId")}");
            var result = await context.AcquireTokenAsync(graphPath, new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential(clientId, clientSecret));

            return result.AccessToken;
        }

        [HttpGet("/Authorize/GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var scopes = new string[] { _configuration.GetValue<string>("AzureAd:GraphPath") };

            var confidentialClient = ConfidentialClientApplicationBuilder
            .Create(_configuration.GetValue<string>("AzureAd:ClientId"))
            .WithAuthority($"{_configuration.GetValue<string>("AzureAd:Instance")}{_configuration.GetValue<string>("AzureAd:TenantId")}")
            .WithClientSecret(_configuration.GetValue<string>("AzureAd:ClientSecret"))
            .Build();

            GraphServiceClient graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) =>
            {

                // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
                var authResult = await confidentialClient.AcquireTokenForClient(scopes)
                .ExecuteAsync();

                // Add the access token in the Authorization header of the API
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);

            }));

            // Make a Microsoft Graph API query
            var users = await graphServiceClient.Users
            .Request()
            .GetAsync();
            return Ok(users);
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
            return Ok(user.UserName);
        }

        [HttpGet("/Authorize/GetUserById")]
        public async Task<IActionResult> GetById(string id = null)
        {
            id = Request.Query["id"];
            if (id == null)
            {
                return NotFound("Usuário não informado.");
            }
            var user = await _userManager.FindByIdAsync(id);

            return user != null
                ? Ok(user)
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
            var user = await _userManager.FindByNameAsync(name);

            return user != null
            ? Ok(user)
            : NotFound("Usuário não encontrado.");
        }

        [HttpPost]
        [Route("/Authorize/UserLogin")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Acquire the access token.
            var scopes = new string[] { _configuration.GetValue<string>("AzureAd:GraphPath") };

            var redirectUri = Url.Action(nameof(Login), "Authorize", null, Request.Scheme);

            var app = PublicClientApplicationBuilder
            .Create(_configuration.GetValue<string>("AzureAd:ClientId"))
            .WithAuthority($"{_configuration.GetValue<string>("AzureAd:Instance")}{_configuration.GetValue<string>("AzureAd:TenantId")}/v2.0")
            .WithRedirectUri(redirectUri)
            .Build();

            var result = await app
            .AcquireTokenByUsernamePassword(scopes, model.Username, model.Password)
            .ExecuteAsync();

            var accessToken = result.AccessToken;

            // Do something with the access token, such as calling Microsoft Graph API
            return Ok(accessToken);
        }

        [HttpPost]
        [Route("/Authorize/RegisterAdminMaster/")]
        public async Task<IActionResult> RegisterAdminMaster([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.displayName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "AdminMaster already exists!" });

            //var result = await _userManager.CreateAsync(user, model.Password);
            //if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "AdminMaster creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.AdminMaster))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.AdminMaster));

            if (await _roleManager.RoleExistsAsync(UserRoles.AdminMaster))
            {
                //await _userManager.AddToRoleAsync(user, UserRoles.AdminMaster);
            }

            return Ok(new Response { Status = "Success", Message = "AdminMaster created successfully!" });
        }

        [HttpPost]
        [Route("/Authorize/RegisterAdmin/")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.displayName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Admin already exists!" });

            //var result = await _userManager.CreateAsync(user, model.Password);
            //if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Admin creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                //await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new Response { Status = "Success", Message = "Admin created successfully!" });
        }

        [HttpPost]
        [Route("/Authorize/RegisterCoordenador")]
        public async Task<IActionResult> RegisterCoordenador([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.displayName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Coordenador already exists!" });

            //var result = await _userManager.CreateAsync(user, model.Password);
            //if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Coordenador creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Coordenador))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Coordenador));

            if (await _roleManager.RoleExistsAsync(UserRoles.Coordenador))
            {
                //await _userManager.AddToRoleAsync(user, UserRoles.Coordenador);
            }
            return Ok(new Response { Status = "Success", Message = "Coordenador created successfully!" });
        }

        [HttpPost]
        [Route("/Authorize/RegisterProfessor")]
        public async Task<IActionResult> RegisterProfessor([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.displayName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Professor already exists!" });

            //var result = await _userManager.CreateAsync(user, model.Password);
            //if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Professor creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Professor))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Professor));

            if (await _roleManager.RoleExistsAsync(UserRoles.Professor))
            {
                //await _userManager.AddToRoleAsync(user, UserRoles.Professor);
            }
            return Ok(new Response { Status = "Success", Message = "Professor created successfully!" });
        }

        [HttpPost]
        [Route("/Authorize/RegisterUser")]
        public async Task<IActionResult> CreateUserAsync([FromBody] RegisterModel model)
        {
            AuthenticateController authController = new AuthenticateController(
                _userManager, _roleManager, _configuration, _repository, _context, _httpContextAccessor, _serviceProvider
                );

            var graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        var accessToken = await authController.GetAppTokenAsync();
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                    }));

            var newUser = new User
            {
                DisplayName = model.displayName,
                MailNickname = model.mailNickname,
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = false,
                    Password = model.password
                },
                AccountEnabled = true
            };
            await graphClient.Users.Request().AddAsync(newUser);

            return Ok();
        }
    }

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
            .Include(m => m.Materias)
            .ToListAsync();

            return curso.Any()
            ? Ok(curso)
            : NoContent();
        }

        [HttpGet("/Cursos/GetCursoById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var curso = await _context.Cursos
            .Include(m => m.Materias)
            .ToListAsync();

            return curso != null
            ? Ok(curso)
            : NotFound("Curso não encontrado.");
        }

        [HttpGet("/Cursos/GetCursoByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var curso = await _context.Cursos
            .Include(m => m.Materias)
            .ToListAsync();

            return curso != null
            ? Ok(curso)
            : NotFound("Curso não encontrado.");
        }

        //[Authorize(Roles = "AdminMaster, Admin")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("/Cursos/PostCursos")]
        public async Task<IActionResult> Post([FromBody] CursosRequestModel request)
        {
            // Create new Curso object and set its properties
            var curso = new Curso
            {
                Nome = request.Nome,
                Turno = request.Turno,
                Sala = request.Sala,
                Professor = request.Professor,
                Periodo = request.Periodo,
                Periodos = new List<Periodo>()
            };

            // Add the new Curso to the context using the AddCurso method
            _repository.AddCurso(curso);

            // Create new Periodo object and set its properties
            var periodo = new Periodo
            {
                PeriodoId = request.Periodo,
                CursoId = curso.Id
            };

            curso.Periodos.Add(periodo);

            // Add the new Curso to the context
            _context.Cursos.Add(curso);

            // Add the new Periodo to the context
            _context.Periodos.Add(periodo);

            return await _repository.SaveChangesAsync()
            ? Ok("Curso criado com sucesso!")
            : BadRequest("Erro ao criar curso.");
        }

        //[Authorize(Roles = "AdminMaster, Admin, Coordenador, Professor")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("/Cursos/PutCursoById/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CursosRequestModel cursosRequestModel)
        {
            var dbCursos = await _repository.GetCurso(id);
            if (dbCursos == null) return NotFound("Curso não encontrado.");

            dbCursos.Sala = cursosRequestModel.Sala ?? dbCursos.Sala;
            dbCursos.Professor = cursosRequestModel.Professor ?? dbCursos.Professor;

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
            var cursos = await _repository.GetCurso(id);
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
            .Include(m => m.Cursos)
            .ToListAsync();

            return materia.Any()
            ? Ok(materia)
            : NoContent();
        }

        [HttpGet("/Materias/GetMateriasById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var materia = await _context.Materias
            .Include(m => m.Cursos)
            .ToListAsync();

            return materia != null
            ? Ok(materia)
            : NotFound("Matéria não encontrada.");
        }

        [HttpGet("/Materias/GetMateriasByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var materia = await _context.Materias
            .Include(m => m.Cursos)
            .ToListAsync();

            return materia != null
            ? Ok(materia)
            : NotFound("Matéria não encontrada.");
        }

        //[Authorize(Roles = "AdminMaster, Admin")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("/Materias/PostMateria")]
        public async Task<IActionResult> Post([FromBody] MateriasRequestModel request)
        {
            // Create new Materia object and set its properties
            var materia = new Materia
            {
                Nome = request.Nome,
                DSemana = request.DSemana,
                Professor = request.Professor
            };

            materia.CursoId = request.CursoId;
            materia.PeriodoId = request.PeriodoId;

            materia.Cursos = _context.Cursos.Find(request.CursoId);
            materia.Periodos = _context.Periodos.Find(request.PeriodoId);

            // Add the new Materia to the context using the AddMateria method
            _repository.AddMateria(materia);

            // Return the created Materia
            return await _repository.SaveChangesAsync()
            ? Ok("Matéria criada com sucesso!")
            : BadRequest("Erro ao criar Matéria.");
        }

        //[Authorize(Roles = "AdminMaster, Admin, Coordenador, Professor")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("/Materias/PutMateriasById/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MateriasRequestModel materiasRequestModel)
        {
            var dbMaterias = await _repository.GetMateria(id);
            if (dbMaterias == null) return NotFound("Matéria não encontrada.");

            dbMaterias.Nome = materiasRequestModel.Nome ?? dbMaterias.Nome;
            dbMaterias.DSemana = materiasRequestModel.DSemana ?? dbMaterias.DSemana;
            dbMaterias.Professor = materiasRequestModel.Professor ?? dbMaterias.Professor;

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
            var materias = await _repository.GetMateria(id);
            if (materias == null) return NotFound("Matéria não encontrada.");

            _repository.DeleteMateria(materias);

            return await _repository.SaveChangesAsync()
            ? Ok("Matéria removida com sucesso.")
            : BadRequest("Erro ao remover Matéria.");
        }
    }
}