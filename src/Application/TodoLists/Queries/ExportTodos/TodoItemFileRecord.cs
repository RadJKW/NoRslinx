using NoRslinx.Application.Common.Mappings;
using NoRslinx.Domain.Entities;

namespace NoRslinx.Application.TodoLists.Queries.ExportTodos;
public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; set; }

    public bool Done { get; set; }
}
