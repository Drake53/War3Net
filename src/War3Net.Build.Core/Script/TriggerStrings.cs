// ------------------------------------------------------------------------------
// <copyright file="TriggerStrings.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using War3Net.Build.Extensions;

namespace War3Net.Build.Script
{
    public abstract class TriggerStrings
    {
        internal TriggerStrings()
        {
        }

        internal TriggerStrings(StreamReader reader)
        {
            ReadFrom(reader);
        }

        public List<TriggerString> Strings { get; init; } = new();

        internal void ReadFrom(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                Strings.Add(reader.ReadTriggerString());
            }

            _encoding = reader.CurrentEncoding;
        }

        internal void WriteTo(StreamWriter writer)
        {
            foreach (var triggerString in Strings)
            {
                writer.WriteTriggerString(triggerString);
            }
        }
    }
}