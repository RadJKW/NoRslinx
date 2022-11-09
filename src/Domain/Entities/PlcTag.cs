// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root forZ more information.

using Newtonsoft.Json;

namespace NoRslinx.Domain.Entities;
public class PlcTag : BaseAuditableEntity
{
    private bool _value;
    public int PlcId { get; set; }
    public string? SymbolName { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }
    public TagTypeId TagTypeId { get; set; }
    public TagType? TagType { get; set; }

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

    [JsonIgnore]
    public MicrologixPlc Plc { get; set; } = null!;
}

public class TagType
{
    public TagTypeId TagTypeId { get; set; }
    public string? Name { get; set; }

}

