using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoRslinx.Application.Common.Models;
using NoRslinx.Application.TodoItems.Commands.CreateTodoItem;
using NoRslinx.Application.TodoItems.Commands.DeleteTodoItem;
using NoRslinx.Application.TodoItems.Commands.UpdateTodoItem;
using NoRslinx.Application.TodoItems.Commands.UpdateTodoItemDetail;
using NoRslinx.Application.TodoItems.Queries.GetTodoItemsWithPagination;

namespace WebApi.Controllers;
public class TodoItemsController : ApiControllerBase
{
    /// <summary>
    /// Get All TodoItems With Pagination
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<PaginatedList<TodoItemBriefDto>>> GetTodoItemsWithPagination([FromQuery] GetTodoItemsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Create TodoItem
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateTodoItemCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Update TodoItem
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateTodoItemCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }


    /// <summary>
    /// Update TodoItem's Details
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("[action]")]
    public async Task<ActionResult> UpdateItemDetails(int id, UpdateTodoItemDetailCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    /// <summary>
    /// Delete a TodoItem
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteTodoItemCommand(id));

        return NoContent();
    }
}
