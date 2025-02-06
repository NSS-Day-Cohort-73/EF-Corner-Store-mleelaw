using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using CornerStore.Models;
using CornerStore.Models.DTO;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core and provides dummy value for testing
builder.Services.AddNpgsql<CornerStoreDbContext>(
    builder.Configuration["CornerStoreDbConnectionString"] ?? "testing"
);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//endpoints!

app.MapGet(
    "/cashiers/{id}",
    (int id, CornerStoreDbContext db) =>
    {
        Cashier cashier = db
            .Cashiers.Include(c => c.Orders)
            .ThenInclude(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .FirstOrDefault(c => c.Id == id);

        if (cashier == null)
            return Results.NotFound();

        return Results.Ok(
            new CashierDTO
            {
                Id = cashier.Id,
                FirstName = cashier.FirstName,
                LastName = cashier.LastName,
                Orders = cashier
                    .Orders.Select(o => new OrderDTO
                    {
                        Id = o.Id,
                        PaidOnDate = o.PaidOnDate,
                        OrderProducts = o
                            .OrderProducts.Select(op => new OrderProductDTO
                            {
                                ProductId = op.ProductId,
                                Product = new GetOrderProductDTO
                                {
                                    ProductName = op.Product.ProductName,
                                    Price = op.Product.Price,
                                    Brand = op.Product.Brand,
                                },
                                Quantity = op.Quantity,
                            })
                            .ToList(),
                    })
                    .ToList(),
            }
        );
    }
);

app.MapPost(
    "/cashiers",
    (AddCashierDTO addCashier, CornerStoreDbContext db) =>
    {
        Cashier cashier = new Cashier
        {
            FirstName = addCashier.FirstName,
            LastName = addCashier.LastName,
        };

        db.Cashiers.Add(cashier);
        db.SaveChanges();
        return Results.Created($"/cashiers/{cashier.Id}", cashier);
    }
);

app.MapGet(
    "/products",
    (string? search, CornerStoreDbContext db) =>
    {
        IQueryable<Product> query = db.Products.Include(p => p.Category);

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p =>
                p.ProductName.ToLower().Contains(search.ToLower())
                || p.Category.CategoryName.ToLower().Contains(search.ToLower())
            );
        }

        return Results.Ok(query.ToList());
    }
);

app.MapPost(
    "/products",
    (ProductDTO addProduct, CornerStoreDbContext db) =>
    {
        var product = new Product
        {
            ProductName = addProduct.ProductName,
            Price = addProduct.Price,
            Brand = addProduct.Brand,
            CategoryId = addProduct.CategoryId,
        };

        db.Products.Add(product);
        db.SaveChanges();
        return Results.Created($"/products/{product.Id}", product);
    }
);

app.MapPut(
    "/products/{id}",
    (int id, ProductDTO updateProduct, CornerStoreDbContext db) =>
    {
        var product = db.Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return Results.NotFound("Product Id Does Not Exist");
        }

        product.ProductName = updateProduct.ProductName;
        product.Price = updateProduct.Price;
        product.Brand = updateProduct.Brand;
        product.CategoryId = updateProduct.CategoryId;

        db.SaveChanges();
        return Results.NoContent();
    }
);

app.MapGet(
    "/orders/{id}",
    (int id, CornerStoreDbContext db) =>
    {
        var order = db
            .Orders.Include(c => c.Cashier)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .ThenInclude(p => p.Category)
            .FirstOrDefault(c => c.Id == id);

        if (order == null)
            return Results.NotFound("Order Id Does Not Exist");

        return Results.Ok(
            new OrderDTO
            {
                Id = order.Id,
                PaidOnDate = order.PaidOnDate,
                CashierId = order.CashierId,
                Cashier = new CashierDTO
                {
                    Id = order.Cashier.Id,
                    FirstName = order.Cashier.FirstName,
                    LastName = order.Cashier.LastName,
                },
                OrderProducts = order
                    ?.OrderProducts.Select(np => new OrderProductDTO
                    {
                        ProductId = np.ProductId,
                        Quantity = np.Quantity,
                        Product = new GetOrderProductDTO
                        {
                            ProductName = np.Product.ProductName,
                            Price = np.Product.Price,
                            Brand = np.Product.Brand,
                            CategoryId = np.Product.CategoryId,
                            Category = new CategoryDTO
                            {
                                Id = np.Product.Category.Id,
                                CategoryName = np.Product.Category.CategoryName,
                            },
                        },
                    })
                    .ToList(),
            }
        );
    }
);

app.MapGet(
    "/orders",
    (DateTime? orderDate, CornerStoreDbContext db) =>
    {
        IQueryable<OrderDTO> query = db.Orders.Select(o => new OrderDTO
        {
            Id = o.Id,
            PaidOnDate = o.PaidOnDate,
            CashierId = o.CashierId,
            Cashier = new CashierDTO
            {
                Id = o.Cashier.Id,
                FirstName = o.Cashier.FirstName,
                LastName = o.Cashier.LastName,
            },
        });

        if (orderDate != null)
        {
            query = query.Where(o => o.PaidOnDate == orderDate);
        }

        return Results.Ok(query);
    }
);

app.MapDelete(
    "/orders/{seeYaLaterOrder}",
    (int seeYaLaterOrder, CornerStoreDbContext db) =>
    {
        Order order = db.Orders.FirstOrDefault(o => o.Id == seeYaLaterOrder);

        db.Orders.Remove(order);
        db.SaveChanges();
        return Results.NoContent();
    }
);

app.MapPost(
    "/orders",
    (Order newOrder, CornerStoreDbContext db) =>
    {
        Order order = new Order
        {
            CashierId = newOrder.CashierId,
            PaidOnDate = newOrder.PaidOnDate,
            OrderProducts = newOrder.OrderProducts
        };

        db.Orders.Add(order);
        db.SaveChanges(); 

        Order completeOrder = db.Orders
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .First(o => o.Id == order.Id);


        return Results.Created($"/orders/{order.Id}", completeOrder);
    }
);
app.Run();

//don't move or change this!
public partial class Program { }
