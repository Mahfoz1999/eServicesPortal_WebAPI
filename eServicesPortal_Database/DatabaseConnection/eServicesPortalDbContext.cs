using eServicesPortal_Models.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eServicesPortal_Database.DatabaseConnection;

public class eServicesPortalDbContext : IdentityDbContext<User>
{
    public eServicesPortalDbContext(DbContextOptions<eServicesPortalDbContext> options) : base(options)
    { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
