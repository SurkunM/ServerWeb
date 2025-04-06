using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShopEF.Model;

namespace ShopEF;

public class ShopDbProgram
{
    private static Product CreateProduct(Category category, string name, decimal price)
    {
        return new Product
        {
            Name = name,
            Price = price,
            Category = category
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

    private static OrderProduct CreateOrderProducts(Order order, Product product, int count)
    {
        return new OrderProduct
        {
            Product = product,
            Order = order,
            ProductCount = count
        };
    }

    private static void CrateAndAddData(ShopDbContext shopDb)
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

        shopDb.SaveChanges();
    }

    private static void UpdateProductPrice(ShopDbContext shopDb, string name, decimal price)
    {
        var product = shopDb.Products.FirstOrDefault(p => p.Name == name);

        if (product is null)
        {
            Console.WriteLine("Ошибка! Продукт не был найден");

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
            Console.WriteLine("Ошибка! Продукт не был найден");

            return;
        }

        shopDb.Remove(product);
        shopDb.SaveChanges();

        Console.WriteLine("Продукт удален");
    }

    private static Product? GetMostPurchasedProduct(ShopDbContext shopDb)
    {
        return shopDb.OrderProducts
            .OrderByDescending(op => op.ProductCount)
            .Select(op => op.Product)
            .FirstOrDefault();
    }

    private static Dictionary<Customer, decimal> GetCustomersAndSpentMoneySumDictionary(ShopDbContext shopDb)
    {
        return shopDb.Orders
            .Include(o => o.OrderProducts)
            .Include(o => o.Customer)
            .Select(o => new
            {
                Id = o.Customer,
                Sum = o.OrderProducts.Sum(op => op.ProductCount * op.Product.Price)
            })
            .ToDictionary(b => b.Id, b => b.Sum);
    }

    private static Dictionary<Category, int> GetCategoryAndPurchasedProductsCountDictionary(ShopDbContext shopDb)
    {
        return shopDb.OrderProducts
            .Include(op => op.Product)
                .ThenInclude(p => p.Category)
            .GroupBy(op => op.Product.Category)
            .ToDictionary(c => c.Key, op => op.Sum(op => op.ProductCount));
    }

    public static void Main(string[] args)
    {
        try
        {
            using var shopDb = new ShopDbContext();

            shopDb.Database.EnsureCreated();

            CrateAndAddData(shopDb);

            UpdateProductPrice(shopDb, "Вода", 20);
            DeleteProduct(shopDb, "Вода");

            var mostPurchasedProduct = GetMostPurchasedProduct(shopDb);

            if (mostPurchasedProduct is null)
            {
                Console.WriteLine("Список товаров пуст");
            }
            else
            {
                Console.WriteLine("Самый часто покупаемый товар: {0}", mostPurchasedProduct.Name);
            }

            Console.WriteLine();

            var customersAndSpentMoneySumDictionary = GetCustomersAndSpentMoneySumDictionary(shopDb);

            if (customersAndSpentMoneySumDictionary.Count == 0)
            {
                Console.WriteLine("Коллекция пуста");
            }
            else
            {
                foreach (var customer in customersAndSpentMoneySumDictionary)
                {
                    Console.WriteLine("Покупатель {0}, сумма заказа {1:f2}", customer.Key.FirstName, customer.Value);
                }
            }

            Console.WriteLine();

            var categoryAndPurchasedProductsCountDictionary = GetCategoryAndPurchasedProductsCountDictionary(shopDb);

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
        catch (SqlException)
        {
            Console.WriteLine("Выполнился некорректный запрос к БД или произошла ошибка соединения с БД.");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Ошибка прав доступа к БД или данная БД сейчас используется другим пользователем.");
        }
        catch (Exception)
        {
            Console.WriteLine("Произошла ошибка.");
        }
    }
}