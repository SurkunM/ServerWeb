using ShopEF.Model;

namespace ShopEF;

public class DbInitializer
{
    private readonly ShopDbContext _shopDb;

    public DbInitializer(ShopDbContext dbContext)
    {
        _shopDb = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    private static Product CreateProduct(string name, decimal price)
    {
        return new Product
        {
            Name = name,
            Price = price
        };
    }

    private static Customer CreateCustomer(string firstName, string lastName, string middleName, string email, string phone)
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

    private static CategoryProduct CreateCategoryProduct(Category category, Product product)
    {
        return new CategoryProduct
        {
            Category = category,
            Product = product
        };
    }

    private static OrderProduct CreateOrderProducts(Order order, Product product, int count)
    {
        return new OrderProduct
        {
            Product = product,
            Order = order,
            ProductsCount = count
        };
    }

    public void Initialize()
    {
        _shopDb.Database.EnsureCreated();

        var category0 = new Category
        {
            Name = "Продукты питания"
        };

        var category1 = new Category
        {
            Name = "Морепродукты"
        };

        var product1 = CreateProduct("Минтай", 35);
        var product2 = CreateProduct("Морская капуста", 25);

        var productCategories1 = CreateCategoryProduct(category0, product1);
        var productCategories2 = CreateCategoryProduct(category1, product1);

        product1.ProductCategories = new List<CategoryProduct>
        {
            productCategories1,
            productCategories2
        };

        var productCategories3 = CreateCategoryProduct(category0, product2);
        var productCategories4 = CreateCategoryProduct(category1, product2);

        product2.ProductCategories = new List<CategoryProduct>
        {
            productCategories3,
            productCategories4
        };

        _shopDb.Products.Add(product1);
        _shopDb.Products.Add(product2);

        var category2 = new Category
        {
            Name = "Напитки"
        };

        var product3 = CreateProduct("Чай", 15);
        var product4 = CreateProduct("Вода", 5);

        var productCategories5 = CreateCategoryProduct(category0, product3);
        var productCategories6 = CreateCategoryProduct(category2, product3);

        product3.ProductCategories = new List<CategoryProduct>
        {
            productCategories5,
            productCategories6
        };

        var productCategories7 = CreateCategoryProduct(category0, product4);
        var productCategories8 = CreateCategoryProduct(category2, product4);

        product4.ProductCategories = new List<CategoryProduct>
        {
            productCategories7,
            productCategories8
        };

        _shopDb.Products.Add(product3);
        _shopDb.Products.Add(product4);

        var category3 = new Category
        {
            Name = "Молочные продукты"
        };

        var product5 = CreateProduct("Молоко", 25);
        var product6 = CreateProduct("Сыр", 120);

        var productCategories9 = CreateCategoryProduct(category0, product5);
        var productCategories10 = CreateCategoryProduct(category2, product5);
        var productCategories13 = CreateCategoryProduct(category3, product5);

        product5.ProductCategories = new List<CategoryProduct>
        {
            productCategories9,
            productCategories10,
            productCategories13
        };

        var productCategories11 = CreateCategoryProduct(category0, product6);
        var productCategories12 = CreateCategoryProduct(category3, product6);

        product6.ProductCategories = new List<CategoryProduct>
        {
            productCategories11,
            productCategories12
        };

        _shopDb.Products.Add(product5);
        _shopDb.Products.Add(product6);

        var customer1 = CreateCustomer("Иван", "Иванов", "Иванович", "Ivanov@mail.ru", "5123");
        var customer2 = CreateCustomer("Степан", "Степанов", "Степанович", "S2000@mail.ru", "2000");

        var order1 = new Order
        {
            Customer = customer1,
            OrderDate = new DateTime(2025, 3, 15, 12, 30, 02)
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
            OrderDate = new DateTime(2025, 3, 14, 10, 00, 48)
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

        _shopDb.Orders.Add(order1);
        _shopDb.Orders.Add(order2);

        _shopDb.SaveChanges();
    }
}
