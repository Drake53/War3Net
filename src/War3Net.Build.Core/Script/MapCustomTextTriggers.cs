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

        private const int NewFormatId = unchecked((int)0x80000004);

        /// <summary>
        /// Initializes a new instance of the <see cref="MapCustomTextTriggers"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        /// <param name="encoding"></param>
        public MapCustomTextTriggers(MapCustomTextTriggersFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        internal MapCustomTextTriggers(BinaryReader reader, Encoding encoding)
        {
            ReadFrom(reader, encoding);
        }

        public MapCustomTextTriggersFormatVersion FormatVersion { get; set; }

        public bool UseNewFormat { get; set; }

        public string GlobalCustomScriptComment { get; set; }

        public CustomTextTrigger GlobalCustomScriptCode { get; set; }

        public List<CustomTextTrigger> CustomTextTriggers { get; init; } = new();

        internal void ReadFrom(BinaryReader reader, Encoding encoding)
        {
            FormatVersion = (MapCustomTextTriggersFormatVersion)reader.ReadInt32();
            if (!Enum.IsDefined(typeof(MapCustomTextTriggersFormatVersion), FormatVersion))
            {
                if ((int)FormatVersion != NewFormatId)
                {
                    throw new NotSupportedException($"Unknown version of '{FileName}': {FormatVersion}");
                }

                FormatVersion = reader.ReadInt32<MapCustomTextTriggersFormatVersion>();
                UseNewFormat = true;
            }

            if (FormatVersion >= MapCustomTextTriggersFormatVersion.Tft)
            {
                GlobalCustomScriptComment = reader.ReadChars();
                GlobalCustomScriptCode = reader.ReadCustomTextTrigger(encoding, FormatVersion, UseNewFormat);
            }

            if (UseNewFormat)
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    CustomTextTriggers.Add(reader.ReadCustomTextTrigger(encoding, FormatVersion, UseNewFormat));
                }
            }
            else
            {
                nint customTextTriggerCount = reader.ReadInt32();
                for (nint i = 0; i < customTextTriggerCount; i++)
                {
                    CustomTextTriggers.Add(reader.ReadCustomTextTrigger(encoding, FormatVersion, UseNewFormat));
                }
            }
        }

        internal void WriteTo(BinaryWriter writer, Encoding encoding)
        {
            if (UseNewFormat)
            {
                writer.Write(NewFormatId);
            }

            writer.Write((int)FormatVersion);
            if (FormatVersion >= MapCustomTextTriggersFormatVersion.Tft)
            {
                writer.WriteString(GlobalCustomScriptComment);
                writer.Write(GlobalCustomScriptCode, encoding, FormatVersion, UseNewFormat);
            }

            if (!UseNewFormat)
            {
                writer.Write(CustomTextTriggers.Count);
            }

            foreach (var customTextTrigger in CustomTextTriggers)
            {
                writer.Write(customTextTrigger, encoding, FormatVersion, UseNewFormat);
            }
        }
    }
}