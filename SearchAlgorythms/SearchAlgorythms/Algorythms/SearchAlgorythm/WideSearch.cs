﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using SearchAlgorythms.GraphExtension;
using SearchAlgorythms.Top;

namespace SearchAlgorythms.Algorythms.SearchAlgorythm
{
    public class WideSearch : ISearchAlgorythm
    {
        protected void Pause(int value = 0)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < value)
                Application.DoEvents();
        }

        public GraphTop GoChippestNeighbour(GraphTop top)
        {
            GraphTop chippest = top;
            var neighbours = top.GetNeighbours();
            foreach (var neighbour in neighbours) 
            {
                if (IsRightNeighbour(chippest, neighbour))
                    chippest = neighbour;
            }
            if (chippest.IsRightCellToColour()) 
                chippest.MarkAsPath();
            return chippest;
        }

        public virtual bool IsRightNeighbour(GraphTop top1, GraphTop top2)
        {
            return top1.Value >= top2.Value && top2.IsVisited && !top2.IsEnd;
        }

        public virtual bool IsRightCellToVisit(Button button)
        {
            GraphTop top = button as GraphTop;
            return top is null ? false : !top.IsVisited;
        }

        protected Queue<GraphTop> queue = new Queue<GraphTop>();

        public bool DestinationFound { get; set; }

        public virtual void ExtractNeighbours(Button button)
        {
            var currentTop = button as GraphTop;
            if (currentTop is null)
                return;
            foreach (var neigbour in currentTop.GetNeighbours())
            {
                if (neigbour.Value == 0 && !neigbour.IsStart)
                    neigbour.Value = currentTop.Value + 1;
                queue.Enqueue(neigbour);
            }
        }

        public virtual void FindDestionation(GraphTop start)
        {
            var currentTop = start;
            Visit(currentTop);
            while (!IsDestination(currentTop))
            {
                currentTop = queue.Dequeue();
                if (IsRightCellToVisit(currentTop))
                    Visit(currentTop);
                Pause(10);              
            }
            DestinationFound = queue.Count == 0 
                && !currentTop.IsEnd ? false : true;
        }

        public virtual void DrawPath(GraphTop end)
        {
            GraphTop top = end;
            while (!top.IsStart)
            {
                top = GoChippestNeighbour(top);
                Pause(250);
            }
        }

        public bool IsDestination(Button button)
        {
            var top = button as GraphTop;
            if (top is null)
                return false;
            return top.IsEnd || queue.Count == 0;
        }

        public void Visit(Button button)
        {
            var top = button as GraphTop;
            if (top is null)
                return;
            top.IsVisited = true;
            if (top.IsRightCellToColour())
                top.MarkAsVisited();
            ExtractNeighbours(top);
        }
    }
}
