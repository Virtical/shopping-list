namespace shopping_list.Models;

public class Purchase
{
    public Purchase(string name, double count, int type, string listId, string userId)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Count = count;
        Type = type;
        ListId = listId;
        UserId = userId;
    }
    
    public string Id { get; init; }
    public string UserId { get; init; }
    public string Name { get; set; }
    public double Count { get; set; }
    public int Type { get; set; }
    public string ListId { get; init; }
}