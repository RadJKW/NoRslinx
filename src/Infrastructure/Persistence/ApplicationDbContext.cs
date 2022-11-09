using System.Reflection;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NoRslinx.Application.Common.Interfaces;
using NoRslinx.Domain.Entities;
using NoRslinx.Domain.Enums;
using NoRslinx.Infrastructure.Common;
using NoRslinx.Infrastructure.Persistence.Interceptors;

namespace NoRslinx.Infrastructure.Persistence;
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<MicrologixPlc> MicrologixPlcs => Set<MicrologixPlc>();

    public DbSet<PlcTag> PlcTags => Set<PlcTag>();

    public DbSet<TagType> TagTypes => Set<TagType>();



    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
