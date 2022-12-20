// ------------------------------------------------------------------------------
// <copyright file="TriggerString.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    public sealed partial class TriggerString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerString"/> class.
        /// </summary>
        public TriggerString()
        {
        }

        // Amount of blank lines before "STRING".
        public uint EmptyLineCount { get; set; }

        public uint Key { get; set; }

        public uint KeyPrecision { get; set; }

        // Text between "STRING" and the opening brace.
        public string? Comment { get; set; }

        public string? Value { get; set; }

        public override string? ToString() => Value;
    }
}