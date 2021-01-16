// ------------------------------------------------------------------------------
// <copyright file="MapCustomTextTriggers.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed class MapCustomTextTriggers
    {
        public const string FileName = "war3map.wct";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapCustomTextTriggers"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        /// <param name="subVersion"></param>
        public MapCustomTextTriggers(MapCustomTextTriggersFormatVersion formatVersion, MapCustomTextTriggersSubVersion? subVersion)
        {
            FormatVersion = formatVersion;
            SubVersion = subVersion;
        }

        internal MapCustomTextTriggers(BinaryReader reader, Encoding encoding)
        {
            ReadFrom(reader, encoding);
        }

        public MapCustomTextTriggersFormatVersion FormatVersion { get; set; }

        public MapCustomTextTriggersSubVersion? SubVersion { get; set; }

        public string GlobalCustomScriptComment { get; set; }

        public CustomTextTrigger GlobalCustomScriptCode { get; set; }

        public List<CustomTextTrigger> CustomTextTriggers { get; init; } = new();

        internal void ReadFrom(BinaryReader reader, Encoding encoding)
        {
            var version = reader.ReadInt32();
            if (Enum.IsDefined(typeof(MapCustomTextTriggersFormatVersion), version))
            {
                FormatVersion = (MapCustomTextTriggersFormatVersion)version;
                SubVersion = null;
            }
            else if (Enum.IsDefined(typeof(MapCustomTextTriggersSubVersion), version))
            {
                FormatVersion = reader.ReadInt32<MapCustomTextTriggersFormatVersion>();
                SubVersion = (MapCustomTextTriggersSubVersion)version;
            }
            else
            {
                throw new NotSupportedException($"Unknown version of '{FileName}': {version}");
            }

            if (FormatVersion >= MapCustomTextTriggersFormatVersion.Tft)
            {
                GlobalCustomScriptComment = reader.ReadChars();
                GlobalCustomScriptCode = reader.ReadCustomTextTrigger(encoding, FormatVersion, SubVersion);
            }

            if (SubVersion is null || SubVersion == MapCustomTextTriggersSubVersion.NewBETA)
            {
                nint customTextTriggerCount = reader.ReadInt32();
                for (nint i = 0; i < customTextTriggerCount; i++)
                {
                    CustomTextTriggers.Add(reader.ReadCustomTextTrigger(encoding, FormatVersion, SubVersion));
                }
            }
            else
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    CustomTextTriggers.Add(reader.ReadCustomTextTrigger(encoding, FormatVersion, SubVersion));
                }
            }
        }

        internal void WriteTo(BinaryWriter writer, Encoding encoding)
        {
            if (SubVersion is not null)
            {
                writer.Write((int)SubVersion);
            }

            writer.Write((int)FormatVersion);
            if (FormatVersion >= MapCustomTextTriggersFormatVersion.Tft)
            {
                writer.WriteString(GlobalCustomScriptComment);
                writer.Write(GlobalCustomScriptCode, encoding, FormatVersion, SubVersion);
            }

            if (SubVersion is null || SubVersion == MapCustomTextTriggersSubVersion.NewBETA)
            {
                writer.Write(CustomTextTriggers.Count);
            }

            foreach (var customTextTrigger in CustomTextTriggers)
            {
                writer.Write(customTextTrigger, encoding, FormatVersion, SubVersion);
            }
        }
    }
}