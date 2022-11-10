using Microsoft.EntityFrameworkCore;
using NoRslinx.Domain.Entities;

namespace NoRslinx.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<MicrologixPlc> MicrologixPlcs { get; }

    DbSet<PlcTag> PlcTags { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
