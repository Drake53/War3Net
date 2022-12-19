// ------------------------------------------------------------------------------
// <copyright file="ForceData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;

using War3Net.Build.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class ForceData
    {
        internal void ReadFrom(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            throw new NotImplementedException();
        }

        internal void WriteTo(Utf8JsonWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Flags), (int)Flags);
            writer.Write(nameof(Players), Players);
            writer.WriteString(nameof(Name), Name);

            writer.WriteEndObject();
        }
    }
}