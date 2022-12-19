// ------------------------------------------------------------------------------
// <copyright file="RectangleMargins.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;

namespace War3Net.Build.Common
{
    public sealed partial class RectangleMargins
    {
        internal void ReadFrom(ref Utf8JsonReader reader)
        {
            throw new NotImplementedException();
        }

        internal void WriteTo(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Left), Left);
            writer.WriteNumber(nameof(Right), Right);
            writer.WriteNumber(nameof(Bottom), Bottom);
            writer.WriteNumber(nameof(Top), Top);

            writer.WriteEndObject();
        }
    }
}