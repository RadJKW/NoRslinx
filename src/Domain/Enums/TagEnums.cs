// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRslinx.Domain.Enums;
public enum TagTypeId : int
{
    Output = 0,
    Input = 1,
    Status = 2,
    Binary = 3,
    Timer = 4,
    Counter = 5,
    Control = 6,
    Integer = 7,
    Float = 8,
    Unknown = 99
}
