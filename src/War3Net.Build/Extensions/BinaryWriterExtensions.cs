// ------------------------------------------------------------------------------
// <copyright file="BinaryWriterExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Extensions
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