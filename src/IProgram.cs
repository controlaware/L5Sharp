﻿using L5Sharp.Enums;

namespace L5Sharp
{
    public interface IProgram : ILogixComponent
    {
        ProgramType Type { get; }
        bool TestEdits { get; }
        bool Disabled { get; }
        IComponentCollection<ITag<IDataType>> Tags { get; }
        IComponentCollection<IRoutine> Routines { get; }
    }
}