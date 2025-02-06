using CornerStore.Models;
using Microsoft.EntityFrameworkCore;

public class CornerStoreDbContext : DbContext
{
    public DbSet<Cashier> Cashiers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }

    public CornerStoreDbContext(DbContextOptions<CornerStoreDbContext> context)
        : base(context) { }

    //allows us to configure the schema when migrating as well as seed data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Category>()
            .HasData(
                new Category[]
                {
                    new Category { Id = 1, CategoryName = "Beverages" },
                    new Category { Id = 2, CategoryName = "Snacks" },
                    new Category { Id = 3, CategoryName = "Frozen Foods" },
                    new Category { Id = 4, CategoryName = "Socks" },
                    new Category { Id = 5, CategoryName = "Dairy" },
                    new Category { Id = 6, CategoryName = "Canned Goods" },
                    new Category { Id = 7, CategoryName = "Shoes" },
                }
            );
        modelBuilder
            .Entity<Product>()
            .HasData(
                new Product[]
                {
                    new Product
                    {
                        Id = 1,
                        ProductName = "cola",
                        Price = 1.99M,
                        Brand = "Dish",
                        CategoryId = 4,
                    },
                    new Product
                    {
                        Id = 2,
                        ProductName = "Cho",
                        Price = 3.99M,
                        Brand = "Lays",
                        CategoryId = 2,
                    },
                    new Product
                    {
                        Id = 3,
                        ProductName = "Ice Cream",
                        Price = 4.99M,
                        Brand = "Ben & Jerry's",
                        CategoryId = 3,
                    },
                    new Product
                    {
                        Id = 4,
                        ProductName = "Bananas",
                        Price = 0.99M,
                        Brand = "Dole",
                        CategoryId = 1,
                    },
                    new Product
                    {
                        Id = 5,
                        ProductName = "Milk",
                        Price = 3.49M,
                        Brand = "Dairy Pure",
                        CategoryId = 5,
                    },
                    new Product
                    {
                        Id = 6,
                        ProductName = "Soup",
                        Price = 1.99M,
                        Brand = "Campbell's",
                        CategoryId = 6,
                    },
                    new Product
                    {
                        Id = 7,
                        ProductName = "water",
                        Price = 5.99M,
                        Brand = "Bounty",
                        CategoryId = 7,
                    },
                }
            );

        modelBuilder
            .Entity<Cashier>()
            .HasData(
                new Cashier[]
                {
                    new Cashier
                    {
                        Id = 1,
                        FirstName = "John",
                        LastName = "Smith",
                    },
                    new Cashier
                    {
                        Id = 2,
                        FirstName = "Jane",
                        LastName = "Doe",
                    },
                    new Cashier
                    {
                        Id = 3,
                        FirstName = "Bob",
                        LastName = "Jones",
                    },
                    new Cashier
                    {
                        Id = 4,
                        FirstName = "Sarah",
                        LastName = "Wilson",
                    },
                    new Cashier
                    {
                        Id = 5,
                        FirstName = "Mike",
                        LastName = "Brown",
                    },
                    new Cashier
                    {
                        Id = 6,
                        FirstName = "Lisa",
                        LastName = "Davis",
                    },
                    new Cashier
                    {
                        Id = 7,
                        FirstName = "Tom",
                        LastName = "Miller",
                    },
                }
            );

        modelBuilder
            .Entity<Order>()
            .HasData(
                new Order[]
                {
                    new Order
                    {
                        Id = 1,
                        CashierId = 1,
                        PaidOnDate = DateTime.Parse("2024-02-03"),
                    },
                    new Order
                    {
                        Id = 2,
                        CashierId = 2,
                        PaidOnDate = DateTime.Parse("2024-02-03"),
                    },
                    new Order
                    {
                        Id = 3,
                        CashierId = 3,
                        PaidOnDate = DateTime.Parse("2024-02-03"),
                    },
                    new Order
                    {
                        Id = 4,
                        CashierId = 4,
                        PaidOnDate = DateTime.Parse("2024-02-03"),
                    },
                    new Order
                    {
                        Id = 5,
                        CashierId = 5,
                        PaidOnDate = DateTime.Parse("2024-02-03"),
                    },
                    new Order
                    {
                        Id = 6,
                        CashierId = 6,
                        PaidOnDate = DateTime.Parse("2024-02-03"),
                    },
                    new Order
                    {
                        Id = 7,
                        CashierId = 7,
                        PaidOnDate = DateTime.Parse("2024-02-03"),
                    },
                }
            );

        modelBuilder
            .Entity<OrderProduct>()
            .HasData(
                new OrderProduct[]
                {
                    new OrderProduct
                    {
                        Id = 1,
                        OrderId = 1,
                        ProductId = 1,
                        Quantity = 2,
                    },
                    new OrderProduct
                    {
                        Id = 2,
                        OrderId = 2,
                        ProductId = 2,
                        Quantity = 1,
                    },
                    new OrderProduct
                    {
                        Id = 3,
                        OrderId = 3,
                        ProductId = 3,
                        Quantity = 3,
                    },
                    new OrderProduct
                    {
                        Id = 4,
                        OrderId = 4,
                        ProductId = 4,
                        Quantity = 5,
                    },
                    new OrderProduct
                    {
                        Id = 5,
                        OrderId = 5,
                        ProductId = 5,
                        Quantity = 1,
                    },
                    new OrderProduct
                    {
                        Id = 6,
                        OrderId = 6,
                        ProductId = 6,
                        Quantity = 4,
                    },
                    new OrderProduct
                    {
                        Id = 7,
                        OrderId = 7,
                        ProductId = 7,
                        Quantity = 2,
                    },
                }
            );
    }
}
