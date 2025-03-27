using UnitOfWorkTask.Model;
using UnitOfWorkTask.Model.RepositoryAbstractions;
using ShopEF;

namespace UnitOfWorkTask;

internal class UnitOfWorkProgram
{
    private static void EditBuyerEmail(UnitOfWork uow, int id, string newEmail)
    {
        var buyerRep = uow.GetRepository<IBuyerRepository>();
        uow.BeginTransaction();

        try
        {
            var buyer = buyerRep?.GetById(id);

            if (buyer is null)
            {
                uow.RollbackTransaction();

                Console.WriteLine($"Покупатель с id:{id} не найден");

                return;
            }

            buyer.Email = newEmail;

            uow.Save();

            Console.WriteLine("Email покупателя успешно изменен");
        }
        catch (Exception e)
        {
            uow.RollbackTransaction();

            Console.WriteLine($"Ошибка! Транзакция отменена. {e}");

            throw;
        }
    }

    private static void DeleteProduct(UnitOfWork uow, int id)
    {
        var productRep = uow.GetRepository<IProductRepository>();
        uow.BeginTransaction();

        try
        {
            var product = productRep?.GetById(id);

            if (product is null)
            {
                uow.RollbackTransaction();

                Console.WriteLine($"Продукт с id: {id} не найден");

                return;
            }

            productRep?.Delete(product);

            uow.Save();

            Console.WriteLine("Продукт успешно удален");
        }
        catch (Exception e)
        {
            uow.RollbackTransaction();

            Console.WriteLine($"Ошибка! Транзакция отменена. {e}");

            throw;
        }
    }

    private static void SetLinqQueriesAndPrintResults(UnitOfWork uow)
    {
        var productRep = uow.GetRepository<IProductRepository>();
        var mostPurchasedProduct = productRep?.GetMostPurchasedProduct();

        if (mostPurchasedProduct is null)
        {
            Console.WriteLine("Список товаров пуст");
        }
        else
        {
            Console.WriteLine("Самый часто покупаемый товар: {0}", mostPurchasedProduct.Name);
        }

        var ordersRep = uow.GetRepository<IOrderRepository>();
        var buyersAndSpentMoneySumDictionary = ordersRep?.GetBuyersAndSpentMoneySumDictionary();

        if (buyersAndSpentMoneySumDictionary is null)
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

        var categoryRep = uow.GetRepository<ICategoryRepository>();
        var categoryAndPurchasedProductsCountDictionary = categoryRep?.GetCategoryAndPurchasedProductsCountDictionary();

        if (categoryAndPurchasedProductsCountDictionary is null)
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
        using var shopDb = new ShopDbContext();

        try
        {
            using var uow = new UnitOfWork(shopDb);

            EditBuyerEmail(uow, 1, "newEmail@email.ru");
            DeleteProduct(uow, 6);
            SetLinqQueriesAndPrintResults(uow);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка в работе программы: {e}");
        }
    }
}