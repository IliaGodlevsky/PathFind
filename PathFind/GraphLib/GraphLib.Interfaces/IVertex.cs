﻿using Common.Interfaces;
using System.Collections.Generic;

namespace GraphLib.Interface
{
    /// <summary>
    /// Represents a vertex of graph
    /// </summary>
    public interface IVertex : IDefault
    {
        bool IsObstacle { get; set; }

        IVertexCost Cost { get; set; }

        IList<IVertex> Neighbours { get; set; }

        ICoordinate Position { get; set; }
    }
}