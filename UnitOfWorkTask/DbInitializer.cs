using UnitOfWorkTask.Model.Entities;

namespace UnitOfWorkTask;

class DbInitializer
{

    private readonly ShopDbContext _dbContext;

    public DbInitializer(ShopDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public void Initialize()
    {
        throw new Exception();
        if (!_dbContext.Database.EnsureCreated())
        {
            return;
        }

        CrateAndAddData(_dbContext);

        _dbContext.SaveChanges();
    }



    private void CrateAndAddData(ShopDbContext shopDb)
    {
        var category1 = new Category
        {
            Name = "Морепродукты"
        };

        var product1 = CreateProduct(category1, "Минтай", 35);
        var product2 = CreateProduct(category1, "Морская капуста", 25);

        shopDb.Products.Add(product1);
        shopDb.Products.Add(product2);

        var category2 = new Category
        {
            Name = "Напитки"
        };

        var product3 = CreateProduct(category2, "Чай", 15);
        var product4 = CreateProduct(category2, "Вода", 5);

        shopDb.Products.Add(product3);
        shopDb.Products.Add(product4);

        var category3 = new Category
        {
            Name = "Молочные продукты"
        };

        var product5 = CreateProduct(category3, "Молоко", 25);
        var product6 = CreateProduct(category3, "Сыр", 120);

        shopDb.Products.Add(product5);
        shopDb.Products.Add(product6);

        var customer1 = CreateCustomer("Иван", "Иванов", "Иванович", "Ivanov@mail.ru", "5123");
        var customer2 = CreateCustomer("Степан", "Степанов", "Степанович", "S2000@mail.ru", "2000");

        var order1 = new Order
        {
            Customer = customer1,
            OrderDate = new DateTime(2025, 3, 15, 12, 30, 02),
        };

        var orderProducts1 = CreateOrderProducts(order1, product3, 1);
        var orderProducts2 = CreateOrderProducts(order1, product2, 2);
        var orderProducts3 = CreateOrderProducts(order1, product5, 2);

        order1.OrderProducts = new List<OrderProduct>
        {
            orderProducts1,
            orderProducts2,
            orderProducts3
        };

        var order2 = new Order
        {
            Customer = customer2,
            OrderDate = new DateTime(2025, 3, 14, 10, 00, 48),
        };

        var orderProducts4 = CreateOrderProducts(order2, product2, 1);
        var orderProducts5 = CreateOrderProducts(order2, product1, 1);
        var orderProducts6 = CreateOrderProducts(order2, product3, 4);


        order2.OrderProducts = new List<OrderProduct>
        {
            orderProducts4,
            orderProducts5,
            orderProducts6
        };

        shopDb.Orders.Add(order1);
        shopDb.Orders.Add(order2);
    }

    private Product CreateProduct(Category category, string name, decimal price)
    {
        return new Product
        {
            Name = name,
            Price = price,
            Category = category
        };
    }

    private Customer CreateCustomer(string firstName, string lastName, string middleName, string email, string phone)
    {
        return new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            Phone = phone,
            Email = email
        };
    }

    private OrderProduct CreateOrderProducts(Order order, Product product, int count)
    {
        return new OrderProduct
        {
            Product = product,
            Order = order,
            ProductCount = count
        };
    }
}
