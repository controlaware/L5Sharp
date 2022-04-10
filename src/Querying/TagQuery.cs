﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using L5Sharp.L5X;
using L5Sharp.Serialization;

namespace L5Sharp.Querying
{
    internal class TagQuery : ComponentQuery<ITag<IDataType>>, ITagQuery
    {
        public TagQuery(IEnumerable<XElement> elements, IL5XSerializer<ITag<IDataType>> serializer) 
            : base(elements, serializer)
        {
        }

        public ITagQuery WithDataType(string typeName)
        {
            var tags = Elements.Where(t => string.Equals(t.Attribute(L5XAttribute.DataType.ToString())?.Value,
                typeName, StringComparison.OrdinalIgnoreCase));

            return new TagQuery(tags, Serializer);
        }
    }
}