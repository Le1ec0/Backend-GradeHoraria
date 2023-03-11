using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using GradeHoraria.Models;
using GradeHoraria.Context;
using GradeHoraria.Repositories;
using GradeHoraria.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public class Startup
{
    private readonly ICollection<SecurityKey> _signingKeys;
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        _signingKeys = GradeHoraria.Helpers.TokenMiddleware.GetSigningKeys().Result;
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
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.Authority = Configuration.GetValue<string>("AzureAD:Instance") + Configuration.GetValue<string>("AzureAD:TenantId") + "/v2.0";
            options.Audience = Configuration.GetValue<string>("AzureAD:ClientId");
            options.ClaimsIssuer = Configuration.GetValue<string>("AzureAD:Instance") + Configuration.GetValue<string>("AzureAD:TenantId") + "/v2.0";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudiences = new[] {
                    Configuration.GetValue<string>("AzureAD:ClientId"),
                    Configuration.GetValue<string>("AzureAD:ClientAPI"),
                    },

                ValidateIssuer = true,
                ValidIssuer = Configuration.GetValue<string>("AzureAD:Instance") + Configuration.GetValue<string>("AzureAD:TenantId") + "/v2.0",

                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = (SecurityKey)_signingKeys.First(),
            };
        });

        services.AddAuthorization();

        services.AddControllers();

        services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddUserManager<UserManager<ApplicationUser>>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders()
        .AddRoles<IdentityRole>();

        services.AddScoped<IGradeRepository, GradeRepository>();
        services.AddScoped<RoleManager<IdentityRole>>();
        services.AddTransient<TokenMiddleware>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddHostedService<SeedRoles>();

        // Set the UserIdClaimType to the name of the claim that contains the user id
        services.Configure<IdentityOptions>(options =>
        {
            options.ClaimsIdentity.UserNameClaimType = "name";
        });
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

        app.UseMiddleware<TokenMiddleware>();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseHttpsRedirection();
        app.UseCors("AllowAllOrigins");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}