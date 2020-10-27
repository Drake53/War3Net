// ------------------------------------------------------------------------------
// <copyright file="MapWidgetsHeader.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed class MapWidgetsHeader
    {
        internal const uint HeaderSignature = 0x6F643357; // "W3do"
        internal const MapWidgetsVersion LatestVersion = MapWidgetsVersion.TFT;
        internal const MapWidgetsSubVersion LatestSubVersion = MapWidgetsSubVersion.V11;

        private MapWidgetsVersion _version;
        private MapWidgetsSubVersion _subVersion;
        private uint _dataCount;

        public MapWidgetsVersion Version
        {
            get => _version;
            internal set => _version = value;
        }

        public MapWidgetsSubVersion SubVersion
        {
            get => _subVersion;
            internal set => _subVersion = value;
        }

        public uint DataCount => _dataCount;

        internal bool UseTftParser => _version == MapWidgetsVersion.TFT && _subVersion == MapWidgetsSubVersion.V11;

        public static MapWidgetsHeader Parse(Stream stream, bool leaveOpen = false)
        {
            var header = new MapWidgetsHeader();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                if (reader.ReadUInt32() != HeaderSignature)
                {
                    throw new InvalidDataException($"Expected file header signature at the start of a .doo file.");
                }

                header._version = reader.ReadInt32<MapWidgetsVersion>();
                header._subVersion = reader.ReadInt32<MapWidgetsSubVersion>();
                header._dataCount = reader.ReadUInt32();
            }

            return header;
        }

        internal static MapWidgetsHeader GetDefault(uint dataCount)
        {
            var header = new MapWidgetsHeader();
            header._version = LatestVersion;
            header._subVersion = LatestSubVersion;
            header._dataCount = dataCount;
            return header;
        }
    }
}