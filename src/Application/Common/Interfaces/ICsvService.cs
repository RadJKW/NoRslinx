using NoRslinx.Application.PlcTags;
using NoRslinx.Application.TodoLists.Queries.ExportTodos;

namespace NoRslinx.Application.Common.Interfaces;
public interface ICsvService
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);

    byte[] BuildPlcTagsFile(IEnumerable<TagRecord> records);
}
