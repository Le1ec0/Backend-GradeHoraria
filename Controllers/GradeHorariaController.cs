using GradeHoraria.Context;
using GradeHoraria.Models;
using GradeHoraria.Repositories;
using GradeHoraria.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace GradeHoraria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGradeRepository _repository;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration,
        IGradeRepository repository, ApplicationDbContext context, IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _repository = repository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /*[HttpGet("GetAllUsers")]
         public async Task<IActionResult> GetAllUsers()
         {
             var scopes = new string[] { _configuration.GetValue<string>("AzureAD:Scope") };

             var confidentialClient = ConfidentialClientApplicationBuilder
             .Create(_configuration.GetValue<string>("AzureAD:ClientId"))
             .WithAuthority($"{_configuration.GetValue<string>("AzureAD:Instance")}{_configuration.GetValue<string>("AzureAD:TenantId")}")
             .WithClientSecret(_configuration.GetValue<string>("AzureAD:ClientSecret"))
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

        /*[HttpGet("ImportAllUsers")]
        public async Task<IActionResult> ImportAllUsers()
        {
            var scopes = new string[] { _configuration.GetValue<string>("AzureAD:Scope") };

            var confidentialClient = ConfidentialClientApplicationBuilder
            .Create(_configuration.GetValue<string>("AzureAD:ClientId"))
            .WithAuthority($"{_configuration.GetValue<string>("AzureAD:Instance")}{_configuration.GetValue<string>("AzureAD:TenantId")}")
            .WithClientSecret(_configuration.GetValue<string>("AzureAD:ClientSecret"))
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
                var newUser = new ApplicationUser
                {
                    Id = user.Id,
                    UserName = user.DisplayName,
                    Email = user.Mail ?? user.UserPrincipalName
                };

                // Add the new User to the context using the AddUser method
                await _userManager.CreateAsync(newUser);
                await _repository.AddUser(newUser);
            }

            // Return the created User
            return await _repository.SaveChangesAsync()
            ? Ok("Usuários importados com sucesso!")
            : BadRequest("Erro ao importar usuários.");
        }*/


        /*[HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById()
        {
            var scopes = new string[] { _configuration.GetValue<string>("AzureAD:Scope") };

            var confidentialClient = ConfidentialClientApplicationBuilder
            .Create(_configuration.GetValue<string>("AzureAD:ClientId"))
            .WithAuthority($"{_configuration.GetValue<string>("AzureAD:Instance")}{_configuration.GetValue<string>("AzureAD:TenantId")}")
            .WithClientSecret(_configuration.GetValue<string>("AzureAD:ClientSecret"))
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
        [Route("GetLoggedUser")]
        public async Task<IActionResult> GetLoggedUser()
        {
            var name = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var preferredUsername = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;

            var userClaims = new { Name = name, PreferredUsername = preferredUsername };

            return Ok(userClaims);
        }

        [HttpGet("GetUserByName")]
        public async Task<IActionResult> GetUserByName(string UserName)
        {
            var user = await _context.Users
            .Where(u => u.UserName.Contains(UserName))
            .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(string UserEmail)
        {
            var user = await _context.Users
            .Where(u => u.Email.Contains(UserEmail))
            .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("Nenhum usuário encontrado com esse e-mail.");
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("UserLogin")]
        public async Task<IActionResult> UserLogin([FromBody] LoginModel model)
        {
            var scopes = new string[] { _configuration.GetValue<string>("AzureAD:Scope") };

            var redirectUri = Url.Action(nameof(UserLogin), "User", null, Request.Scheme);

            var publicClient = PublicClientApplicationBuilder
            .Create(_configuration.GetValue<string>("AzureAD:ClientId"))
            .WithAuthority($"{_configuration.GetValue<string>("AzureAD:Instance")}{_configuration.GetValue<string>("AzureAD:TenantId")}/v2.0")
            .WithRedirectUri(redirectUri)
            .Build();

            // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
            var authResult = await publicClient
            .AcquireTokenByUsernamePassword(scopes, model.Username, model.Password)
            .ExecuteAsync();

            var idToken = authResult.IdToken;
            var accessToken = authResult.AccessToken;

            // Make a Microsoft Graph API query using the acquired token
            GraphServiceClient graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider(requestMessage =>
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);

                return Task.FromResult(0);
            }));

            // Make a Microsoft Graph API query
            var user = await graphServiceClient.Me
            .Request()
            .GetAsync();

            var newUser = await _userManager.FindByNameAsync(user.DisplayName);
            if (newUser == null)
            {
                newUser = new ApplicationUser
                {
                    Id = user.Id,
                    UserName = user.DisplayName,
                    Email = user.Mail ?? user.UserPrincipalName,
                    NormalizedUserName = user.DisplayName.ToUpperInvariant(),
                    NormalizedEmail = (user.Mail ?? user.UserPrincipalName).ToUpperInvariant(),
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                await _userManager.CreateAsync(newUser);

                var userRoles = await _userManager.GetRolesAsync(newUser);

                if (userRoles.Count == 0)
                {
                    // If the user doesn't have any roles, add them to the default user role
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Usuario);
                }

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.DisplayName),
                    new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                // Add the new User to the context using the AddUser method
                await _repository.AddUser((ApplicationUser)newUser);
                await _repository.SaveChangesAsync();

            }

            return user != null
            ? Ok(new
            {
                Token = idToken,
                User = user
            })
            : NotFound("Usuário não encontrado.");
        }

        [HttpPost]
        [Route("RegisterUserAAD")]
        public async Task<IActionResult> CreateUserAsync([FromBody] RegisterModel model)
        {
            var scopes = new string[] { _configuration.GetValue<string>("AzureAD:Scope") };

            var confidentialClient = ConfidentialClientApplicationBuilder
            .Create(_configuration.GetValue<string>("AzureAD:ClientId"))
            .WithAuthority($"{_configuration.GetValue<string>("AzureAD:Instance")}{_configuration.GetValue<string>("AzureAD:TenantId")}")
            .WithClientSecret(_configuration.GetValue<string>("AzureAD:ClientSecret"))
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
        [Route("CreateAdminUser")]
        public async Task<IActionResult> CreateAdminUser()
        {
            var adminmaster = new ApplicationUser
            {
                UserName = _configuration["AdminMaster:UserName"],
                NormalizedUserName = _configuration["AdminMaster:UserName"].ToUpperInvariant(),
                Email = _configuration["AdminMaster:UserEmail"],
                NormalizedEmail = _configuration["AdminMaster:UserEmail"].ToUpperInvariant(),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            /*var result = await _userManager.CreateAsync(adminmaster, _configuration["AdminMaster:UserPassword"]);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminmaster, UserRoles.AdminMaster);
                await _repository.AddUser((ApplicationUser)adminmaster);
                await _repository.SaveChangesAsync();
            }*/

            await _userManager.CreateAsync(adminmaster, _configuration["AdminMaster:UserPassword"]);
            await _userManager.AddToRoleAsync(adminmaster, UserRoles.AdminMaster);
            await _repository.AddUser((ApplicationUser)adminmaster);
            await _repository.SaveChangesAsync();

            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("AssignRoles")]
        public async Task<IActionResult> AssignRoles([FromBody] ChangeRoleModel model)
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var user = await _userManager.FindByNameAsync(userName);

            var loggedInUserRoles = await _userManager.GetRolesAsync(user);
            if (!loggedInUserRoles.Contains(UserRoles.AdminMaster))
            {
                return Forbid("Usuário não é AdminMaster.");
            }

            var userToUpdate = await _userManager.FindByEmailAsync(model.Email);
            if (userToUpdate == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            var currentRoles = await _userManager.GetRolesAsync(userToUpdate);
            await _userManager.RemoveFromRolesAsync(userToUpdate, currentRoles);

            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role == null)
            {
                return NotFound("Role não encontrada.");
            }

            await _userManager.AddToRoleAsync(userToUpdate, model.RoleName);
            await _repository.SaveChangesAsync();

            return Ok();
        }
    }

    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IGradeRepository _repository;
        private readonly ApplicationDbContext _context;
        public CursosController(IGradeRepository repository, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _repository = repository;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("GetAllCursos")]
        public async Task<IActionResult> Get()
        {
            var curso = await _context.Cursos
            .Include(m => m.Materias)
            .ToListAsync();

            return curso.Any()
            ? Ok(curso)
            : NoContent();
        }

        [HttpGet("GetCursoById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var curso = await _context.Cursos
            .Include(m => m.Materias)
            .ToListAsync();

            return curso != null
            ? Ok(curso)
            : NotFound("Curso não encontrado.");
        }

        [HttpGet("GetCursoByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var curso = await _context.Cursos
            .Include(m => m.Materias)
            .ToListAsync();

            return curso != null
            ? Ok(curso)
            : NotFound("Curso não encontrado.");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("PostCursos")]
        public async Task<IActionResult> Post([FromBody] CursosRequestModel request)
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var user = await _userManager.FindByNameAsync(userName);

            var loggedInUserRoles = await _userManager.GetRolesAsync(user);
            if (!loggedInUserRoles.Contains(UserRoles.AdminMaster) || !loggedInUserRoles.Contains(UserRoles.Admin))
            {
                return Forbid("Usuário não é AdminMaster/Admin.");
            }
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("PutCursoById/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CursosRequestModel cursosRequestModel)
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var user = await _userManager.FindByNameAsync(userName);

            var loggedInUserRoles = await _userManager.GetRolesAsync(user);
            if (!loggedInUserRoles.Contains(UserRoles.AdminMaster) || !loggedInUserRoles.Contains(UserRoles.Admin) || !loggedInUserRoles.Contains(UserRoles.Coordenador))
            {
                return Forbid("Usuário não é AdminMaster/Admin/Coordenador.");
            }
            var dbCursos = await _repository.GetCurso(id);
            if (dbCursos == null) return NotFound("Curso não encontrado.");

            dbCursos.Sala = cursosRequestModel.Sala ?? dbCursos.Sala;
            dbCursos.Professor = cursosRequestModel.Professor ?? dbCursos.Professor;

            _repository.UpdateCurso(dbCursos);

            return await _repository.SaveChangesAsync()
            ? Ok("Curso atualizado com sucesso.")
            : BadRequest("Erro ao atualizar curso.");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("DeleteCursoById/{id}")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var user = await _userManager.FindByNameAsync(userName);

            var loggedInUserRoles = await _userManager.GetRolesAsync(user);
            if (!loggedInUserRoles.Contains(UserRoles.AdminMaster))
            {
                return Forbid("Usuário não é Admin.");
            }
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IGradeRepository _repository;
        private readonly ApplicationDbContext _context;
        public MateriasController(IGradeRepository repository, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _repository = repository;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("GetAllMaterias")]
        public async Task<IActionResult> Get()
        {
            var materia = await _context.Materias
            .Include(m => m.Cursos)
            .ToListAsync();

            return materia.Any()
            ? Ok(materia)
            : NoContent();
        }

        [HttpGet("GetMateriasById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var materia = await _context.Materias
            .Include(m => m.Cursos)
            .ToListAsync();

            return materia != null
            ? Ok(materia)
            : NotFound("Matéria não encontrada.");
        }

        [HttpGet("GetMateriasByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var materia = await _context.Materias
            .Include(m => m.Cursos)
            .ToListAsync();

            return materia != null
            ? Ok(materia)
            : NotFound("Matéria não encontrada.");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("PostMateria")]
        public async Task<IActionResult> Post([FromBody] MateriasRequestModel request)
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var user = await _userManager.FindByNameAsync(userName);

            var loggedInUserRoles = await _userManager.GetRolesAsync(user);
            if (!loggedInUserRoles.Contains(UserRoles.AdminMaster) || !loggedInUserRoles.Contains(UserRoles.Admin))
            {
                return Forbid("Usuário não é AdminMaster/Admin.");
            }
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("PutMateriasById/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MateriasRequestModel materiasRequestModel)
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var user = await _userManager.FindByNameAsync(userName);

            var loggedInUserRoles = await _userManager.GetRolesAsync(user);
            if (!loggedInUserRoles.Contains(UserRoles.AdminMaster) || !loggedInUserRoles.Contains(UserRoles.Admin) || !loggedInUserRoles.Contains(UserRoles.Coordenador))
            {
                return Forbid("Usuário não é AdminMaster/Admin/Coordenador.");
            }
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("DeleteMateriasById/{id}/")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var user = await _userManager.FindByNameAsync(userName);

            var loggedInUserRoles = await _userManager.GetRolesAsync(user);
            if (!loggedInUserRoles.Contains(UserRoles.AdminMaster) || !loggedInUserRoles.Contains(UserRoles.Admin))
            {
                return Forbid("Usuário não é AdminMaster/Admin.");
            }
            var materias = await _repository.GetMateria(id);
            if (materias == null) return NotFound("Matéria não encontrada.");

            _repository.DeleteMateria(materias);

            return await _repository.SaveChangesAsync()
            ? Ok("Matéria removida com sucesso.")
            : BadRequest("Erro ao remover Matéria.");
        }
    }
}