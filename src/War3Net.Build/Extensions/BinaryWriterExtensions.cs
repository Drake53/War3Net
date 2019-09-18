// ------------------------------------------------------------------------------
// <copyright file="BinaryWriterExtensions.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build
{
    public static class BinaryWriterExtensions
    {
        public static void WriteString(this BinaryWriter writer, string s)
        {
            bool endsWithNullChar = false;
            foreach (var c in s)
            {
                writer.Write(c);
                endsWithNullChar = c == char.MinValue;
            }

            if (!endsWithNullChar)
            {
                writer.Write(char.MinValue);
            }
        }
    }
}