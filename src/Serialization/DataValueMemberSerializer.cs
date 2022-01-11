﻿using System;
using System.Xml.Linq;
using L5Sharp.Common;
using L5Sharp.Components;
using L5Sharp.Core;
using L5Sharp.Enums;
using L5Sharp.Extensions;

namespace L5Sharp.Serialization
{
    internal class DataValueMemberSerializer : IXSerializer<IMember<IDataType>>
    {
        private static readonly XName ElementName = LogixNames.DataValueMember;

        public XElement Serialize(IMember<IDataType> component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            if (component.DataType is not IAtomicType atomic)
                throw new InvalidOperationException($"{ElementName} must have an atomic data type.");

            var element = new XElement(ElementName);

            element.AddAttribute(component, m => m.Name);
            element.AddAttribute(component, m => m.DataType);
            element.AddAttribute(component, m => m.Radix);

            var value = atomic.Format(component.Radix);
            element.Add(new XAttribute(LogixNames.Value, value));

            return element;
        }

        public IMember<IDataType> Deserialize(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (element.Name != ElementName)
                throw new ArgumentException(
                    $"Expecting element with name {LogixNames.DataValueMember} but got {element.Name}");

            var name = element.GetComponentName();
            var atomic = (IAtomicType)DataType.New(element.Attribute(LogixNames.DataType)?.Value!);
            var radix = element.GetAttribute<IMember<IDataType>, Radix>(m => m.Radix) ?? Radix.Default(atomic);
            var value = element.Attribute(LogixNames.Value)?.Value ??
                        throw new ArgumentException("The provided element does not have a value attribute.");
            atomic.SetValue(value);

            return Member.Create(name, atomic, radix);
        }
    }
}