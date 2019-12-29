// ------------------------------------------------------------------------------
// <copyright file="MapWidgetsHeader.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;

namespace War3Net.Build.Widget
{
    public sealed class MapWidgetsHeader
    {
        internal const uint HeaderSignature = 0x6F643357; // "W3do"
        internal const MapWidgetsVersion LatestVersion = MapWidgetsVersion.TFT;
        internal const uint LatestSubVersion = 11;

        private MapWidgetsVersion _version;
        private uint _subVersion;
        private uint _dataCount;

        public MapWidgetsVersion Version => _version;

        public uint SubVersion => _subVersion;

        public uint DataCount => _dataCount;

        public static MapWidgetsHeader Parse(Stream stream, bool leaveOpen = false)
        {
            var header = new MapWidgetsHeader();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                if (reader.ReadUInt32() != HeaderSignature)
                {
                    throw new InvalidDataException();
                }

                header._version = (MapWidgetsVersion)reader.ReadUInt32();
                if (!Enum.IsDefined(typeof(MapWidgetsVersion), header._version))
                {
                    throw new NotSupportedException($"Version {header._version} for .doo files is not supported.");
                }

                header._subVersion = reader.ReadUInt32();
                /*if (header._subVersion != LatestSubVersion)
                {
                    throw new NotSupportedException($"Subversion {header._subVersion} is not supported.");
                }*/

                header._dataCount = reader.ReadUInt32();
            }

            return header;
        }
    }
}