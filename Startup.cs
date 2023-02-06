using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using GradeHoraria.Context;
using GradeHoraria.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GradeHoraria.Models;
using System.Text;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public async Task CreateRoles(IServiceProvider serviceProvider)
    {
        var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        List<string> roleNames = new List<string> { UserRoles.AdminMaster, UserRoles.Admin, UserRoles.Coordenador, UserRoles.Professor, UserRoles.Usuario };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await RoleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var newRole = new IdentityRole(roleName);
                roleResult = await RoleManager.CreateAsync(newRole);
            }
        }

        //Here you could create a super user who will maintain the web app
        var adminmaster = new IdentityUser
        {
            UserName = Configuration["AdminMaster:UserName"],
            Email = Configuration["AdminMaster:UserEmail"],
        };

        //Ensure you have these values in your appsettings.json file
        string UserPassword = Configuration["AdminMaster:UserPassword"];
        var _user = await UserManager.FindByEmailAsync(Configuration["AdminMaster:AdminUserEmail"]);

        if (_user == null)
        {
            var createAdminMaster = await UserManager.CreateAsync(adminmaster, UserPassword);
            if (createAdminMaster.Succeeded)
            {
                await UserManager.AddToRoleAsync(adminmaster, "AdminMaster");

            }
        }
    }

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

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })

        .AddJwtBearer(options =>
        {
            options.Authority = $"{Configuration.GetSection("AzureAd")["Instance"]}{Configuration.GetSection("AzureAd")["TenantId"]}";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                //ValidAudience = Configuration.GetSection("AzureAd")["Audience"],
                ValidAudience = "https://graph.microsoft.com",

                ValidateIssuer = true,
                //ValidIssuer = Configuration.GetSection("AzureAd")["Issuer"]
                ValidIssuer = "https://sts.windows.net/e182f34b-6958-474c-9143-88349addfba9/"
            };
        });

        services.AddControllers();

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