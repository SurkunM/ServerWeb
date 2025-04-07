using UnitOfWorkTask.Model.Entities;
using UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;
using UnitOfWorkTask.Model.UnitOfWork;

namespace UnitOfWorkTask;

internal class UnitOfWorkProgram
{
    private static void EditCustomerEmail(UnitOfWork uow, int id, string newEmail)
    {
        try
        {
            var customerRepository = uow.GetRepository<ICustomerRepository>();
            uow.BeginTransaction();

            var customer = customerRepository.GetById(id);

            if (customer is null)
            {
                uow.RollbackTransaction();

                Console.WriteLine($"Покупатель с id: {id} не найден");

                return;
            }

            customer.Email = newEmail;

            uow.Save();

            Console.WriteLine("Email покупателя успешно изменен");
        }
        catch (Exception)
        {
            uow.RollbackTransaction();

            Console.WriteLine($"Ошибка! Транзакция отменена.");

            throw;
        }
    }

    private static void DeleteProduct(UnitOfWork uow, int id)
    {
        try
        {
            var productRepository = uow.GetRepository<IProductRepository>();
            uow.BeginTransaction();

            var product = productRepository.GetById(id);

            if (product is null)
            {
                uow.RollbackTransaction();

                Console.WriteLine($"Продукт с id: {id} не найден");

                return;
            }

            productRepository?.Delete(product);

            uow.Save();

            Console.WriteLine("Продукт успешно удален");
        }
        catch (Exception)
        {
            uow.RollbackTransaction();

            Console.WriteLine($"Ошибка! Транзакция отменена.");

            throw;
        }
    }

    private static void SetLinqQueriesAndPrintResults(UnitOfWork uow)//TODO: 8. Нужно будет учесть пункты из задачи ShopEF
    {
        var productRepository = uow.GetRepository<IProductRepository>();
        var mostPurchasedProduct = productRepository.GetMostPurchasedProduct();

        if (mostPurchasedProduct is null)
        {
            Console.WriteLine("Список товаров пуст");
        }
        else
        {
            Console.WriteLine("Самый часто покупаемый товар: {0}", mostPurchasedProduct.Name);
        }

        var ordersRepository = uow.GetRepository<IOrderRepository>();
        var buyersAndSpentMoneySumDictionary = ordersRepository?.GetCustomersAndSpentMoneySumDictionary();

        if (buyersAndSpentMoneySumDictionary is null || buyersAndSpentMoneySumDictionary.Count == 0)
        {
            Console.WriteLine("Список покупок пуст");
        }
        else
        {
            foreach (var buyer in buyersAndSpentMoneySumDictionary)
            {
                Console.WriteLine("Покупатель: {0}, сумма затрат на покупки: {1}", buyer.Key, buyer.Value);
            }
        }

        var categoryRepository = uow.GetRepository<ICategoryRepository>();
        var categoryAndPurchasedProductsCountDictionary = categoryRepository?.GetCategoryAndPurchasedProductsCountDictionary();

        if (categoryAndPurchasedProductsCountDictionary is null || categoryAndPurchasedProductsCountDictionary.Count == 0)
        {
            Console.WriteLine("Коллекция пуста");
        }
        else
        {
            foreach (var category in categoryAndPurchasedProductsCountDictionary)
            {
                Console.WriteLine("Категория: {0}, куплено товаров: {1}", category.Key.Name, category.Value);
            }
        }
    }

    public static void Main(string[] args)
    {
        try
        {
            using var shopDb = new ShopDbContext();
            using var uow = new UnitOfWork(shopDb);

            //shopDb.Database.EnsureCreated();

            EditCustomerEmail(uow, 1, "test@email.ru");
            //DeleteProduct(uow, 6);
            SetLinqQueriesAndPrintResults(uow);
        }
        catch (Exception)
        {
            Console.WriteLine($"Ошибка в работе программы.");
        }
    }

    private static void CrateAndAddData(ShopDbContext shopDb) //TODO: Объявить в начале! Перед Main
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
}