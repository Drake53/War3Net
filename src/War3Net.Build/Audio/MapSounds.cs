// ------------------------------------------------------------------------------
// <copyright file="MapSounds.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace War3Net.Build.Audio
{
    public sealed class MapSounds : IEnumerable<Sound>
    {
        public const string FileName = "war3map.w3s";
        public const MapSoundsFormatVersion LatestVersion = MapSoundsFormatVersion.ReforgedV3;

        private readonly List<Sound> _sounds;

        private MapSoundsFormatVersion _version;

        public MapSounds(params Sound[] sounds)
            : this(LatestVersion, sounds)
        {
        }

        public MapSounds(MapSoundsFormatVersion version, params Sound[] sounds)
        {
            _sounds = new List<Sound>(sounds);
            _version = version;
        }

        private MapSounds()
        {
            _sounds = new List<Sound>();
        }

        public static MapSounds Default => new MapSounds(Array.Empty<Sound>());

        public static bool IsRequired => false;

        public MapSoundsFormatVersion FormatVersion
        {
            get => _version;
            set => _version = value;
        }

        public int Count => _sounds.Count;

        public static MapSounds Parse(Stream stream, bool leaveOpen = false)
        {
            try
            {
                var mapSounds = new MapSounds();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    mapSounds._version = (MapSoundsFormatVersion)reader.ReadUInt32();
                    if (mapSounds._version < MapSoundsFormatVersion.Normal || mapSounds._version > LatestVersion)
                    {
                        throw new NotSupportedException($"Unknown version of '{FileName}': {mapSounds._version}");
                    }

                    var soundCount = reader.ReadUInt32();
                    for (var i = 0; i < soundCount; i++)
                    {
                        var sound = Sound.Parse(stream, mapSounds._version, true);
                        mapSounds._sounds.Add(sound);
                    }
                }

                return mapSounds;
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

        public static void Serialize(MapSounds mapSounds, Stream stream, bool leaveOpen = false)
        {
            mapSounds.SerializeTo(stream, leaveOpen);
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write((uint)_version);

                writer.Write(_sounds.Count);
                foreach (var sound in _sounds)
                {
                    sound.WriteTo(writer, _version);
                }
            }
        }

        public Sound GetSound(int index)
        {
            return _sounds[index];
        }

        public void SetSounds(params Sound[] sounds)
        {
            _sounds.Clear();
            _sounds.AddRange(sounds);
        }

        public IEnumerator<Sound> GetEnumerator()
        {
            return ((IEnumerable<Sound>)_sounds).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Sound>)_sounds).GetEnumerator();
        }
    }
}