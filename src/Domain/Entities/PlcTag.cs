// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRslinx.Domain.Entities;
public class PlcTag : BaseAuditableEntity
{
    public int PlcId { get; set; }
    public string? SymbolName { get; set; }
    public string? Address { get; set; }

    public string? Description { get; set; }

    public MicrologixPlc Plc { get; set; } = null!;

    private bool _value;

    public bool Value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                _value = value;
                AddDomainEvent(new PlcTagValueChangedEvent(this));
            }
        }
    }
}

