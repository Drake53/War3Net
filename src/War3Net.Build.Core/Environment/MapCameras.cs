// ------------------------------------------------------------------------------
// <copyright file="MapCameras.cs" company="Drake53">
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

namespace War3Net.Build.Environment
{
    public sealed class MapCameras
    {
        public const string FileName = "war3map.w3c";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapCameras"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapCameras(MapCamerasFormatVersion formatVersion, bool useNewFormat)
        {
            FormatVersion = formatVersion;
            UseNewFormat = useNewFormat;
        }

        internal MapCameras(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public MapCamerasFormatVersion FormatVersion { get; set; }

        public bool UseNewFormat { get; set; }

        public List<Camera> Cameras { get; init; } = new();

        public override string ToString() => FileName;

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<MapCamerasFormatVersion>();
            UseNewFormat = true;

            nint cameraCount = reader.ReadInt32();
            var oldStreamPosition = reader.BaseStream.Position;
            try
            {
                for (nint i = 0; i < cameraCount; i++)
                {
                    Cameras.Add(reader.ReadCamera(FormatVersion, UseNewFormat));
                }
            }
            catch (Exception e) when (e is DecoderFallbackException || e is EndOfStreamException || e is InvalidDataException)
            {
                reader.BaseStream.Position = oldStreamPosition;

                UseNewFormat = false;
                Cameras.Clear();
                for (nint i = 0; i < cameraCount; i++)
                {
                    Cameras.Add(reader.ReadCamera(FormatVersion, UseNewFormat));
                }
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);

            writer.Write(Cameras.Count);
            foreach (var camera in Cameras)
            {
                writer.Write(camera, FormatVersion, UseNewFormat);
            }
        }
    }
}