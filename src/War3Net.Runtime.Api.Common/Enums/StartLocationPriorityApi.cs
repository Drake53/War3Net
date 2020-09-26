// ------------------------------------------------------------------------------
// <copyright file="StartLocationPriorityApi.cs" company="Drake53">
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
    public static class StartLocationPriorityApi
    {
        public static readonly StartLocationPriority MAP_LOC_PRIO_LOW = ConvertStartLocPrio((int)StartLocationPriority.Type.Low);
        public static readonly StartLocationPriority MAP_LOC_PRIO_HIGH = ConvertStartLocPrio((int)StartLocationPriority.Type.High);
        public static readonly StartLocationPriority MAP_LOC_PRIO_NOT = ConvertStartLocPrio((int)StartLocationPriority.Type.Not);

        public static StartLocationPriority ConvertStartLocPrio(int i)
        {
            return StartLocationPriority.GetStartLocationPriority(i);
        }
    }
}