namespace shopping_list.Models;

public class List
{
    public List(string name, string userId)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        UserId = userId;
    }
    
    public string Id { get; init; }
    public string UserId { get; init; }
    public string Name { get; set; }
}