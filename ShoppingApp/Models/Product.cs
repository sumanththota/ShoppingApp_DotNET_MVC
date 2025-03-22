namespace ShoppingApp.Models;

using System.ComponentModel.DataAnnotations;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!; //make sure that the name is not null
    public double Price { get; set; }
    public Category Category { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
    public int Quantity { get; set; }
}