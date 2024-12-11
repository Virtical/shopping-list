using shopping_list.Models;

namespace shopping_list.Contracts;

public record PutPurchaseRequest(string Id, string Name, double Count, int Type);