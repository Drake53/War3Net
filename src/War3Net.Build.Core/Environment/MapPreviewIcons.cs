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

using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.Build.Widget;
using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed class MapPreviewIcons : IEnumerable<MapPreviewIcon>
    {
        public const string FileName = "war3map.mmp";
        public const MapPreviewIconsFormatVersion LatestVersion = MapPreviewIconsFormatVersion.Normal;

        private readonly List<MapPreviewIcon> _icons;

        private MapPreviewIconsFormatVersion _version;

        public MapPreviewIcons(MapInfo info, MapEnvironment environment, MapUnits? units)
        {
            var padding = info.CameraBoundsComplements;

            var minX = environment.Left + (128 * padding.Left);
            var width = environment.Right - (128 * padding.Right) - minX;

            var minY = environment.Bottom + (128 * padding.Bottom);
            var height = environment.Top - (128 * padding.Top) - minY;

            var ratio = width / height;
            var sizeX = 255 * (ratio > 1 ? 1 : ratio);
            var sizeY = 255 * (ratio > 1 ? 1 / ratio : 1);

            var lowerX = 1 + (0.5f * (255 - sizeX));
            var upperY = 1 + (0.5f * (255 + sizeY));

            sizeX /= width;
            sizeY /= height;

            _icons = new List<MapPreviewIcon>();
            if (units != null)
            {
                foreach (var unit in units)
                {
                    if (MapPreviewIconProvider.TryGetIcon(unit.TypeId, unit.Owner, out var iconType, out var iconColor))
                    {
                        _icons.Add(new MapPreviewIcon()
                        {
                            IconType = iconType,
                            X = (byte)(lowerX + (sizeX * (unit.PositionX - minX))),
                            Y = (byte)(upperY - (sizeY * (unit.PositionY - minY))),
                            Color = iconColor,
                        });
                    }
                }
            }
        }

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

        public static MapPreviewIcons Default => new MapPreviewIcons(Array.Empty<MapPreviewIcon>());

        public static bool IsRequired => false;

        public MapPreviewIconsFormatVersion FormatVersion
        {
            get => _version;
            set => _version = value;
        }

        public static MapPreviewIcons Parse(Stream stream, bool leaveOpen = false)
        {
            try
            {
                var data = new MapPreviewIcons();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    data._version = reader.ReadInt32<MapPreviewIconsFormatVersion>();

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