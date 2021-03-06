﻿using ConsoleVersion.View.Abstraction;
using ConsoleVersion.View.Interface;
using Console = Colorful.Console;

namespace ConsoleVersion.View
{
    internal sealed class AxisView : IAxisView
    {
        public AxisView(FramedAxis axis,
            int cursorTop)
        {
            this.cursorTop = cursorTop;
            this.axis = axis;
        }

        public void Show()
        {
            Console.SetCursorPosition(0, cursorTop);
            Console.Write(axis.GetFramedAxis());
        }

        private readonly FramedAxis axis;
        private readonly int cursorTop;
    }
}