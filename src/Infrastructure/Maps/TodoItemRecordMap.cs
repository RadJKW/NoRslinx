using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper.Configuration;
using NoRslinx.Application.TodoLists.Queries.ExportTodos;

namespace NoRslinx.Infrastructure.Maps;
public partial class TodoItemRecordMap : ClassMap<TodoItemRecord>
{

    public TodoItemRecordMap()
    {


        AutoMap(CultureInfo.InvariantCulture);

        // convert the TodoItemRecord from bool to string
        Map(m => m.Done).Convert(c => c.Value.Done ? "Yes" : "No");
        Map(m => m.Title).Convert(c => EmojiRegex().Replace(c.Value.Title!, string.Empty));

    }

    [GeneratedRegex("[^\\u0000-\\u007F]+", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex EmojiRegex();
}
