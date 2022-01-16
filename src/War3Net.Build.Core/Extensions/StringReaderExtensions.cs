// ------------------------------------------------------------------------------
// <copyright file="StringReaderExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Script;

namespace War3Net.Build.Extensions
{
    public static class StringReaderExtensions
    {
        public static TriggerData ReadTriggerData(this StringReader reader) => new TriggerData(reader);
    }
}