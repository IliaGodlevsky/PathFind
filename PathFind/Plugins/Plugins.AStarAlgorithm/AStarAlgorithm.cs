﻿using Algorithm.Interfaces;
using Algorithm.Realizations.Heuristic;
using Algorithm.Realizations.StepRules;
using AssembleClassesLib.Attributes;
using GraphLib.Interfaces;
using Plugins.DijkstraALgorithm;

namespace Plugins.AStarAlgorithm
{
    /// <summary>
    /// A realization of the A* algorithm
    /// </summary>
    /// <remarks><see cref="https://en.wikipedia.org/wiki/A*_search_algorithm"/></remarks>
    [ClassName("A* algorithm")]
    public class AStarAlgorithm : DijkstraAlgorithm
    {
        public AStarAlgorithm(IGraph graph, IEndPoints endPoints)
            : this(graph, endPoints, new DefaultStepRule(), new ChebyshevDistance())
        {

        }

        public AStarAlgorithm(IGraph graph, IEndPoints endPoints,
            IStepRule stepRule, IHeuristic function)
            : base(graph, endPoints, stepRule)
        {
            heuristic = function;
        }

        public AStarAlgorithm(IGraph graph, IEndPoints endPoints, IHeuristic function)
            : this(graph, endPoints, new DefaultStepRule(), function)
        {

        }

        public AStarAlgorithm(IGraph graph, IEndPoints endPoints, IStepRule stepRule)
            : this(graph, endPoints, stepRule, new ChebyshevDistance())
        {

        }

        protected override double GetVertexRelaxedCost(IVertex neighbour)
        {
            return base.GetVertexRelaxedCost(neighbour)
                   + heuristic.Calculate(CurrentVertex, endPoints.Target);
        }

        protected readonly IHeuristic heuristic;
    }
}
