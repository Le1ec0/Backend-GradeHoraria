using GradeHoraria.Context;
using GradeHoraria.Models;
using GradeHoraria.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authorization;

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
        public AuthenticateController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration,
        IGradeRepository repository, ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _repository = repository;
            _context = context;
        }

        /*[HttpGet("/Authorize/GetAllUsers")]
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
         }*/

        [HttpGet("/Authorize/ImportAllUsers")]
        public async Task<IActionResult> ImportAllUsers()
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

            foreach (var user in users)
            {
                var newUser = new IdentityUser
                {
                    Id = user.Id,
                    UserName = user.UserPrincipalName,
                };

                // Add the new User to the context using the AddUser method
                await _userManager.CreateAsync(newUser);
                await _repository.AddUser(newUser);
            }

            // Return the created User
            return await _repository.SaveChangesAsync()
            ? Ok("Usuários importados com sucesso!")
            : BadRequest("Erro ao importar usuários.");
        }


        /*[HttpGet("/Authorize/GetUserById")]
        public async Task<IActionResult> GetUserById()
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

        [HttpGet("/Authorize/GetUserByName")]
        public async Task<IActionResult> GetUserByName()
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
        }*/

        [HttpPost]
        [Route("/Authorize/UserLogin")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var scopes = new string[] { _configuration.GetValue<string>("AzureAd:GraphPath") };

            var redirectUri = Url.Action(nameof(Login), "Authorize", null, Request.Scheme);

            var publicClient = PublicClientApplicationBuilder
            .Create(_configuration.GetValue<string>("AzureAd:ClientId"))
            .WithAuthority($"{_configuration.GetValue<string>("AzureAd:Instance")}{_configuration.GetValue<string>("AzureAd:TenantId")}/v2.0")
            .WithRedirectUri(redirectUri)
            .Build();

            // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
            var authResult = await publicClient
            .AcquireTokenByUsernamePassword(scopes, model.Username, model.Password)
            .ExecuteAsync();

            var accessToken = authResult.AccessToken;

            // Make a Microsoft Graph API query using the acquired token
            GraphServiceClient graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider(requestMessage =>
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                return Task.FromResult(0);
            }));

            // Make a Microsoft Graph API query
            var users = await graphServiceClient.Me
            .Request()
            .GetAsync();

            //foreach (var user in users)
            //{
            var newUser = new IdentityUser
            {
                Id = users.Id,
                UserName = users.UserPrincipalName,
            };

            // Add the new User to the context using the AddUser method
            await _userManager.CreateAsync(newUser);
            await _repository.AddUser(newUser);
            await _repository.SaveChangesAsync();
            //}

            // Return the access token along with the user information
            return users != null
            ? Ok(new { Token = accessToken, User = users })
            : NotFound("Usuário não encontrado.");

            /*var newUser = await _userManager.FindByIdAsync(user.Id);
            if (newUser == null)
            {
                newUser = new IdentityUser
                {
                    Id = user.Id,
                    UserName = user.DisplayName.Replace(" ", "_"),
                    Email = user.Mail ?? user.UserPrincipalName
                };

                await _userManager.CreateAsync(newUser);
                await _repository.AddUser(newUser);
            }

            // Return the access token along with the user information
            return user != null
            ? Ok(new { Token = accessToken, User = user })
            : NotFound("Usuário não encontrado.");*/
        }

        [HttpPost]
        [Route("/Authorize/RegisterUser")]
        public async Task<IActionResult> CreateUserAsync([FromBody] RegisterModel model)
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

            var newUser = new User
            {
                DisplayName = model.displayName,
                MailNickname = model.mailNickname,
                UserPrincipalName = model.userPrincipalName,
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = false,
                    Password = model.password
                },
                AccountEnabled = true
            };
            await graphServiceClient.Users.Request().AddAsync(newUser);

            return Ok();
        }

        [HttpPost]
        [Route("/Authorize/AssignRoles")]
        public async Task<IActionResult> AssignRoles([FromBody] ChangeRoleModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role == null)
            {
                return NotFound("Role não encontrada.");
            }

            await _userManager.AddToRoleAsync(user, model.RoleName);

            return Ok();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class CursoController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IGradeRepository _repository;
        private readonly ApplicationDbContext _context;
        public CursoController(IGradeRepository repository, ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _repository = repository;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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

        [Authorize(Roles = "AdminMaster, Admin")]
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

        [Authorize(Roles = "AdminMaster, Admin, Coordenador, Professor")]
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

        [Authorize(Roles = "AdminMaster")]
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IGradeRepository _repository;
        private readonly ApplicationDbContext _context;
        public MateriasController(IGradeRepository repository, ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _repository = repository;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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

        [Authorize(Roles = "AdminMaster, Admin")]
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

        [Authorize(Roles = "AdminMaster, Admin, Coordenador, Professor")]
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

        [Authorize(Roles = "AdminMaster")]
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