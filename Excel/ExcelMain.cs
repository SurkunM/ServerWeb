using ClosedXML.Excel;

namespace Excel;

internal class ExcelMain
{
    private static void ConvertToExcelTable(List<Person> personsList)
    {
        ArgumentNullException.ThrowIfNull(personsList);

        using var workbook = new XLWorkbook();

        var worksheet = workbook.AddWorksheet();
        worksheet.Range("A1:D1").Merge();

        worksheet.Cell("A1").SetValue("Сотрудники");
        worksheet.Cell("A1").Style.Font.FontSize = 12;
        worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        var table = worksheet.Cell("A2").InsertTable(personsList, "Сотрудники");

        table.Field("LastName").Name = "Фамилия";
        table.Field("FirstName").Name = "Имя";
        table.Field("Age").Name = "Возраст";
        table.Field("Phone").Name = "Телефон";

        table.Rows().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        table.ShowAutoFilter = false;
        table.Theme = XLTableTheme.TableStyleMedium5;

        worksheet.Columns().AdjustToContents();
        workbook.SaveAs("..\\..\\..\\ExcelFiles\\result.xlsx");
    }

    static void Main(string[] args)
    {
        var personsList = new List<Person>
        {
            new("Иван", "Иванов", 44, "23001"),
            new("Николай", "Абрамов", 35, "23011"),
            new("Василий", "Степанов", 22, "23002"),
            new("Петр", "Степанов", 25, "23004"),
            new("Анна", "Абрамова", 31, "23012"),
            new("Ольга", "Степанова", 42, "23038")
        };

        try
        {
            ConvertToExcelTable(personsList);
        }
        catch (IOException)
        {
            Console.WriteLine("Произошла ошибка при попытке сохранения excel-файла. Недостаточно прав для сохранения или файл уже открыт другим процессом.");
        }
        catch (Exception)
        {
            Console.WriteLine("В работе программы произошла ошибка.");
        }
    }
}