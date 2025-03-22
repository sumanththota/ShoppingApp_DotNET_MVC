using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Models;

using System.ComponentModel.DataAnnotations;

public class CartItem
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public string Name { get; set; }


    public decimal Price { get; set; }

    [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
    public int Quantity { get; set; }
    public int CartId { get; set; }
    public Cart Cart { get; set; } = default!;


}