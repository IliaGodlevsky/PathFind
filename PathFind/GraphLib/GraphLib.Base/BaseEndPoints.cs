﻿using Common.Extensions;
using Conditional;
using GraphLib.Extensions;
using GraphLib.Interfaces;
using GraphLib.NullRealizations.NullObjects;
using NullObject.Extensions;
using System;
using System.Threading.Tasks;

namespace GraphLib.Base
{
    public abstract class BaseEndPoints : IEndPoints
    {
        protected BaseEndPoints()
        {
            Reset();
            conditional = new Conditional<IVertex>(UnsetSource, v => Source.IsEqual(v))
                  .PerformIf(UnsetTarget, v => Target.IsEqual(v))
                  .PerformIf(SetSource, CanSetSource)
                  .PerformIf(ReplaceSource, v => Source.IsIsolated())
                  .PerformIf(SetTarget, CanSetTarget)
                  .PerformIf(ReplaceTarget, v => Target.IsIsolated());
        }

        public bool HasEndPointsSet => !Source.IsIsolated() && !Target.IsIsolated();

        public IVertex Source { get; private set; }

        public IVertex Target { get; private set; }

        public void SubscribeToEvents(IGraph graph)
        {
            Task.Run(() => graph.ForEach(SubscribeVertex));
        }

        public void UnsubscribeFromEvents(IGraph graph)
        {
            Task.Run(() => graph.ForEach(UnsubscribeVertex));
        }

        public void Reset()
        {
            Source = new NullVertex();
            Target = new NullVertex();
        }

        public bool IsEndPoint(IVertex vertex)
        {
            return vertex.IsEqual(Source) || vertex.IsEqual(Target);
        }

        public bool CanBeEndPoint(IVertex vertex)
        {
            return !IsEndPoint(vertex) && !vertex.IsIsolated();
        }

        protected bool CanSetSource(IVertex vertex)
        {
            return Source.IsNull() && CanBeEndPoint(vertex);
        }

        protected bool CanSetTarget(IVertex vertex)
        {
            return !Source.IsNull() && Target.IsNull() && CanBeEndPoint(vertex);
        }

        protected virtual void SetEndPoints(object sender, EventArgs e)
        {
            bool IsNotIsolated(IVertex vertex) => vertex?.IsIsolated() == false;
            conditional.PerformFirstSuitable(sender as IVertex, IsNotIsolated);
        }

        protected virtual void SetSource(IVertex vertex)
        {
            Source = vertex;
            (vertex as IMarkable)?.MarkAsSource();
        }

        protected virtual void SetTarget(IVertex vertex)
        {
            Target = vertex;
            (vertex as IMarkable)?.MarkAsTarget();
        }

        protected virtual void UnsetSource(IVertex vertex)
        {
            (vertex as IMarkable)?.MarkAsRegular();
            Source = new NullVertex();
        }

        protected virtual void UnsetTarget(IVertex vertex)
        {
            (vertex as IMarkable)?.MarkAsRegular();
            Target = new NullVertex();
        }

        protected virtual void ReplaceSource(IVertex vertex)
        {
            UnsetSource(Source);
            SetSource(vertex);
        }

        protected virtual void ReplaceTarget(IVertex vertex)
        {
            UnsetTarget(Target);
            SetTarget(vertex);
        }

        protected abstract void SubscribeVertex(IVertex vertex);
        protected abstract void UnsubscribeVertex(IVertex vertex);

        private readonly Conditional<IVertex> conditional;
    }
}
