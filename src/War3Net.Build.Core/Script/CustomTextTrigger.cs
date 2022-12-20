// ------------------------------------------------------------------------------
// <copyright file="CustomTextTrigger.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    public sealed partial class CustomTextTrigger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTextTrigger"/> class.
        /// </summary>
        public CustomTextTrigger()
        {
        }

        public string? Code { get; set; }

        public override string? ToString() => Code;
    }
}