﻿using Common.ValueRanges;
using GraphLib.Realizations.Coordinates;
using System;
using static ConsoleVersion.Resource.Resources;

namespace ConsoleVersion.InputClass
{
    internal static class Input
    {
        /// <summary>
        /// Return user's console input in range of values
        /// </summary>
        /// <param name="accompanyingMessage">An input message</param>
        /// <param name="upperRangeValue">An upper value of input range</param>
        /// <param name="lowerRangeValue">A lower value of input range</param>
        /// <returns>A number in the range from 
        /// <paramref name="lowerRangeValue"/> to 
        /// <paramref name="upperRangeValue"/></returns>
        /// <exception cref="System.IO.IOException"></exception>
        public static int InputNumber(string accompanyingMessage,
            int upperRangeValue, int lowerRangeValue = 0)
        {
            var rangeOfValidInput = new InclusiveValueRange<int>(upperRangeValue, lowerRangeValue);
            return InputNumber(accompanyingMessage, rangeOfValidInput);
        }

        /// <summary>
        /// Return user's console input in range of values
        /// </summary>
        /// <param name="accompanyingMessage"></param>
        /// <param name="rangeOfValidInput"></param>
        /// <returns>A number in the range
        /// <paramref name="rangeOfValidInput"/></returns>
        /// <exception cref="System.IO.IOException"></exception>
        public static int InputNumber(string accompanyingMessage,
            IValueRange<int> rangeOfValidInput)
        {
            string userInput;
            do
            {
                Console.Write(accompanyingMessage);
                userInput = Console.ReadLine();
            } while (!IsValidInput(userInput, rangeOfValidInput));

            return Convert.ToInt32(userInput);
        }

        /// <summary>
        /// Returns <see cref="Coordinate2D"/> where X belongs to 
        /// [<paramref name="upperPossibleXValue"/>, 0]
        /// and where Y belongs to [<paramref name="upperPossibleYValue"/>, 0]
        /// </summary>
        /// <param name="upperPossibleXValue">An upper value of X
        /// coordinate in range where a lower value is 0</param>
        /// <param name="upperPossibleYValue">An upper value of Y 
        /// coordinate in range where a lower value is 0</param>
        /// <returns>An instance of <see cref="Coordinate2D"/></returns>
        public static Coordinate2D InputPoint(int upperPossibleXValue, int upperPossibleYValue)
        {
            int xCoordinate = InputNumber(XCoordinateInputMsg, upperPossibleXValue);
            int yCoordinate = InputNumber(YCoordinateInputMsg, upperPossibleYValue);

            return new Coordinate2D(xCoordinate, yCoordinate);
        }

        private static bool IsValidInput(string userInput,
            IValueRange<int> rangeOfValidInput)
        {
            return int.TryParse(userInput, out var input)
                && rangeOfValidInput.Contains(input);
        }
    }
}
