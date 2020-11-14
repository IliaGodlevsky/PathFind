﻿using System.Text;

namespace Common.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendFormatLine(this StringBuilder builder,
            string format, params object[] args)
        {
            return builder.AppendFormat(format, args).AppendLine();
        }
    }
}