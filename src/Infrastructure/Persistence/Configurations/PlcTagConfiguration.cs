using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoRslinx.Domain.Entities;

namespace NoRslinx.Infrastructure.Persistence.Configurations;
public class PlcTagConfiguration : IEntityTypeConfiguration<PlcTag>
{
    public void Configure(EntityTypeBuilder<PlcTag> builder)
    {
        builder.HasIndex(t => new { t.SymbolName, t.PlcId }).IsUnique();
    }
}
