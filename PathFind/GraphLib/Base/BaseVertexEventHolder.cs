﻿using Common.ValueRanges;
using GraphLib.Extensions;
using GraphLib.Interface;
using GraphLib.Vertex.Cost;
using System;
using System.Linq;

namespace GraphLib.Base
{
    public abstract class BaseVertexEventHolder : IVertexEventHolder
    {
        public IGraph Graph { get; set; }

        public virtual void ChangeVertexCost(object sender, EventArgs e)
        {
            if (sender is IVertex vertex)
            {
                if (!vertex.IsObstacle)
                {
                    int delta = GetWheelDelta(e) > 0 ? 1 : -1;
                    int newCost = vertex.Cost.CurrentCost + delta;
                    int boundedCost = Range.VertexCostRange.ReturnInRange(newCost);
                    vertex.Cost = new VertexCost(boundedCost);
                }
            }
        }

        public virtual void SetStartVertex(IVertex vertex)
        {
            if (vertex.IsValidToBeExtreme())
            {
                vertex.MarkAsStart();
                Graph.Start = vertex;
            }
        }

        public virtual void SetDestinationVertex(IVertex vertex)
        {
            if (vertex.IsValidToBeExtreme())
            {
                vertex.MarkAsEnd();
                Graph.End = vertex;
            }
        }

        public virtual void Reverse(object sender, EventArgs e)
        {
            if (sender is IVertex vertex)
            {
                if (vertex.IsObstacle)
                {
                    MakeVertex(vertex);
                }
                else
                {
                    MakeObstacle(vertex);
                }
            }
        }

        public void UnsubscribeVertices()
        {
            Graph?.AsParallel().ForAll(UnsubscribeFromEvents);
        }

        public void SubscribeVertices()
        {
            Graph?.AsParallel().ForAll(SubscribeToEvents);
        }

        public virtual void ChooseExtremeVertices(object sender, EventArgs e)
        {
            if (sender is IVertex vertex)
            {
                if (!IsStartChosen())
                {
                    SetStartVertex(vertex);
                }
                else if (IsStartChosen() && !IsEndChosen())
                {
                    SetDestinationVertex(vertex);
                }
            }
        }

        protected abstract void UnsubscribeFromEvents(IVertex vertex);

        protected abstract void SubscribeToEvents(IVertex vertex);

        protected abstract int GetWheelDelta(EventArgs e);

        private void MakeObstacle(IVertex vertex)
        {
            if (vertex.IsSimpleVertex() && !vertex.IsVisited)
            {
                vertex.Isolate();
                vertex.IsObstacle = true;
                vertex.SetToDefault();
                vertex.MarkAsObstacle();
            }
        }

        private void MakeVertex(IVertex vertex)
        {
            vertex.IsObstacle = false;
            vertex.MarkAsSimpleVertex();
            vertex.SetNeighbours(Graph);
            vertex.ConnectWithNeighbours();
        }

        private bool IsStartChosen()
        {
            return !Graph.Start.IsDefault;
        }

        private bool IsEndChosen()
        {
            return !Graph.End.IsDefault;
        }


    }
}