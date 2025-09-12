// ------------------------------------------------------------------------------
// <copyright file="SylkParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace War3Net.IO.Slk
{
    public sealed class SylkParser
    {
        public SylkTable Parse(Stream input, bool leaveOpen = false)
        {
            var lines = new List<string>();
            using var reader = new StreamReader(input, Encoding.UTF8, true, 1024, leaveOpen);
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return Parse(lines);
        }

        public SylkTable Parse(List<string> lines)
        {
            int? maxX = null;
            int? maxY = null;

            var bLine = lines.FirstOrDefault(x => x.StartsWith("B;", StringComparison.InvariantCultureIgnoreCase));
            if (!string.IsNullOrWhiteSpace(bLine))
            {
                var parts = bLine.Split(';', StringSplitOptions.TrimEntries);
                foreach (var part in parts)
                {
                    if (part.StartsWith("X", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (int.TryParse(part.Substring(1), out int x))
                        {
                            maxX = x;
                        }
                    }
                    else if (part.StartsWith("Y", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (int.TryParse(part.Substring(1), out int y))
                        {
                            maxY = y;
                        }
                    }
                    else if (part.StartsWith("D", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var dContent = part.Substring(1).Trim();
                        var dParts = dContent.Split(' ');
                        if (dParts.Length == 2)
                        {
                            if (int.TryParse(dParts[0], out int y))
                            {
                                maxY ??= y;
                            }
                            if (int.TryParse(dParts[1], out int x))
                            {
                                maxX ??= x;
                            }
                        }
                        else if (dParts.Length == 4)
                        {
                            int.TryParse(dParts[0], out int startY);
                            int.TryParse(dParts[1], out int startX);
                            if (int.TryParse(dParts[2], out int y))
                            {
                                maxY ??= (y - startY);
                            }
                            if (int.TryParse(dParts[3], out int x))
                            {
                                maxX ??= (x - startX);
                            }
                        }
                    }
                }
            }

            var _table = new SylkTable(maxX ?? 0, maxY ?? 0);


            int nextX = 0;
            int nextY = 0;

            foreach (var line in lines)
            {
                var isCell = line.StartsWith("C;", StringComparison.InvariantCultureIgnoreCase);
                var isFormatting = line.StartsWith("F;", StringComparison.InvariantCultureIgnoreCase);
                if (isCell || isFormatting)
                {
                    int? x = null;
                    int? y = null;
                    object value = null;

                    var parts = line.Split(";", StringSplitOptions.TrimEntries);
                    foreach (var part in parts)
                    {
                        if (part.StartsWith("X", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (int.TryParse(part.Substring(1), out int parsedX))
                            {
                                x = parsedX - 1;
                            }
                        }
                        else if (part.StartsWith("Y", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (int.TryParse(part.Substring(1), out int parsedY))
                            {
                                y = parsedY - 1;
                            }
                        }
                        else if (part.StartsWith("K", StringComparison.InvariantCultureIgnoreCase))
                        {
                            value = ParseValueString(part.Substring(1));
                        }
                    }

                    if (isFormatting && x == null && y == null)
                    {
                        continue;
                    }

                    x ??= nextX;
                    y ??= nextY;

                    nextX = x.Value;
                    nextY = y.Value;

                    if (isCell)
                    {
                        nextX++;
                    }

                    if (maxX.HasValue && nextX >= maxX)
                    {
                        nextX = 0;
                        nextY++;
                    }

                    if (value != null)
                    {
                        if (x.Value >= _table.Width || y.Value >= _table.Height)
                        {
                            _table.Resize(Math.Max(x.Value+1, _table.Width), Math.Max(y.Value+1, _table.Height));
                        }

                        _table[x.Value, y.Value] = value;
                    }
                }
            }

            return _table;
        }

        private object ParseValueString(string value)
        {
            if (value.StartsWith('"'))
            {
                if (!value.EndsWith('"'))
                {
                    value = value + "\"";
                }

                return value.Substring(1, value.Length - 2);
            }
            else if (string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else if (string.Equals(value, bool.FalseString, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else if (string.Equals(value, "#VALUE!", StringComparison.Ordinal) || string.Equals(value, "#REF!", StringComparison.Ordinal) || string.IsNullOrEmpty(value))
            {
                return 0;
            }
            else if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
            {
                return intValue;
            }
            else if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var floatValue))
            {
                return floatValue;
            }
            else if (int.TryParse("0" + value, NumberStyles.Integer, CultureInfo.InvariantCulture, out intValue))
            {
                return intValue;
            }
            else if (float.TryParse("0" + value, NumberStyles.Float, CultureInfo.InvariantCulture, out floatValue))
            {
                return floatValue;
            }

            return null;
        }
    }
}