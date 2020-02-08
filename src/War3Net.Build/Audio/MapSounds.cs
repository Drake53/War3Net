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

using War3Net.Common.Extensions;

namespace War3Net.Build.Audio
{
    public sealed class MapSounds : IEnumerable<Sound>
    {
        public const string FileName = "war3map.w3s";
        public const MapSoundsFormatVersion LatestVersion = MapSoundsFormatVersion.Reforged;

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

        public MapSoundsFormatVersion FormatVersion
        {
            get => _version;
            set => _version = value;
        }

        public static MapSounds Default => new MapSounds(Array.Empty<Sound>());

        public static bool IsRequired => false;

        public int Count => _sounds.Count;

        public static MapSounds Parse(Stream stream, bool leaveOpen = false)
        {
            try
            {
                var mapSounds = new MapSounds();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    var version = (MapSoundsFormatVersion)reader.ReadUInt32();
                    if (version < MapSoundsFormatVersion.Normal || version > LatestVersion)
                    {
                        throw new NotSupportedException($"Unknown version of {FileName}: {mapSounds._version}");
                    }

                    mapSounds._version = version;
                    var soundCount = reader.ReadUInt32();
                    for (var i = 0; i < soundCount; i++)
                    {
                        var sound = Sound.Parse(stream, true);
                        mapSounds._sounds.Add(sound);

                        if (version == MapSoundsFormatVersion.Reforged)
                        {
                            var repeatSoundName = reader.ReadChars();
                            var unk1 = reader.ReadByte();
                            var repeatSoundPath = reader.ReadChars();

                            if (repeatSoundName != sound.Name || repeatSoundPath != sound.FilePath)
                            {
                                throw new InvalidDataException();
                            }

                            var unk2 = reader.ReadInt32();
                            var unk3 = reader.ReadByte();
                            var unk4 = reader.ReadInt32();
                            var unk5 = reader.ReadByte();
                            var unk6 = reader.ReadInt32();
                            var unk7 = reader.ReadInt32();
                        }
                    }
                }

                return mapSounds;
            }
            catch (DecoderFallbackException e)
            {
                throw new InvalidDataException($"The {FileName} file contains invalid characters.", e);
            }
            catch (EndOfStreamException e)
            {
                throw new InvalidDataException($"The {FileName} file is missing data, or its data is invalid.", e);
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
                    sound.WriteTo(writer);
                    if (_version == MapSoundsFormatVersion.Reforged)
                    {
                        writer.WriteString(sound.Name);
                        writer.Write((byte)0);
                        writer.WriteString(sound.FilePath);

                        writer.Write(-1);
                        writer.Write((byte)0);
                        writer.Write(-1);
                        writer.Write((byte)0);
                        writer.Write(0);
                        writer.Write(0);
                    }
                }
            }
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