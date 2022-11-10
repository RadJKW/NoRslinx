using Microsoft.AspNetCore.Mvc;
using NoRslinx.Application.PlcTags
    ;
namespace WebApi.Controllers;
public class PlcTagsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<TagList>> Get()
    {
        return await Mediator.Send(new GetTagsQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDetailsDto>> Get(int id)
    {
        return await Mediator.Send(new GetTagDetailsQuery(id));
    }

    /// <summary>
    /// Exports a Plc{id}'s Tags to CSV file
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("file/{id}")]
    public async Task<FileResult> GetFile(int id)
    {
        var vm = await Mediator.Send(new ExportPlcTagsQuery(PlcId: id));

        return File(vm.Content, vm.ContentType, vm.FileName);
    }


}
