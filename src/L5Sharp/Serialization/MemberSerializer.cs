﻿using System.Runtime.CompilerServices;
using System.Xml.Linq;
using L5Sharp.Abstractions;
using L5Sharp.Core;
using L5Sharp.Enumerations;
using L5Sharp.Extensions;
using L5Sharp.Utilities;

[assembly: InternalsVisibleTo("L5Sharp.Serialization.Tests")]

namespace L5Sharp.Serialization
{
    internal class MemberSerializer : IComponentSerializer<Member>
    {
        public XElement Serialize(Member component)
        {
            var element = new XElement(nameof(Member));
            element.Add(new XAttribute(nameof(component.Name), component.Name));
            element.Add(new XAttribute(nameof(component.DataType), component.DataType.Name));
            element.Add(new XAttribute(nameof(component.Dimension), component.Dimension));
            element.Add(new XAttribute(nameof(component.Radix), component.Radix));
            element.Add(new XAttribute(nameof(component.ExternalAccess), component.ExternalAccess));

            if (!string.IsNullOrEmpty(component.Description))
                element.Add(new XElement(nameof(component.Description), new XCData(component.Description)));

            return element;
        }

        public Member Deserialize(XElement element)
        {
            var dataType = element.GetDataType();
            
            return new Member(element.GetName(), dataType, element.GetDimension(), element.GetRadix(),
                element.GetExternalAccess(), element.GetDescription());
        }
    }
}