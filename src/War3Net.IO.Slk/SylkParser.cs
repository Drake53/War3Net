// ------------------------------------------------------------------------------
// <copyright file="SylkParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace War3Net.IO.Slk
{
    public sealed class SylkParser
    {
        private SylkTable _table;
        private int? _lastX;
        private int? _lastY;

        public SylkParser()
        {
            _lastX = null;
            _lastY = null;
        }

        public SylkTable Parse(Stream input, bool leaveOpen = false)
        {
            using var reader = new StreamReader(input, Encoding.UTF8, true, 1024, leaveOpen);

            var isOnFirstLine = true;
            while (true)
            {
                var line = reader.ReadLine();
                var fields = line.Split(';');
                var recordType = fields[0];

                string GetField(string fieldName)
                {
                    foreach (var field in fields)
                    {
                        if (field.StartsWith(fieldName, StringComparison.Ordinal))
                        {
                            return field.Substring(fieldName.Length);
                        }
                    }

                    throw new InvalidDataException($"Record does not contain mandatory field of type '{fieldName}'.");
                }

                string? GetOptionalField(string fieldName)
                {
                    foreach (var field in fields)
                    {
                        if (field.StartsWith(fieldName, StringComparison.Ordinal))
                        {
                            return field.Substring(fieldName.Length);
                        }
                    }

                    return null;
                }

                if (isOnFirstLine)
                {
                    isOnFirstLine = false;
                    if (recordType != "ID")
                    {
                        throw new InvalidDataException("SYLK file must start with 'ID'.");
                    }

                    GetField("P");
                }
                else
                {
                    switch (recordType)
                    {
                        case "ID":
                            throw new InvalidDataException("Record type 'ID' can only occur on the first line.");

                        case "B":
                            if (_table != null)
                            {
                                throw new InvalidDataException("Only one record of type 'B' may be present.");
                            }

                            _table = new SylkTable(
                                int.Parse(GetField("X"), NumberStyles.Integer, CultureInfo.InvariantCulture),
                                int.Parse(GetField("Y"), NumberStyles.Integer, CultureInfo.InvariantCulture));

                            break;

                        case "C":
                            if (_table == null)
                            {
                                throw new InvalidDataException("Unable to parse record of type 'C' before encountering a record of type 'B'.");
                            }

                            SetCellContent(
                                GetOptionalField("X"),
                                GetOptionalField("Y"),
                                GetOptionalField("K") ?? string.Empty);

                            break;

                        case "E":
                            return _table;

                        default:
                            throw new NotSupportedException($"Support for record type '{recordType}' is not implemented. Only records of type 'ID', 'B', 'C', and 'E' are supported.");
                    }
                }
            }
        }

        /// <param name="x">The cell's 1-indexed X position.</param>
        /// <param name="y">The cell's 1-indexed Y position.</param>
        private void SetCellContent(string? x, string? y, string value)
        {
            var xi = x is not null
                ? (int.Parse(x, NumberStyles.Integer, CultureInfo.InvariantCulture) - 1)
                : _lastX ?? throw new InvalidDataException("Column for cell is not defined.");

            var yi = y is not null
                ? (int.Parse(y, NumberStyles.Integer, CultureInfo.InvariantCulture) - 1)
                : _lastY ?? throw new InvalidDataException("Row for cell is not defined.");

            if (value.Length >= 2 && value[0] == '"' && value[^1] == '"')
            {
                _table[xi, yi] = value[1..^1];
            }
            else if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var @int))
            {
                _table[xi, yi] = @int;
            }
            else if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var @float))
            {
                _table[xi, yi] = @float;
            }
            else if (string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                _table[xi, yi] = true;
            }
            else if (string.Equals(value, bool.FalseString, StringComparison.OrdinalIgnoreCase))
            {
                _table[xi, yi] = false;
            }
            else if (string.Equals(value, "#VALUE!", StringComparison.Ordinal) || string.Equals(value, "#REF!", StringComparison.Ordinal))
            {
                _table[xi, yi] = 0;
            }
            else
            {
                throw new NotSupportedException($"Unable to parse value '{value}'. Can only parse strings, integers, floats, and booleans.");
            }

            _lastX = xi;
            _lastY = yi;
        }
    }
}