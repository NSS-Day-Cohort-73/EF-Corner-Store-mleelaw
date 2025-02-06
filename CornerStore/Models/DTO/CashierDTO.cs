namespace CornerStore.Models.DTO;

public class CashierDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<OrderDTO> Orders { get; set; }
}
