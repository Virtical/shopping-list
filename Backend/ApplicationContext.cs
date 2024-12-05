using Microsoft.EntityFrameworkCore;

namespace shopping_list;
public sealed class ApplicationContext : DbContext
{
    public DbSet<Purchase> Purchases { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }
}