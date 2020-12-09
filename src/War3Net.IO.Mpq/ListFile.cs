// ------------------------------------------------------------------------------
// <copyright file="ListFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

namespace War3Net.IO.Mpq
{
    public sealed class ListFile
    {
        public const string FileName = "(listfile)";

        internal ListFile()
        {
        }

        internal ListFile(StreamReader reader)
        {
            ReadFrom(reader);
        }

        public List<string> FileNames { get; init; } = new();

        internal void ReadFrom(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                var fileName = reader.ReadLine();
                if (!string.IsNullOrEmpty(fileName))
                {
                    FileNames.Add(fileName);
                }
            }
        }

        internal void WriteTo(StreamWriter writer)
        {
            foreach (var fileName in FileNames)
            {
                writer.WriteLine(fileName);
            }
        }
    }
}