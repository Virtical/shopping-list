using Microsoft.EntityFrameworkCore;

namespace shopping_list.DataBase;
public sealed class ApplicationContext : DbContext
{
    public DbSet<Purchase> Purchases { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }
}