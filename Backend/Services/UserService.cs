using Microsoft.EntityFrameworkCore;

using shopping_list.DataBase;

namespace shopping_list.Services;

public interface IUserService
{
    Task<User?> RegistrationAsync(User user);
    Task<User?> GetUserByIdAsync(string id);
    Task<User?> GetUserByLoginAndPasswordAsync(string login, string password);
}
public class UserService : IUserService
{
    private readonly ApplicationContext dbContext;

    public UserService(ApplicationContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public async Task<User?> GetUserByIdAsync(string id)
    {
        return await dbContext.Users.FindAsync(id);
    }
    
    public async Task<User?> GetUserByLoginAndPasswordAsync(string login, string password)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(user => user.Login == login && user.Password == password);
    }
    
    public async Task<User?> RegistrationAsync(User user)
    {
        if (await dbContext.Users.AnyAsync(u => u.Login == user.Login))
        {
            return null;
            
        }
        user.Id = Guid.NewGuid().ToString();
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return user;
    }
}