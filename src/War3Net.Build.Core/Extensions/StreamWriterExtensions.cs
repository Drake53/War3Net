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
    public static class StreamWriterExtensions
    {
        public static void Write(this StreamWriter writer, TriggerStrings triggerStrings) => triggerStrings.WriteTo(writer);

        public static void Write(this StreamWriter writer, MapTriggerString mapTriggerString) => mapTriggerString.WriteTo(writer);
    }
}