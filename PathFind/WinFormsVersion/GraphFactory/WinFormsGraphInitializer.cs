﻿using GraphLibrary.GraphFactory;
using SearchAlgorythms.Graph;
using SearchAlgorythms.Top;
using System.Drawing;

namespace SearchAlgorythms.GraphFactory
{
    public class WinFormsGraphInitializer : AbstractGraphInitializer
    {

        public WinFormsGraphInitializer(VertexInfo[,] info, int placeBetweenTops)
            : base(info, placeBetweenTops)
        {
           
        }

        public override IVertex CreateVertex(VertexInfo info)
        {
            return new WinFormsVertex(info);
        }

        public override AbstractGraph GetGraph()
        {
            return new WinFormsGraph(vertices);
        }

        public override void SetGraph(int width, int height)
        {
            vertices = new WinFormsVertex[width, height];
        }
    }
}