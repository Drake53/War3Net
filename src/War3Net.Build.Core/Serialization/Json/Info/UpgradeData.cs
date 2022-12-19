// ------------------------------------------------------------------------------
// <copyright file="UpgradeData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;

using War3Net.Build.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class UpgradeData
    {
        internal void ReadFrom(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            throw new NotImplementedException();
        }

        internal void WriteTo(Utf8JsonWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.Write(nameof(Players), Players);
            writer.WriteNumber(nameof(Id), Id);
            writer.WriteNumber(nameof(Level), Level);
            writer.WriteNumber(nameof(Availability), (int)Availability);

            writer.WriteEndObject();
        }
    }
}