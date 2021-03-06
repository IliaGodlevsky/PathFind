﻿using GraphLib.Interfaces;
using GraphLib.Interfaces.Factories;
using GraphLib.Realizations.NeighboursCoordinates;

namespace GraphLib.Realizations.Factories.NeighboursCoordinatesFactories
{
    public sealed class AroundNeighboursCoordinatesFactory : INeighboursCoordinatesFactory
    {
        public INeighboursCoordinates CreateNeighboursCoordinates(ICoordinate coordinate)
        {
            return new AroundNeighboursCoordinates(coordinate);
        }
    }
}