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
        public const uint LatestVersion = 1;

        private readonly List<Sound> _sounds;

        private uint _version;

        public MapSounds(params Sound[] sounds)
        {
            _sounds = new List<Sound>(sounds);
            _version = LatestVersion;
        }

        private MapSounds()
        {
            _sounds = new List<Sound>();
        }

        public static MapSounds Parse(Stream stream, bool leaveOpen = false)
        {
            var mapSounds = new MapSounds();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                mapSounds._version = reader.ReadUInt32();
                if (mapSounds._version != LatestVersion)
                {
                    throw new Exception();
                }

                var soundCount = reader.ReadUInt32();
                for (var i = 0; i < soundCount; i++)
                {
                    mapSounds._sounds.Add(Sound.Parse(stream, true));
                }
            }

            return mapSounds;
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write(LatestVersion);

                writer.Write(_sounds.Count);
                foreach (var region in _sounds)
                {
                    region.WriteTo(writer);
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