// ------------------------------------------------------------------------------
// <copyright file="RegenerationTypeApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Common.Enums;

namespace War3Net.Runtime.Common.Api.Enums
{
    public static class RegenerationTypeApi
    {
        public static readonly RegenerationType REGENERATION_TYPE_NONE = ConvertRegenType((int)RegenerationType.Type.None);
        public static readonly RegenerationType REGENERATION_TYPE_ALWAYS = ConvertRegenType((int)RegenerationType.Type.Always);
        public static readonly RegenerationType REGENERATION_TYPE_BLIGHT = ConvertRegenType((int)RegenerationType.Type.Blight);
        public static readonly RegenerationType REGENERATION_TYPE_DAY = ConvertRegenType((int)RegenerationType.Type.Day);
        public static readonly RegenerationType REGENERATION_TYPE_NIGHT = ConvertRegenType((int)RegenerationType.Type.Night);

        public static RegenerationType ConvertRegenType(int i)
        {
            return RegenerationType.GetRegenerationType(i);
        }
    }
}