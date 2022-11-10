using System.Globalization;
using CsvHelper;
using NoRslinx.Application.Common.Interfaces;
using NoRslinx.Application.PlcTags;
using NoRslinx.Application.TodoLists.Queries.ExportTodos;
using NoRslinx.Infrastructure.Maps;

namespace NoRslinx.Infrastructure.Services;
public class CsvService : ICsvService
{
    /// <summary>
    /// Builds a CSV file of TodoItems to export
    /// </summary>
    /// <param name="records"></param>
    /// <returns></returns>
    public byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records)
    {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            // remove any characters from the strings that are not allowed in CSV files
            csvWriter.Context.RegisterClassMap<TodoItemRecordMap>();
            csvWriter.WriteRecords(records);
        }
        var bytes = memoryStream.ToArray();

        return bytes;
    }

    //implement BuildPlcTagsFile

    public byte[] BuildPlcTagsFile(IEnumerable<TagRecord> records)
    {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            csvWriter.Context.RegisterClassMap<PlcTagRecordMap>();
            csvWriter.WriteRecords(records);
        }


        return memoryStream.ToArray();
    }

}
