using System.ComponentModel.DataAnnotations;

namespace CornerStore.Models;

public class Order
{
    public int Id { get; set; }

    [Required]
    public int CashierId { get; set; }

    public Cashier Cashier { get; set; }
    public DateTime? PaidOnDate { get; set; }
    public List<OrderProduct> OrderProducts { get; set; }

    public decimal Total
    {
        get
        {
            return OrderProducts
                    ?.Where(op => op?.Product != null)
                    .Sum(orderP => orderP.Product.Price * orderP.Quantity) ?? 0m;
        }
    }
}
