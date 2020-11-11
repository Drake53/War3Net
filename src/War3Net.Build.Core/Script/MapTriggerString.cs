// ------------------------------------------------------------------------------
// <copyright file="MapTriggerString.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.Build.Script
{
    public sealed class MapTriggerString
    {
        private uint _emptyLines;
        private uint _keyLength;
        private uint _key;
        private string? _comment;
        private string? _value;

        private MapTriggerString()
        {
        }

        public uint Key => _key;

        public string? Comment => _comment;

        public string? Value => _value;

        public static MapTriggerString Parse(Stream stream, bool leaveOpen)
        {
            using (var reader = new StreamReader(stream, leaveOpen: leaveOpen))
            {
                return ReadFrom(reader);
            }
        }

        public static MapTriggerString ReadFrom(StreamReader reader)
        {
            var triggerString = new MapTriggerString();

            while (true)
            {
                var line = reader.ReadLine();
                if (line is null)
                {
                    break;
                }

                if (line.StartsWith("STRING ", StringComparison.Ordinal))
                {
                    // Read key
                    var keyString = line[7..].Trim();
                    triggerString._keyLength = (uint)keyString.Length;
                    triggerString._key = uint.TryParse(keyString, out var result) ? result : 0;

                    // Read comment
                    var isFirstLine = true;
                    while (true)
                    {
                        line = reader.ReadLine();
                        if (string.Equals(line, "{", StringComparison.Ordinal))
                        {
                            break;
                        }
                        else if (line.StartsWith("//", StringComparison.Ordinal) || triggerString._comment is not null)
                        {
                            if (isFirstLine)
                            {
                                triggerString._comment = line[2..];
                                isFirstLine = false;
                            }
                            else
                            {
                                triggerString._comment += $"\r\n{line}";
                            }
                        }
                        else
                        {
                            throw new InvalidDataException("Expected opening brace or comment.");
                        }
                    }

                    // Read value
                    isFirstLine = true;
                    while (true)
                    {
                        line = reader.ReadLine();
                        if (string.Equals(line, "}", StringComparison.Ordinal))
                        {
                            break;
                        }

                        if (isFirstLine)
                        {
                            triggerString._value = line;
                            isFirstLine = false;
                        }
                        else
                        {
                            triggerString._value += $"\r\n{line}";
                        }
                    }

                    break;
                }
                else
                {
                    triggerString._emptyLines++;
                }
            }

            return triggerString;
        }

        public void WriteTo(StreamWriter writer)
        {
            for (var i = 0; i < _emptyLines; i++)
            {
                writer.WriteLine();
            }

            if (_value is not null)
            {
                writer.WriteLine($"STRING {_key.ToString($"D{_keyLength}")}");
                if (_comment is not null)
                {
                    writer.WriteLine($"//{_comment}");
                }

                writer.WriteLine("{");
                writer.WriteLine(_value);
                writer.WriteLine("}");
            }
        }
    }
}