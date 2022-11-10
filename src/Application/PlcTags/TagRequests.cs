using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NoRslinx.Application.Common.Interfaces;
using NoRslinx.Application.Common.Mappings;
using NoRslinx.Application.Common.Models;
using NoRslinx.Application.MicrologixPlcs;
using NoRslinx.Domain.Entities;


namespace NoRslinx.Application.PlcTags
{
    #region Create
    public record CreateTagCommand : IRequest<int>
    {

    }

    #endregion

    #region GetTags<List>
    public record GetTagsQuery : IRequest<TagList>;

    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, TagList>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTagsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TagList> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            return new TagList
            {
                Tags = await _context.PlcTags
                    .AsNoTracking()
                    .ProjectTo<TagDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken)
            };
        }
    }


    #endregion

    #region GetTagDetails
    public record GetTagDetailsQuery(int Id) : IRequest<TagDetailsDto>;

    public class GetTagDetailsQueryHandler : IRequestHandler<GetTagDetailsQuery, TagDetailsDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTagDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TagDetailsDto> Handle(GetTagDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _context.PlcTags
                .Where(p => p.Id == request.Id)
                .ProjectTo<TagDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken) ?? new TagDetailsDto();
        }
    }

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

    #region ExportPlcTags
    public record ExportPlcTagsQuery(int PlcId) : IRequest<CsvFileVm>;

    public class ExportPlcTagsQueryHandler : IRequestHandler<ExportPlcTagsQuery, CsvFileVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICsvService _csvService;

        public ExportPlcTagsQueryHandler(IApplicationDbContext context, IMapper mapper, ICsvService csvService)
        {
            _context = context;
            _mapper = mapper;
            _csvService = csvService;
        }

        public async Task<CsvFileVm> Handle(ExportPlcTagsQuery request, CancellationToken cancellationToken)
        {
            var records = await _context.PlcTags
                .Where(p => p.PlcId == request.PlcId)
                .ProjectTo<TagRecord>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var vm = new CsvFileVm
            {
                FileName = "PlcTags.csv",
                ContentType = "text/csv",
                Content = _csvService.BuildPlcTagsFile(records)
            };

            return vm;
        }
    }
    #endregion

    #region Requests(Model)/DTOs

    /// <summary>
    /// Minimal PlcTag Properties
    /// </summary>
    public class TagDto : IMapFrom<PlcTag>
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
    public class TagDetailsDto : TagDto, IMapFrom<PlcTag>
    {
        // make the TagDto properties appear above TagDetailsDto Properties
        public string? TagType { get; set; }
        public PlcDto Plc { get; set; } = new PlcDto();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PlcTag, TagDetailsDto>()
                .ForMember(d => d.TagType, opt => opt.MapFrom(s => s.TagTypeId.ToString()))
                .ForMember(d => d.Plc, opt => opt.MapFrom(s => s.Plc));
        }
    }

    public class TagList
    {
        public List<TagDto> Tags { get; set; } = new();
    }
    public record TagRecord : IMapFrom<PlcTag>
    {
        public int Id { get; set; }
        public int PlcId { get; set; }
        public string? SymbolName { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? TagType { get; set; }

        public void Mapping(Profile profile)
        {
            // map TagRecord.Tagtype from the Full name of the PlcTag.TagType enum
            profile.CreateMap<PlcTag, TagRecord>()
                .ForMember(d => d.TagType, opt => opt.MapFrom(s => s.TagTypeId.ToString()));

        }

    }

    #endregion
}


