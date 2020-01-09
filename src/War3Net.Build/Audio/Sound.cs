// ------------------------------------------------------------------------------
// <copyright file="Sound.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Numerics;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Audio
{
    public sealed class Sound
    {
        private string _variableName;
        private string _filePath;
        private string _eaxSetting; // TODO: enum?

        private SoundFlags _flags;

        private int _fadeInRate;
        private int _fadeOutRate;
        private int _volume;
        private float _pitch;

        private float _pitchVariance; // used when RANDOMPITCH flag is set
        private int _priority;

        private SoundChannel _channel;
        private float _minDistance;
        private float _maxDistance;
        private float _distanceCutoff;

        private float _coneInside;
        private float _coneOutside;
        private int _coneOutsideVolume;
        private Vector3 _coneOrientation;

        public string Name => _variableName;

        public string FilePath => _filePath;

        public string EaxSetting => _eaxSetting;

        public SoundFlags Flags => _flags;

        public int FadeInRate => _fadeInRate;

        public int FadeOutRate => _fadeOutRate;

        public int Volume => _volume;

        public float Pitch => _pitch;

        public SoundChannel Channel => _channel;

        public float MinDistance => _minDistance;

        public float MaxDistance => _maxDistance;

        public float DistanceCutoff => _distanceCutoff;

        public float ConeAngleInside => _coneInside;

        public float ConeAngleOutside => _coneOutside;

        public int ConeOutsideVolume => _coneOutsideVolume;

        public Vector3 ConeOrientation => _coneOrientation;

        public static Sound Parse(Stream stream, bool leaveOpen)
        {
            var sound = new Sound();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                sound._variableName = reader.ReadChars();
                sound._filePath = reader.ReadChars();
                sound._eaxSetting = reader.ReadChars();
                sound._flags = (SoundFlags)reader.ReadInt32();

                sound._fadeInRate = reader.ReadInt32();
                sound._fadeOutRate = reader.ReadInt32();

                sound._volume = reader.ReadInt32();
                sound._pitch = reader.ReadSingle();

                sound._pitchVariance = reader.ReadSingle();
                sound._priority = reader.ReadInt32();

                sound._channel = (SoundChannel)reader.ReadInt32();
                sound._minDistance = reader.ReadSingle();
                sound._maxDistance = reader.ReadSingle();
                sound._distanceCutoff = reader.ReadSingle();

                sound._coneInside = reader.ReadSingle();
                sound._coneOutside = reader.ReadSingle();
                sound._coneOutsideVolume = reader.ReadInt32();
                sound._coneOrientation = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }

            return sound;
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                WriteTo(writer);
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.WriteString(_variableName);
            writer.WriteString(_filePath);
            writer.WriteString(_eaxSetting);
            writer.Write((int)_flags);

            writer.Write(_fadeInRate);
            writer.Write(_fadeOutRate);
            writer.Write(_volume);
            writer.Write(_pitch);

            writer.Write(_pitchVariance);
            writer.Write(_priority);

            writer.Write((int)_channel);
            writer.Write(_minDistance);
            writer.Write(_maxDistance);
            writer.Write(_distanceCutoff);

            writer.Write(_coneInside);
            writer.Write(_coneOutside);
            writer.Write(_coneOutsideVolume);
            writer.Write(_coneOrientation.X);
            writer.Write(_coneOrientation.Y);
            writer.Write(_coneOrientation.Z);
        }
    }
}