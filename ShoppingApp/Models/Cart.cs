namespace ShoppingApp.Models;

public class Cart
{
    public int Id { get; set; }
    public string? UserId { get; set; } = string.Empty; // for tracking individual users
    public string SessionId { get; set; } = string.Empty;
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
