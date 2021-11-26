﻿using Ardalis.SmartEnum;

namespace L5Sharp.Enums
{
    /// <summary>
    /// Represents a set of categories that a <c>DataType</c> can belong to.
    /// </summary>
    /// <remarks>
    /// L5X only exports User class types.
    /// <see cref="Atomic"/> types are value types (i.e. Bool, Int, Real, etc..).
    /// <see cref="Predefined"/> types represent other built in types (i.e. Timer, Counter, Message, etc.).
    /// 
    /// </remarks>
    public class DataTypeClass : SmartEnum<DataTypeClass>
    {
        private DataTypeClass(string name, int value) : base(name, value)
        {
        }
        
        /// <summary>
        /// Represents a unknown data type class.
        /// </summary>
        public static readonly DataTypeClass Undefined = new DataTypeClass("Undefined", -1);

        /// <summary>
        /// Represents a data type that has value and radix. See <see cref="IAtomic"/>
        /// </summary>
        /// <example>
        /// Bool, Sint, Int, Dint, Lint, Real.
        /// </example>
        public static readonly DataTypeClass Atomic = new DataTypeClass("Atomic", 0);

        /// <summary>
        /// Represents a built in data type that must be preconfigured.
        /// </summary>
        /// <example>
        /// String, Timer, Counter.
        /// </example>
        public static readonly DataTypeClass Predefined = new DataTypeClass("ProductDefined", 1);

        /// <summary>
        /// Represents a type that is defined for Logix modules.
        /// </summary>
        public static readonly DataTypeClass Io = new DataTypeClass("IO", 2);

        /// <summary>
        /// Represents a type that is defined by the user.
        /// </summary>
        public static readonly DataTypeClass User = new DataTypeClass("User", 3);
        
        /// <summary>
        /// Represents a type that is defined by a Logix AOI.
        /// </summary>
        public static readonly DataTypeClass AddOnDefined = new DataTypeClass("AddOn", 4);
    }
}