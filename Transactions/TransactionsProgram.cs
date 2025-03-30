using Microsoft.Data.SqlClient;
using System.Data;

namespace Transactions;

internal class TransactionsProgram
{
    private static void CreateCategoryUsingTransaction(SqlConnection connection, string categoryName)
    {
        using var transaction = connection.BeginTransaction();

        try
        {
            var categoryCreateSql = """
            INSERT INTO Category(Name)
            VALUES (@categoryName);
            """;

            using var command = new SqlCommand(categoryCreateSql, connection);

            command.Parameters.Add(new SqlParameter("@categoryName", SqlDbType.NVarChar)
            {
                Value = categoryName
            });

            command.Transaction = transaction;
            command.ExecuteNonQuery();

            throw new Exception();

            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
        }
    }

    private static void CreateCategoryWithoutTransaction(SqlConnection connection, string categoryName)
    {
        var categoryCreateSql = """
        INSERT INTO Category(Name) 
        VALUES (@categoryName);
        """;

        using var command = new SqlCommand(categoryCreateSql, connection);

        command.Parameters.Add(new SqlParameter("@categoryName", SqlDbType.NVarChar)
        {
            Value = categoryName
        });

        command.ExecuteNonQuery();

        throw new Exception();
    }

    public static void Main(string[] args)
    {
        var connectionString = "Server=.;Initial Catalog=Shop;Integrated Security=true;TrustServerCertificate=True;";

        try
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            CreateCategoryUsingTransaction(connection, "Хоз.товары");
            CreateCategoryWithoutTransaction(connection, "Морепродукты");
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
            Console.WriteLine($"Транзакция прервана! Произошла ошибка: {e}");
        }
    }
}