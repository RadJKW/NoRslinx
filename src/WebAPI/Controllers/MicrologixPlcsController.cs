using Microsoft.AspNetCore.Mvc;
using NoRslinx.Application.Common.Models;
using NoRslinx.Application.MicrologixPlcs;
namespace WebApi.Controllers;

public class MicrologixPlcsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PlcList>> Get()
    {
        return await Mediator.Send(new GetPlcsQuery());

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlcDetailsList>> Get(int id)
    {
        return await Mediator.Send(new GetPlcQuery(id));
    }

    /// <summary>
    /// Create Micrologix PLC
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreatePlcCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Update Micrologix PLC
    /// </summary>
    /// <param name="id"></param>
    /// <param name="plc"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<PlcDto>> Update(int id, UpdatePlcDto plc)
    {

        var command = new UpdatePlcCommand(id, plc);

        if (id != command.Id)
        {
            return BadRequest();
        }

        return Ok(await Mediator.Send(command));

    }

    /// <summary>
    /// Delete Micrologix PLC
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeletePlcCommand(id));

        return NoContent();
    }

}
