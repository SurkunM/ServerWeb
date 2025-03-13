using Microsoft.Data.SqlClient;
using System.Data;

namespace ADO.NET;

internal class ADONetMain
{
    static private int GetProductsCount(SqlConnection connection)
    {
        var sql = $"SELECT COUNT(*) FROM Product";

        using var command = new SqlCommand(sql, connection);

        return (int)command.ExecuteScalar();
    }

    static private void PrintDataReaderProductsAndCategoryNames(SqlConnection connection)
    {
        var sql = "SELECT p.Id, p.Name, p.Price, c.Name " +
            "FROM Product p " +
            "INNER JOIN Category c " +
                "ON p.CategoryId = c.Id";

        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"id: {reader[0]}, продукт: {reader[1]}, цена: {reader[2]}, категория: {reader[3]}");
        }
    }

    static private void PrintDataSetProductsAndCategoryNames(SqlConnection connection)
    {
        var sql = "SELECT p.Id AS Id, p.Name AS Name, p.Price AS Price, c.Name AS CategoryName " +
            "FROM Product p " +
            "INNER JOIN Category c " +
                "ON p.CategoryId = c.Id";

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

    static private void CreateProduct(SqlConnection connection, string name, int categoryId, decimal price)
    {
        var sql = "INSERT INTO Product(Name, CategoryId, Price) " +
            "VALUES (@productName, @productCategoryId, @productPrice)";

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

    static private void DeleteProduct(SqlConnection connection, string productName)
    {
        var sql = $"DELETE TOP(1) FROM Product " +
            "WHERE Name = @productName";

        using var command = new SqlCommand(sql, connection);

        command.Parameters.Add(new SqlParameter("@productName", SqlDbType.NVarChar)
        {
            Value = productName
        });

        command.ExecuteNonQuery();
    }

    static private void CreateCategory(SqlConnection connection, string categoryName)
    {
        var sql = "INSERT INTO Category(Name) " +
            "VALUES (@categoryName)";

        using var command = new SqlCommand(sql, connection);

        command.Parameters.Add(new SqlParameter("@categoryName", SqlDbType.NVarChar)
        {
            Value = categoryName
        });

        command.ExecuteNonQuery();
    }

    static private void EditProductPrice(SqlConnection connection, string productName, decimal price)
    {
        var sql = "UPDATE Product " +
            "SET Price = @price " +
            "WHERE Name = @productName";

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

    static void Main(string[] args)
    {
        var connectionString = @"Server=.;Initial Catalog=Shop;Integrated Security=true;TrustServerCertificate=True;";

        try
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var productCount = GetProductsCount(connection);
            Console.WriteLine("Количество продуктов: {0}", productCount);

            CreateCategory(connection, "Молочные продукты");
            CreateProduct(connection, "Груша", 1, 32);

            EditProductPrice(connection, "Яблоко", 55);
            DeleteProduct(connection, "Груша");

            PrintDataReaderProductsAndCategoryNames(connection);
            Console.WriteLine();

            PrintDataSetProductsAndCategoryNames(connection);
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