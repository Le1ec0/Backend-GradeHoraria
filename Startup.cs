using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using GradeHoraria.Context;
using GradeHoraria.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GradeHoraria.Models;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        Configuration = configuration;
    }
    private async Task CreateRoles(IServiceProvider serviceProvider)
    {
        var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        string[] roleNames = { "AdminMaster", "Admin", "Coordenador", "Professor", "Usuário" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await RoleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                //create the roles and seed them to the database: Question 1
                roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        //Here you could create a super user who will maintain the web app
        var adminmaster = new ApplicationUser
        {
            UserName = Configuration["AdminMaster:UserName"],
            Email = Configuration["AdminMaster:UserEmail"],
        };
        //Ensure you have these values in your appsettings.json file
        string userPWD = Configuration["AdminMaster:UserPassword"];
        var _user = await UserManager.FindByEmailAsync(Configuration["AdminMaster:AdminUserEmail"]);

        if (_user == null)
        {
            var createAdminMaster = await UserManager.CreateAsync(adminmaster, userPWD);
            if (createAdminMaster.Succeeded)
            {
                await UserManager.AddToRoleAsync(adminmaster, "AdminMaster");

            }
        }
    }

    /*private async Task CreateRoles()
    {
        // List of roles from UserRoles.cs class
        //List<string> roles = new List<string> { UserRoles.AdminMaster, UserRoles.Admin, UserRoles.Coordenador, UserRoles.Professor, UserRoles.Usuario };
        List<string> roles = new List<string> { "AdminMaster", "Admin", "Coordenador", "Professor", "Usuario" };

        foreach (var role in roles)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                // Create a new role using the role name from UserRoles.cs class
                var newRole = new IdentityRole(role);
                await _roleManager.CreateAsync(newRole);
            }
        }
    }*/

    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });

        services.AddHttpContextAccessor();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
            });
        });

        // For Entity Framework
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("SQLConnection"),
            sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure();
            });
        });

        // Add Azure Active Directory Authentication
        /*services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"))
            .EnableTokenAcquisitionToCallDownstreamApi(new string[] { Configuration[("AzureAd:GraphPath")] })
            .AddMicrosoftGraph(Configuration.GetSection("DownstreamApi"))
            .AddInMemoryTokenCaches();*/

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = $"{Configuration["AzureAd:Instance"]}/{Configuration["AzureAd:TenantId"]}/v2.0";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidAudiences = new[] { Configuration["AzureAd:ClientId"] }
            };
        });

        services.AddIdentity<IdentityUser, IdentityRole>()
        .AddUserManager<UserManager<IdentityUser>>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<UserManager<IdentityUser>>();
        services.AddScoped<RoleManager<IdentityRole>>();
        services.AddScoped<IGradeRepository, GradeRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
            //Cors Policy
            app.UseCors(options => options.WithOrigins("http://127.0.0.1:5500").AllowAnyHeader().AllowAnyMethod());
        }
        else
        {
            app.UseHsts();
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.UseCors("AllowAllOrigins");
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        //CreateRoles().Wait();
    }
}