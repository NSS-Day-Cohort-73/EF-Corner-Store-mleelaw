namespace CornerStore.Models.DTO;

public class OrderProductDTO


{

    public int Id { get; set; }
    public int ProductId { get; set; }
    public GetOrderProductDTO Product { get; set; }

    public int OrderId { get; set; }

    public int Quantity { get; set; }
}
