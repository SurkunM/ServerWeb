using Microsoft.Data.SqlClient;
using ShopEF.Model;

namespace ShopEF;

internal class ShopProgram
{
    static private Product CrateProduct(ShopDbContext db, Category category, string name, decimal price)
    {
        var product = new Product
        {
            Name = name,
            Price = price,
            Category = category
        };

        db.Products.Add(product);

        return product;
    }

    static private Buyer CreateBuyer(string firstName, string lastName, string middleName, string email, int phone)
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

    static void Main(string[] args)
    {
        try
        {
            using var shopDb = new ShopDbContext();

            shopDb.Database.EnsureDeleted();
            shopDb.Database.EnsureCreated();

            var category1 = new Category
            {
                Name = "Морепродукты"
            };

            var product1 = CrateProduct(shopDb, category1, "Минтай", 35);
            var product2 = CrateProduct(shopDb, category1, "Морская капуста", 25);

            var category2 = new Category
            {
                Name = "Напитки"
            };

            var product3 = CrateProduct(shopDb, category2, "Чай", 15);
            var product4 = CrateProduct(shopDb, category2, "Вода", 5);

            var category3 = new Category
            {
                Name = "Молочные продукты"
            };

            var product5 = CrateProduct(shopDb, category3, "Молоко", 25);
            var product6 = CrateProduct(shopDb, category3, "Сыр", 20);

            var buyer1 = CreateBuyer("Иван", "Иванов", "Иванович", "Ivanov@mail.ru", 5123);
            var buyer2 = CreateBuyer("Степан", "Степанов", "Степанович", "S2000@mail.ru", 2000);

            var order1 = new Order
            {
                Buyer = buyer1,
                OrderDate = new DateTime(2025, 3, 15, 12, 30, 02),
                Products = new List<Product>
                {
                    product1,
                    product3,
                    product5
                }
            };

            shopDb.Orders.Add(order1);

            var order2 = new Order
            {
                Buyer = buyer2,
                OrderDate = new DateTime(2025, 3, 14, 10, 00, 48),
                Products = new List<Product>
                {
                    product1,
                    product2,
                    product3,
                    product6
                }
            };

            shopDb.Orders.Add(order2);

            shopDb.SaveChanges();
        }
        catch (SqlException)
        {
            Console.WriteLine("Выполнился не корректный запрос к БД или произошла ошибка соединения с БД.");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Ошибка прав доступа к БД или данное БД сейчас используется другим пользователем.");
        }
    }
}