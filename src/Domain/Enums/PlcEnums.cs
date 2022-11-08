// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace NoRslinx.Domain.Enums;
public enum PlcType
{
    /// <summary>
    /// Control Logix-class PLC. Synonym for lgx, logix, controllogix, contrologix, compactlogix, clgx.
    /// </summary>
    ControlLogix = 0,

    /// <summary>
    /// PLC/5 PLC. Synonym for plc5, plc.
    /// </summary>
    Plc5 = 1,

    /// <summary>
    /// SLC 500 PLC. Synonym for slc500, slc.
    /// </summary>
    Slc500 = 2,

    /// <summary>
    /// Control Logix-class PLC using the PLC/5 protocol. Synonym for lgxpccc, logixpccc, lgxplc5, lgx_pccc, logix_pccc, lgx_plc5.
    /// </summary>
    LogixPccc = 3,

    /// <summary>
    /// Micro800-class PLC. Synonym for micrologix800, mlgx800, micro800.
    /// </summary>
    Micro800 = 4,

    /// <summary>
    /// MicroLogix PLC. Synonym for micrologix, mlgx.
    /// </summary>
    MicroLogix = 5,

    /// <summary>
    /// Omron PLC. Synonym for omron-njnx, omron-nj, omron-nx, njnx, nx1p2
    /// </summary>
    Omron = 6,
}

/// <summary>
/// Communication Protocols supported by the library
/// </summary>
public enum Protocol
{
    /// <summary>
    /// Allen-Bradley specific flavor of EIP
    /// </summary>
    ab_eip = 0,

    /// <summary>
    /// A Modbus TCP implementation used by many PLCs
    /// </summary>
    modbus_tcp = 1
}

public enum DebugLevel
{
    /// <summary>
    /// No debug messages
    /// </summary>
    None = 0,

    /// <summary>
    /// Only error messages
    /// </summary>
    Error = 1,

    /// <summary>
    /// Error and warning messages
    /// </summary>
    Warning = 2,

    /// <summary>
    /// Error, warning, and informational messages
    /// </summary>
    Info = 3,

    /// <summary>
    /// Error, warning, informational, and debug messages
    /// </summary>
    Debug = 4
}
