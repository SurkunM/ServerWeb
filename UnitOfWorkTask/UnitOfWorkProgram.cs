using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UnitOfWorkTask.Model.Entities;
using UnitOfWorkTask.Model.Repositories;
using UnitOfWorkTask.Model.RepositoryAbstractions.Interfaces;
using UnitOfWorkTask.Model.UnitOfWork;
using UnitOfWorkTask.Model.UnitOfWorkAbstractions;

namespace UnitOfWorkTask;

internal class UnitOfWorkProgram
{
    private static void EditCustomerEmail(IUnitOfWorkTransaction uow, int id, string newEmail)
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

    private static Product? GetMostPurchasedProduct(IUnitOfWorkTransaction uow)
    {
        return uow.GetRepository<IProductRepository>().GetMostPurchasedProduct();
    }

    private static Dictionary<Customer, decimal> GetCustomersAndSpentMoneySumDictionary(IUnitOfWorkTransaction uow)
    {
        return uow.GetRepository<IOrderRepository>().GetCustomersAndSpentMoneySumDictionary();
    }

    private static Dictionary<Category, int> GetCategoryAndPurchasedProductsCountDictionary(IUnitOfWorkTransaction uow)
    {
        return uow.GetRepository<ICategoryRepository>().GetCategoryAndPurchasedProductsCountDictionary();
    }

    private static IServiceProvider DependencyInjection(IServiceCollection serviceCollection, IConfigurationRoot configuration)
    {
        serviceCollection.AddDbContext<ShopDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("ShopConnection"));
            options.UseLazyLoadingProxies();
        }, ServiceLifetime.Transient, ServiceLifetime.Transient);

        serviceCollection.AddTransient<DbInitializer>();
        serviceCollection.AddTransient<IUnitOfWorkTransaction, UnitOfWork>();

        serviceCollection.AddTransient<ICategoryRepository, CategoryRepository>();
        serviceCollection.AddTransient<ICustomerRepository, CustomerRepository>();
        serviceCollection.AddTransient<IProductRepository, ProductRepository>();
        serviceCollection.AddTransient<IOrderRepository, OrderRepository>();

        return serviceCollection.BuildServiceProvider();
    }

    public static void Main(string[] args)
    {
        try
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceCollection = new ServiceCollection();
            var serviceProvider = DependencyInjection(serviceCollection, configuration);

            using var scope = serviceProvider.CreateScope();

            try
            {
                var dbInitializer = serviceProvider.GetRequiredService<DbInitializer>();
                dbInitializer.Initialize();
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<UnitOfWorkProgram>>();
                logger.LogError(ex, "При создании базы данных произошла ошибка.");

                throw;
            }

            var uow = serviceProvider.GetRequiredService<IUnitOfWorkTransaction>();

            //EditCustomerEmail(uow, 1, "Di_Test@email.ru");
            //DeleteProduct(uow, 6);

            var mostPurchasedProduct = GetMostPurchasedProduct(uow);

            if (mostPurchasedProduct is null)
            {
                Console.WriteLine("Список товаров пуст");
            }
            else
            {
                Console.WriteLine("Самый часто покупаемый товар: {0}", mostPurchasedProduct.Name);
            }

            Console.WriteLine();

            var customersAndSpentMoneySumDictionary = GetCustomersAndSpentMoneySumDictionary(uow);

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

            var categoryAndPurchasedProductsCountDictionary = GetCategoryAndPurchasedProductsCountDictionary(uow);

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
        catch (Exception)
        {
            Console.WriteLine($"Ошибка в работе программы.");
        }
    }
}