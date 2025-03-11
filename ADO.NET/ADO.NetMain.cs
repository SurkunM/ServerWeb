using Microsoft.Data.SqlClient;
using System.Data;

namespace ADO.NET;

internal class ADONetMain
{
    static private int GetProductsCount(SqlConnection connection, string table)
    {
        var sql = $"SELECT COUNT(*) FROM {table}";

        using var command = new SqlCommand(sql, connection);

        var result = (int)command.ExecuteScalar();

        return result;
    }

    static private void PrintColumn(SqlConnection connection, string table)
    {
        var sql = $"SELECT * FROM {table}";

        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"{reader[1]}");
        }
    }

    static private void DeleteColumn(SqlConnection connection, string table, string columnName)
    {
        var sql = $"DELETE TOP(1) FROM [Shop].[dbo].[{table}]" +
                $"WHERE Name = '{columnName}'";

        using var command = new SqlCommand(sql, connection);

        command.ExecuteNonQuery();
    }

    static private void CreateColumn(SqlConnection connection, string table, string columnName)
    {
        var sql = $"INSERT INTO dbo.{table}(Name)" +
                " VALUES (@categoryName)";
        using var command = new SqlCommand(sql, connection);

        command.Parameters.Add(new SqlParameter("@categoryName", SqlDbType.NVarChar)
        {
            Value = columnName
        });

        command.ExecuteNonQuery();
    }

    static void Main(string[] args)
    {
        var connectionString = @"Server=.;Initial Catalog=Shop;Integrated Security=true;TrustServerCertificate=True;";

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        var categoryTable = "Category";
        var productTable = "Product";

        Console.WriteLine(GetProductsCount(connection, categoryTable));

        var categoryName = "HouseholdGoods";

        CreateColumn(connection, categoryTable, categoryName);
        DeleteColumn(connection, categoryName, categoryTable);


        PrintColumn(connection, productTable);//Сделать PrintBD, try-catch
    }
}