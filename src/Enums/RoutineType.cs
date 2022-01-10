﻿using System;
using Ardalis.SmartEnum;
using L5Sharp.Core;

namespace L5Sharp.Enums
{
    /// <summary>
    /// An enumeration of all Logix Routine types.
    /// </summary>
    /// <remarks>
    /// Routine type indicates what programming style/language the <see cref="IRoutine"/> object contains.
    /// The type of routine will determine what <see cref="ILogixContent"/> is generated by the routine object.
    /// </remarks>
    public abstract class RoutineType : SmartEnum<RoutineType, string>
    {
        private RoutineType(string name, string value) : base(name, value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract ILogixContent CreateContent();

        /// <summary>
        /// Gets the <see cref="RoutineType"/> option based on the specified <see cref="TContent"/> type.
        /// </summary>
        /// <typeparam name="TContent">The <see cref="ILogixContent"/> type to get a <see cref="RoutineType"/> option for.</typeparam>
        /// <returns>The <see cref="RoutineType"/> instance that represents the specified type.</returns>
        public static RoutineType ForType<TContent>() where TContent : ILogixContent
        {
            if (typeof(TContent).IsAssignableFrom(typeof(LadderLogic)))
                return Rll;
            
            if (typeof(TContent).IsAssignableFrom(typeof(TContent)))
                return Fbd;
            
            //todo need to finish this method...

            return Typeless;
        }

        /// <summary>
        /// Represents no <see cref="RoutineType"/> value.
        /// </summary>
        public static readonly RoutineType Typeless = new TypelessType();
        
        /// <summary>
        /// Represents a Relay Ladder Logic routine type. 
        /// </summary>
        public static readonly RoutineType Rll = new LadderLogicType();
        
        /// <summary>
        /// Represents a Function Block Diagram routine type. 
        /// </summary>
        public static readonly RoutineType Fbd = new FunctionBlockType();
        
        /// <summary>
        /// Represents a Sequential Function Chart routine type. 
        /// </summary>
        public static readonly RoutineType Sfc = new SequentialFunctionType();
        
        /// <summary>
        /// Represents a Structured Text routine type.
        /// </summary>
        public static readonly RoutineType St = new StructuredTextType();
        

        private class TypelessType : RoutineType
        {
            public TypelessType() : base(nameof(Typeless), "Typeless")
            {
                
            }

            public override ILogixContent CreateContent()
            {
                throw new NotSupportedException($"Can not create content for RoutineType '{Name}'.");
            }
        }
        
        private class LadderLogicType : RoutineType
        {
            public LadderLogicType() : base(nameof(Rll), nameof(Rll).ToUpper())
            {
                
            }

            public override ILogixContent CreateContent()
            {
                return new LadderLogic();
            }
        }
        
        private class FunctionBlockType : RoutineType
        {
            public FunctionBlockType() : base(nameof(Fbd), nameof(Fbd).ToUpper())
            {
                
            }

            public override ILogixContent CreateContent()
            {
                throw new NotImplementedException();
            }
        }
        
        private class SequentialFunctionType : RoutineType
        {
            public SequentialFunctionType() : base(nameof(Sfc), nameof(Sfc).ToUpper())
            {
                
            }

            public override ILogixContent CreateContent()
            {
                throw new NotImplementedException();
            }
        }
        
        private class StructuredTextType : RoutineType
        {
            public StructuredTextType() : base(nameof(St), nameof(St).ToUpper())
            {
                
            }

            public override ILogixContent CreateContent()
            {
                throw new NotImplementedException();
            }
        }
    }
}