using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoRslinx.Application.TodoLists.Commands.CreateTodoList;
using NoRslinx.Application.TodoLists.Commands.DeleteTodoList;
using NoRslinx.Application.TodoLists.Commands.UpdateTodoList;
using NoRslinx.Application.TodoLists.Queries.ExportTodos;
using NoRslinx.Application.TodoLists.Queries.GetTodos;

namespace WebApi.Controllers;
public class TodoListsController : ApiControllerBase
{
    /// <summary>
    /// Get All Todos (Virtual Model)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<TodosVm>> Get()
    {
        return await Mediator.Send(new GetTodosQuery());
    }

    /// <summary>
    /// Export single TodoList to CSV file
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<FileResult> Get(int id)
    {
        var vm = await Mediator.Send(new ExportTodosQuery { ListId = id });

        return File(vm.Content, vm.ContentType, vm.FileName);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateTodoListCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateTodoListCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteTodoListCommand(id));

        return NoContent();
    }
}
