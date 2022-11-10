using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NoRslinx.Application.Common.Exceptions;
using NoRslinx.Application.Common.Interfaces;
using NoRslinx.Application.Common.Mappings;
using NoRslinx.Application.PlcTags;
using NoRslinx.Domain.Entities;

namespace NoRslinx.Application.MicrologixPlcs
{
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

    #region GetPlcById
    public record GetPlcQuery(int Id) : IRequest<PlcDetailsList>;
    public class GetPlcQueryHandler : IRequestHandler<GetPlcQuery, PlcDetailsList>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPlcQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PlcDetailsList> Handle(GetPlcQuery request, CancellationToken cancellationToken)
        {
            return new PlcDetailsList
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

    #region CreatePlc
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

    #region Update
    public record UpdatePlcCommand(int Id, UpdatePlcDto EditPlc) : IRequest<PlcDto>;

    public class UpdatePlcCommandHandler : IRequestHandler<UpdatePlcCommand, PlcDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdatePlcCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PlcDto> Handle(UpdatePlcCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.MicrologixPlcs
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(MicrologixPlc), request.Id);
            }

            foreach (var entityProperty in entity.GetType().GetProperties())
            {
                var editProperty = request.EditPlc.GetType().GetProperty(entityProperty.Name);
                if (editProperty != null)
                {
                    var editValue = editProperty.GetValue(request.EditPlc);
                    if (editValue != null)
                    {
                        entityProperty.SetValue(entity, editValue);
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PlcDto>(entity);
        }
    }

    #endregion

    #region Delete
    public record DeletePlcCommand(int Id) : IRequest;

    public class DeletePlcCommandHandler : IRequestHandler<DeletePlcCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeletePlcCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeletePlcCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.MicrologixPlcs
                .Where(p => p.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(MicrologixPlc), request.Id);
            }

            _context.MicrologixPlcs.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

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

            Tags = new List<TagDetailsDto>();
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

        public IList<TagDetailsDto> Tags { get; set; }
        #endregion

        public void Mapping(Profile profile)
        {
            profile.CreateMap<MicrologixPlc, DetailsPlcDto>()
                .ForMember(d => d.PlcType, opt => opt.MapFrom(s => (int)s.PlcType))
                .ForMember(d => d.Protocol, opt => opt.MapFrom(s => (int)s.Protocol))
                .ForMember(d => d.DebugLevel, opt => opt.MapFrom(s => (int)s.DebugLevel))
                .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.PlcTags));
        }
    }

    public class PlcDetailsList
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

    public class UpdatePlcDto
    {
        public string? Name { get; set; }
        public string? IpAddress { get; set; }
        public string? Location { get; set; }
        public string? Program { get; set; }
    }

    #endregion

    #region Validators
    // TODO: implement later
    #endregion
};
