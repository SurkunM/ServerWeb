using Microsoft.Data.SqlClient;

namespace Transactions;

internal class TransactionsProgram
{
    static private void CreateCategoryUsingTransaction(SqlConnection connection)
    {
        var transaction = connection.BeginTransaction();

        try
        {
            var sql1 = "INSERT INTO Category(Name) " +
                "VALUES(N'Зерновые')";

            using var command = new SqlCommand(sql1, connection);

            command.Transaction = transaction;
            command.ExecuteNonQuery();

            throw new Exception();
        }
        catch (Exception)
        {
            transaction.Rollback();
            Console.WriteLine("Транзакция прервана! Произошла ошибка при создания новой 'категории'");
        }
    }

    static private void CreateCategoryWithoutUsingTransaction(SqlConnection connection)
    {
        var sql1 = "INSERT INTO Category(Name) " +
            "VALUES(N'Морепродукты')";

        using var command = new SqlCommand(sql1, connection);

        command.ExecuteNonQuery();

        throw new Exception();
    }

    static void Main(string[] args)
    {
        var connectionString = @"Server=.;Initial Catalog=Shop;Integrated Security=true;TrustServerCertificate=True;";

        try
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            CreateCategoryUsingTransaction(connection);
            CreateCategoryWithoutUsingTransaction(connection);
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