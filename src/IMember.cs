﻿using System;
using System.Collections.Generic;
using L5Sharp.Components;
using L5Sharp.Core;
using L5Sharp.Enums;

namespace L5Sharp
{
    /// <summary>
    /// Represents a member or component of a complex data type structure.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Members are used to define the structure of a <see cref="IComplexType"/>.
    /// Since each member holds a generic type reference to it's data type,
    /// the structure forms a hierarchical tree of nested members and types.
    /// Members with non-empty <see cref="Dimension"/> have instantiated elements that can be accessed by index.
    /// Use the static factory <see cref="Member"/> class to create instances of IMember.
    /// </para>
    /// </remarks>
    /// <typeparam name="TDataType">Represents the <see cref="IDataType"/> of the <c>Member</c> component.</typeparam>
    /// <footer>
    /// See <a href="https://literature.rockwellautomation.com/idc/groups/literature/documents/rm/1756-rm084_-en-p.pdf">
    /// `Logix 5000 Controllers Import/Export`</a> for more information.
    /// </footer>
    public interface IMember<out TDataType> :
        ILogixComponent,
        IPrototype<IMember<TDataType>>,
        IEnumerable<IMember<TDataType>>
        where TDataType : IDataType
    {
        /// <summary>
        /// Gets the DataType of the <c>Member</c> Component.
        /// </summary>
        /// <value>
        /// Any object that implements <see cref="IDataType"/>. <c>DataType</c> can not be null.
        /// <see cref="Types.Undefined"/> represents the lack of a known or instantiable type.
        /// </value>
        TDataType DataType { get; }

        /// <summary>
        /// Gets the Dimension of the <c>Member</c> component.
        /// </summary>
        Dimensions Dimension { get; }

        /// <summary>
        /// Gets the Radix of the <c>Member</c> component.
        /// </summary>
        Radix Radix { get; }

        /// <summary>
        /// Gets the ExternalAccess of the <c>Member</c> component.
        /// </summary>
        ExternalAccess ExternalAccess { get; }
        
        /// <summary>
        /// Gets a <c>Member</c> element at the specified index.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A <c>Member</c> with non-empty dimensions will have instantiated member elements.
        /// Each element of the <c>Member</c> array are of the same type as the root/seed type of the member.
        /// The <c>Member</c> <see cref="Radix"/>, <see cref="ExternalAccess"/>,
        /// and <see cref="ILogixComponent.Description"/> are propagated to each element on construction. 
        /// </para>
        /// </remarks>
        /// <param name="index">The index of the element to get.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// When index is less than 0 or greater than <see cref="Core.Dimensions.Length"/>
        /// </exception>
        IMember<TDataType> this[int index] { get; }
    }
}