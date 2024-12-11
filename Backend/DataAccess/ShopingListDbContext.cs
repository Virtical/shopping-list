using Microsoft.EntityFrameworkCore;
using shopping_list.Models;

namespace shopping_list.DataAccess;
public sealed class ShopingListDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    private readonly IConfiguration _configuration;

    public ShopingListDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public DbSet<Users> Users => Set<Users>();
    public DbSet<List> Lists => Set<List>();
    public DbSet<Purchase> Purchases => Set<Purchase>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Database"));
    }
}