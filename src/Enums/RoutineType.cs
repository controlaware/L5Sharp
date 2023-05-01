﻿using L5Sharp.Components;

namespace L5Sharp.Enums;

/// <summary>
/// An enumeration of all Logix Routine types.
/// </summary>
/// <remarks>
/// Routine type indicates what programming style/language the <see cref="Routine"/> object contains.
/// The type of routine will determine what <see cref="ILogixContent"/> is generated by the routine object.
/// </remarks>
public sealed class RoutineType : LogixEnum<RoutineType, string>
{
    private RoutineType(string name, string value) : base(name, value)
    {
    }

    /// <summary>
    /// Represents no <see cref="RoutineType"/> value.
    /// </summary>
    public static readonly RoutineType Typeless = new(nameof(Typeless), nameof(Typeless));
        
    /// <summary>
    /// Represents a Relay Ladder Logic routine type. 
    /// </summary>
    public static readonly RoutineType Rll = new(nameof(Rll), "RLL");
        
    /// <summary>
    /// Represents a Function Block Diagram routine type. 
    /// </summary>
    public static readonly RoutineType Fbd = new(nameof(Fbd), "FBD");
        
    /// <summary>
    /// Represents a Sequential Function Chart routine type. 
    /// </summary>
    public static readonly RoutineType Sfc = new(nameof(Sfc), "SFC");
        
    /// <summary>
    /// Represents a Structured Text routine type.
    /// </summary>
    public static readonly RoutineType St = new(nameof(St), "ST");
}