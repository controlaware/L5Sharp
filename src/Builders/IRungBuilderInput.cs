﻿using System;

namespace L5Sharp.Builders
{
    public interface IRungBuilderInput : IRungBuilderSegment
    {
        IRungBuilderInput And(string text);
        IRungBuilderBranch Or(Action<IRungBuilderStart> branch);
        IRungBuilderOutput Then(string text);
    }
}