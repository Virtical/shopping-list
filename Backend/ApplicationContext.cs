namespace shopping_list;
using Microsoft.EntityFrameworkCore;
 
public sealed class ApplicationContext : DbContext
{
    public DbSet<Purchase> Purchases { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }
}