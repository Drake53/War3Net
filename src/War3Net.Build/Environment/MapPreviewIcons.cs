// ------------------------------------------------------------------------------
// <copyright file="MapPreviewIcons.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace War3Net.Build.Environment
{
    public sealed class MapPreviewIcons : IEnumerable<MapPreviewIcon>
    {
        public const string FileName = "war3map.mmp";
        public const MapPreviewIconsFormatVersion LatestVersion = MapPreviewIconsFormatVersion.Normal;

        private readonly List<MapPreviewIcon> _icons;

        private MapPreviewIconsFormatVersion _version;

        public MapPreviewIcons(IEnumerable<MapPreviewIcon> icons)
        {
            _icons = new List<MapPreviewIcon>(icons);
        }

        public MapPreviewIcons(params MapPreviewIcon[] icons)
        {
            _icons = new List<MapPreviewIcon>(icons);
        }

        private MapPreviewIcons()
        {
            _icons = new List<MapPreviewIcon>();
        }

        public MapPreviewIconsFormatVersion FormatVersion
        {
            get => _version;
            set => _version = value;
        }

        public static MapPreviewIcons Default => new MapPreviewIcons(Array.Empty<MapPreviewIcon>());

        public static bool IsRequired => false;

        public static MapPreviewIcons Parse(Stream stream, bool leaveOpen = false)
        {
            try
            {
                var data = new MapPreviewIcons();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    data._version = (MapPreviewIconsFormatVersion)reader.ReadInt32();

                    var iconCount = reader.ReadInt32();
                    for (var i = 0; i < iconCount; i++)
                    {
                        data._icons.Add(MapPreviewIcon.Parse(stream, true));
                    }
                }

                return data;
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

        public static void Serialize(MapPreviewIcons mapIcons, Stream stream, bool leaveOpen = false)
        {
            mapIcons.SerializeTo(stream, leaveOpen);
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write((int)_version);

                writer.Write(_icons.Count);
                foreach (var icon in _icons)
                {
                    icon.WriteTo(writer);
                }
            }
        }

        public IEnumerator<MapPreviewIcon> GetEnumerator()
        {
            return ((IEnumerable<MapPreviewIcon>)_icons).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<MapPreviewIcon>)_icons).GetEnumerator();
        }
    }
}