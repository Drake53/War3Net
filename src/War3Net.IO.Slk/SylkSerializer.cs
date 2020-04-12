// ------------------------------------------------------------------------------
// <copyright file="SylkSerializer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace War3Net.IO.Slk
{
    public sealed class SylkSerializer
    {
        private SylkTable _table;

        public SylkSerializer(SylkTable table)
        {
            _table = table;
        }

        public void SerializeTo(Stream stream, bool leaveOpen)
        {
            using var writer = new StreamWriter(stream, new UTF8Encoding(false, true), 1024, leaveOpen);

            writer.WriteLine("ID;P");
            writer.WriteLine($"B;X{_table.Width};Y{_table.Height}");
            for (var row = 1; row <= _table.Height; row++)
            {
                var isFirstCellOnThisRow = true;
                for (var column = 1; column <= _table.Width; column++)
                {
                    var cellContent = _table[column - 1, row - 1];
                    if (cellContent is null)
                    {
                        continue;
                    }

                    if (cellContent is string s)
                    {
                        cellContent = $"\"{s}\"";
                    }

                    if (isFirstCellOnThisRow)
                    {
                        writer.WriteLine($"C;X{column};Y{row};K{cellContent}");
                        isFirstCellOnThisRow = false;
                    }
                    else
                    {
                        writer.WriteLine($"C;X{column};K{cellContent}");
                    }
                }
            }

            writer.WriteLine("E");
        }
    }
}