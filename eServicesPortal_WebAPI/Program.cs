using eServicesPortal_Database.DatabaseConnection;
using eServicesPortal_DTO.Mail;
using eServicesPortal_Services.AuthenticationService;
using eServicesPortal_Services.UserService;
using eServicesPortal_WebAPI.Middleware;
using eServicesPortal_WebAPI.Util;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
Assembly assembly = Assembly.GetExecutingAssembly();
builder.Services.AddControllers();
builder.Services.TryAddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.TryAddScoped<IUserService, UserService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
builder.Services.AddTransient<Mediator>();
builder.Services.AddCommendTransients();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services
   .AddDbContext<eServicesPortalDbContext>(options =>
   {
       string? connectionString = builder.Configuration.GetConnectionString("DefaultMySQL");
       options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
   });
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(1));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureControllers();

var app = builder.Build();
app.UseCors(cors => cors
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true)
              .AllowCredentials());
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseResponseCaching();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
