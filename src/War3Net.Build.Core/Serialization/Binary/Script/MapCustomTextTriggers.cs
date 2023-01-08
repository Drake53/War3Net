// ------------------------------------------------------------------------------
// <copyright file="MapCustomTextTriggers.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class MapCustomTextTriggers
    {
        internal MapCustomTextTriggers(BinaryReader reader, Encoding encoding)
        {
            ReadFrom(reader, encoding);
        }

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

            if (FormatVersion >= MapCustomTextTriggersFormatVersion.v1)
            {
                GlobalCustomScriptComment = reader.ReadChars();
                GlobalCustomScriptCode = reader.ReadCustomTextTrigger(encoding, FormatVersion, SubVersion);
            }

            if (SubVersion is null || SubVersion == MapCustomTextTriggersSubVersion.v1)
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
            if (FormatVersion >= MapCustomTextTriggersFormatVersion.v1)
            {
                writer.WriteString(GlobalCustomScriptComment);
                writer.Write(GlobalCustomScriptCode, encoding, FormatVersion, SubVersion);
            }

            if (SubVersion is null || SubVersion == MapCustomTextTriggersSubVersion.v1)
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