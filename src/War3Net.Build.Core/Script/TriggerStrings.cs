// ------------------------------------------------------------------------------
// <copyright file="TriggerStrings.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Script
{
    public abstract partial class TriggerStrings
    {
        internal TriggerStrings()
        {
        }

        public List<TriggerString> Strings { get; init; } = new();
    }
}