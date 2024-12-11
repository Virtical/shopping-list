using shopping_list.Models;

namespace shopping_list.Contracts;

public record CreatePurchaseRequest(string Name, double Count, int Type, string ListId);