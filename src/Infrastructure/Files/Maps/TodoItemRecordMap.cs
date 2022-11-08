using System.Globalization;
using CsvHelper.Configuration;
using NoRslinx.Application.TodoLists.Queries.ExportTodos;

namespace NoRslinx.Infrastructure.Files.Maps;
public class TodoItemRecordMap : ClassMap<TodoItemRecord>
{
    public TodoItemRecordMap()
    {
        AutoMap(CultureInfo.InvariantCulture);

        // convert the TodoItemRecord from bool to string
        Map(m => m.Done).Convert(c => c.Value.Done ? "Yes" : "No");

    }
}
