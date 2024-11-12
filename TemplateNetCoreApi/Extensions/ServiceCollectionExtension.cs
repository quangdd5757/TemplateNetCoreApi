using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using TemplateNetCoreApi.Core.Mappings;
using TemplateNetCoreApi.Core.Models;
using TemplateNetCoreApi.Repo.Data;
using TemplateNetCoreApi.Service.Filters.ActionFilters;
using TemplateNetCoreApi.Service.Interfaces;
using TemplateNetCoreApi.Service.Services;

namespace TemplateNetCoreApi.Extensions;

public static class ServiceExtension
{
    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
    // sử dụng cho TH SqlServer
    //services.AddDbContext<RepositoryContext>(
    //    opts => opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
    //        b => b.MigrationsAssembly("StudentTeacher.Repo")));
    // sử dụng cho TH MySql
        services.AddDbContext<RepositoryContext>(
    options =>
    {
        options.UseMySql(configuration.GetConnectionString("SqlConnection"),
            ServerVersion.Parse(configuration.GetSection("ServerVersion").Value));
    }
    );

    // config repository manager (chính là manager service)
    public static void ConfigureRepositoryManager(this IServiceCollection services)
        => services.AddScoped<IRepositoryManager, RepositoryManager>();

    // config auto mapping
    public static void ConfigureMapping(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        var mapperConfig = new MapperConfiguration(map =>
        {
            map.AddProfile<TeacherMappingProfile>();
            map.AddProfile<StudentMappingProfile>();
            map.AddProfile<UserMappingProfile>();
        });
        services.AddSingleton(mapperConfig.CreateMapper());
    }

    // config cache 30s
    public static void ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers(config =>
        {
            config.CacheProfiles.Add("30SecondsCaching", new CacheProfile
            {
                Duration = 30
            });
        });
    }
    public static void ConfigureResponseCaching(this IServiceCollection services) => services.AddResponseCaching();

    // config identity framework
    public static void ConfigureIdentity(this IServiceCollection services)
    // khai báo sử dụng IdentityUser và IdentityRole, với IdentityDbContext tại kế thừa RepositoryContext
    {
        var builder = services.AddIdentity<User, Role>(o =>
        {
            o.Password.RequireDigit = false;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<RepositoryContext>()
        .AddDefaultTokenProviders();
    }

    // config JWT Bearer - authentication + authorization
    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = configuration.GetSection("jwtConfig");
        var secretKey = jwtConfig["secret"];
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig["validIssuer"],
                ValidAudience = jwtConfig["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });
    }

    // config swagger
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Student Teacher API",
                Version = "v1",
                Description = "Student Teacher API Services.",
                Contact = new OpenApiContact
                {
                    Name = "Ajide Habeeb."
                },
            });
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }

    // add valid attribute cho controller
    public static void RegisterDependencies(this IServiceCollection services)
    {
        services.AddScoped<ValidationFilterAttribute>();
        services.AddScoped<ValidateTeacherExists>();
        services.AddScoped<ValidateStudentExistsForTeacher>();
    }
}
