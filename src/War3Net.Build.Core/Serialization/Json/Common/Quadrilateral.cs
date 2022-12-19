// ------------------------------------------------------------------------------
// <copyright file="Quadrilateral.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;

using War3Net.Build.Extensions;

namespace War3Net.Build.Common
{
    public sealed partial class Quadrilateral
    {
        internal void ReadFrom(ref Utf8JsonReader reader)
        {
            throw new NotImplementedException();
        }

        internal void WriteTo(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.Write(nameof(BottomLeft), BottomLeft);
            writer.Write(nameof(TopRight), TopRight);
            writer.Write(nameof(TopLeft), TopLeft);
            writer.Write(nameof(BottomRight), BottomRight);

            writer.WriteEndObject();
        }
    }
}