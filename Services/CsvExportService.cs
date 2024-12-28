using CsvHelper;
using System.Globalization;

namespace Land.Services
{
    public class CsvExportService
    {
        public void ExportToCsv<T>(IEnumerable<T> data, string filePath)
        {
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(data);
        }
    }

}
