using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NoRslinx.Application.Common.Interfaces;
using NoRslinx.Application.Common.Models;

namespace NoRslinx.Application.TodoLists.Queries.ExportTodos;
public record ExportTodosQuery(int ListId) : IRequest<CsvFileVm>;

public class ExportTodosQueryHandler : IRequestHandler<ExportTodosQuery, CsvFileVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICsvService _fileBuilder;

    public ExportTodosQueryHandler(IApplicationDbContext context, IMapper mapper, ICsvService fileBuilder)
    {
        _context = context;
        _mapper = mapper;
        _fileBuilder = fileBuilder;
    }

    public async Task<CsvFileVm> Handle(ExportTodosQuery request, CancellationToken cancellationToken)
    {
        var records = await _context.TodoItems
                .Where(t => t.ListId == request.ListId)
                .ProjectTo<TodoItemRecord>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        var vm = new CsvFileVm
        {
            FileName = "TodoItems.csv",
            ContentType = "text/csv",
            Content = _fileBuilder.BuildTodoItemsFile(records)
        };

        return vm;
    }
}
