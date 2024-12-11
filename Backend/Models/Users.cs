namespace shopping_list.Models;

public class Users
{
    public Users(string login, string password)
    {
        Id = Guid.NewGuid().ToString();
        Login = login;
        Password = password;
    }
    public string Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}