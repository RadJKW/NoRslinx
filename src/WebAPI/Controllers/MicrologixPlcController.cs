using Microsoft.AspNetCore.Mvc;
using NoRslinx.Application.Common.Models;
using NoRslinx.Application.MicrologixPlcs;
namespace WebApi.Controllers;

public class MicrologixPlcController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PlcList>> ListAllPlcs()
    {
        return await Mediator.Send(new GetPlcsQuery());

    }
    [HttpGet("{id}")]
    public async Task<ActionResult<PlcListDetails>> ListPlcDetails(int id)
    {
        return await Mediator.Send(new GetPlcQuery(id));
    }

}
