// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NoRslinx.Domain.Entities;

namespace NoRslinx.Application.Common.Interfaces;

public interface IRsLogixDbImporter
{
    // only return the Plc without the list of tags when the user requests it
    MicrologixPlc Plc { get; }

    public void Convert();

}
