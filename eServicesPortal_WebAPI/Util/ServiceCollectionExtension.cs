using eServicesPortal_Commends.Commends.AuthenticationCommends.Commend;
using eServicesPortal_Commends.Commends.AuthenticationCommends.CommendHandler;
using eServicesPortal_Commends.Commends.AuthenticationCommends.Query;
using eServicesPortal_Commends.Commends.AuthenticationCommends.QueryHandler;
using eServicesPortal_Commends.Commends.MailCommends.Commend;
using eServicesPortal_Commends.Commends.MailCommends.CommendHandler;
using eServicesPortal_Commends.Commends.UserCommends.Query;
using eServicesPortal_Commends.Commends.UserCommends.QueryHandler;
using eServicesPortal_Database.DatabaseConnection;
using eServicesPortal_DTO.Auth;
using eServicesPortal_Models.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace eServicesPortal_WebAPI.Util;

public static class ServiceCollectionExtension
{
    #region Base Settings
    public static void ConfigureControllers(this IServiceCollection services)
    {
        _ = services.AddControllers(config =>
        {
            config.CacheProfiles.Add("30SecondsCaching", new CacheProfile
            {
                Duration = 30
            });
        });
    }
    public static void ConfigureResponseCaching(this IServiceCollection services)
    {
        services.AddResponseCaching();
    }
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        return services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
    }
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        IdentityBuilder builder = services.AddIdentity<User, IdentityRole>(o =>
        {
            o.Password.RequireDigit = false;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.User.RequireUniqueEmail = true;
            o.SignIn.RequireConfirmedEmail = true;
        }).AddEntityFrameworkStores<eServicesPortalDbContext>()
        .AddDefaultTokenProviders();
    }

    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection jwtConfig = configuration.GetSection("jwtConfig");
        string? secretKey = jwtConfig["secret"];
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtConfig:validAudience"],
                    ValidIssuer = configuration["JwtConfig:validIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s: configuration["JwtConfig:secret"]!))
                };
            });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        _ = services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "eServices Portal WebAPI",
                Version = "v1",
                Description = "eServices Portal WebAPI Services.",
                Contact = new OpenApiContact
                {
                    Name = "Mahfoz Khalil ."
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
    #endregion
    public static void AddCommendTransients(this IServiceCollection services)
    {
        services.AddTransient<IRequestHandler<GetCurrentUserQuery, User>, GetCurrentUserQueryHandler>();
        services.AddTransient<IRequestHandler<GetAllUsersQuery, IEnumerable<User>>, GetAllUsersQueryHandler>();
        services.AddTransient<IRequestHandler<SendEmailCommend>, SendEmailCommendHandler>();

        services.AddTransient<IRequestHandler<GetTokenQuery, JwtSecurityToken>, GetTokenQueryHandler>();
        services.AddTransient<IRequestHandler<SignUpCommend, IdentityResult>, SignUpCommendHandler>();
        services.AddTransient<IRequestHandler<LogInCommend, TokenModel>, LogInCommendHanlder>();
    }
}
