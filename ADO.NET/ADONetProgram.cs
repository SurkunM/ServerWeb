using Microsoft.Data.SqlClient;
using System.Data;

namespace ADO.NET;

internal class ADONetProgram
{
    private static int GetProductsCount(SqlConnection connection)
    {
        var sql = @"
        SELECT COUNT(*) 
        FROM Product;
        ";

        using var command = new SqlCommand(sql, connection);

        return (int)command.ExecuteScalar();
    }

    private static void PrintDataReaderProductsAndCategoryNames(SqlConnection connection)
    {
        var sql = @"
        SELECT p.Id, p.Name, p.Price, c.Name 
        FROM Product p 
        INNER JOIN Category c 
            ON p.CategoryId = c.Id;
        ";

        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"id: {reader[0]}, продукт: {reader[1]}, цена: {reader[2]}, категория: {reader[3]}");
        }
    }

    private static void PrintDataSetProductsAndCategoryNames(SqlConnection connection)
    {
        var sql = @"
        SELECT p.Id, p.Name, p.Price, c.Name AS CategoryName
        FROM Product p 
        INNER JOIN Category c 
            ON p.CategoryId = c.Id;
        ";

        var dataSet = new DataSet();

        using var dataAdapter = new SqlDataAdapter(sql, connection);
        dataAdapter.Fill(dataSet);

        foreach (DataTable table in dataSet.Tables)
        {
            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine($"id: {row["Id"]}, продукт: {row["Name"]}, цена: {row["Price"]}, категория: {row["CategoryName"]}");
            }
        }
    }

    private static void CreateProduct(SqlConnection connection, string name, int categoryId, decimal price)
    {
        var sql = @"
        INSERT INTO Product(Name, CategoryId, Price) 
        VALUES (@productName, @productCategoryId, @productPrice);
        ";

        using var command = new SqlCommand(sql, connection);

        command.Parameters.Add(new SqlParameter("@productName", SqlDbType.NVarChar)
        {
            Value = name
        });

        command.Parameters.Add(new SqlParameter("@productCategoryId", SqlDbType.Int)
        {
            Value = categoryId
        });

        command.Parameters.Add(new SqlParameter("@productPrice", SqlDbType.Decimal)
        {
            Value = price
        });

        command.ExecuteNonQuery();
    }

    private static void DeleteProduct(SqlConnection connection, string productName)
    {
        var sql = @"
        DELETE FROM Product 
        WHERE Name = @productName;
        ";

        using var command = new SqlCommand(sql, connection);

        command.Parameters.Add(new SqlParameter("@productName", SqlDbType.NVarChar)
        {
            Value = productName
        });

        command.ExecuteNonQuery();
    }

    private static void CreateCategory(SqlConnection connection, string categoryName)
    {
        var sql = @"
        INSERT INTO Category(Name) 
        VALUES (@categoryName);
        ";

        using var command = new SqlCommand(sql, connection);

        command.Parameters.Add(new SqlParameter("@categoryName", SqlDbType.NVarChar)
        {
            Value = categoryName
        });

        command.ExecuteNonQuery();
    }

    private static void EditProductPrice(SqlConnection connection, string productName, decimal price)
    {
        var sql = @"
        UPDATE Product 
        SET Price = @price 
        WHERE Name = @productName;
        ";

        using var command = new SqlCommand(sql, connection);

        command.Parameters.Add(new SqlParameter("@price", SqlDbType.Decimal)
        {
            Value = price
        });

        command.Parameters.Add(new SqlParameter("@productName", SqlDbType.NVarChar)
        {
            Value = productName
        });

        command.ExecuteNonQuery();
    }

    public static void Main(string[] args)
    {
        var connectionString = "Server=.;Initial Catalog=Shop;Integrated Security=true;TrustServerCertificate=True;";

        try
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            CreateCategory(connection, "Молочные продукты");
            CreateCategory(connection, "Напитки");
            CreateCategory(connection, "Фрукты");

            CreateProduct(connection, "Молоко", 1, 20);
            CreateProduct(connection, "Вода", 2, 5);
            CreateProduct(connection, "Яблоко", 3, 22);
            CreateProduct(connection, "Груша", 3, 32);

            EditProductPrice(connection, "Яблоко", 65);
            DeleteProduct(connection, "Груша");

            Console.WriteLine("Количество продуктов: {0}", GetProductsCount(connection));

            PrintDataReaderProductsAndCategoryNames(connection);
            Console.WriteLine();

            PrintDataSetProductsAndCategoryNames(connection);
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