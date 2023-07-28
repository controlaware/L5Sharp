﻿using System;
using L5Sharp.Enums;

namespace L5Sharp.Types.Atomics;

/// <summary>
/// Represents a <b>LINT</b> Logix atomic data type, or a type analogous to a <see cref="long"/>.
/// </summary>
public sealed class LINT : AtomicType, IComparable, IConvertible
{
    private readonly long _value;

    /// <summary>
    /// Creates a new default <see cref="LINT"/> type.
    /// </summary>
    public LINT()
    {
        _value = default;
        Radix = Radix.Decimal;
    }

    /// <summary>
    /// Creates a new <see cref="LINT"/> with the provided value.
    /// </summary>
    /// <param name="value">The value to initialize the type with.</param>
    public LINT(long value)
    {
        _value = value;
        Radix = Radix.Decimal;
    }

    /// <summary>
    /// Creates a new <see cref="LINT"/> value with the provided radix format.
    /// </summary>
    /// <param name="radix">The <see cref="Enums.Radix"/> number format of the value.</param>
    public LINT(Radix radix)
    {
        _value = default;
        if (radix is null) throw new ArgumentNullException(nameof(radix));
        if (!radix.SupportsType(this))
            throw new ArgumentException($"Invalid Radix {radix} for atomic type {Name}.", nameof(radix));
        Radix = radix;
    }

    /// <summary>
    /// Creates a new <see cref="LINT"/> with the provided value.
    /// </summary>
    /// <param name="value">The value to initialize the type with.</param>
    /// <param name="radix">The optional radix format of the value.</param>
    public LINT(long value, Radix radix)
    {
        _value = value;
        if (radix is null) throw new ArgumentNullException(nameof(radix));
        if (!radix.SupportsType(this))
            throw new ArgumentException($"Invalid Radix {radix} for atomic type {Name}.", nameof(radix));
        Radix = radix;
    }

    /// <inheritdoc />
    public override string Name => nameof(LINT);

    /// <inheritdoc />
    public override Radix Radix { get; }

    /// <summary>
    /// Gets bit member's data type value at the specified bit index. 
    /// </summary>
    /// <param name="bit">The zero based bit index of the value to get.</param>
    /// <returns>A <see cref="BOOL"/> representing the value of the specified bit value (0/1).</returns>
    /// <exception cref="ArgumentOutOfRangeException"><c>bit</c> is out of range of the atomic type bit length.</exception>
    public BOOL this[int bit] =>
        Member(bit.ToString())?.DataType.As<BOOL>() ??
        throw new ArgumentOutOfRangeException($"The bit index {bit} is out of range for a {Name} atomic value.");

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        return obj switch
        {
            null => 1,
            LINT typed => _value.CompareTo(typed._value),
            AtomicType atomic => _value.CompareTo((long)Convert.ChangeType(atomic, typeof(long))),
            ValueType value => _value.CompareTo((long)Convert.ChangeType(value, typeof(long))),
            _ => throw new ArgumentException($"Cannot compare logix type {obj.GetType().Name} with {GetType().Name}.")
        };
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj switch
        {
            LINT value => _value == value._value,
            AtomicType atomic => _value.Equals((long)Convert.ChangeType(atomic, typeof(long))),
            ValueType value => _value.Equals(Convert.ChangeType(value, typeof(long))),
            _ => false
        };
    }

    /// <inheritdoc />
    public override byte[] GetBytes() => BitConverter.GetBytes(_value);

    /// <inheritdoc />
    public override int GetHashCode() => _value.GetHashCode();

    /// <summary>
    /// Parses the provided string value to a new <see cref="LINT"/>.
    /// </summary>
    /// <param name="value">The string value to parse.</param>
    /// <returns>A <see cref="LINT"/> representing the parsed value.</returns>
    /// <exception cref="FormatException">The <see cref="Radix"/> format can not be inferred from <c>value</c>.</exception>
    public static LINT Parse(string value)
    {
        if (long.TryParse(value, out var result))
            return new LINT(result);

        var radix = Radix.Infer(value);
        var atomic = radix.Parse(value);
        var converted = (long)Convert.ChangeType(atomic, typeof(long));
        return new LINT(converted, radix);
    }
    
    /// <summary>
    /// Executes the logic to update the atomic value and forward the data changed event up the type/member hierarchy. 
    /// </summary>
    /// <param name="sender">The member sending the change event.</param>
    /// <param name="e">The event args of the event.</param>
    /// <remarks>
    /// Atomic members (bits) represent the value of the type. When the member data changed event is triggered,
    /// we want to intercept that on the parent atomic type in order to change/update the reflected value. We can do that
    /// by getting the changed member (sender) name (bit number) and value (bit value) and setting to get the updated
    /// value. However, since atomic types ar immutable, we have to send the changed value up the chain to the parent
    /// member (Tag, DataValue, DataValueMember) so that it can replace it's data type with the new atomic value. This is
    /// captured in <see cref="LogixMember"/>.
    /// </remarks>
    protected override void OnMemberDataChanged(object sender, EventArgs e)
    {
        var member = (LogixMember)sender;
        var bit = int.Parse(member.Name);
        var value = member.DataType.As<BOOL>();
        var result = value ? _value | (long)1 << bit : _value & ~(1 << bit);
        RaiseDataChanged(new LINT(result, Radix));
    }

    // Contains the implicit .NET conversions for the type.

    #region Conversions

    /// <summary>
    /// Converts the provided <see cref="long"/> to a <see cref="LINT"/> value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="LINT"/> value.</returns>
    public static implicit operator LINT(long value) => new(value);

    /// <summary>
    /// Converts the provided <see cref="LINT"/> to a <see cref="long"/> value.
    /// </summary>
    /// <param name="atomic">The value to convert.</param>
    /// <returns>A <see cref="long"/> type value.</returns>
    public static implicit operator long(LINT atomic) => atomic._value;

    /// <summary>
    /// Implicitly converts a <see cref="string"/> to a <see cref="LINT"/> value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A new <see cref="LINT"/> value.</returns>
    public static implicit operator LINT(string value) => Parse(value);

    /// <summary>
    /// Implicitly converts the provided <see cref="LINT"/> to a <see cref="string"/> value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A new <see cref="string"/> value.</returns>
    public static implicit operator string(LINT value) => value.ToString();

    #endregion

    // Contains the IConvertible implementation for the type. I am explicitly implementing this interface for each
    // atomic type to avoid polluting the API, and to have the implementation as performant as possible.
    // To perform conversion, use the recommended .NET Convert.ChangeType() method and specify the target type.

    #region Convertible

    /// <inheritdoc />
    TypeCode IConvertible.GetTypeCode() => TypeCode.Object;

    /// <inheritdoc />
    bool IConvertible.ToBoolean(IFormatProvider provider) => _value != 0;

    /// <inheritdoc />
    byte IConvertible.ToByte(IFormatProvider provider) => (byte)_value;

    /// <inheritdoc />
    char IConvertible.ToChar(IFormatProvider provider) => (char)_value;

    /// <inheritdoc />
    DateTime IConvertible.ToDateTime(IFormatProvider provider)
    {
        var milliseconds = _value / 1000;
        var microseconds = _value % 1000;
        var ticks = microseconds * (TimeSpan.TicksPerMillisecond / 1000);
        return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).AddTicks(ticks).DateTime;
    }

    /// <inheritdoc />
    decimal IConvertible.ToDecimal(IFormatProvider provider) =>
        throw new InvalidCastException($"Conversion from {Name} to {nameof(Decimal)} is not supported.");

    /// <inheritdoc />
    double IConvertible.ToDouble(IFormatProvider provider) => _value;

    /// <inheritdoc />
    short IConvertible.ToInt16(IFormatProvider provider) => (short)_value;

    /// <inheritdoc />
    int IConvertible.ToInt32(IFormatProvider provider) => (int)_value;

    /// <inheritdoc />
    long IConvertible.ToInt64(IFormatProvider provider) => _value;

    /// <inheritdoc />
    sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte)_value;

    /// <inheritdoc />
    float IConvertible.ToSingle(IFormatProvider provider) => _value;

    /// <inheritdoc />
    string IConvertible.ToString(IFormatProvider provider) => ToString();

    /// <inheritdoc />
    object IConvertible.ToType(Type conversionType, IFormatProvider provider)
    {
        var convertible = (IConvertible)this;

        return Type.GetTypeCode(conversionType) switch
        {
            TypeCode.Boolean => convertible.ToBoolean(provider),
            TypeCode.Byte => convertible.ToByte(provider),
            TypeCode.Char => convertible.ToChar(provider),
            TypeCode.DateTime => convertible.ToDateTime(provider),
            TypeCode.Decimal => convertible.ToDecimal(provider),
            TypeCode.Double => convertible.ToDouble(provider),
            TypeCode.Empty => throw new ArgumentNullException(nameof(conversionType)),
            TypeCode.Int16 => convertible.ToInt16(provider),
            TypeCode.Int32 => convertible.ToInt32(provider),
            TypeCode.Int64 => convertible.ToInt64(provider),
            TypeCode.Object => ToAtomic(conversionType),
            TypeCode.SByte => convertible.ToSByte(provider),
            TypeCode.Single => convertible.ToSingle(provider),
            TypeCode.String => ToString(),
            TypeCode.UInt16 => convertible.ToUInt16(provider),
            TypeCode.UInt32 => convertible.ToUInt32(provider),
            TypeCode.UInt64 => convertible.ToUInt64(provider),
            TypeCode.DBNull => throw new InvalidCastException(
                "Conversion for type code 'DbNull' not supported by AtomicType."),
            _ => throw new InvalidCastException($"Conversion for {conversionType.Name} not supported by AtomicType.")
        };
    }

    /// <inheritdoc />
    ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort)_value;

    /// <inheritdoc />
    uint IConvertible.ToUInt32(IFormatProvider provider) => (uint)_value;

    /// <inheritdoc />
    ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong)_value;

    /// <summary>
    /// Converts the current atomic type to the specified atomic type.
    /// </summary>
    /// <param name="conversionType">The atomic type to convert to.</param>
    /// <returns>A <see cref="object"/> representing the converted atomic type value.</returns>
    /// <exception cref="InvalidCastException">The specified type is not a valid atomic type.</exception>
    private object ToAtomic(Type conversionType)
    {
        if (conversionType == typeof(BOOL))
            return new BOOL(_value != 0);
        if (conversionType == typeof(SINT))
            return new SINT((sbyte)_value);
        if (conversionType == typeof(INT))
            return new INT((short)_value);
        if (conversionType == typeof(DINT))
            return new DINT((int)_value);
        if (conversionType == typeof(LINT))
            return new LINT(_value);
        if (conversionType == typeof(REAL))
            return new REAL(_value);
        if (conversionType == typeof(LREAL))
            return new LREAL(_value);
        if (conversionType == typeof(USINT))
            return new USINT((byte)_value);
        if (conversionType == typeof(UINT))
            return new UINT((ushort)_value);
        if (conversionType == typeof(UDINT))
            return new UDINT((uint)_value);
        if (conversionType == typeof(ULINT))
            return new ULINT((ulong)_value);

        throw new InvalidCastException($"Cannot convert from {GetType().Name} to {conversionType.Name}.");
    }

    #endregion
}