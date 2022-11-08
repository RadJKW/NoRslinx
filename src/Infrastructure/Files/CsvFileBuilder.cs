using System.Globalization;
using CsvHelper;
using NoRslinx.Application.Common.Interfaces;
using NoRslinx.Application.TodoLists.Queries.ExportTodos;
using NoRslinx.Infrastructure.Files.Maps;

namespace NoRslinx.Infrastructure.Files;
public class CsvFileBuilder : ICsvFileBuilder
{
    public byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records)
    {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Configuration.RegisterClassMap<TodoItemRecordMap>();
            csvWriter.WriteRecords(records);
        }

        return memoryStream.ToArray();
    }
}
