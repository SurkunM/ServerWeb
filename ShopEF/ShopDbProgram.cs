using Microsoft.Data.SqlClient;
using ShopEF.Model;

namespace ShopEF;

public class ShopDbProgram
{
    public static void Main(string[] args)
    {
        try
        {
            using var shopDb = new ShopDbContext();

            var dbInitializer = new DbInitializer(shopDb);
            dbInitializer.Initialize();

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
                Console.WriteLine("Начало вывода:");
                foreach (var customer in customersAndSpentMoneySumDictionary)
                {
                    Console.WriteLine("Покупатель {0}, сумма заказа {1:f2}", customer.Key.FirstName, customer.Value);
                }
                Console.WriteLine("Конец вывода.");
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
            Console.WriteLine("Произошла ошибка в работе программы.");
        }
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
            .GroupBy(op => op.Product)
            .OrderByDescending(g => g.Sum(op => op.ProductsCount))
            .Select(g => g.Key)
            .FirstOrDefault();
    }

    private static Dictionary<Customer, decimal> GetCustomersAndSpentMoneySumDictionary(ShopDbContext shopDb)
    {
        return shopDb.OrderProducts
            .GroupBy(op => op.Order.Customer)
            .Select(g => new
            {
                Customer = g.Key,
                Sum = g.Sum(op => op.Product.Price * op.ProductsCount)
            })
            .ToDictionary(x => x.Customer, y => y.Sum);
    }

    private static Dictionary<Category, int> GetCategoryAndPurchasedProductsCountDictionary(ShopDbContext shopDb)
    {
        return shopDb.ProductCategories
                .Select(pc => new
                {
                    pc.Category,
                    ProductsCount = pc.Product.OrderProducts.Sum(op => op.ProductsCount)
                })
                .GroupBy(x => x.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Sum(pc => pc.ProductsCount)
                })
                .ToDictionary(g => g.Category, g => g.Count);
    }
}