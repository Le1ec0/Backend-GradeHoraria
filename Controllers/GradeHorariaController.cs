using GradeHoraria.Context;
using GradeHoraria.Models;
using GradeHoraria.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

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
        private readonly ConfigurationManager<OpenIdConnectConfiguration> _configManager;
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
            _configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
        $"{_configuration.GetValue<string>("AzureAD:Instance") + _configuration.GetValue<string>("AzureAD:TenantId") + "/v2.0"}/.well-known/openid-configuration",
        new OpenIdConnectConfigurationRetriever());
        }

        /*[HttpPost]
        [Route("GetAllUsers")]
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
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var accessToken = authHeader.Substring("Bearer ".Length).Trim();

                var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                var user = await _userManager.FindByNameAsync(userName);

                if (user == null)
                {
                    return NotFound("Usuário não encontrado.");
                }

                var photoBase64String = user.PhotoBytes != null
                ? Convert.ToBase64String(user.PhotoBytes)
                : null;

                var roles = await _userManager.GetRolesAsync(user);

                var userClaims = new
                {
                    Name = userName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault(),
                    photoBase64String = photoBase64String
                };

                return Ok(userClaims);
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the request
                return BadRequest(ex.Message);
            }
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

            // Make a Microsoft Graph API query to check if the user exists in AAD
            var user = await graphServiceClient.Users
            .Request()
            .Filter($"mail eq '{model.Username}' or userPrincipalName eq '{model.Username}'")
            .Select("displayName,mail,userPrincipalName")
            .GetAsync();

            if (user.Count == 0)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Make a Microsoft Graph API query to get the profile photo
            byte[]? photoBytes = null;

            try
            {
                var photoStream = await graphServiceClient.Me.Photo.Content.Request().GetAsync();

                // Convert the photoStream to a byte array
                using (var memoryStream = new MemoryStream())
                {
                    await photoStream.CopyToAsync(memoryStream);
                    photoBytes = memoryStream.ToArray();
                }
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    throw; // re-throw if it's not a "photo not found" exception
                }
            }

            var newUser = await _userManager.FindByNameAsync(user[0].DisplayName);
            if (newUser == null)
            {
                newUser = new ApplicationUser
                {
                    Id = user[0].Id,
                    UserName = user[0].DisplayName,
                    Email = user[0].Mail ?? user[0].UserPrincipalName,
                    NormalizedUserName = user[0].DisplayName.ToUpperInvariant(),
                    NormalizedEmail = (user[0].Mail ?? user[0].UserPrincipalName).ToUpperInvariant(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PhotoBytes = photoBytes
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
                    new Claim(ClaimTypes.Name, user[0].DisplayName),
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
        [Route("RegisterUser")]
        public async Task<IActionResult> CreateUserAsync([FromBody] RegisterModel model)
        {
            var newUser = await _userManager.FindByNameAsync(model.displayName);
            if (newUser != null)
            {
                return BadRequest("User already exists");
            }

            if (newUser == null)
            {
                newUser = new ApplicationUser
                {
                    UserName = model.displayName,
                    Email = model.userPrincipalName,
                    NormalizedUserName = model.displayName.ToUpperInvariant(),
                    NormalizedEmail = model.userPrincipalName.ToUpperInvariant(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PhotoBytes = model.photoBytes
                };

                await _userManager.CreateAsync(newUser);
                var userRoles = await _userManager.GetRolesAsync(newUser);

                var result = await _userManager.CreateAsync(newUser, model.password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                if (!await _roleManager.RoleExistsAsync(UserRoles.Usuario))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Usuario));
                }

                await _userManager.AddToRoleAsync(newUser, UserRoles.Usuario);

                var authClaims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, newUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                authClaims.AddRange(await _userManager.GetClaimsAsync(newUser));
                authClaims.AddRange((await _userManager.GetRolesAsync(newUser)).Select(r => new Claim(ClaimTypes.Role, r)));

                // Add the new User to the context using the AddUser method
                await _repository.AddUser(newUser);
                await _repository.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPost]
        [Route("RegisterUserAAD")]
        public async Task<IActionResult> CreateAADUserAsync([FromBody] RegisterModel model)
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
            var admin = new ApplicationUser
            {
                UserName = _configuration["Admin:UserName"],
                NormalizedUserName = _configuration["Admin:UserName"].ToUpperInvariant(),
                Email = _configuration["Admin:UserEmail"],
                NormalizedEmail = _configuration["Admin:UserEmail"].ToUpperInvariant(),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            /*var result = await _userManager.CreateAsync(admin, _configuration["Admin:UserPassword"]);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, UserRoles.Admin);
                await _repository.AddUser((ApplicationUser)admin);
                await _repository.SaveChangesAsync();
            }*/

            await _userManager.CreateAsync(admin, _configuration["Admin:UserPassword"]);
            await _userManager.AddToRoleAsync(admin, UserRoles.Admin);
            await _repository.AddUser((ApplicationUser)admin);
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

            var loggedInUserRole = await _userManager.GetRolesAsync(user);
            if (!loggedInUserRole.Any(r => r == UserRoles.Admin))
            {
                return Unauthorized("Usuário não é Admin.");
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

            var loggedInUserRole = await _userManager.GetRolesAsync(user);
            if (!loggedInUserRole.Any(r => r == UserRoles.Admin))
            {
                return Unauthorized("Usuário não é Admin.");
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
            if (!loggedInUserRoles.Any(r => r == UserRoles.Admin || r == UserRoles.Coordenador))
            {
                return Unauthorized("Usuário não é Admin/Coordenador.");
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
            if (!loggedInUserRoles.Any(r => r == UserRoles.Admin))
            {
                return Unauthorized("Usuário não é Admin.");
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
            if (!loggedInUserRoles.Any(r => r == UserRoles.Admin))
            {
                return Unauthorized("Usuário não é Admin.");
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
            if (!loggedInUserRoles.Any(r => r == UserRoles.Admin || r == UserRoles.Coordenador))
            {
                return Unauthorized("Usuário não é Admin/Coordenador.");
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
            if (!loggedInUserRoles.Any(r => r == UserRoles.Admin))
            {
                return Unauthorized("Usuário não é Admin.");
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