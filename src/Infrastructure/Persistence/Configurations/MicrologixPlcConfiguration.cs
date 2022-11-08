// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoRslinx.Domain.Entities;

namespace NoRslinx.Infrastructure.Persistence.Configurations;
public class MicrologixPlcConfiguration : IEntityTypeConfiguration<MicrologixPlc>
{
    public void Configure(EntityTypeBuilder<MicrologixPlc> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.IpAddress)
        .HasMaxLength(200)
            .IsRequired();

        // ensure that no two plc's can have the same IpAddress
        builder.HasIndex(t => t.IpAddress).IsUnique();

        // when the plc is deleted, set the foreign key of the tags to null
        builder.HasMany(t => t.PlcTags)
            .WithOne(t => t.Plc)
            .HasForeignKey(t => t.PlcId)
            .OnDelete(DeleteBehavior.Cascade);



    }
}
