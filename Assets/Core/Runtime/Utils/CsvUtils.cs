using System;
using System.Linq;

namespace Core.Runtime.Utils
{
    public static class CsvUtils
    {
        public static (int rows, int columns) GetCsvDimensions(string csv, bool includeHeader = true)
        {
            if (csv == null || string.IsNullOrEmpty(csv))
                throw new Exception("Error: no csv provided, or csv empty for dimensions analysis.");

            var lines = csv.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var rows = lines.Length;

            var columns = 0;
            if (rows > 0)
                columns = lines[0].Split(GetCsvSeparator(csv)).Length;

            return (rows - (includeHeader ? 0 : 1), columns);
        }
        
        public static char GetCsvSeparator(string csvHeader)
        {
            if (csvHeader is null || csvHeader.Length == 0)
                throw new Exception("Error: no header provided for csv separator analysis.");
            return csvHeader.Count(c => c == ',') > csvHeader.Count(c => c == ';')  ? ',' : ';';
        }
    }
}