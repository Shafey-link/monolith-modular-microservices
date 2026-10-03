namespace ECommerce.Models;

public class Order
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = "Pending";
}
