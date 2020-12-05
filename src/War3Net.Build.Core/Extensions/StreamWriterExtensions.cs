// ------------------------------------------------------------------------------
// <copyright file="StreamWriterExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Script;

namespace War3Net.Build.Extensions
{
    // Unlike BinaryWriterExtensions, the extension method names here cannot simply be 'Write", because StreamWriter contains a Write(object) method.
    public static class StreamWriterExtensions
    {
        public static void WriteTriggerStrings(this StreamWriter writer, TriggerStrings triggerStrings) => triggerStrings.WriteTo(writer);

        public static void WriteTriggerString(this StreamWriter writer, TriggerString triggerString) => triggerString.WriteTo(writer);
    }
}