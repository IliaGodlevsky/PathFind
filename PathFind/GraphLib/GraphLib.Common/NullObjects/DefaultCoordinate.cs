﻿using Common.Attributes;
using GraphLib.Interfaces;
using System;
using System.Collections.Generic;

namespace GraphLib.Common.NullObjects
{
    /// <summary>
    /// An empty coordinate with default realization
    /// </summary>
    [Default]
    [Serializable]
    public sealed class DefaultCoordinate : ICoordinate
    {
        public IEnumerable<int> CoordinatesValues => new int[] { };

        public override bool Equals(object pos)
        {
            return pos is DefaultCoordinate;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
