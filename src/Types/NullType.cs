﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace L5Sharp.Types;

/// <summary>
/// Represents a null <see cref="L5Sharp.LogixType"/> implementation, or a type that is neither atomic, structure, array,
/// or string.
/// </summary>
/// <remarks>This would be the default for any tag that has not data type defined.</remarks>
// ReSharper disable once InconsistentNaming
public sealed class NullType : LogixType
{
    private static readonly NullType Singleton = new();

    private NullType()
    {
    }

    /// <inheritdoc />
    public override string Name => "NULL";

    /// <inheritdoc />
    public override IEnumerable<Member> Members => Enumerable.Empty<Member>();

    /// <inheritdoc />
    public override void Update(LogixType type) =>
        throw new NotSupportedException($"Updating {GetType()} data is not supported.");

    /// <inheritdoc />
    public override XElement Serialize() => new(L5XName.Data);

    /// <summary>
    /// Gets the singleton instance of the <see cref="NullType"/> logix type.
    /// </summary>
    public static readonly NullType Instance = Singleton;
}