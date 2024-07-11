using LocaleConverter;
using System.Xml.Serialization;

// 최신 버전의 .Net에서 ExcelDataReader를 사용하기 위해 필요한 조치
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

var root = args.Length > 0 ? args[0] : @"D:\Mines\MinesClient\Locale";
var output = args.Length > 1 ? args[1] : @"D:\Mines\MinesClient\Locale";

Directory.CreateDirectory(output);

var file = new ExcelFile(Path.GetFullPath(Path.Combine(root, "Locale.xlsx")));
var ds = file.Import();

Console.WriteLine("Convert to C# Struct...");
{
    var builder = new LocaleStringTableBuilder();

    var columns = ds.Tables[0].Columns;
    var keyColumn = columns[0];
    var rows = ds.Tables[0].Select();
    foreach (var row in rows)
    {
        for (int i = 1; i < columns.Count; ++i)
        {
            var keyObject = row[keyColumn];
            if (keyObject == DBNull.Value)
                continue;

            var column = columns[i];
            var valueObject = row[column];
            if (valueObject == DBNull.Value)
                valueObject = "";

            builder[column.ColumnName].Add(new LocaleString()
            {
                Key = keyObject?.ToString() ?? string.Empty,
                Value = valueObject?.ToString() ?? string.Empty,
            });
        }
    }

    foreach (var kvp in builder.All)
    {
        using var fs = File.Create(Path.Combine(output, $"{kvp.Key}.xml"));
        var s = new XmlSerializer(kvp.Value.GetType());
        s.Serialize(fs, kvp.Value);
    }
}
Console.WriteLine("Complete.");
