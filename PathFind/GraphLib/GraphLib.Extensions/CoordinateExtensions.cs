﻿using Common.Extensions;
using Common.ValueRanges;
using GraphLib.Interfaces;
using System;
using System.Linq;

namespace GraphLib.Extensions
{
    public static class CoordinateExtensions
    {
        /// <summary>
        /// Compares two coordinates
        /// </summary>
        /// <param name="self"></param>
        /// <param name="coordinate"></param>
        /// <returns>true if all of the coordinates values of <paramref name="self"/> 
        /// equals to the corresponding coordinates of <paramref name="coordinate"/>;
        /// false if not, or if they have not equal number of coordinates values
        /// or any of parametres is null</returns>
        public static bool IsEqual(this ICoordinate self, ICoordinate coordinate)
        {
            #region InvariantsObservance
            if (self == null || coordinate == null)
            {
                return false;
            }
            #endregion

            return self.CoordinatesValues.SequenceEqual(coordinate.CoordinatesValues);
        }

        public static bool IsCardinal(this ICoordinate coordinate, ICoordinate neighbour)
        {
            var vertexCoordinates = coordinate.CoordinatesValues.ToArray();
            var neighbourCoordinates = neighbour.CoordinatesValues.ToArray();
            return vertexCoordinates.IsCardinal(neighbourCoordinates);
        }

        public static bool IsClose(this ICoordinate self, ICoordinate neighbour)
        {
            var selfCoordinateValues = self.CoordinatesValues.ToArray();
            var neighbourCoordinateValues = neighbour.CoordinatesValues.ToArray();
            return selfCoordinateValues.IsClose(neighbourCoordinateValues);
        }

        /// <summary>
        /// Checks whether coordinate is within graph
        /// </summary>
        /// <param name="self"></param>
        /// <param name="graph"></param>
        /// <returns>true if <paramref name="self"/> coordinates 
        /// values is not greater than the corresponding dimension if graph 
        /// and is not lesser than 0; false if it is or coordinate has more or 
        /// less coordinates values than <paramref name="graph"/> has dimensions</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of parametres is null</exception>
        public static bool IsWithinGraph(this ICoordinate self, IGraph graph)
        {
            bool IsWithin(int coordinate, int graphDimension)
            {
                var range = new InclusiveValueRange<int>(graphDimension - 1, 0);
                return range.Contains(coordinate);
            }

            return self.IsWithinGraph(graph, IsWithin);
        }

        public static bool HaveEqualDimensionsNumber(this ICoordinate self, ICoordinate coordinate)
        {
            return self.CoordinatesValues.HaveEqualLength(coordinate.CoordinatesValues);
        }

        /// <summary>
        /// Checks whether coordinate is within graph according to <paramref name="predicate"/>
        /// </summary>
        /// <param name="self"></param>
        /// <param name="graph"></param>
        /// <param name="predicate"></param>
        /// <exception cref="ArgumentNullException">Thrown when any of parametres is null</exception>
        public static bool IsWithinGraph(this ICoordinate self, IGraph graph, Func<int, int, bool> predicate)
        {
            #region InvariantsObservance
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }
            #endregion

            return self.CoordinatesValues.Match(graph.DimensionsSizes, predicate);
        }
    }
}
