using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NoRslinx.Application.Common.Exceptions;
using NoRslinx.Application.Common.Interfaces;
using NoRslinx.Application.Common.Mappings;
using NoRslinx.Application.PlcTags;
using NoRslinx.Domain.Entities;
using NoRslinx.Domain.Enums;

namespace NoRslinx.Application.MicrologixPlcs;
#region Create
public record CreatePlcCommand : IRequest<int>
{
    public string? Name { get; init; }
    public string? IpAddress { get; init; }
    public string? Location { get; init; }
    public string? Description { get; init; }
}
public class CreatePlcCommandHandler : IRequestHandler<CreatePlcCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreatePlcCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePlcCommand request, CancellationToken cancellationToken)
    {
        var entity = new MicrologixPlc
        {
            Name = request.Name,
            IpAddress = request.IpAddress,
            Location = request.Location,
            Description = request.Description
        };

        _context.MicrologixPlcs.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

#endregion

#region GetById
public record GetPlcQuery(int Id) : IRequest<PlcListDetails>;
public class GetPlcQueryHandler : IRequestHandler<GetPlcQuery, PlcListDetails>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPlcQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PlcListDetails> Handle(GetPlcQuery request, CancellationToken cancellationToken)
    {
        return new PlcListDetails
        {
            Plcs = await _context.MicrologixPlcs
                .AsNoTracking()
                .Include(p => p.PlcTags.Where(p => p.PlcId == request.Id))
                .Where(p => p.Id == request.Id)
                .ProjectToListAsync<DetailsPlcDto>(_mapper.ConfigurationProvider)
        };
    }
}

#endregion
#region GetPlcs
public record GetPlcsQuery : IRequest<PlcList>;
public class GetPlcsQueryHandler : IRequestHandler<GetPlcsQuery, PlcList>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPlcsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PlcList> Handle(GetPlcsQuery request, CancellationToken cancellationToken)
    {
        return new PlcList
        {
            Plcs = await _context.MicrologixPlcs
                .AsNoTracking()
                .ProjectTo<PlcDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
        };
    }
}

#endregion

#region Update
public record UpdatePlcCommand : IRequest
{
    public int Id { get; init; }

    public string? Name { get; init; }

    public string? Location { get; init; }

    public string? Description { get; init; }
}

#endregion

#region Delete
public record DeletePlcCommand(int Id) : IRequest;

#endregion


#region ExportPlc
#endregion

#region Requests(Model) / DTOs
public class PlcDto : IMapFrom<MicrologixPlc>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? IpAddress { get; set; }
    public string? Location { get; set; }
    public string? Program { get; set; }

}
public class DetailsPlcDto : IMapFrom<MicrologixPlc>
{

    public DetailsPlcDto()
    {

        Tags = new List<DetailsTagDto>();
    }

    #region ---Properties---
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? IpAddress { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public string? Program { get; set; }

    public int PlcType { get; set; }
    public int Protocol { get; set; }

    //public int Timeout { get; set; }

    public int DebugLevel { get; set; }

    public IList<DetailsTagDto> Tags { get; set; }
    #endregion

    public void Mapping(Profile profile)
    {
        profile.CreateMap<MicrologixPlc, DetailsPlcDto>()
            .ForMember(d => d.PlcType, opt => opt.MapFrom(s => (int)s.PlcType))
            .ForMember(d => d.Protocol, opt => opt.MapFrom(s => (int)s.Protocol))
            //.ForMember(d => d.Timeout, opt => opt.MapFrom(s => (int)s.Timeout))
            .ForMember(d => d.DebugLevel, opt => opt.MapFrom(s => (int)s.DebugLevel))
            .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.PlcTags));
    }

}

public class PlcListDetails
{
    //public IList<PlcType> PlcTypes { get; set; } = new List<PlcType>();
    //public IList<Protocol> PlcProtocols { get; set; } = new List<Protocol>();

    //public IList<DebugLevel> DebugLevels { get; set; } = new List<DebugLevel>();

    //public IList<TagType> TagTypes { get; set; } = new List<TagType>();
    public IList<DetailsPlcDto> Plcs { get; set; } = new List<DetailsPlcDto>();

}

public class PlcList
{
    public IList<PlcDto> Plcs { get; set; } = new List<PlcDto>();
}

#endregion

#region Eventhandlers
#endregion
