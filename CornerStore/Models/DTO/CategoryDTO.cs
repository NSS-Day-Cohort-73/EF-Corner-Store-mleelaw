namespace CornerStore.Models.DTO;

public class CategoryDTO
{
    public int Id { get; set; }

    public string CategoryName { get; set; }
    public List<Category> Categories { get; set; }
}