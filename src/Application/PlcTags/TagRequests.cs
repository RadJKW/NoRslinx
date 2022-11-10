using AutoMapper;
using MediatR;
using NoRslinx.Application.Common.Interfaces;
using NoRslinx.Application.Common.Mappings;
using NoRslinx.Domain.Entities;


namespace NoRslinx.Application.PlcTags;
#region Create
public record CreateTagCommand : IRequest<int>
{

}

#endregion

#region Read/Get
#endregion

#region Update
public record UpdateTagCommand : IRequest
{

}
#endregion

#region Delete
public record DeleteTagCommand(int Id) : IRequest;
#endregion

#region ImportTags
#endregion

#region ExportTags
#endregion

#region Requests(Model)/DTOs

/// <summary>
/// Minimal PlcTag Properties
/// </summary>
public class BreifPlcTagDto : IMapFrom<PlcTag>
{
    public int Id { get; set; }
    public int PlcId { get; set; }
    public string? SymbolName { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }

}

/// <summary>
/// All PlcTag Properties 
/// </summary>
public class DetailsTagDto : IMapFrom<PlcTag>
{
    public int Id { get; set; }
    public int PlcId { get; set; }
    public string? SymbolName { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }
    public int TagTypeId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<PlcTag, DetailsTagDto>()
            .ForMember(d => d.TagTypeId, opt => opt.MapFrom(s => (int)s.TagTypeId));
    }
}
#endregion

#region Eventhandlers
#endregion
