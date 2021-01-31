// ------------------------------------------------------------------------------
// <copyright file="TriggerString.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.Build.Script
{
    public sealed class TriggerString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerString"/> class.
        /// </summary>
        public TriggerString()
        {
        }

        internal TriggerString(StreamReader reader)
        {
            ReadFrom(reader);
        }

        // Amount of blank lines before "STRING".
        public uint EmptyLineCount { get; set; }

        public uint Key { get; set; }

        public uint KeyPrecision { get; set; }

        // Text between "STRING" and the opening brace.
        public string? Comment { get; set; }

        public string? Value { get; set; }

        internal void ReadFrom(StreamReader reader)
        {
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
                    Key = uint.TryParse(keyString, out var result) ? result : 0;
                    KeyPrecision = (uint)keyString.Length;

                    // Read comment
                    var isFirstLine = true;
                    while (true)
                    {
                        line = reader.ReadLine();
                        if (line is null)
                        {
                            throw new InvalidDataException("Expected opening brace or comment.");
                        }
                        else if (string.Equals(line, "{", StringComparison.Ordinal))
                        {
                            break;
                        }
                        else if (line.StartsWith("//", StringComparison.Ordinal) || Comment is not null)
                        {
                            if (isFirstLine)
                            {
                                Comment = line[2..];
                                isFirstLine = false;
                            }
                            else
                            {
                                Comment += $"\r\n{line}";
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
                            Value = line;
                            isFirstLine = false;
                        }
                        else
                        {
                            Value += $"\r\n{line}";
                        }
                    }

                    break;
                }
                else
                {
                    EmptyLineCount++;
                }
            }
        }

        internal void WriteTo(StreamWriter writer)
        {
            for (nuint i = 0; i < EmptyLineCount; i++)
            {
                writer.WriteLine();
            }

            if (Value is not null)
            {
                writer.WriteLine($"STRING {Key.ToString($"D{KeyPrecision}")}");
                if (Comment is not null)
                {
                    writer.WriteLine($"//{Comment}");
                }

                writer.WriteLine("{");
                writer.WriteLine(Value);
                writer.WriteLine("}");
            }
        }
    }
}