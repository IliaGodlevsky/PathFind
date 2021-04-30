﻿using Common.Attributes;
using GraphLib.Interfaces;

namespace GraphLib.Common.NullObjects
{
    [Null]
    public sealed class NullCost : IVertexCost
    {
        public int CurrentCost => default;

        public override bool Equals(object obj)
        {
            return obj is NullCost;
        }

        public override int GetHashCode()
        {
            return CurrentCost.GetHashCode();
        }
    }
}