// ------------------------------------------------------------------------------
// <copyright file="MapCameras.cs" company="Drake53">
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

namespace War3Net.Build.Environment
{
    public sealed class MapCameras : IEnumerable<Camera>
    {
        public const string FileName = "war3map.w3c";
        public const MapCamerasFormatVersion LatestVersion = MapCamerasFormatVersion.Normal;

        private readonly List<Camera> _cameras;

        private MapCamerasFormatVersion _version;
        private bool _newFormat;

        public MapCameras(params Camera[] cameras)
        {
            _cameras = new List<Camera>(cameras);
            _version = LatestVersion;
            _newFormat = true;
        }

        private MapCameras()
        {
            _cameras = new List<Camera>();
        }

        public static MapCameras Default = new MapCameras(Array.Empty<Camera>());

        public static bool IsRequired => false;

        public MapCamerasFormatVersion FormatVersion
        {
            get => _version;
            set => _version = value;
        }

        public bool UseNewFormat
        {
            get => _newFormat;
            set => _newFormat = value;
        }

        public int Count => _cameras.Count;

        public static MapCameras Parse(Stream stream, bool leaveOpen = false)
        {
            try
            {
                var mapCameras = new MapCameras();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    mapCameras._version = reader.ReadInt32<MapCamerasFormatVersion>();
                    mapCameras._newFormat = true;

                    var cameraCount = reader.ReadUInt32();
                    try
                    {
                        for (var i = 0; i < cameraCount; i++)
                        {
                            mapCameras._cameras.Add(Camera.Parse(stream, true, true));
                        }
                    }
                    catch
                    {
                        stream.Position = 8;

                        mapCameras._newFormat = false;
                        mapCameras._cameras.Clear();
                        for (var i = 0; i < cameraCount; i++)
                        {
                            mapCameras._cameras.Add(Camera.Parse(stream, false, true));
                        }
                    }
                }

                return mapCameras;
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

        public static void Serialize(MapCameras mapCameras, Stream stream, bool leaveOpen = false)
        {
            mapCameras.SerializeTo(stream, leaveOpen);
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write((uint)_version);

                writer.Write(_cameras.Count);
                foreach (var camera in _cameras)
                {
                    camera.WriteTo(writer, _newFormat);
                }
            }
        }

        public IEnumerator<Camera> GetEnumerator()
        {
            return ((IEnumerable<Camera>)_cameras).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_cameras).GetEnumerator();
        }
    }
}