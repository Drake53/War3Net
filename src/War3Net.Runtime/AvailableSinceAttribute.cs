// ------------------------------------------------------------------------------
// <copyright file="AvailableSinceAttribute.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Build.Common;

namespace War3Net.Runtime
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Field)]
    public sealed class AvailableSinceAttribute : Attribute
    {
        public AvailableSinceAttribute(GamePatch gamePatch)
        {
            AvailableSince = gamePatch;
        }

        public GamePatch AvailableSince { get; private set; }
    }
}