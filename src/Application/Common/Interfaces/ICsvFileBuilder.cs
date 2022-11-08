using NoRslinx.Application.TodoLists.Queries.ExportTodos;

namespace NoRslinx.Application.Common.Interfaces;
public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
