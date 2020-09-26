// ------------------------------------------------------------------------------
// <copyright file="VersionApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Enums;

namespace War3Net.Runtime.Api.Common.Enums
{
    public static class VersionApi
    {
        public static readonly Version VERSION_REIGN_OF_CHAOS = ConvertVersion((int)Version.Type.ReignOfChaos);
        public static readonly Version VERSION_FROZEN_THRONE = ConvertVersion((int)Version.Type.FrozenThrone);

        public static Version ConvertVersion(int i)
        {
            return Version.GetVersion(i);
        }
    }
}