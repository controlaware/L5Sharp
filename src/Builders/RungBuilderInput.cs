﻿using System;

namespace L5Sharp.Builders
{
    internal class RungBuilderInput : IRungBuilderInput
    {
        private readonly RungBuilderContext _context;

        public RungBuilderInput(RungBuilderContext context)
        {
            _context = context;
        }

        public IRungBuilderInput And(string text)
        {
            _context.Append(text);
            return this;
        }

        public IRungBuilderBranch Or(Action<IRungBuilderStart> branch)
        {
            _context.BranchStart();
            _context.BranchAppend();
            branch.Invoke(_context.Start);
            return _context.Branch;
        }

        public IRungBuilderOutput Then(string text)
        {
            _context.Append(text);
            return _context.Output;
        }

        public IRungBuilder Compile()
        {
            return _context.Builder;
        }
    }
}