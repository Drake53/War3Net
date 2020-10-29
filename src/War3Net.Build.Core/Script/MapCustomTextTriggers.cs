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

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed class MapCustomTextTriggers
    {
        public const string FileName = "war3map.wct";

        private const int NewFormatId = unchecked((int)0x80000004);

        private readonly List<string> _customTextTriggers;

        private MapCustomTextTriggersFormatVersion _version;
        private bool _newFormat;
        private string _rootTriggerComment;
        private string _rootTriggerText;

        private MapCustomTextTriggers()
        {
            _customTextTriggers = new List<string>();
        }

        public static bool IsRequired => false;

        public MapCustomTextTriggersFormatVersion FormatVersion
        {
            get => _version;
            set => _version = value;
        }

        public bool UseNewFormat
        {
            get => _newFormat;
            set => _newFormat = value;
        }

        public static MapCustomTextTriggers Parse(Stream stream, bool leaveOpen = false)
        {
            try
            {
                var triggers = new MapCustomTextTriggers();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    triggers._version = (MapCustomTextTriggersFormatVersion)reader.ReadInt32();
                    if (!Enum.IsDefined(typeof(MapCustomTextTriggersFormatVersion), triggers._version))
                    {
                        if ((int)triggers._version != NewFormatId)
                        {
                            throw new NotSupportedException($"Unknown version of '{FileName}': {triggers._version}");
                        }

                        triggers._version = reader.ReadInt32<MapCustomTextTriggersFormatVersion>();
                        triggers._newFormat = true;
                    }

                    if (triggers._version >= MapCustomTextTriggersFormatVersion.Tft)
                    {
                        triggers._rootTriggerComment = reader.ReadChars();
                        triggers._rootTriggerText = ParseCustomTextTriggerText(reader);
                    }

                    if (triggers._newFormat)
                    {
                        while (stream.Position < stream.Length)
                        {
                            triggers._customTextTriggers.Add(ParseCustomTextTriggerText(reader));
                        }
                    }
                    else
                    {
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            triggers._customTextTriggers.Add(ParseCustomTextTriggerText(reader));
                        }
                    }
                }

                return triggers;
            }
            catch (DecoderFallbackException e)
            {
                throw new InvalidDataException($"The '{FileName}' file contains invalid characters.", e);
            }
            catch (EndOfStreamException e)
            {
                throw new InvalidDataException($"The '{FileName}' file is missing data, or its data is invalid.", e);
            }
            catch
            {
                throw;
            }
        }

        public static void Serialize(MapCustomTextTriggers mapTriggers, Stream stream, bool leaveOpen = false)
        {
            mapTriggers.SerializeTo(stream, leaveOpen);
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                if (_newFormat)
                {
                    writer.Write(NewFormatId);
                }

                writer.Write((int)_version);
                if (_version >= MapCustomTextTriggersFormatVersion.Tft)
                {
                    writer.WriteString(_rootTriggerComment);
                    writer.Write(Encoding.UTF8.GetBytes(_rootTriggerText).Length);
                    writer.WriteString(_rootTriggerText, false);
                }

                if (!_newFormat)
                {
                    writer.Write(_customTextTriggers.Count);
                }

                for (var i = 0; i < _customTextTriggers.Count; i++)
                {
                    var triggerText = _customTextTriggers[i];
                    writer.Write(Encoding.UTF8.GetBytes(triggerText).Length);
                    writer.WriteString(triggerText, false);
                }
            }
        }

        private static string ParseCustomTextTriggerText(BinaryReader reader)
        {
            var length = reader.ReadInt32();
            if (length == 0)
            {
                return string.Empty;
            }

            var bytes = reader.ReadBytes(length);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}