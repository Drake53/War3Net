// ------------------------------------------------------------------------------
// <copyright file="CustomTextTrigger.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed class CustomTextTrigger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTextTrigger"/> class.
        /// </summary>
        public CustomTextTrigger()
        {
        }

        internal CustomTextTrigger(BinaryReader reader, Encoding encoding, MapCustomTextTriggersFormatVersion formatVersion, MapCustomTextTriggersSubVersion? subVersion)
        {
            ReadFrom(reader, encoding, formatVersion, subVersion);
        }

        public string? Code { get; set; }

        internal void ReadFrom(BinaryReader reader, Encoding encoding, MapCustomTextTriggersFormatVersion formatVersion, MapCustomTextTriggersSubVersion? subVersion)
        {
            var length = reader.ReadInt32();
            if (length == 0)
            {
                Code = string.Empty;
            }
            else
            {
                var bytes = reader.ReadBytes(length);
                Code = encoding.GetString(bytes, 0, bytes.Length);
            }
        }

        internal void WriteTo(BinaryWriter writer, Encoding encoding, MapCustomTextTriggersFormatVersion formatVersion, MapCustomTextTriggersSubVersion? subVersion)
        {
            writer.Write(encoding.GetBytes(Code ?? string.Empty).Length);
            writer.WriteString(Code, false);
        }
    }
}