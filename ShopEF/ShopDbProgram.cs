using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShopEF.Model;

namespace ShopEF;

public class ShopDbProgram
{
    private static Product CrateProduct(Category category, string name, decimal price)
    {
        var product = new Product
        {
            Name = name,
            Price = price,
            Category = category
        };

        return product;
    }

    private static Buyer CreateBuyer(string firstName, string lastName, string middleName, string email, string phone)
    {
        var buyer = new Buyer
        {
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            Phone = phone,
            Email = email
        };

        return buyer;
    }

    private static OrderProduct CreateOrderProducts(Order order, Product product)
    {
        var orderProduct = new OrderProduct
        {
            Product = product,
            Order = order
        };

        return orderProduct;
    }

    private static void AddContext(ShopDbContext shopDb)
    {
        var category1 = new Category
        {
            Name = "Морепродукты"
        };

        var product1 = CrateProduct(category1, "Минтай", 35);
        var product2 = CrateProduct(category1, "Морская капуста", 25);

        shopDb.Products.Add(product1);
        shopDb.Products.Add(product2);

        var category2 = new Category
        {
            Name = "Напитки"
        };

        var product3 = CrateProduct(category2, "Чай", 15);
        var product4 = CrateProduct(category2, "Вода", 5);

        shopDb.Products.Add(product3);
        shopDb.Products.Add(product4);

        var category3 = new Category
        {
            Name = "Молочные продукты"
        };

        var product5 = CrateProduct(category3, "Молоко", 25);
        var product6 = CrateProduct(category3, "Сыр", 120);

        shopDb.Products.Add(product5);
        shopDb.Products.Add(product6);

        var buyer1 = CreateBuyer("Иван", "Иванов", "Иванович", "Ivanov@mail.ru", "5123");
        var buyer2 = CreateBuyer("Степан", "Степанов", "Степанович", "S2000@mail.ru", "2000");

        var order1 = new Order
        {
            Buyer = buyer1,
            OrderDate = new DateTime(2025, 3, 15, 12, 30, 02),
        };

        var orderProducts1 = CreateOrderProducts(order1, product3);
        var orderProducts2 = CreateOrderProducts(order1, product2);
        var orderProducts3 = CreateOrderProducts(order1, product2);

        order1.OrderProducts = new List<OrderProduct>
        {
            orderProducts1,
            orderProducts2,
            orderProducts3
        };

        var order2 = new Order
        {
            Buyer = buyer2,
            OrderDate = new DateTime(2025, 3, 14, 10, 00, 48),
        };

        var orderProducts4 = CreateOrderProducts(order2, product2);
        var orderProducts5 = CreateOrderProducts(order2, product1);
        var orderProducts6 = CreateOrderProducts(order2, product3);


        order2.OrderProducts = new List<OrderProduct>
        {
            orderProducts4,
            orderProducts5,
            orderProducts6
        };

        shopDb.Orders.Add(order1);
        shopDb.Orders.Add(order2);

        shopDb.SaveChanges();
    }

    private static void UpdateProductPrice(ShopDbContext shopDb, string name, decimal price)
    {
        var product = shopDb.Products.FirstOrDefault(p => p.Name == name);

        if (product is null)
        {
            Console.WriteLine("Не удалось изменить цену. Продукт не был найден");

            return;
        }

        product.Price = price;
        shopDb.SaveChanges();

        Console.WriteLine("Цена изменена");
    }

    private static void DeleteProduct(ShopDbContext shopDb, string name)
    {
        var product = shopDb.Products.FirstOrDefault(p => p.Name == name);

        if (product is null)
        {
            Console.WriteLine("Не удалось удалить. Продукт не был найден");

            return;
        }

        shopDb.Remove(product);
        shopDb.SaveChanges();

        Console.WriteLine("Продукт удален");
    }

    private static void SetLinqQueries(ShopDbContext shopDb)
    {
        var productsArray = shopDb.Products
            .Include(op => op.OrderProducts)
            .ToArray();

        var mostPurchasedProduct = productsArray
            .OrderByDescending(p => p.OrderProducts.Count)
            .FirstOrDefault(); ;

        if (mostPurchasedProduct is null)
        {
            Console.WriteLine("Список товаров пуст");
        }
        else
        {
            Console.WriteLine("Самый часто покупаемый товар: {0}", mostPurchasedProduct.Name);
        }

        var ordersArray = shopDb.Orders
            .Include(o => o.OrderProducts)
            .ToArray();

        var buyersAndSpentMoneySumDictionary = ordersArray
            .Select(o => new
            {
                Id = o.BuyerId,
                Sum = o.OrderProducts.Sum(op => op.Product!.Price)
            })
            .ToDictionary(b => b.Id, b => b.Sum);

        if (buyersAndSpentMoneySumDictionary.Count == 0)
        {
            Console.WriteLine("Коллекция пуста");
        }
        else
        {
            foreach (var buyer in buyersAndSpentMoneySumDictionary)
            {
                Console.WriteLine("Покупатель {0}, сумма заказа {1}", buyer.Key, buyer.Value);
            }
        }

        var categoriesArray = shopDb.Categories
            .Include(p => p.Products)
                .ThenInclude(p => p.OrderProducts)
            .ToArray();

        var categoryAndPurchasedProductsCountDictionary = categoriesArray
            .ToDictionary(c => c, c => c.Products.Sum(p => p.OrderProducts.Count));

        if (categoryAndPurchasedProductsCountDictionary.Count == 0)
        {
            Console.WriteLine("Коллекция пуста");
        }
        else
        {
            foreach (var category in categoryAndPurchasedProductsCountDictionary)
            {
                Console.WriteLine("Категория: {0}, товаров куплено: {1}", category.Key.Name, category.Value);
            }
        }
    }

    public static void Main(string[] args)
    {
        try
        {
            using var shopDb = new ShopDbContext();

            shopDb.Database.Migrate();

            AddContext(shopDb);

            UpdateProductPrice(shopDb, "Вода", 20);
            DeleteProduct(shopDb, "Вода");

            SetLinqQueries(shopDb);
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Выполнился некорректный запрос к БД или произошла ошибка соединения с БД. {e}");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"Ошибка прав доступа к БД или данная БД сейчас используется другим пользователем. {e}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Произошла ошибка: {e}");
        }
    }
}